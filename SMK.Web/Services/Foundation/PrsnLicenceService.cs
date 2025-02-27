using CertificateWS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Utility;
using SMK.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;
using static CertificateWS.CertificateWSSoapClient;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class PrsnLicenceService : GenericService
    {
        protected readonly IConfiguration configuration;

        public PrsnLicenceService(SMKWEBContext context, SessionManager smgr, IConfiguration configuration)
            : base(context, smgr)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// 取得證書的主邏輯
        /// </summary>
        /// <param name="prsnId"></param>
        /// <param name="IsSync"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<List<PrsnLicenceViewModel>>> GetPrsnLicence(string prsnId, bool IsSync = false)
        {
            var CheckOpen = configuration["OpenQuickSmoking"];
            //強制同步QuitSmoke
            if (IsSync && CheckOpen == "True")
            {
                return await GetPrsnLicence(prsnId);
            }

            var list = await GetPrsnLicences(prsnId);
            if (list.Count == 0)
            {
                //沒資料時嘗試同步QuitSmoke
                return await GetPrsnLicence(prsnId);
            }

            //有資料則抓資料庫
            return new LogicRtnModel<List<PrsnLicenceViewModel>>()
            {
                Data = list
            };
        }

        /// <summary>
        /// 從QuitSmoking取得證書資料
        /// </summary>
        /// <param name="prsnId"></param>
        /// <returns></returns>
        private async Task<LogicRtnModel<List<PrsnLicenceViewModel>>> GetPrsnLicence(string prsnId)
        {
            var token = configuration["QuickSmokingToken"];

            Authentication2 auth = new Authentication2()
            {
                Token = token
            };
            CertificateWSSoapClient? client = null;
            GetCertificateResponse response;
            var logicRtnModel = new LogicRtnModel<List<PrsnLicenceViewModel>>();

            List<QuitSmokingViewModel> quitSmokingData = new List<QuitSmokingViewModel>();
            try
            {
                client = new CertificateWSSoapClient(EndpointConfiguration.CertificateWSSoap12);
                await client.OpenAsync();
                if (client.State == CommunicationState.Faulted)
                {
                    Console.WriteLine("Unable to connect to the proxy.");
                    throw new Exception("連線失敗(Faulted)");
                }
                response = await client.GetCertificateAsync(auth, prsnId);

                try
                {
                    if (response.GetCertificateResult[0] != '[')
                    {
                        response.GetCertificateResult = '[' + response.GetCertificateResult;
                    }
                    if (response.GetCertificateResult[^1] != ']')
                    {
                        response.GetCertificateResult += ']';
                    }
                }
                catch (Exception e)
                {
                    logicRtnModel = new LogicRtnModel<List<PrsnLicenceViewModel>>("加入【】，發生錯誤(Exception)" + e.Message)
                    {
                        StackTrace = e.StackTrace
                    };
                }

                var data = JsonConvert.DeserializeObject<List<QuitSmokingViewModel>>(response.GetCertificateResult);
                quitSmokingData.AddRange(data);
                if (client != null && client.State != CommunicationState.Closing)
                {
                    await client.CloseAsync();
                }
            }
            catch (CommunicationException e)
            {
                client?.Abort();
                logicRtnModel = new LogicRtnModel<List<PrsnLicenceViewModel>>("發生錯誤(CommunicationException)" + e.Message)
                {
                    StackTrace = e.StackTrace
                };
            }
            catch (TimeoutException e)
            {
                client?.Abort();
                logicRtnModel = new LogicRtnModel<List<PrsnLicenceViewModel>>("發生錯誤(TimeoutException)" + e.Message)
                {
                    StackTrace = e.StackTrace
                };
            }
            catch (Exception e)
            {
                client?.Abort();
                logicRtnModel = new LogicRtnModel<List<PrsnLicenceViewModel>>("發生錯誤(Exception)" + e.Message)
                {
                    StackTrace = e.StackTrace
                };
            }
            if (quitSmokingData == null || quitSmokingData.Count == 0 || quitSmokingData[0] == null)
            {
                return new LogicRtnModel<List<PrsnLicenceViewModel>>("查無證照資料");
            }
            List<PrsnLicence> prsnLicences = await getPrsnLicence(quitSmokingData);

            using (var txn = context.GetTransactionScope())
            {
                try
                {
                    var remove = context.PrsnLicence.Where(p => p.PrsnId == prsnId);
                    if (remove.Count() > 0)
                    {
                        context.RemoveRange(remove);
                    }
                    context.PrsnLicence.AddRange(prsnLicences);
                    var result = await context.SaveChangesWithAuditAsync(identity.Account, "更新");

                    await txn.CommitAsync();

                    var list = await GetPrsnLicences(prsnId);
                    logicRtnModel = new LogicRtnModel<List<PrsnLicenceViewModel>>() { Data = list };
                }
                catch (Exception err)
                {
                    await txn.RollbackAsync();
                    return new LogicRtnModel<List<PrsnLicenceViewModel>>(MsgType.SaveFail, err.Message)
                    {
                        StackTrace = err.StackTrace
                    };
                }
            }
            return logicRtnModel;
        }

        /// <summary>
        /// 轉換QuitSmoking 至 PrsnLicence
        /// </summary>
        /// <param name="quitSmokingViewModels"></param>
        /// <returns></returns>
        private async Task<List<PrsnLicence>> getPrsnLicence(List<QuitSmokingViewModel> quitSmokingViewModels)
        {
            var licenceTypeMap = await context.QsLicenceMap.ToListAsync();
            return quitSmokingViewModels.Select(p => new PrsnLicence()
            {
                PrsnId = p.PersonId,
                LicenceType = licenceTypeMap.FirstOrDefault(q => q.CTypeSNO == p.CTypeSno)?.LicenceType,
                LicenceNo = p.CTypeString,
                CertPublicDate = p.CertPublicDate.ToDate(),
                CertStartDate = p.CertStartDate.ToDate(),
                CertEndDate = p.CertEndDate.ToDate(),
                Remark = "",
                UpdatedAt = DateTime.Now,
                UpdatedBy = identity.Account
            }).ToList();
        }
        /// <summary>
        /// 取得醫事人員證書清單
        /// </summary>
        /// <param name="PrsnId"></param>
        /// <returns></returns>
        private async Task<List<PrsnLicenceViewModel>> GetPrsnLicences(string PrsnId)
        {
            var list = from inner in context.PrsnLicence.Where(p => p.PrsnId == PrsnId)
                       join outer in context.GenLicenceType on inner.LicenceType equals outer.LicenceType
                       select new PrsnLicenceViewModel()
                       {
                           Id = inner.Id,
                           PrsnId = inner.PrsnId,
                           LicenceNo = inner.LicenceNo,
                           CertStartDate = inner.CertStartDate,
                           CertPublicDate = inner.CertPublicDate,
                           CertEndDate = inner.CertEndDate,
                           LicenceType = inner.LicenceType,
                           LicenceName = outer.LicenceName,
                           CreatedAt = inner.CreatedAt,
                           UpdatedAt = inner.UpdatedAt,
                           UpdatedBy = inner.UpdatedBy,
                           Remark = inner.Remark
                       };
            return await list.ToListAsync();
        }
        public async Task<LogicRtnModel<string>> GetAllPrsnLicence()
        {
            var token = configuration["QuickSmokingToken"];

            Authentication2 auth = new Authentication2()
            {
                Token = token
            };
            CertificateWSSoapClient? client = null;
            GetAllCertificateResponse response1;
            var logicRtnModel = new LogicRtnModel<string>();

            try
            {
                client = new CertificateWSSoapClient(EndpointConfiguration.CertificateWSSoap12);
                await client.OpenAsync();
                if (client.State == CommunicationState.Faulted)
                {
                    System.Console.WriteLine("Unable to connect to the proxy.");
                    throw new Exception("連線失敗(Faulted)");
                }
                response1 = await client.GetAllCertificateAsync(auth);
                logicRtnModel = new LogicRtnModel<string>() { Data = response1.GetAllCertificateResult };
                if (client != null && client.State != CommunicationState.Closing)
                {
                    await client.CloseAsync();
                }
            }
            catch (CommunicationException e)
            {
                client?.Abort();
                logicRtnModel = new LogicRtnModel<string>("發生錯誤(CommunicationException)" + e.Message)
                {
                    StackTrace = e.StackTrace
                };
            }
            catch (TimeoutException e)
            {
                client?.Abort();
                logicRtnModel = new LogicRtnModel<string>("發生錯誤(TimeoutException)" + e.Message)
                {
                    StackTrace = e.StackTrace
                };
            }
            catch (Exception e)
            {
                client?.Abort();
                logicRtnModel = new LogicRtnModel<string>("發生錯誤(Exception)" + e.Message)
                {
                    StackTrace = e.StackTrace
                };
            }
            return logicRtnModel;
        }
    }
}
