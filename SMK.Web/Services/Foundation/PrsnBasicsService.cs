using Microsoft.EntityFrameworkCore;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Utility;
using SMK.Web.Models;
using SMK.Web.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;
using Yozian.Extension;
using SMK.Web.Extensions;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class PrsnBasicsService : GenericService
    {
        protected readonly PrsnLicenceService prsnLicenceService;
        private readonly PrsnEmailService prsnEmailService;

        public PrsnBasicsService(SMKWEBContext context, SessionManager smgr, PrsnLicenceService prsnLicenceService,
            PrsnEmailService prsnEmailService)
            : base(context, smgr)
        {
            this.prsnLicenceService = prsnLicenceService;
            this.prsnEmailService = prsnEmailService;
        }
        /// <summary>
        /// 建立醫事人員資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PrsnBasicViewModel>> CreatePrsn(PrsnBasicViewModel model)
        {
            var logicRtnModel = new LogicRtnModel<PrsnBasicViewModel>()
            {
                Data = model
            };
            try
            {
                var prsnBasic = context.PrsnBasic.Find(model.PrsnId);
                if (prsnBasic != null)
                {
                    return new LogicRtnModel<PrsnBasicViewModel>("身分證字號重複");
                }
                prsnBasic = new PrsnBasic()
                {
                    PrsnId = model.PrsnId,
                    PrsnName = model.PrsnName,
                    PrsnBirthday = model.PrsnBirthday.TwDateToDateTime(),
                    PrsnType = model.PrsnType,
                    MajorSpecialistNo = model.MajorSpecialistNo,
                    SubSpecialistNo = model.SubSpecialistNo,
                    Remark = model.Remark,
                    Pemail = model.Pemail,
                };
                var prsnEmails = await Query(c => c.PrsnEmail.Where(x => x.PrsnId == prsnBasic.PrsnId));
                if (prsnEmails.IsSuccess)
                {
                    context.PrsnEmail.RemoveRange(prsnEmails.Data);
                }
                if (!string.IsNullOrWhiteSpace(model.PrsnEmails))
                {
                    var emails = model.PrsnEmails.Split(",").Distinct();
                    foreach (var email in emails)
                    {
                        if (!string.IsNullOrWhiteSpace(email))
                        {
                            context.PrsnEmail.Add(new PrsnEmail()
                            {
                                PrsnId = prsnBasic.PrsnId,
                                Pemail = email
                            });
                        }
                    }
                }

                var createRtnModel = await Create(prsnBasic, new PrsnBasicsValidator().Validate);
                logicRtnModel.extends(createRtnModel);
                if (createRtnModel.IsSuccess)
                {
                    await context.Entry(prsnBasic).ReloadAsync();
                    logicRtnModel.Data = PrsnBasicViewModel.GetPrsnBasicViewModel(prsnBasic);
                }
                return logicRtnModel;
            }
            catch (Exception err)
            {
                return new LogicRtnModel<PrsnBasicViewModel>()
                {
                    IsSuccess = false,
                    StackTrace = err.StackTrace,
                    ErrMsg = err.Message
                };
            }
        }

        public async Task<LogicRtnModel<PrsnBasicViewModel>> UpdatePrsn(PrsnBasicViewModel model)
        {
            using (var txn = context.GetTransactionScope())
                try
                {
                    var prsnBasic = context.PrsnBasic.Find(model.PrsnId);
                    if (prsnBasic == null)
                    {
                        return new LogicRtnModel<PrsnBasicViewModel>("查無身分證資料");
                    }
                    prsnBasic.PrsnName = model.PrsnName;
                    prsnBasic.PrsnType = model.PrsnType;
                    prsnBasic.PrsnBirthday = model.PrsnBirthday.TwDateToDateTime();
                    prsnBasic.MajorSpecialistNo = model.MajorSpecialistNo;
                    if (!string.IsNullOrEmpty(model.SubSpecialistNo))
                        prsnBasic.SubSpecialistNo = model.SubSpecialistNo;
                    prsnBasic.Remark = model.Remark;
                    context.PrsnBasic.Update(prsnBasic);

                    var prsnEmails = await Query(c => c.PrsnEmail.Where(x => x.PrsnId == prsnBasic.PrsnId));
                    if (prsnEmails.IsSuccess)
                    {
                        context.PrsnEmail.RemoveRange(prsnEmails.Data);
                    }
                    if (!string.IsNullOrWhiteSpace(model.PrsnEmails))
                    {
                        var emails = model.PrsnEmails.Split(",").Distinct();
                        foreach (var email in emails)
                        {
                            if (!string.IsNullOrWhiteSpace(email))
                            {
                                context.PrsnEmail.Add(new PrsnEmail()
                                {
                                    PrsnId = prsnBasic.PrsnId,
                                    Pemail = email
                                });
                            }
                        }
                    }

                    await context.SaveChangesWithAuditAsync(identity.Account, "更新");
                    await context.Entry(prsnBasic).ReloadAsync();
                    await txn.CommitAsync();

                    var data = PrsnBasicViewModel.GetPrsnBasicViewModel(prsnBasic);
                    data.PrsnEmails = model.PrsnEmails;
                    return new LogicRtnModel<PrsnBasicViewModel>()
                    {
                        Data = data
                    };
                }
                catch (Exception err)
                {
                    await txn.RollbackAsync();
                    return new LogicRtnModel<PrsnBasicViewModel>()
                    {
                        IsSuccess = false,
                        StackTrace = err.StackTrace,
                        ErrMsg = err.Message
                    };
                }
        }

        /// <summary>
        /// 合約醫事人員報表查詢
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<PrsnContactReportViewModel>>> QueryPrsnContactList(PrsnBasicQueryModel model)
        {
            LogicRtnModel<PagedModel<PrsnContactReportViewModel>> logicRtnModel = new LogicRtnModel<PagedModel<PrsnContactReportViewModel>>()
            {
                IsSuccess = true,
                Data = null
            };

            //var prsnContract = context.PrsnContract
            //                       .WhereWhen(!string.IsNullOrEmpty(model.PrsnId), x => x.PrsnId == model.PrsnId)
            //                       .WhereWhen(!string.IsNullOrEmpty(model.PrsnName),
            //                       x => context.PrsnBasic.Any(y => x.PrsnId == y.PrsnId && y.PrsnName == model.PrsnName));
            switch (model.HospStatus)
            {
                case "1":
                    model.HospStatus = "申請合約";
                    break;
                case "2":
                    model.HospStatus = "合約有效";
                    break;
                case "3":
                    model.HospStatus = "合約終止";
                    break;
                default:
                    break;
            }
            string startdate = model.PrsnStartDate.HasValue ? model.PrsnStartDate.Value.ToString().ToYYYYMMDDFromTaiwan() : "";
            string enddate = model.PrsnEndDate.HasValue ? model.PrsnEndDate.Value.ToString().ToYYYYMMDDFromTaiwan() : "";
            var prsnContract = context.PrsnContactReport
                .WhereWhen(!string.IsNullOrEmpty(model.HospId), x => x.醫事機構代碼 == model.HospId)
                .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.院區別 == model.HospSeqNo)
                .WhereWhen(!string.IsNullOrEmpty(model.HospStatus), x => x.機構狀態 == model.HospStatus)
                .WhereWhen(!string.IsNullOrEmpty(model.PrsnType), x => x.人員類別 == model.PrsnType)
                .WhereWhen(!string.IsNullOrEmpty(startdate), x => string.Compare(startdate, x.人員合約起日) <= 0)
                .WhereWhen(!string.IsNullOrEmpty(enddate), x => string.IsNullOrEmpty(x.人員合約迄日) || string.Compare(x.人員合約迄日, startdate) <= 0)
                .WhereWhen(!string.IsNullOrEmpty(model.PrsnId), x => x.身分證號 == model.PrsnId)
                //.WhereWhen(!string.IsNullOrEmpty(model.PrsnType), x => x.人員類別代號 == model.PrsnType)//多餘的，它會影響到人員類別代號的判斷
                .WhereWhen(model.CouldInstruct && model.CouldTreat, x => x.服務類型 == "衛教" || x.服務類型 == "用藥")//兩個都勾選
                .WhereWhen(model.CouldInstruct && model.CouldTreat == false, x => x.服務類型 == "衛教")//只勾選衛教
                .WhereWhen(model.CouldInstruct == false && model.CouldTreat, x => x.服務類型 == "用藥")//只勾選用藥
                //.WhereWhen(model.CouldInstruct, x => x.服務類型 == "衛教")
                //.WhereWhen(model.CouldTreat, x => x.服務類型 == "用藥")
                .WhereWhen(!string.IsNullOrEmpty(model.PrsnName), x => context.PrsnBasic.Any(y => x.身分證號 == y.PrsnId && y.PrsnName == model.PrsnName))
                .Select(x => new PrsnContactReportViewModel()
                {
                    人員合約起日 = x.人員合約起日,
                    人員合約迄日 = x.人員合約迄日,
                    人員類別 = x.人員類別,
                    出生日期 = x.出生日期,
                    姓名 = x.姓名,
                    服務類型 = x.服務類型,
                    機構名稱 = x.機構名稱,
                    機構狀態 = x.機構狀態,
                    身分證號 = x.身分證號,
                    醫事機構代碼 = x.醫事機構代碼,
                    院區別 = x.院區別
                });
            if (!prsnContract.Any())
            {
                logicRtnModel.IsSuccess = false;
                logicRtnModel.ErrMsg = "查無建檔資料";
                return logicRtnModel;
            }
            //var result = from contact in prsnContract
            //             join outer in context.PrsnBasic on contact.PrsnId equals outer.PrsnId
            //             join inner in context.GenSpecial on outer.MajorSpecialistNo equals inner.SpecialistNo into majorDetail
            //             from majorResult in majorDetail.DefaultIfEmpty()
            //             join inner1 in context.GenSpecial on outer.SubSpecialistNo equals inner1.SpecialistNo into subDetail
            //             from subResult in subDetail.DefaultIfEmpty()
            //             join inner2 in context.GenPrsnType on outer.PrsnType equals inner2.PrsnType into prsnTypeDetail
            //             from prsnTypResult in prsnTypeDetail.DefaultIfEmpty()
            //             select new PrsnContactReportViewModel()
            //             {
            //                 PrsnId = outer.PrsnId,
            //                 PrsnName = outer.PrsnName,
            //                 PrsnBirthday = outer.PrsnBirthday,
            //                 PrsnType = outer.PrsnType,
            //                 PrsnTypeName = prsnTypResult != null ? prsnTypResult.PrsnTypeName : string.Empty,
            //                 MajorSpecialistNo = outer.MajorSpecialistNo,
            //                 SubSpecialistNo = outer.SubSpecialistNo,
            //                 Remark = outer.Remark,
            //                 Pemail = outer.Pemail,
            //                 MajorSpecialistName = majorResult != null ? majorResult.SpecialistName : string.Empty,
            //                 SubSpecialistName = subResult != null ? subResult.SpecialistName : string.Empty,
            //                 PrsnStartDate = contact.PrsnStartDate,
            //                 PrsnEndDate = contact.PrsnEndDate
            //             };


            return await QueryPaging(model, prsnContract);
        }


        /// <summary>
        /// 醫事人員清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<PrsnBasicViewModel>>> QueryPrsnBasicList(PrsnBasicQueryModel model)
        {
            LogicRtnModel<PagedModel<PrsnBasicViewModel>> logicRtnModel = new LogicRtnModel<PagedModel<PrsnBasicViewModel>>()
            {
                IsSuccess = true,
                Data = null
            };

            var prsnBasic = context.PrsnBasic
                                   .WhereWhen(!string.IsNullOrEmpty(model.PrsnId), x => x.PrsnId == model.PrsnId)
                                   .WhereWhen(!string.IsNullOrEmpty(model.PrsnName), x => x.PrsnName == model.PrsnName);
            if (!prsnBasic.Any())
            {
                logicRtnModel.IsSuccess = false;
                logicRtnModel.ErrMsg = "查無建檔資料";
                return logicRtnModel;
            }
            var result = from outer in prsnBasic
                         join inner in context.GenSpecial on outer.MajorSpecialistNo equals inner.SpecialistNo into majorDetail
                         from majorResult in majorDetail.DefaultIfEmpty()
                         join inner1 in context.GenSpecial on outer.SubSpecialistNo equals inner1.SpecialistNo into subDetail
                         from subResult in subDetail.DefaultIfEmpty()
                         join inner2 in context.GenPrsnType on outer.PrsnType equals inner2.PrsnType into prsnTypeDetail
                         from prsnTypResult in prsnTypeDetail.DefaultIfEmpty()
                         select new PrsnBasicViewModel()
                         {
                             PrsnId = outer.PrsnId,
                             PrsnName = outer.PrsnName,
                             PrsnBirthday = outer.PrsnBirthday,
                             PrsnType = outer.PrsnType,
                             PrsnTypeName = prsnTypResult != null ? prsnTypResult.PrsnTypeName : string.Empty,
                             MajorSpecialistNo = outer.MajorSpecialistNo,
                             SubSpecialistNo = outer.SubSpecialistNo,
                             Remark = outer.Remark,
                             Pemail = outer.Pemail,
                             MajorSpecialistName = majorResult != null ? majorResult.SpecialistName : string.Empty,
                             SubSpecialistName = subResult != null ? subResult.SpecialistName : string.Empty
                         };


            return await QueryPaging(model, result);
        }
        public async Task<LogicRtnModel<PrsnBasicViewModel>> QueryPrsnBasic(string prsnId)
        {
            LogicRtnModel<PrsnBasicViewModel> logicRtnModel = new LogicRtnModel<PrsnBasicViewModel>()
            {
                IsSuccess = true
            };

            var prsnBasic = await context.PrsnBasic.FindAsync(prsnId);
            if (prsnBasic == null)
            {
                logicRtnModel.IsSuccess = false;
                logicRtnModel.ErrMsg = "查無建檔資料";
                return logicRtnModel;
            }
            var prsnBasicmodel = PrsnBasicViewModel.GetPrsnBasicViewModel(prsnBasic);
            prsnBasicmodel.prsnContracts = GetPrsnContract(prsnId);
            var prsnLicence = await prsnLicenceService.GetPrsnLicence(prsnId);
            if (prsnLicence.IsSuccess)
                prsnBasicmodel.prsnLicences = prsnLicence.Data;

            var prsnEmails = await prsnEmailService.QueryPrsnEmails(prsnId);
            if (prsnEmails.IsSuccess)
            {
                var emails = new List<string>();
                foreach (var prsnEmailViewModel in prsnEmails.Data)
                {
                    emails.Add(prsnEmailViewModel.Pemail);
                }

                prsnBasicmodel.PrsnEmails = string.Join(",", emails);
            }
            
            logicRtnModel.Data = prsnBasicmodel;
            return logicRtnModel;
        }

        /// <summary>
        /// 更新醫事人員合約
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PrsnContractViewModel>> InsertPrsnContract(PrsnContractViewModel model)
        {
            LogicRtnModel<PrsnContractViewModel> logicRtnModel = new LogicRtnModel<PrsnContractViewModel>()
            {
                IsSuccess = true
            };

            bool IsPrsnContract = await context.PrsnContract.AnyAsync(p => p.HospId == model.HospId
                                                                    && p.HospSeqNo == model.HospSeqNo
                                                                    && p.PrsnId == model.PrsnId
                                                                    && string.IsNullOrEmpty(p.PrsnStartDate));
            if (IsPrsnContract)
            {
                logicRtnModel.IsSuccess = false;
                logicRtnModel.ErrMsg = "已有重複資料申請中";
                return logicRtnModel;
            }

            var prsnContract = new PrsnContract()
            {
                HospId = model.HospId,
                HospSeqNo = model.HospSeqNo,
                PrsnId = model.PrsnId,
                PrsnStartDate = model.PrsnStartDate.TwDateToDateTime(),
                SmkcontractType = model.SmkcontractType,
                PrsnEndDate = model.PrsnEndDate.TwDateToDateTime(),
                EndReasonNo = model.EndReasonNo,
                CouldTreat = model.CouldTreat,
                CouldInstruct = model.CouldInstruct,
                Remark = model.Remark,
                CreateDate = DateTime.Now.ToDate(),
                ModifyDate = DateTime.Now.ToDate()
            };
            var reult = await Create(prsnContract);
            if (!reult.IsSuccess)
            {
                logicRtnModel.SetMsgType(MsgType.CreateFail);
                logicRtnModel.IsSuccess = false;
                logicRtnModel.ErrMsg = "新增失敗";
                return logicRtnModel;
            }
            //await context.Entry(prsnContract).ReloadAsync();
            //logicRtnModel.Data = GetPrsnContract(prsnContract.Id);
            return logicRtnModel;
        }


        /// <summary>
        /// 更新HospContract的時間
        /// </summary>
        /// <param name="modelUpdateFileTime"></param>
        /// <param name="hospid"></param>
        /// <param name="hospseqno"></param>
        /// <param name="CreateDate"></param>
        /// <param name="SMKContractType"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<HospContractViewModel>> UploadFileTime(DateTime modelUpdateFileTime ,int Id)
        {
            LogicRtnModel<HospContractViewModel> logicRtnModel = new LogicRtnModel<HospContractViewModel>()
            {
                IsSuccess = true
            };
            var HospContractDate = context.HospContract.Where(x => x.Id == Id).FirstOrDefault();
            HospContractDate.UpdateFileTime = modelUpdateFileTime;

            var reult = await Update(HospContractDate, null, true, x => x.UpdateFileTime);

            if (!reult.IsSuccess)
            {
                logicRtnModel.SetMsgType(MsgType.SaveFail);
                logicRtnModel.IsSuccess = false;
                logicRtnModel.ErrMsg = reult.StackTrace;
                return logicRtnModel;
            }

            return logicRtnModel;
        }

        public async Task<LogicRtnModel<IEnumerable<PrsnLicenceViewModel>>> AddPrsnLicense(PrsnLicenceViewModel model)
        {
            var logicRtnModel = new LogicRtnModel<IEnumerable<PrsnLicenceViewModel>>()
            {
                IsSuccess = true
            };

            bool IsPrsnLicence = await context.PrsnLicence.AnyAsync(p => p.PrsnId == model.PrsnId
                                                                    && p.LicenceNo == model.LicenceNo
                                                                    && p.LicenceType == model.LicenceType
                                                                    && string.IsNullOrEmpty(p.CertStartDate));
            if (IsPrsnLicence)
            {
                logicRtnModel.IsSuccess = false;
                logicRtnModel.ErrMsg = "已有重複資料申請中";
                return logicRtnModel;
            }
            var data = await prsnLicenceService.GetPrsnLicence(model.PrsnId);
            var prsnLicences = data.Data;
            prsnLicences.Add(new PrsnLicenceViewModel()
            {
                PrsnId = model.PrsnId,
                CertEndDate = model.CertEndDate,
                CertPublicDate = model.CertPublicDate==null?"": model.CertPublicDate,
                CertStartDate = model.CertStartDate,
                CreatedAt = DateTime.Now,
                LicenceName = model.LicenceName,
                LicenceType = model.LicenceType,
                LicenceNo = model.LicenceNo,
                Remark = model.Remark,
                UpdatedBy = model.UpdatedBy,
                UpdatedAt = DateTime.Now
            });

            var prsnLicence = new PrsnLicence()
            {
                PrsnId = model.PrsnId,
                CertEndDate = model.CertEndDate,
                CertPublicDate = model.CertPublicDate,
                CertStartDate = model.CertStartDate,
                CreatedAt = DateTime.Now,
                LicenceType = model.LicenceType,
                LicenceNo = model.LicenceNo,
                Remark = model.Remark,
                UpdatedBy = model.UpdatedBy,
                UpdatedAt = DateTime.Now
            };
            var reult = await Create(prsnLicence);
            if (!reult.IsSuccess)
            {
                logicRtnModel.SetMsgType(MsgType.CreateFail);
                logicRtnModel.IsSuccess = false;
                logicRtnModel.ErrMsg = "新增失敗";
                return logicRtnModel;
            }
            await context.Entry(prsnLicence).ReloadAsync();
            logicRtnModel.Data = prsnLicences;
            return logicRtnModel;
        }

        public async Task<LogicRtnModel<PrsnLicenceViewModel>> UpdatePrsnLicense(PrsnLicenceViewModel model)
        {
            LogicRtnModel<PrsnLicenceViewModel> logicRtnModel = new LogicRtnModel<PrsnLicenceViewModel>()
            {
                IsSuccess = true
            };

            var prsnLicence = await context.PrsnLicence.FindAsync(model.Id);
            if (prsnLicence == null)
            {
                logicRtnModel.IsSuccess = false;
                logicRtnModel.ErrMsg = "查無建檔資料";
                return logicRtnModel;
            }
            prsnLicence.LicenceNo = model.LicenceNo;
            prsnLicence.LicenceType = model.LicenceType;
            prsnLicence.CertStartDate = model.CertStartDate;
            prsnLicence.CertEndDate = model.CertEndDate;
            prsnLicence.Remark = model.Remark;
            prsnLicence.UpdatedAt = DateTime.Now;

            var reult = await Update(prsnLicence, null, true,
                x => x.LicenceNo,
                x => x.LicenceType,
                x => x.CertStartDate,
                x => x.CertEndDate,
                x => x.Remark,
                x => x.UpdatedAt);

            if (!reult.IsSuccess)
            {
                logicRtnModel.SetMsgType(MsgType.SaveFail);
                logicRtnModel.IsSuccess = false;
                logicRtnModel.ErrMsg = reult.StackTrace;
                return logicRtnModel;
            }
            logicRtnModel.Data = model;
            return logicRtnModel;
        }

        public async Task<LogicRtnModel<IEnumerable<PrsnContractViewModel>>> AddPrsnContract(PrsnContractViewModel model)
        {
            var logicRtnModel = new LogicRtnModel<IEnumerable<PrsnContractViewModel>>()
            {
                IsSuccess = true
            };

            bool IsPrsnContract = await context.PrsnContract.AnyAsync(p => p.HospId == model.HospId
                                                                    && p.HospSeqNo == model.HospSeqNo
                                                                    && p.PrsnId == model.PrsnId
                                                                    && string.IsNullOrEmpty(p.PrsnStartDate));
            if (IsPrsnContract)
            {
                logicRtnModel.IsSuccess = false;
                logicRtnModel.ErrMsg = "已有重複資料申請中";
                return logicRtnModel;
            }

            HospBasic hospBasic = null;
            if (!string.IsNullOrEmpty(model.HospId))
            {
                hospBasic = await context.HospBasic
                    .Where(e => e.HospId == model.HospId && e.HospSeqNo == model.HospSeqNo)
                    .FirstAsync();
            }

            GenSmkcontract smkContract = null;
            if (!string.IsNullOrEmpty(model.SmkcontractType))
            {
                smkContract = await context.GenSmkcontract
                    .Where(e => e.SmkcontractType == model.SmkcontractType)
                    .FirstAsync();
            }

            GenPrsnEndReason genPrsnEndReason = null;
            if (!string.IsNullOrEmpty(model.EndReasonNo))
            {
                genPrsnEndReason = await context.GenPrsnEndReason
                    .Where(e => e.EndReasonNo == model.EndReasonNo)
                    .FirstAsync();
            }

            var prsnContracts = GetPrsnContract(model.PrsnId);
            prsnContracts.Add(new PrsnContractViewModel()
            {
                HospId = model.HospId,
                HospSeqNo = model.HospSeqNo,
                HospName = hospBasic != null ? hospBasic.HospName : "",
                PrsnId = model.PrsnId,
                PrsnStartDate = model.PrsnStartDate.TwDateToDateTime(),
                SmkcontractType = model.SmkcontractType,
                SmkcontractTypeNam = smkContract != null ? smkContract.SmkcontractName : "",
                PrsnEndDate = model.PrsnEndDate.TwDateToDateTime(),
                EndReasonNo = model.EndReasonNo,
                EndReasonName = genPrsnEndReason != null ? genPrsnEndReason.EndReasonName : "",
                CouldTreat = model.CouldTreat,
                CouldInstruct = model.CouldInstruct,
                Remark = model.Remark,
                CreateDate = DateTime.Now.ToDate(),
                ModifyDate = DateTime.Now.ToDate()
            });
            
            var prsnContract = new PrsnContract()
            {
                HospId = model.HospId,
                HospSeqNo = model.HospSeqNo,
                PrsnId = model.PrsnId,
                PrsnStartDate = model.PrsnStartDate.TwDateToDateTime(),
                SmkcontractType = model.SmkcontractType,
                PrsnEndDate = model.PrsnEndDate.TwDateToDateTime(),
                EndReasonNo = model.EndReasonNo,
                CouldTreat = model.CouldTreat,
                CouldInstruct = model.CouldInstruct,
                Remark = model.Remark,
                CreateDate = DateTime.Now.ToDate(),
                ModifyDate = DateTime.Now.ToDate()
            };
            var reult = await Create(prsnContract);
            if (!reult.IsSuccess)
            {
                logicRtnModel.SetMsgType(MsgType.CreateFail);
                logicRtnModel.IsSuccess = false;
                logicRtnModel.ErrMsg = "新增失敗";
                return logicRtnModel;
            }
            await context.Entry(prsnContract).ReloadAsync();
            logicRtnModel.Data = prsnContracts;
            return logicRtnModel;
        }


        /// <summary>
        /// 更新醫事人員合約
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PrsnContractViewModel>> UpdatePrsnContract(PrsnContractViewModel model)
        {
            LogicRtnModel<PrsnContractViewModel> logicRtnModel = new LogicRtnModel<PrsnContractViewModel>()
            {
                IsSuccess = true
            };

            var prsnContract = await context.PrsnContract.FindAsync(model.Id);
            if (prsnContract == null)
            {
                logicRtnModel.IsSuccess = false;
                logicRtnModel.ErrMsg = "查無建檔資料";
                return logicRtnModel;
            }
            prsnContract.SmkcontractType = model.SmkcontractType;
            prsnContract.PrsnEndDate = model.PrsnEndDate.TwDateToDateTime();
            prsnContract.EndReasonNo = model.EndReasonNo;
            prsnContract.CouldTreat = model.CouldTreat;
            prsnContract.CouldInstruct = model.CouldInstruct;
            prsnContract.Remark = model.Remark;
            prsnContract.ModifyDate = DateTime.Now.ToDate();

            var reult = await Update(prsnContract, null, true,
                x => x.SmkcontractType,
                x => x.PrsnEndDate,
                x => x.EndReasonNo,
                x => x.CouldTreat,
                x => x.CouldInstruct,
                x => x.Remark,
                x => x.ModifyDate);

            if (!reult.IsSuccess)
            {
                logicRtnModel.SetMsgType(MsgType.SaveFail);
                logicRtnModel.IsSuccess = false;
                logicRtnModel.ErrMsg = reult.StackTrace;
                return logicRtnModel;
            }
            logicRtnModel.Data = GetPrsnContract(model.Id);
            return logicRtnModel;
        }


        /// <summary>
        /// 取得醫事人員合約清單
        /// </summary>
        /// <param name="PrsnId"></param>
        /// <returns></returns>
        public List<PrsnContractViewModel> GetPrsnContract(string PrsnId)
        {
            var list = from inner in context.PrsnContract.Where(p => p.PrsnId == PrsnId)
                       join basic in context.HospBasic on new { inner.HospId, inner.HospSeqNo } equals new { basic.HospId, basic.HospSeqNo }
                       join prsnEndReason in context.GenPrsnEndReason on inner.EndReasonNo equals prsnEndReason.EndReasonNo into prsnReson
                       from reasonResult in prsnReson.DefaultIfEmpty()
                       join smkcontract in context.GenSmkcontract on inner.SmkcontractType equals smkcontract.SmkcontractType
                       let IsOffend = context.HospContract.Any(contract => inner.HospId == contract.HospId
                                                                       && inner.HospSeqNo == contract.HospSeqNo
                                                                       && contract.EndReasonNo == "22")
                       select new PrsnContractViewModel()
                       {
                           Id = inner.Id,
                           HospId = inner.HospId,
                           PrsnId = inner.PrsnId,
                           SmkcontractType = inner.SmkcontractType,
                           PrsnStartDate = inner.PrsnStartDate,
                           PrsnEndDate = inner.PrsnEndDate,
                           CreateDate = inner.CreateDate,
                           ModifyDate = inner.ModifyDate,
                           ModifyPersonNo = inner.ModifyPersonNo,
                           Remark = inner.Remark,
                           HospSeqNo = inner.HospSeqNo,
                           CouldTreat = inner.CouldTreat,
                           CouldInstruct = inner.CouldInstruct,
                           EndReasonNo = inner.EndReasonNo,
                           EndReasonName = reasonResult == null ? "" : reasonResult.EndReasonName,
                           HospName = basic.HospName,
                           SmkcontractTypeNam = smkcontract.SmkcontractName,
                           IsOffend = IsOffend
                       };
            return list.ToList();
        }

       
        /// <summary>
        /// 依照醫院合約Id取得醫事人員合約
        /// </summary>
        /// <param name="PrsnId"></param>
        /// <returns></returns>
        public PrsnContractViewModel GetPrsnContract(int Id)
        {
            var list = from inner in context.PrsnContract.Where(p => p.Id == Id)
                       join basic in context.HospBasic on new { inner.HospId, inner.HospSeqNo } equals new { basic.HospId, basic.HospSeqNo }
                       join prsnEndReason in context.GenPrsnEndReason on inner.EndReasonNo equals prsnEndReason.EndReasonNo into prsnReson
                       from reasonResult in prsnReson.DefaultIfEmpty()
                       join smkcontract in context.GenSmkcontract on inner.SmkcontractType equals smkcontract.SmkcontractType
                       let IsOffend = context.HospContract.Any(contract => inner.HospId == contract.HospId
                                                                       && inner.HospSeqNo == contract.HospSeqNo
                                                                       && contract.EndReasonNo == "22")
                       select new PrsnContractViewModel()
                       {
                           Id = inner.Id,
                           HospId = inner.HospId,
                           PrsnId = inner.PrsnId,
                           SmkcontractType = inner.SmkcontractType,
                           PrsnStartDate = inner.PrsnStartDate,
                           PrsnEndDate = inner.PrsnEndDate,
                           CreateDate = inner.CreateDate,
                           ModifyDate = inner.ModifyDate,
                           ModifyPersonNo = inner.ModifyPersonNo,
                           Remark = inner.Remark,
                           HospSeqNo = inner.HospSeqNo,
                           CouldTreat = inner.CouldTreat,
                           CouldInstruct = inner.CouldInstruct,
                           EndReasonNo = inner.EndReasonNo,
                           EndReasonName = reasonResult == null ? "" : reasonResult.EndReasonName,
                           HospName = basic.HospName,
                           SmkcontractTypeNam = smkcontract.SmkcontractName
                       };
            return list.FirstOrDefault();
        }
    }
}
