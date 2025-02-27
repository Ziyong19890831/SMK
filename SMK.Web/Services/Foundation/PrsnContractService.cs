using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Data.Utility;
using SMK.Web.Extensions;
using SMK.Web.Helpers;
using SMK.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;
using Yozian.Extension;
using Yozian.WebCore.Library.Utility.Excel;

namespace SMK.Web.Services.Foundation
{
    /// <summary>
    /// 機構合約
    /// </summary>
    [ScopedService]
    public class PrsnContractService : GenericService
    {
        private readonly PersistenceService persistenceService;
        public PrsnContractService(SMKWEBContext context, SessionManager smgr,
            PersistenceService persistenceService)
           : base(context, smgr)
        {
            this.persistenceService = persistenceService;
        }
        /// <summary>
        /// 取得待核准的合約清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<PrsnContractListViewModel>>> GetPrsnContracts(PrsnContractQueryModel model)
        {

            #region ref con1200
            //select PrsnBasic.*,HospBasic.HospID,HospBasic.HospSeqNo,HospBasic.HospName,HospBasic.HospStatus,GenPrsnType.PrsnTypeName,PrsnContract.CreateDate
            //from PrsnBasic
            //left join GenPrsnType on PrsnBasic.PrsnType = GenPrsnType.PrsnType
            //left join PrsnContract on PrsnBasic.PrsnID = PrsnContract.PrsnID
            //left join HospBasic on PrsnContract.HospID = HospBasic.HospID and HospBasic.HospSeqNo = PrsnContract.HospSeqNo
            //where(PrsnContract.PrsnStartDate is null or rtrim(PrsnContract.PrsnStartDate) = '')
            //order by PrsnBasic.PrsnID
            #endregion

            var prsnContract = context.PrsnContract
                                 .WhereWhen(!string.IsNullOrEmpty(model.PrsnId), x => x.PrsnId == model.PrsnId)
                                 .WhereWhen(!string.IsNullOrEmpty(model.SmkcontractType), x => x.SmkcontractType == model.SmkcontractType)
                                 .WhereWhen(!string.IsNullOrEmpty(model.PrsnStartDate), x => x.PrsnStartDate == model.PrsnStartDate.WestToTwDate())
                                 .WhereWhen(!string.IsNullOrEmpty(model.StartCreateDate),
                                            x => string.Compare(x.CreateDate, model.StartCreateDate.TwDateToDateTime()) >= 0)
                                 .WhereWhen(!string.IsNullOrEmpty(model.EndCreateDate),
                                            x => string.Compare(x.CreateDate, model.EndCreateDate.TwDateToDateTime()) <= 0)
                                 .WhereWhen(!string.IsNullOrEmpty(model.LicenceType),
                                            x => context.PrsnLicence.Any(q => q.PrsnId == x.PrsnId && q.LicenceType == model.LicenceType))
                                 .WhereWhen(model.IsApproval, x => x.PrsnStartDate == null || x.PrsnStartDate.Trim() == "")
                                 .WhereWhen(!string.IsNullOrEmpty(model.HospId), x => x.HospId == model.HospId)
                                 .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo);
            var prsnbasic = context.PrsnBasic
                                  .WhereWhen(!string.IsNullOrEmpty(model.PrsnName), x => x.PrsnName == model.PrsnName)
                                  .WhereWhen(!string.IsNullOrEmpty(model.PrsnType), x => x.PrsnType == model.PrsnType);

            var data = from contract in prsnContract
                       join basic in prsnbasic on contract.PrsnId equals basic.PrsnId
                       join prsnTyp in context.GenPrsnType on basic.PrsnType equals prsnTyp.PrsnType into gPrsnTyp
                       from prsnTypResult in gPrsnTyp.DefaultIfEmpty()
                       join gsmk in context.GenSmkcontract on contract.SmkcontractType equals gsmk.SmkcontractType into gsmkDetails
                       from m in gsmkDetails.DefaultIfEmpty()
                       join hospbasic in context.HospBasic on new { contract.HospId, contract.HospSeqNo } equals new { hospbasic.HospId, hospbasic.HospSeqNo }
                       select new PrsnContractListViewModel()
                       {
                           Id = contract.Id,
                           HospId = hospbasic.HospId,
                           HospSeqNo = hospbasic.HospSeqNo,
                           HospName = hospbasic.HospName,
                           HospStatus = hospbasic.HospStatus.ParseNoError<HospStatus>(),
                           SmkcontractType = m != null ? m.SmkcontractType : string.Empty,
                           SmkcontractTypeNam = m != null ? m.SmkcontractName : string.Empty,
                           PrsnId = contract.PrsnId,
                           PrsnName = basic.PrsnName,
                           PrsnTypeNam = prsnTypResult != null ? prsnTypResult.PrsnTypeName : string.Empty,
                           PrsnStartDate = contract.PrsnStartDate,
                           PrsnEndDate = contract.PrsnEndDate
                       };
            return await QueryPaging(model.get(), data);
        }
        /// <summary>
        /// 取得待核准的合約清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<PrsnContractListViewModel>>> GetHospPrsnContracts(PrsnContractQueryModel model)
        {
            var prsnContract = context.PrsnContract
                                 .Where(x => x.HospId == model.HospId && x.HospSeqNo == model.HospSeqNo);

            var data = from contract in prsnContract
                       join basic in context.PrsnBasic on contract.PrsnId equals basic.PrsnId
                       join prsnTyp in context.GenPrsnType on basic.PrsnType equals prsnTyp.PrsnType into gPrsnTyp
                       from prsnTypResult in gPrsnTyp.DefaultIfEmpty()
                       join gsmk in context.GenSmkcontract on contract.SmkcontractType equals gsmk.SmkcontractType into gsmkDetails
                       from m in gsmkDetails.DefaultIfEmpty()
                       join endReason in context.GenPrsnEndReason on contract.EndReasonNo equals endReason.EndReasonNo into gendReason
                       from endReasonResult in gendReason.DefaultIfEmpty()
                       join hospbasic in context.HospBasic on new { contract.HospId, contract.HospSeqNo } equals new { hospbasic.HospId, hospbasic.HospSeqNo }
                       select new PrsnContractListViewModel()
                       {
                           Id = contract.Id,
                           HospId = hospbasic.HospId,
                           HospSeqNo = hospbasic.HospSeqNo,
                           HospName = hospbasic.HospName,
                           HospStatus = hospbasic.HospStatus.ParseNoError<HospStatus>(),
                           SmkcontractType = m != null ? m.SmkcontractType : string.Empty,
                           SmkcontractTypeNam = m != null ? m.SmkcontractName : string.Empty,
                           PrsnId = contract.PrsnId,
                           PrsnName = basic.PrsnName,
                           PrsnTypeNam = prsnTypResult != null ? prsnTypResult.PrsnTypeName : string.Empty,
                           PrsnStartDate = contract.PrsnStartDate,
                           PrsnEndDate = contract.PrsnEndDate,
                           EndReasonNo = endReasonResult != null ? endReasonResult.EndReasonName : string.Empty
                       };
            return await QueryPaging(model.get(), data);
        }
        public async Task<LogicRtnModel<bool>> JudgeContracts(List<PrsnContractViewModel> contracts, string prsnStartDate)
        {
            LogicRtnModel<bool> logicRtnModel = new LogicRtnModel<bool>()
            {
                IsSuccess = true
            };
            using (var txn = context.GetTransactionScope())
            {
                try
                {
                    foreach (var contract in contracts)
                    {
                        var rtnModel = await FindOne<PrsnContract>(
                            (context) => context.Where(m => m.Id == contract.Id)
                        );
                        PrsnContract _contract;
                        if (!rtnModel.IsSuccess)
                        {
                            throw new Exception($"查無合約資料{contract.PrsnId}");
                        }
                        _contract = rtnModel.Data;
                        _contract.PrsnStartDate = prsnStartDate.TwDateToDateTime();
                        _contract.ModifyDate = DateTime.Now.ToDate();
                        context.PrsnContract.Update(_contract);
                    }

                    var result = await context.SaveChangesWithAuditAsync(identity.Account, "更新");
                    logicRtnModel.Msg = "審核成功";
                    await txn.CommitAsync();
                    return logicRtnModel;
                }
                catch (Exception err)
                {
                    await txn.RollbackAsync();
                    return new LogicRtnModel<bool>(MsgType.SaveFail, err.Message)
                    {
                        StackTrace = err.StackTrace
                    };
                }
            }
        }

        public async Task<List<PrsnContractViewModel>> GetPrsnContractAPI(string PrsnId)
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
            return await list.ToListAsync();
        }

        public async Task<IEnumerable<PrsnContract>> QueryPrsnContracts(string hospId, string hospSeqNo)
        {
            var rtnModel = await Query(c =>
                c.PrsnContract.Where(e => e.HospId == hospId &&
                                          e.HospSeqNo == hospSeqNo &&
                                          (e.PrsnEndDate == null || e.PrsnEndDate == "")));
            return rtnModel.IsSuccess ? rtnModel.Data : null;
        }

        public async Task<LogicRtnModel<PrsnContractQueryModel>> UploadApplication(IFormFile formFile)
        {
            LogicRtnModel<PrsnContractQueryModel> logicRtnModel = new LogicRtnModel<PrsnContractQueryModel>();
            string success_Msg = string.Empty;
            int all_Count = 0;
            int Update_Count = 0;
            int Next_Count = 0;
            #region 【三】新增戒菸服務人員
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                ExcelReader excelReader = new ExcelReader();
                List<List<string>> content = excelReader.ReadAsLIstOfList(ms, "【三】新增戒菸服務人員");
                all_Count = 0;
                Update_Count = 0;
                Next_Count = 0;
                bool foundSecond = false;
                using (var txn = context.GetTransactionScope())
                {
                    foreach (List<string> row in content)
                    {
                        try
                        {
                            if (content.First() == row)
                            {
                                logicRtnModel = CheckExcelSheet(row, 0);
                                if (logicRtnModel.IsSuccess)
                                {
                                    foundSecond = true;
                                    continue;
                                }
                                else
                                {
                                    return logicRtnModel;
                                }
                            }
                            if (foundSecond)
                            {
                                logicRtnModel = CheckExcelHeader_New(row);
                                if (logicRtnModel.IsSuccess)
                                {
                                    foundSecond = false;
                                    continue;
                                }
                                else
                                {
                                    return logicRtnModel;
                                }
                            }
                            if (string.IsNullOrEmpty(row[2]))
                            {
                                Next_Count++;
                                continue;
                            }
                            string rowdata = string.Empty;
                            ApplyPrsnNew obj = new ApplyPrsnNew();
                            for (int i = 0; i < 24; i++)
                            {
                                logicRtnModel = FillObjByIndex_New(obj, i, row, content.IndexOf(row), "【三】新增戒菸服務人員");
                                if (!logicRtnModel.IsSuccess)
                                {
                                    return logicRtnModel;
                                }
                            }
                            obj.UpdatedBy = identity.Account;
                            if (!context.ApplyPrsnNew.Any(x => x.Serial_Number == obj.Serial_Number))
                            {
                                context.ApplyPrsnNew.Add(obj);
                            }
                            else
                            {
                                context.ApplyPrsnNew.Update(obj);
                                Update_Count++;
                            }
                            all_Count++;
                        }
                        catch (Exception e)
                        {
                            logicRtnModel.IsSuccess = false;
                            logicRtnModel.Data = new PrsnContractQueryModel()
                            {
                                err = new List<string>()
                            };
                            logicRtnModel.ErrMsg = e.InnerException.Message;
                            logicRtnModel.Data.err.Add(e.InnerException.Message);
                            return logicRtnModel;
                        }
                    }
                    try
                    {
                        DataInsertLog dataInsertLog = new Services.DataInsertLogService().dataInsertLog($"新增戒菸服務人員 - {formFile.FileName}", all_Count);
                        await context.DataInsertLog.AddRangeAsync(dataInsertLog);
                        await context.SaveChangesAsync();
                        await txn.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        await txn.RollbackAsync();
                        logicRtnModel.IsSuccess = false;
                        logicRtnModel.ErrMsg = e.InnerException.Message;
                        logicRtnModel.Data.err.Add(e.InnerException.Message);
                        return logicRtnModel;
                    }
                }
            }
            logicRtnModel.Msg += $"【三】新增戒菸服務人員，需執行 {all_Count + Next_Count} 筆資料。(寫入 {all_Count - Update_Count} 筆，更新 {Update_Count} 筆，跳過 {Next_Count} 筆)";
            success_Msg = logicRtnModel.Msg;

            #endregion

            #region 【五】解除戒菸服務人員
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                ExcelReader excelReader = new ExcelReader();
                List<List<string>> content = excelReader.ReadAsLIstOfList(ms, "【五】解除戒菸服務人員");
                all_Count = 0;
                Update_Count = 0;
                Next_Count = 0;
                bool foundSecond = false;
                using (var txn = context.GetTransactionScope())
                {
                    foreach (List<string> row in content)
                    {
                        try
                        {
                            if (content.First() == row)
                            {
                                logicRtnModel = CheckExcelSheet(row, 1);
                                if (logicRtnModel.IsSuccess)
                                {
                                    foundSecond = true;
                                    continue;
                                }
                                else
                                {
                                    return logicRtnModel;
                                }
                            }
                            if (foundSecond)
                            {
                                logicRtnModel = CheckExcelHeader_End(row);
                                if (logicRtnModel.IsSuccess)
                                {
                                    foundSecond = false;
                                    continue;
                                }
                                else
                                {
                                    return logicRtnModel;
                                }
                            }
                            if (string.IsNullOrEmpty(row[2]))
                            {
                                Next_Count++;
                                continue;
                            }
                            string rowdata = string.Empty;
                            ApplyPrsnEnd ApplyPrsnEnd_obj = new ApplyPrsnEnd();
                            for (int i = 0; i < 12; i++)
                            {
                                logicRtnModel = FillObjByIndex_End(ApplyPrsnEnd_obj, i, row, content.IndexOf(row), "【五】解除戒菸服務人員");
                                if (!logicRtnModel.IsSuccess)
                                {
                                    return logicRtnModel;
                                }
                            }
                            ApplyPrsnEnd_obj.UpdatedBy = identity.Account;
                            ApplyPrsnEnd_obj.Application_Type = ApplicationType.End_order.GetEnumDescription();
                            if (!context.ApplyPrsnEnd.Any(x => x.Serial_Number == ApplyPrsnEnd_obj.Serial_Number))
                            {
                                context.ApplyPrsnEnd.Add(ApplyPrsnEnd_obj);
                            }
                            else
                            {
                                context.ApplyPrsnEnd.Update(ApplyPrsnEnd_obj);
                                Update_Count++;
                            }
                            all_Count++;
                        }
                        catch (Exception e)
                        {
                            logicRtnModel.IsSuccess = false;
                            logicRtnModel.Data = new PrsnContractQueryModel()
                            {
                                err = new List<string>()
                            };
                            logicRtnModel.ErrMsg = e.InnerException.Message;
                            logicRtnModel.Data.err.Add(e.InnerException.Message);
                            return logicRtnModel;
                        }
                    }
                    try
                    {
                        DataInsertLog dataInsertLog = new Services.DataInsertLogService().dataInsertLog($"【五】解除戒菸服務醫事人員 - {formFile.FileName}", all_Count);
                        await context.DataInsertLog.AddRangeAsync(dataInsertLog);
                        await context.SaveChangesAsync();
                        await txn.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        await txn.RollbackAsync();
                        logicRtnModel.IsSuccess = false;
                        logicRtnModel.ErrMsg = e.InnerException.Message;
                        logicRtnModel.Data.err.Add(e.Message);
                        return logicRtnModel;
                    }
                }
            }
            logicRtnModel.Msg = success_Msg + $";【五】解除戒菸服務醫事人員，需執行 {all_Count + Next_Count} 筆資料。(寫入 {all_Count - Update_Count} 筆，更新 {Update_Count} 筆，跳過 {Next_Count} 筆)";
            success_Msg = logicRtnModel.Msg;
            #endregion

            #region 【變更】醫事機構(代碼、名稱、負責人、地址)
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                ExcelReader excelReader = new ExcelReader();
                List<List<string>> content = excelReader.ReadAsLIstOfList(ms, "【變更】人員(ID、姓名)");
                all_Count = 0;
                Update_Count = 0;
                Next_Count = 0;
                bool foundSecond = false;
                bool foundThird = false;
                using (var txn = context.GetTransactionScope())
                {
                    foreach (List<string> row in content)
                    {
                        try
                        {
                            if (content.First() == row)
                            {
                                logicRtnModel = CheckExcelSheet(row, 2);
                                if (logicRtnModel.IsSuccess)
                                {
                                    foundSecond = true;
                                    continue;
                                }
                                else
                                {
                                    return logicRtnModel;
                                }
                            }
                            if (foundSecond)
                            {
                                foundSecond = false;
                                foundThird = true;
                                continue;
                            }
                            if (foundThird)
                            {
                                logicRtnModel = CheckExcelHeader_Change(row);
                                if (logicRtnModel.IsSuccess)
                                {
                                    foundThird = false;
                                    continue;
                                }
                                else
                                {
                                    return logicRtnModel;
                                }
                            }
                            if (string.IsNullOrEmpty(row[2]))
                            {
                                Next_Count++;
                                continue;
                            }
                            string rowdata = string.Empty;
                            ApplyPrsnChange ApplyPrsnChange_obj = new ApplyPrsnChange();
                            for (int i = 0; i < 15; i++)
                            {
                                logicRtnModel = FillObjByIndex_Change(ApplyPrsnChange_obj, i, row, content.IndexOf(row), "【變更】人員(ID、姓名)");
                                if (!logicRtnModel.IsSuccess)
                                {
                                    return logicRtnModel;
                                }
                            }
                            ApplyPrsnChange_obj.UpdatedBy = identity.Account;
                            ApplyPrsnChange_obj.Application_Type = ApplicationType.change.GetEnumDescription();
                            if (!context.ApplyPrsnChange.Any(x => x.Serial_Number == ApplyPrsnChange_obj.Serial_Number))
                            {
                                context.ApplyPrsnChange.Add(ApplyPrsnChange_obj);
                            }
                            else
                            {
                                context.ApplyPrsnChange.Update(ApplyPrsnChange_obj);
                                Update_Count++;
                            }
                            all_Count++;
                        }
                        catch (Exception e)
                        {
                            logicRtnModel.IsSuccess = false;
                            logicRtnModel.Data = new PrsnContractQueryModel()
                            {
                                err = new List<string>()
                            };
                            logicRtnModel.ErrMsg = e.InnerException.Message;
                            logicRtnModel.Data.err.Add(e.Message);
                            return logicRtnModel;
                        }
                    }
                    try
                    {
                        DataInsertLog dataInsertLog = new Services.DataInsertLogService().dataInsertLog($"【變更】人員(ID、姓名) - {formFile.FileName}", all_Count);
                        await context.DataInsertLog.AddRangeAsync(dataInsertLog);
                        await context.SaveChangesAsync();
                        await txn.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        await txn.RollbackAsync();
                        logicRtnModel.IsSuccess = false;
                        logicRtnModel.ErrMsg = e.InnerException.Message;
                        logicRtnModel.Data.err.Add(e.InnerException.Message);
                        return logicRtnModel;
                    }
                }
            }
            logicRtnModel.Msg = success_Msg + $"; 【變更】醫事機構(代碼、名稱、負責人、地址)，需執行 {all_Count + Next_Count} 筆資料。(寫入 {all_Count - Update_Count} 筆，更新 {Update_Count} 筆，跳過 {Next_Count} 筆)";
            #endregion


            logicRtnModel.IsSuccess = true;
            return logicRtnModel;
        }

        private LogicRtnModel<PrsnContractQueryModel> CheckExcelHeader_New(List<string> row)
        {
            PrsnContractQueryModel prsnContractQueryModel = new PrsnContractQueryModel()
            {
                err = new List<string>()
            };
            string[] header = { "序次", "收件年月", "編號", "收件日\n(格式：YYYMMDD)", "合約生效日期\n(格式：YYY/MM/DD)", "申請類型", "院所代碼", "院區別", "院所名稱", "層級", "業務組別", "申請醫事人員姓名", "申請醫事人員ID", "出生日期\n(格式：YYY/MM/DD)", "戒菸證書有效日期\n(格式：YYY/MM/DD)", "職稱", "負責人\n姓名", "負責人\nID", "EMAIL", "醫事人員證書", "戒菸證書字號", "治療+衛教", "SMK\n登錄日期", "SMK登錄註記" };
            if (row.Count != header.Length)
            {
                prsnContractQueryModel.err.Add("新增合約，(欄位數量錯誤)標題列應該有" + header.Length + "欄:" + string.Join(",", header));
            }
            else
            {
                for (int i = 0; i < row.Count; i++)
                {
                    if (row[i].Trim() != header[i])
                    {
                        prsnContractQueryModel.err.Add($"新開辦，標題列第{i + 1}({row[i].Trim()})欄名稱應該為{header[i]}");
                    }
                }
            }

            var rtnModel = new LogicRtnModel<PrsnContractQueryModel>();
            rtnModel.Data = prsnContractQueryModel;
            if (rtnModel.Data.err.Count > 0)
            {
                rtnModel.IsSuccess = false;
                rtnModel.ErrMsg = string.Join("\r\n", rtnModel.Data.err.ToArray());
            }
            else
            {
                rtnModel.IsSuccess = true;
            }
            return rtnModel;
        }
        private LogicRtnModel<PrsnContractQueryModel> CheckExcelHeader_End(List<string> row)
        {
            PrsnContractQueryModel prsnContractQueryModel = new PrsnContractQueryModel()
            {
                err = new List<string>()
            };
            string[] header = { "序次", "收件年月", "編號", "收件日\n(格式：YYYMMDD)", "院所代碼", "院區別", "院所名稱", "醫事人員ID", "解除醫事人員姓名", "職稱", "SMK單機版\n登錄日期", "SMK登錄註記" };
            if (row.Count != header.Length)
            {
                prsnContractQueryModel.err.Add("解除戒菸服務人員，(欄位數量錯誤)標題列應該有" + header.Length + "欄:" + string.Join(",", header));
            }
            else
            {
                for (int i = 0; i < row.Count; i++)
                {
                    if (row[i].Trim() != header[i])
                    {
                        prsnContractQueryModel.err.Add($"解除戒菸服務人員，標題列第{i + 1}({row[i].Trim()})欄名稱應該為{header[i]}");
                    }
                }
            }

            var rtnModel = new LogicRtnModel<PrsnContractQueryModel>();
            rtnModel.Data = prsnContractQueryModel;
            if (rtnModel.Data.err.Count > 0)
            {
                rtnModel.IsSuccess = false;
                rtnModel.ErrMsg = string.Join("\r\n", rtnModel.Data.err.ToArray());
            }
            else
            {
                rtnModel.IsSuccess = true;
            }
            return rtnModel;
        }
        private LogicRtnModel<PrsnContractQueryModel> CheckExcelHeader_Change(List<string> row)
        {
            PrsnContractQueryModel prsnContractQueryModel = new PrsnContractQueryModel()
            {
                err = new List<string>()
            };
            string[] header = { "", "", "", "ID", "姓名", "", "ID", "姓名", "ID", "姓名", "ID", "姓名", "更新日期", "修正備註" };
            if (row.Count != header.Length)
            {
                prsnContractQueryModel.err.Add("【變更】人員，(欄位數量錯誤)標題列應該有" + header.Length + "欄:" + string.Join(",", header));
            }
            else
            {
                for (int i = 0; i < row.Count; i++)
                {
                    if (row[i].Trim() != header[i])
                    {
                        prsnContractQueryModel.err.Add($"【變更】人員，標題列第{i + 1}({row[i].Trim()})欄名稱應該為{header[i]}");
                    }
                }
            }

            var rtnModel = new LogicRtnModel<PrsnContractQueryModel>();
            rtnModel.Data = prsnContractQueryModel;
            if (rtnModel.Data.err.Count > 0)
            {
                rtnModel.IsSuccess = false;
                rtnModel.ErrMsg = string.Join("\r\n", rtnModel.Data.err.ToArray());
            }
            else
            {
                rtnModel.IsSuccess = true;
            }
            return rtnModel;
        }
        private LogicRtnModel<PrsnContractQueryModel> CheckExcelSheet(List<string> row, int array_Num)
        {
            PrsnContractQueryModel prsnContractQueryModel = new PrsnContractQueryModel()
            {
                err = new List<string>()
            };
            string[] CheckSheetName = { "三、 新增戒菸服務人員", "五、 解除戒菸服務醫事人員", "【變更】人員(ID、姓名)" };
            if (CheckSheetName[array_Num] != row[0])
            {
                prsnContractQueryModel.err.Add($"標題列A1為{row[0]}，名稱應該為{CheckSheetName[array_Num]}");
            }


            var rtnModel = new LogicRtnModel<PrsnContractQueryModel>();
            rtnModel.Data = prsnContractQueryModel;
            if (rtnModel.Data.err.Count > 0)
            {
                rtnModel.IsSuccess = false;
                rtnModel.ErrMsg = string.Join("\r\n", rtnModel.Data.err.ToArray());
            }
            else
            {
                rtnModel.IsSuccess = true;
            }
            return rtnModel;
        }
        private LogicRtnModel<PrsnContractQueryModel> FillObjByIndex_New(ApplyPrsnNew obj, int i, List<string> row, int rownumber, string sheetName)
        {
            PrsnContractQueryModel prsnContractQueryModel = new PrsnContractQueryModel()
            {
                err = new List<string>()
            };
            string Total_number = row[0] == "" ? "此筆沒有序號" : row[0];
            switch (i)
            {
                case 0:

                    break;
                case 1:

                    if (row[i].Trim().Length != 5)
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMM，或月分異常");
                    }
                    else
                    {
                        obj.FeeYM = row[i];
                    }

                    break;
                case 2:
                    if (row[i].Trim().Length == 10 || row[i].Trim().Length == 11)
                    {
                        obj.Serial_Number = row[i];
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為(yyy-MM-0000或yyy-MM-000)，或長度異常");
                    }
                    break;
                case 3:
                    if (row[i].Trim().Length == 7 || string.IsNullOrEmpty(row[i]))
                    {
                        obj.FeeYMD = row[i].ToYYYYMMDDFromTaiwan();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMMdd，或月分、日期異常");
                    }
                    break;
                case 4:
                    if (row[i].Trim().Length == 9 || string.IsNullOrEmpty(row[i]))
                    {
                        obj.Contract_Effective_Date = row[i].ToYYYYMMDDFromTaiwan();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyy/MM/dd，或月分、日期異常");
                    }
                    break;
                case 5:
                    if (string.IsNullOrEmpty(row[i]))
                    {
                        obj.Application_Type = " ";
                    }
                    else
                    {
                        ApplicationType[] applicationTypes = new ApplicationType[]
                        {
                            ApplicationType.new_order,
                            ApplicationType.End_order,
                            ApplicationType.change,
                            ApplicationType.Add,
                            ApplicationType.Agree_In_Advance,
                            ApplicationType.Change_Code
                        };
                        bool hasNewOrder = applicationTypes.Any(type => type.GetEnumDescription() == row[i]);
                        var data = applicationTypes.Where(type => type.GetEnumDescription() == row[i]).FirstOrDefault();
                        if (hasNewOrder)
                        {
                            obj.Application_Type = data.GetEnumDescription();
                        }
                        else
                        {
                            obj.Application_Type = ApplicationType.new_order.GetEnumDescription();
                        }
                    }
                    break;
                case 6:
                    if (row[i].Trim().Length == 10 || row[i].Trim().Length == 9)
                    {
                        if (row[i].Trim().Length == 9)
                        {
                            row[i] = "0" + row[i];
                        }
                        obj.HospID = row[i].Trim();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為10");
                    }
                    break;
                case 7:
                    if (row[i].Length == 2 || row[i].Length == 1)
                    {
                        obj.HospSeqNo = row[i].Length == 1 ? "0" + row[i] : row[i];
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為2");
                    }

                    break;
                case 8:
                    if (row[i].Trim().Length < 50)
                    {
                        obj.HospName = row[i].Trim();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過50");
                    }

                    break;
                case 9:
                    if (row[i].Trim().Length < 10)
                    {
                        obj.LastContType = row[i].Trim();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過10");
                    }
                    break;
                case 10:
                    if (row[i].Trim().Length < 10)
                    {
                        obj.BranchName = row[i].Trim();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過10");
                    }
                    break;
                case 11:
                    if (row[i].Trim().Length < 80)
                    {
                        obj.MedicalName = row[i];
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度大於80");
                    }
                    break;
                case 12:
                    if (row[i].Trim().Length == 10 || string.IsNullOrEmpty(row[i]))
                    {
                        obj.MedicalID = row[i].Trim();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度不是10碼");
                    }
                    break;
                case 13:
                    if (row[i].Trim().Length == 9 || string.IsNullOrEmpty(row[i]))
                    {
                        obj.Birthday = row[i].ToYYYYMMDDFromTaiwan();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMMdd，或月分、日期異常");
                    }
                    break;
                case 14:
                    if (row[i].Trim().Length == 9 || string.IsNullOrEmpty(row[i]))
                    {
                        obj.Smoking_Expiration_Date = row[i].ToYYYYMMDDFromTaiwan();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMMdd，或月分、日期異常");
                    }
                    break;
                case 15:
                    if (row[i].Trim().Length < 10)
                    {
                        obj.Professional_Title = row[i].Trim();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過10");
                    }
                    break;
                case 16:
                    if (row[i].Trim().Length < 50)
                    {
                        obj.HospUserName = row[i];
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度大於50");
                    }
                    break;
                case 17:
                    if (row[i].Trim().Length == 10 || row[i] == "" || row[i] == null)
                    {
                        obj.HospUserID = row[i];
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度不是10碼");
                    }
                    break;
                case 18:
                    if (row[i].Trim().Length < 80 || row[i] == "" || row[i] == null)
                    {
                        obj.HospUserMail = row[i];
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度大於80");
                    }
                    break;
                case 19:
                    if (row[i].Trim().Length < 80 || row[i] == "" || row[i] == null)
                    {
                        obj.Medical_Certificate = row[i];
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度大於80");
                    }
                    break;
                case 20:
                    if (row[i].Trim().Length < 80 || row[i] == "" || row[i] == null)
                    {
                        obj.Smoking_Cessation_Certificate = row[i];
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度大於80");
                    }
                    break;
                case 21:
                    if (row[i].Trim().Length < 20)
                    {
                        obj.Treatment = row[i];
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度大於20");
                    }
                    break;
                case 22:
                    if (DateTime.TryParse(row[i], out DateTime date) || row[i] == "" || row[i] == null || row[i] == "-")
                    {
                        if (row[i] == "" || row[i] == "-")
                        {
                            obj.SMKLogDate = null;
                        }
                        else
                        {
                            obj.SMKLogDate = date;
                        }
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyy/MM/dd，或月分、日期異常");
                    }
                    break;
                case 23:
                    if (row[i].Trim().Length < 80)
                    {
                        obj.SMKLogNote = row[i];
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度大於80");
                    }
                    break;
                default:
                    break;
            }
            var rtnModel = new LogicRtnModel<PrsnContractQueryModel>();
            rtnModel.Data = prsnContractQueryModel;
            if (rtnModel.Data.err.Count > 0)
            {
                rtnModel.IsSuccess = false;
                rtnModel.ErrMsg = string.Join("\r\n", rtnModel.Data.err.ToArray());
            }
            else
            {
                rtnModel.IsSuccess = true;
            }
            return rtnModel;
        }
        private LogicRtnModel<PrsnContractQueryModel> FillObjByIndex_End(ApplyPrsnEnd ApplyPrsnEnd_obj, int i, List<string> row, int rownumber, string sheetName)
        {
            PrsnContractQueryModel prsnContractQueryModel = new PrsnContractQueryModel()
            {
                err = new List<string>()
            };
            string Total_number = row[0] == "" ? "此筆沒有序號" : row[0];
            switch (i)
            {
                case 0:

                    break;
                case 1:

                    if (row[i].Trim().Length != 5)
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMM，或月分異常");
                    }
                    else
                    {
                        ApplyPrsnEnd_obj.FeeYM = row[i];
                    }

                    break;
                case 2:
                    if (row[i].Trim().Length == 10 || row[i].Trim().Length == 11)
                    {
                        ApplyPrsnEnd_obj.Serial_Number = row[i];
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為(yyy-MM-000或yyy-MM-0000)，或長度異常");
                    }
                    break;
                case 3:
                    if (row[i].Trim().Length == 7)
                    {
                        ApplyPrsnEnd_obj.FeeYMD = row[i].ToYYYYMMDDFromTaiwan();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMMdd，或月分、日期異常");
                    }
                    break;
                case 4:
                    if (row[i].Trim().Length == 10 || row[i].Trim().Length == 9)
                    {
                        if (row[i].Trim().Length == 9)
                        {
                            row[i] = "0" + row[i];
                        }
                        ApplyPrsnEnd_obj.HospID = row[i].Trim();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為10");
                    }
                    break;
                case 5:
                    if (row[i].Length == 2 || row[i].Length == 1)
                    {
                        ApplyPrsnEnd_obj.HospSeqNo = row[i].Length == 1 ? "0" + row[i] : row[i];
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為2");
                    }

                    break;
                case 6:
                    if (row[i].Trim().Length < 50)
                    {
                        ApplyPrsnEnd_obj.HospName = row[i].Trim();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過50");
                    }
                    break;
                case 7:
                    if (row[i].Trim().Length == 10)
                    {
                        ApplyPrsnEnd_obj.MedicalID = row[i].Trim();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度不是10");
                    }
                    break;
                case 8:
                    if (row[i].Trim().Length < 80)
                    {
                        ApplyPrsnEnd_obj.MedicalName = row[i].Trim();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過80");
                    }
                    break;
                case 9:
                    if (row[i].Trim().Length < 10)
                    {
                        ApplyPrsnEnd_obj.Professional_Title = row[i].Trim();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過10");
                    }
                    break;
                case 10:
                    if (DateTime.TryParse(row[i], out DateTime date) || string.IsNullOrEmpty(row[i]) || row[i] == "-")
                    {
                        if (string.IsNullOrEmpty(row[i]) || row[i] == "-")
                        {
                            ApplyPrsnEnd_obj.SingleChangeDate = null;
                        }
                        else
                        {
                            ApplyPrsnEnd_obj.SingleChangeDate = date;
                        }
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，，格式應該為yyyy/MM/dd，或月分、日期異常");
                    }
                    break;
                case 11:
                    if (row[i].Trim().Length < 80)
                    {
                        ApplyPrsnEnd_obj.SingleNote = row[i].Trim();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過80");
                    }
                    break;
                default:
                    break;
            }
            var rtnModel = new LogicRtnModel<PrsnContractQueryModel>();
            rtnModel.Data = prsnContractQueryModel;
            if (rtnModel.Data.err.Count > 0)
            {
                rtnModel.IsSuccess = false;
                rtnModel.ErrMsg = string.Join("\r\n", rtnModel.Data.err.ToArray());
            }
            else
            {
                rtnModel.IsSuccess = true;
            }
            return rtnModel;
        }
        private LogicRtnModel<PrsnContractQueryModel> FillObjByIndex_Change(ApplyPrsnChange ApplyPrsnChange_obj, int i, List<string> row, int rownumber, string sheetName)
        {
            PrsnContractQueryModel prsnContractQueryModel = new PrsnContractQueryModel()
            {
                err = new List<string>()
            };
            string Total_number = row[0] == "" ? "此筆沒有序號" : row[0];
            switch (i)
            {
                case 0:

                    break;
                case 1:
                    if (row[i].Trim().Length != 5)
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMM，或月分異常");
                    }
                    else
                    {
                        ApplyPrsnChange_obj.FeeYM = row[i];
                    }
                    break;
                case 2:
                    if (row[i].Trim().Length == 10 || row[i].Trim().Length == 11)
                    {
                        ApplyPrsnChange_obj.Serial_Number = row[i];
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為(yyy-MM-000或yyy-MM-0000)，或長度異常");
                    }
                    break;
                case 3:
                    ApplyPrsnChange_obj.ChangeID = row[i] == "V" ? true : false;
                    break;
                case 4:
                    ApplyPrsnChange_obj.ChangeName = row[i] == "V" ? true : false;
                    break;
                case 5:
                    if (row[i].Trim().Length == 7)
                    {
                        ApplyPrsnChange_obj.StartYMD = row[i].ToYYYYMMDDFromTaiwan();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMMdd，或月分、日期異常");
                    }
                    break;
                case 6:
                    if (row[i].Length == 10)
                    {
                        ApplyPrsnChange_obj.ID = row[i];
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為10");
                    }
                    break;
                case 7:
                    if (row[i].Trim().Length < 80)
                    {
                        ApplyPrsnChange_obj.Name = row[i].Trim();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過80");
                    }
                    break;
                case 8:
                    if (row[i].Length == 10 || row[i] == "" || row[i] == null)
                    {
                        ApplyPrsnChange_obj.NewID = row[i];
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為10");
                    }
                    break;
                case 9:
                    if (row[i].Trim().Length < 80 || row[i] == "" || row[i] == null)
                    {
                        ApplyPrsnChange_obj.NewName = row[i].Trim();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過80");
                    }
                    break;
                case 10:
                case 11:
                    break;
                case 12:
                    if (row[i].Trim().Length == 7 || string.IsNullOrEmpty(row[i]) || row[i] == "-")
                    {
                        if (string.IsNullOrEmpty(row[i]) || row[i] == "-")
                            ApplyPrsnChange_obj.SingleChangeDate = null;
                        ApplyPrsnChange_obj.SingleChangeDate = row[i].ToYYYYMMDDFromTaiwan();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMMdd，或月分、日期異常");
                    }
                    break;
                case 13:
                    if (row[i].Trim().Length < 80)
                    {
                        ApplyPrsnChange_obj.SingleNote = row[i].Trim();
                    }
                    else
                    {
                        prsnContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過80");
                    }
                    break;
                default:
                    break;
            }
            var rtnModel = new LogicRtnModel<PrsnContractQueryModel>();
            rtnModel.Data = prsnContractQueryModel;
            if (rtnModel.Data.err.Count > 0)
            {
                rtnModel.IsSuccess = false;
                rtnModel.ErrMsg = string.Join("\r\n", rtnModel.Data.err.ToArray());
            }
            else
            {
                rtnModel.IsSuccess = true;
            }
            return rtnModel;
        }


        /// <summary>
        /// 取得excel上傳的申請概況資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<ApplicationPrsnContractViewModel>>> GetInsertPrsnContracts(PrsnContractQueryModel model)
        {
            if (model.ApplicationType != null)
            {
                ApplicationType changeValue = (ApplicationType)Enum.Parse(typeof(ApplicationType), model.ApplicationType);
                model.ApplicationType = model.ApplicationType != "" || model.ApplicationType != null ? changeValue.GetEnumDescription() : null;
            }

            var ApplyPrsnNew = context.ApplyPrsnNew
                               .WhereWhen(!string.IsNullOrEmpty(model.ApplicationType), x => x.Application_Type == model.ApplicationType)
                               .WhereWhen(!string.IsNullOrEmpty(model.Application_StartCreateDate), x => string.Compare(x.FeeYMD, model.Application_StartCreateDate.TwDateToDateTime()) >= 0)
                               .WhereWhen(!string.IsNullOrEmpty(model.Application_EndCreateDate), x => string.Compare(x.FeeYMD, model.Application_EndCreateDate.TwDateToDateTime()) <= 0)
                               .Select(x => new ApplyPrsnNewVM()
                               {
                                   FeeYM = x.FeeYM,
                                   FeeYMD = x.FeeYMD.ToSlashTaiwanDateFromYYYYMMDD(),
                                   Application_Type = x.Application_Type,
                                   Change_Type = "不適用",
                                   HospID = x.HospID,
                                   HospSeqNo = x.HospSeqNo,
                                   HospName = x.HospName,
                                   MedicalID = x.MedicalID,
                                   MedicalName = x.MedicalName,
                                   Professional_Title = x.Professional_Title,
                                   Treatment = x.Treatment,
                                   Smoking_Expiration_Date = x.Smoking_Expiration_Date,
                                   Contract_Effective_Date = x.Contract_Effective_Date,
                                   SMKLogNote = x.SMKLogNote
                               });

            var changeApplyPrsnNew = ApplyPrsnNew
                                        .Select(x => new ApplicationPrsnContractViewModel()
                                        {
                                            FeeYM = x.FeeYM,
                                            FeeYMD = x.FeeYMD,
                                            Application_Type = x.Application_Type,
                                            Change_Type = x.Change_Type,
                                            HospID = x.HospID,
                                            HospSeqNo = x.HospSeqNo,
                                            HospName = x.HospName,
                                            UserID = x.MedicalID,
                                            UserName = x.MedicalName,
                                            UserTitle = x.Professional_Title,
                                            UserServise = x.Treatment,
                                            Note = x.Note,
                                        });

            var ApplyPrsnEnd = context.ApplyPrsnEnd
                               .WhereWhen(!string.IsNullOrEmpty(model.ApplicationType), x => x.Application_Type == model.ApplicationType)
                               .WhereWhen(!string.IsNullOrEmpty(model.Application_StartCreateDate), x => string.Compare(x.FeeYMD, model.Application_StartCreateDate.TwDateToDateTime()) >= 0)
                               .WhereWhen(!string.IsNullOrEmpty(model.Application_EndCreateDate), x => string.Compare(x.FeeYMD, model.Application_EndCreateDate.TwDateToDateTime()) <= 0)
                               .Select(x => new ApplicationPrsnContractViewModel()
                               {
                                   FeeYM = x.FeeYM,
                                   FeeYMD = x.FeeYMD.ToSlashTaiwanDateFromYYYYMMDD(),
                                   Application_Type = x.Application_Type,
                                   Change_Type = "不適用",
                                   HospID = x.HospID,
                                   HospSeqNo = x.HospSeqNo,
                                   HospName = x.HospName,
                                   UserID = x.MedicalID,
                                   UserName = x.MedicalName,
                                   UserTitle = x.Professional_Title,
                                   UserServise = "不適用",
                                   Note = x.SingleNote,
                               });

            var ApplyHospChange = context.ApplyPrsnChange
                    .WhereWhen(!string.IsNullOrEmpty(model.ApplicationType), x => x.Application_Type == model.ApplicationType)
                    .WhereWhen(!string.IsNullOrEmpty(model.Application_StartCreateDate), x => string.Compare(x.StartYMD, model.Application_StartCreateDate.TwDateToDateTime()) >= 0)
                    .WhereWhen(!string.IsNullOrEmpty(model.Application_EndCreateDate), x => string.Compare(x.StartYMD, model.Application_EndCreateDate.TwDateToDateTime()) <= 0)
                    .Select(x => new ApplyPrsnChangeVM()
                    {
                        FeeYM = x.FeeYM,
                        StartYMD = x.StartYMD.ToSlashTaiwanDateFromYYYYMMDD(),
                        Application_Type = x.Application_Type,
                        ChangeID = x.ChangeID,
                        ChangeName = x.ChangeName,

                        ID = x.ID,
                        Name = x.Name,

                        NewID = x.NewID,
                        NewName = x.NewName,
                        Note = x.SingleNote
                    });

            var changeApplyHospChange = ApplyHospChange
                            .Select(x => new ApplicationPrsnContractViewModel()
                            {
                                FeeYM = x.FeeYM,
                                FeeYMD = x.StartYMD,
                                Application_Type = x.Application_Type,
                                Change_Type = x.ChangeType,
                                HospID = "不適用",
                                HospSeqNo = "不適用",
                                HospName = "不適用",
                                UserID = x.ID,
                                UserName = x.Name,
                                UserTitle = "不適用",
                                UserServise = "不適用",
                                Note = x.new_Note,
                            });


            List<ApplicationPrsnContractViewModel> data = new List<ApplicationPrsnContractViewModel>();

            data.AddRange(changeApplyPrsnNew.ToList());
            data.AddRange(ApplyPrsnEnd.ToList());
            data.AddRange(changeApplyHospChange.ToList());


            var pagedData = PagedModel<ApplicationPrsnContractViewModel>.Create(data.AsQueryable().OrderByDescending(x => x.FeeYMD), model.get());
            pagedData.RecordsTotal = data.Count();
            return new LogicRtnModel<PagedModel<ApplicationPrsnContractViewModel>>()
            {
                IsSuccess = true,
                Data = pagedData,
            };
        }

    }
}
