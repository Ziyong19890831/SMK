using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Data.Utility;
using SMK.Web.Helpers;
using SMK.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;
using Yozian.Extension;
using Dapper;
using System.Data.Common;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.IO;

namespace SMK.Web.Services.Foundation
{
    /// <summary>
    /// 機構合約
    /// </summary>
    [ScopedService]
    public class HospContractService : GenericService
    {
        private readonly PersistenceService persistenceService;
        private readonly DbConnection _conn;

        public HospContractService(SMKWEBContext context, SessionManager smgr,
            PersistenceService persistenceService)
           : base(context, smgr)
        {
            this.persistenceService = persistenceService;
            _conn = context.Database.GetDbConnection();
        }
        /// <summary>
        /// 取得待核准的合約清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<HospBasicContractViewModel>>> GetHospContracts(HospContractQueryModel model)
        {

            #region ref con1200
            //select HospBasic.*,GenSMKContract.SMKContractName, HospContract.CreateDate
            //from HospBasic
            //left join HospContract on HospBasic.HospID = HospContract.HospID and HospBasic.HospSeqNo = HospContract.HospSeqNo
            //left join GenSMKContract on HospContract.SMKContractType = GenSMKContract.SMKContractType
            //where(HospContract.HospStartDate is null or rtrim(HospContract.HospStartDate) = '')
            //order by HospBasic.HospID
            #endregion
            //一間機構會有很多段合約，要個別呈現誰是有效跟終止的合約
            _conn.Execute("exec sp_UpdateHospBasic");

            var contract = context.HospContract
                                 .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospId == model.HospID)
                                 .WhereWhen(!string.IsNullOrEmpty(model.HospStartDate), x => string.Compare(x.HospStartDate, model.HospStartDate.TwDateToDateTime()) >= 0)
                                 .WhereWhen(!string.IsNullOrEmpty(model.HospEndDate), x => string.Compare(x.HospEndDate, model.HospEndDate.TwDateToDateTime()) <= 0)
                                 .WhereWhen(!string.IsNullOrEmpty(model.SMKContractType), x => x.SmkcontractType == model.SMKContractType)
                                 .WhereWhen(!string.IsNullOrEmpty(model.ContractStatus) && model.ContractStatus == ContractStatus.MainContract.ToString("d"),
                                            x => x.SmkcontractType == "01")
                                 .WhereWhen(!string.IsNullOrEmpty(model.ContractStatus) && model.ContractStatus == ContractStatus.ImproveContractType.ToString("d"),
                                            x => x.SmkcontractType != "01")
                                 .WhereWhen(!string.IsNullOrEmpty(model.StartCreateDate),
                                            x => string.Compare(x.CreateDate, model.StartCreateDate.TwDateToDateTime()) >= 0)
                                 .WhereWhen(!string.IsNullOrEmpty(model.EndCreateDate),
                                            x => string.Compare(x.CreateDate, model.EndCreateDate.TwDateToDateTime()) <= 0)
                                 .WhereWhen(model.IsViolation,
                                            x => x.EndReasonNo == "22")
                                 .WhereWhen(model.IsApproval,
                                            x => x.HospStartDate == null || x.HospStartDate.Trim() == "");
            //.Where(x => string.IsNullOrEmpty(x.HospStartDate));

            var hospbasic = context.HospBasic
                                  .WhereWhen(!string.IsNullOrEmpty(model.HospID),
                                              x => x.HospId == model.HospID)
                                  .WhereWhen(!string.IsNullOrEmpty(model.HospName), x => x.HospName == model.HospName)
                                  .WhereWhen(!string.IsNullOrEmpty(model.HospStatus), x => x.HospStatus == model.HospStatus);

            var data = from inner in hospbasic
                       join outer in contract on new { inner.HospId, inner.HospSeqNo } equals new { outer.HospId, outer.HospSeqNo } into outerDetails
                       from c in outerDetails.DefaultIfEmpty()
                       join gsmk in context.GenSmkcontract on c.SmkcontractType equals gsmk.SmkcontractType into gsmkDetails
                       from m in gsmkDetails.DefaultIfEmpty()
                       where ((m.SmkcontractType == model.SMKContractType || string.IsNullOrEmpty(model.SMKContractType)))
                       orderby inner.HospId, inner.HospSeqNo
                       select new HospBasicContractViewModel()
                       {
                           Id = c != null ? c.Id : 0,
                           HospID = inner.HospId,
                           HospSeqNo = inner.HospSeqNo,
                           HospName = inner.HospName,
                           HospStartDate = c != null ? c.HospStartDate : string.Empty,
                           HospEndDate = c != null ? c.HospEndDate : string.Empty,
                           HospStatus = inner.HospStatus.ParseNoError<HospStatus>(),
                           //HospStatus = c != null && string.IsNullOrEmpty(c.EndReasonNo) ? inner.HospStatus.ParseNoError<HospStatus>() : HospStatus.TerminationHosp,
                           ContractStatus = c != null ? (ContractStatus?)(c.SmkcontractType.Equals("01") ? ContractStatus.MainContract : ContractStatus.ImproveContractType) : null,
                           SMKContractType = m != null ? m.SmkcontractType : string.Empty,
                           SMKContractTypeNam = m != null ? m.SmkcontractName : string.Empty,
                           CreateDate = c.CreateDate
                       };

            //合併完後，在做一次篩選，把Full Join的資料篩選掉
            data = data.WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospID == model.HospID)
                                 .WhereWhen(!string.IsNullOrEmpty(model.HospStartDate), x => string.Compare(x.HospStartDate, model.HospStartDate.TwDateToDateTime()) >= 0)
                                 .WhereWhen(!string.IsNullOrEmpty(model.HospEndDate), x => string.Compare(x.HospEndDate, model.HospEndDate.TwDateToDateTime()) <= 0)
                                 .WhereWhen(!string.IsNullOrEmpty(model.SMKContractType), x => x.SMKContractType == model.SMKContractType)
                                 .WhereWhen(!string.IsNullOrEmpty(model.ContractStatus) && model.ContractStatus == ContractStatus.MainContract.ToString("d"),
                                            x => x.SMKContractType == "01")
                                 .WhereWhen(!string.IsNullOrEmpty(model.ContractStatus) && model.ContractStatus == ContractStatus.ImproveContractType.ToString("d"),
                                            x => x.SMKContractType != "01")
                                 .WhereWhen(!string.IsNullOrEmpty(model.StartCreateDate),
                                            x => string.Compare(x.CreateDate, model.StartCreateDate.TwDateToDateTime()) >= 0)
                                 .WhereWhen(!string.IsNullOrEmpty(model.EndCreateDate),
                                            x => string.Compare(x.CreateDate, model.EndCreateDate.TwDateToDateTime()) <= 0)
                                 .WhereWhen(model.IsApproval,
                                            x => x.HospStartDate == null || x.HospStartDate.Trim() == "");


            return await QueryPaging(model.get(), data.WhereWhen(model.IsApproval, x => x.Id != 0));
        }

        /// <summary>
        /// 取得待核准的合約清單(新版)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<HospBasicContractViewModel>>> GetHospContractsNew(HospContractQueryModel model)
        {

            #region ref con1200
            //select HospBasic.*,GenSMKContract.SMKContractName, HospContract.CreateDate
            //from HospBasic
            //left join HospContract on HospBasic.HospID = HospContract.HospID and HospBasic.HospSeqNo = HospContract.HospSeqNo
            //left join GenSMKContract on HospContract.SMKContractType = GenSMKContract.SMKContractType
            //where(HospContract.HospStartDate is null or rtrim(HospContract.HospStartDate) = '')
            //order by HospBasic.HospID
            #endregion
            //一間機構會有很多段合約，要個別呈現誰是有效跟終止的合約
            _conn.Execute("exec sp_UpdateHospBasic");

            var data = context.HospBasic
                        //.Where(a => /* 你的條件 */)  // 這裡添加 HospBasic 表的篩選條件
                        .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospId == model.HospID)
                        //.WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo)
                        .WhereWhen(!string.IsNullOrEmpty(model.HospName), x => x.HospName == model.HospName)
                        .WhereWhen(!string.IsNullOrEmpty(model.HospStatus), x => x.HospStatus == model.HospStatus)
                        .GroupJoin(
                            context.HospContract,
                            HospBasic => new { HospBasic.HospId, HospBasic.HospSeqNo },
                            HospContract => new { HospContract.HospId, HospContract.HospSeqNo },
                            (HospBasic, HospContractGroup) => new { HospBasic, HospContractGroup }
                        )
                        .SelectMany(
                            x => x.HospContractGroup.DefaultIfEmpty(),
                            (x, HospContract) => new { x.HospBasic, HospContract }
                        )
                        //.Where(x => /* 你的條件 */)  // 這裡可以添加涉及 HospBasic 和 HospContract 的條件
                        .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospContract.HospId == model.HospID)
                        .WhereWhen(!string.IsNullOrEmpty(model.HospStartDate), x => string.Compare(x.HospContract.HospStartDate, model.HospStartDate.TwDateToDateTime()) >= 0)
                        .WhereWhen(!string.IsNullOrEmpty(model.HospEndDate), x => (string.Compare(x.HospContract.HospEndDate, model.HospEndDate.TwDateToDateTime()) <= 0) && !string.IsNullOrWhiteSpace(x.HospContract.HospEndDate))
                        .WhereWhen(!string.IsNullOrEmpty(model.ContractStatus) && model.ContractStatus == ContractStatus.MainContract.ToString("d"),
                                x => x.HospContract.SmkcontractType == "01")
                        .WhereWhen(!string.IsNullOrEmpty(model.ContractStatus) && model.ContractStatus == ContractStatus.ImproveContractType.ToString("d"),
                                x => x.HospContract.SmkcontractType != "01")
                        .WhereWhen(!string.IsNullOrEmpty(model.StartCreateDate),
                                x => string.Compare(x.HospContract.CreateDate, model.StartCreateDate.TwDateToDateTime()) >= 0)
                        .WhereWhen(!string.IsNullOrEmpty(model.EndCreateDate),
                                x => string.Compare(x.HospContract.CreateDate, model.EndCreateDate.TwDateToDateTime()) <= 0)
                        .WhereWhen(model.IsViolation,
                                x => x.HospContract.EndReasonNo == "22")
                        .WhereWhen(model.IsApproval,
                                x => x.HospContract.HospStartDate == null || x.HospContract.HospStartDate.Trim() == "")
                        .GroupJoin(
                        context.GenSmkcontract,
                            x => x.HospContract.SmkcontractType,
                            c => c.SmkcontractType,
                            (x, GenSmkcontractGroup) => new { x.HospBasic, x.HospContract, GenSmkcontractGroup }
                        )
                        .SelectMany(
                            x => x.GenSmkcontractGroup.DefaultIfEmpty(),
                            (x, c) => new { x.HospBasic, x.HospContract, c }
                        )
                        //.Where(x => /* 你的條件 */)  // 這裡可以添加涉及前三個表的條件
                        .GroupJoin(
                            context.ApplyContract,
                            x => new { x.HospBasic.HospId, x.HospBasic.HospSeqNo },
                            h => new { HospId = h.HOSP_ID, HospSeqNo = h.HOSP_SEQ_NO },
                            (x, hGroup) => new { x.HospBasic, x.HospContract, x.c, hGroup }
                        )
                        .SelectMany(
                            x => x.hGroup.DefaultIfEmpty(),
                            (x, h) => new HospBasicContractViewModel()
                            {
                                Id = x.HospContract != null ? x.HospContract.Id : 0,
                                HospID = x.HospBasic.HospId,
                                HospSeqNo = x.HospBasic.HospSeqNo,
                                HospName = x.HospBasic.HospName,
                                HospStartDate = x.HospContract != null ? x.HospContract.HospStartDate : string.Empty,
                                HospEndDate = x.HospContract != null ? x.HospContract.HospEndDate : string.Empty,
                                HospStatus = x.HospBasic.HospStatus.ParseNoError<HospStatus>(),
                                HospStatusName = x.HospBasic.HospStatus.ParseNoError<HospStatus>().GetEnumDescription(),
                                ContractStatus = x.HospContract != null ? (ContractStatus?)(x.HospContract.SmkcontractType.Equals("01") ? ContractStatus.MainContract : ContractStatus.ImproveContractType) : null,
                                SMKContractType = x.c != null ? x.c.SmkcontractType : string.Empty,
                                SMKContractTypeNam = x.c != null ? x.c.SmkcontractName : string.Empty,
                                CreateDate = x.HospContract.CreateDate,
                                HospNewEndDate = (x.HospContract.SmkcontractType == "01" || x.HospContract.SmkcontractType == "31") ? h.CONT_E_DATE ?? "" : "",
                            }
                        );

            //.Where(x => /* 你的條件 */)  // 這裡可以添加涉及所有表和計算字段的條件
            //.OrderBy(x => x.醫事機構代碼)
            //.ThenBy(x => x.院區別)
            //.ThenBy(x => x.機構合約起日);

            //合併完後，在做一次篩選，把Full Join的資料篩選掉
            data = data.WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospID == model.HospID)
                                 .WhereWhen(!string.IsNullOrEmpty(model.HospStartDate), x => string.Compare(x.HospStartDate, model.HospStartDate.TwDateToDateTime()) >= 0)
                                 .WhereWhen(!string.IsNullOrEmpty(model.HospEndDate), x => (string.Compare(x.HospEndDate, model.HospEndDate.TwDateToDateTime()) <= 0) && !string.IsNullOrWhiteSpace(x.HospEndDate))
                                 .WhereWhen(!string.IsNullOrEmpty(model.HospNewEndDate), x => (string.Compare(x.HospNewEndDate, model.HospNewEndDate.TwDateToDateTime()) == 0) && !string.IsNullOrWhiteSpace(x.HospNewEndDate))
                                 .WhereWhen(!string.IsNullOrEmpty(model.SMKContractType), x => x.SMKContractType == model.SMKContractType)
                                 .WhereWhen(!string.IsNullOrEmpty(model.ContractStatus) && model.ContractStatus == ContractStatus.MainContract.ToString("d"),
                                            x => x.SMKContractType == "01")
                                 .WhereWhen(!string.IsNullOrEmpty(model.ContractStatus) && model.ContractStatus == ContractStatus.ImproveContractType.ToString("d"),
                                            x => x.SMKContractType != "01")
                                 .WhereWhen(!string.IsNullOrEmpty(model.StartCreateDate),
                                            x => string.Compare(x.CreateDate, model.StartCreateDate.TwDateToDateTime()) >= 0)
                                 .WhereWhen(!string.IsNullOrEmpty(model.EndCreateDate),
                                            x => (string.Compare(x.CreateDate, model.EndCreateDate.TwDateToDateTime()) <= 0) && !string.IsNullOrWhiteSpace(x.CreateDate))
                                 .WhereWhen(model.IsApproval,
                                            x => x.HospStartDate == null || x.HospStartDate.Trim() == "")
                                 .OrderBy(x => x.HospID).ThenBy(x => x.HospSeqNo).ThenBy(x => x.HospStartDate);

            return await QueryPaging(model.get(), data.AsQueryable());
        }
        /// <summary>
        /// 批次刪除
        /// </summary>
        /// <param name="contracts"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<bool>> DeleteContracts(List<HospBasicContractViewModel> contracts, string _folder)
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
                        var rtnModel = await FindOne<HospContract>(
                            (context) => context.Where(m => m.Id == contract.Id)
                        );
                        if (rtnModel.IsSuccess)
                            context.HospContract.Remove(rtnModel.Data);

                        HospBasic hospBasic = context.HospBasic
                                                   .Where(m => context.HospContract.Any(n => m.HospId == contract.HospID
                                                         && m.HospSeqNo == contract.HospSeqNo
                                                         && m.HospId == n.HospId
                                                         && m.HospSeqNo == n.HospSeqNo
                                                         && !string.IsNullOrEmpty(n.HospEndDate)))
                                                   .FirstOrDefault();
                        if (hospBasic != null)
                        {
                            hospBasic.HospStatus = ((int)HospStatus.TerminationHosp).ToString();
                            hospBasic.ModifyDate = DateTime.Now.ToDate();
                            context.HospBasic.Update(hospBasic);
                        }

                        //因2024需求，註解掉刪除資料夾(採取個別刪除)
                        ////需要在這邊加上刪除資料夾
                        //var folderPath = $@"{_folder}\{rtnModel.Data.HospId}\{rtnModel.Data.HospSeqNo}\{rtnModel.Data.SmkcontractType}\{rtnModel.Data.Id}";
                        //if (Directory.Exists(folderPath))
                        //{
                        //    Directory.Delete(folderPath, true);
                        //}
                    }
                    var result = await context.SaveChangesWithAuditAsync(identity.Account, "更新");
                    logicRtnModel.Msg = "批次刪除成功";
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

        public async Task<LogicRtnModel<bool>> JudgeContracts(List<HospBasicContractViewModel> contracts, string hospStartDate)
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
                        var rtnModel = await FindOne<HospContract>(
                            (context) => context.Where(m => m.Id == contract.Id)
                        );
                        HospContract _contract;
                        if (!rtnModel.IsSuccess)
                        {
                            throw new Exception($"查無合約資料{contract.HospID}/{contract.HospSeqNo}/{contract.SMKContractTypeNam}/{contract.HospStartDate}");
                        }
                        _contract = rtnModel.Data;
                        _contract.HospStartDate = hospStartDate.TwDateToDateTime();
                        _contract.ModifyDate = DateTime.Now.ToDate();
                        context.HospContract.Update(_contract);

                        HospBasic hospBasic = context.HospBasic
                                                    .Where(m => context.HospContract.Any(n => m.HospId == contract.HospID
                                                          && m.HospSeqNo == contract.HospSeqNo
                                                          && m.HospId == n.HospId
                                                          && m.HospSeqNo == n.HospSeqNo
                                                          && !string.IsNullOrEmpty(n.HospStartDate)))
                                                    .FirstOrDefault();
                        if (hospBasic != null)
                        {
                            hospBasic.HospStatus = ((int)HospStatus.EnabledHosp).ToString();
                            hospBasic.ModifyDate = DateTime.Now.ToDate();
                            context.HospBasic.Update(hospBasic);
                        }
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

        /// <summary>
        /// 生效日需大於舊機構合約之最大生效日
        /// </summary>
        /// <returns></returns>
        /// <remarks>生效日需大於舊機構合約之最大生效日
        /// sql = "select max(HospStartDate) as maxHospStartDate
        /// from HospContract where HospID='" & wkg_EHospID
        /// & "' and HospSeqNo='" & wkg_EHospSeqNo(g_count) & "'"
        /// </remarks>
        internal async Task<bool> ValidateHospContractExpireDate(string hospId, string hospSeqNo, string hospStartDate)
        {
            if (string.IsNullOrEmpty(hospStartDate)) return false;
            var one = await FindOne<HospContract>(c =>
                c.Where(e => e.HospId == hospId || e.HospSeqNo == hospSeqNo));
            if (!one.IsSuccess) return true;
            return int.Parse(hospStartDate) > int.Parse(one.Data.HospStartDate);
        }

        /// <summary>
        /// 査詢機構合約列表
        /// </summary>
        /// <param name="hospId"></param>
        /// <param name="hospSeqNo"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HospContract>> QueryHospContracts(string hospId, string hospSeqNo)
        {
            var rtnModel = await Query(
                c => c.HospContract.Where(e => e.HospId == hospId &&
                                               e.HospSeqNo == hospSeqNo &&
                                               (e.HospEndDate == null || e.HospEndDate == "")));
            return rtnModel.IsSuccess ? rtnModel.Data : null;
        }

        /// <summary>
        /// 査詢單一個機構合約列表
        /// </summary>
        /// <param name="hospId"></param>
        /// <param name="hospSeqNo"></param>
        /// <returns></returns>
        public HospContract QueryHospContract(int Id)
        {
            var data = context.HospContract.Where(x => x.Id == Id).FirstOrDefault();
            return data;
        }

        /// <summary>
        /// 取得excel上傳機構的申請概況資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<ApplicationHospBasicContractViewModel>>> GetInsertHospContracts(HospContractQueryModel model)
        {
            if (model.ApplicationType != null)
            {
                ApplicationType changeValue = (ApplicationType)Enum.Parse(typeof(ApplicationType), model.ApplicationType);
                model.ApplicationType = model.ApplicationType != "" || model.ApplicationType != null ? changeValue.GetEnumDescription() : null;
            }

            var ApplyHospNew = context.ApplyHospNew
                               .WhereWhen(!string.IsNullOrEmpty(model.ApplicationType), x => x.Application_Type == model.ApplicationType)
                               .WhereWhen(!string.IsNullOrEmpty(model.Application_StartCreateDate), x => string.Compare(x.FeeYMD, model.Application_StartCreateDate.TwDateToDateTime()) >= 0)
                               .WhereWhen(!string.IsNullOrEmpty(model.Application_EndCreateDate), x => string.Compare(x.FeeYMD, model.Application_EndCreateDate.TwDateToDateTime()) <= 0)
                               .Select(x => new ApplicationHospBasicContractViewModel()
                               {
                                   FeeYM = x.FeeYM,
                                   FeeYMD = x.FeeYMD,
                                   Application_Type = x.Application_Type,
                                   Change_Type = "不適用",
                                   HospID = x.HospID,
                                   HospSeqNo = x.HospSeqNo,
                                   HospName = x.HospName,
                                   Get_Note = x.Note,
                               });

            var ApplyHospEnd = context.ApplyHospEnd
                               .WhereWhen(!string.IsNullOrEmpty(model.ApplicationType), x => x.Application_Type == model.ApplicationType)
                               .WhereWhen(!string.IsNullOrEmpty(model.Application_StartCreateDate), x => string.Compare(x.FeeYMD, model.Application_StartCreateDate.TwDateToDateTime()) >= 0)
                               .WhereWhen(!string.IsNullOrEmpty(model.Application_EndCreateDate), x => string.Compare(x.FeeYMD, model.Application_EndCreateDate.TwDateToDateTime()) <= 0)
                               .Select(x => new ApplicationHospBasicContractViewModel()
                               {
                                   FeeYM = x.FeeYM,
                                   FeeYMD = x.FeeYMD,
                                   Application_Type = x.Application_Type,
                                   Change_Type = "不適用",
                                   HospID = x.HospID,
                                   HospSeqNo = x.HospSeqNo,
                                   HospName = x.HospName,
                                   Get_Note = x.Note,
                               });

            var ApplyHospChange = context.ApplyHospChange
                    .WhereWhen(!string.IsNullOrEmpty(model.ApplicationType), x => x.Application_Type == model.ApplicationType)
                    .WhereWhen(!string.IsNullOrEmpty(model.Application_StartCreateDate), x => string.Compare(x.FeeYMD, model.Application_StartCreateDate.TwDateToDateTime()) >= 0)
                    .WhereWhen(!string.IsNullOrEmpty(model.Application_EndCreateDate), x => string.Compare(x.FeeYMD, model.Application_EndCreateDate.TwDateToDateTime()) <= 0)
                    .Select(x => new ApplicationHospBasicContractViewModel()
                    {
                        FeeYM = x.FeeYM,
                        FeeYMD = x.FeeYMD,
                        Application_Type = x.Application_Type,
                        ChangeHospID = x.ChangeHospID,
                        ChangeHospName = x.ChangeHospName,
                        ChangeHospUserName = x.ChangeHospUserName,
                        ChangeHospAddress = x.ChangeHospAddress,

                        HospID = x.HospID,
                        HospSeqNo = x.HospSeqNo,
                        HospName = x.HospName,
                        HospUserName = x.HospUserName,
                        HospAddress = x.HospAddress,

                        NewHospID = x.NewHospID,
                        NewHospSeqNo = x.NewHospSeqNo,
                        NewHospName = x.NewHospName,
                        NewHospUserName = x.NewHospUserName,
                        NewHospAddress = x.NewHospAddress,

                    });

            //var New_ApplyHospChange = ApplyHospChange
            //       .Select(x => new ApplicationHospBasicContractViewModel()
            //       {
            //           FeeYM = x.FeeYM,
            //           FeeYMD = x.FeeYMD,
            //           Application_Type = x.Application_Type,
            //           Change_Type = "不適用",
            //           HospID = x.HospID,
            //           HospSeqNo = x.HospSeqNo,
            //           HospName = x.HospName,
            //           Get_Note = x.new_Note
            //       });

            List<ApplicationHospBasicContractViewModel> data = new List<ApplicationHospBasicContractViewModel>();

            data.AddRange(ApplyHospChange.ToList());
            data.AddRange(ApplyHospEnd.ToList());
            data.AddRange(ApplyHospNew.ToList());


            var pagedData = PagedModel<ApplicationHospBasicContractViewModel>.Create(data.AsQueryable().OrderByDescending(x => x.FeeYMD), model.get());
            pagedData.RecordsTotal = data.Count();
            return new LogicRtnModel<PagedModel<ApplicationHospBasicContractViewModel>>()
            {
                IsSuccess = true,
                Data = pagedData,
            };
        }


        /// <summary>
        /// 取得excel上傳戒菸服務專案申請資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<QuotaHospHospBasicContractViewModel>>> GetInsertQuotaHosp(HospContractQueryModel model)
        {
            var data = context.QuotaHosp
                               .WhereWhen(!string.IsNullOrEmpty(model.QuotaHosp_StartCreateDate), x => string.Compare(x.FeeYMD, model.QuotaHosp_StartCreateDate.TwDateToDateTime()) >= 0)
                               .WhereWhen(!string.IsNullOrEmpty(model.QuotaHosp_EndCreateDate), x => string.Compare(x.FeeYMD, model.QuotaHosp_EndCreateDate.TwDateToDateTime()) <= 0)
                               .Select(x => new QuotaHospHospBasicContractViewModel()
                               {
                                   QuotaYear = x.QuotaYear,
                                   FeeYM = x.FeeYM,
                                   FeeYMD = x.FeeYMD,
                                   HospID = x.HospID,
                                   HospSeqNo = x.HospSeqNo,
                                   HospName = x.HospName,
                                   LastContType = x.LastContType,
                                   ApplyTreatChange = x.ApplyTreat == true ? "是" : "",
                                   ApplyTreatPeople = x.ApplyTreatPeople,
                                   ApplyHealthEduChange = x.ApplyHealthEdu == true ? "是" : "",
                                   ApplyHealthEduPeople = x.ApplyHealthEduPeople,
                                   DesignedTreat = x.DesignedTreat,
                                   DesignedTreatHealthEdu = x.DesignedTreatHealthEdu,
                                   ResultTreat = x.ResultTreat,
                                   ResultHealthEdu = x.ResultHealthEdu,

                                   VPNChangeNote = x.VPNChangeNote,
                                   //DocumentNumber = x.DocumentNumber,
                                   //IssueDate = x.IssueDate,
                                   //Treatment = x.Treatment,
                                   //ScanFileName = x.ScanFileName,
                               });

            var pagedData = PagedModel<QuotaHospHospBasicContractViewModel>.Create(data.AsQueryable().OrderByDescending(x => x.FeeYMD), model.get());
            pagedData.RecordsTotal = data.Count();
            return new LogicRtnModel<PagedModel<QuotaHospHospBasicContractViewModel>>()
            {
                IsSuccess = true,
                Data = pagedData,
            };
        }
    }
}
