using Dapper;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Data.Utility;
using SMK.Web.Exceptions;
using SMK.Web.Helpers;
using SMK.Web.Models;
using SMK.Web.Validator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using SMK.Web.Extensions;
using Yozian.DependencyInjectionPlus.Attributes;
using Yozian.Extension;
using Microsoft.AspNetCore.Http;
using System.IO;
using Yozian.WebCore.Library.Utility.Excel;
using System.Reflection.PortableExecutable;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class HospBasicService : GenericService
    {
        private readonly IDbConnection _conn = null;
        private readonly PersistenceService persistenceService;
        private readonly RtnModelService rtnModelService;
        private readonly HospContractService hospContractService;
        private readonly HospContractTypeService hospContractTypeService;
        private readonly PrsnContractService prsnContractService;

        public HospBasicService(SMKWEBContext context, SessionManager smgr,
            PersistenceService persistenceService,
            RtnModelService rtnModelService,
            HospContractTypeService hospContractTypeService,
            HospContractService hospContractService,
            PrsnContractService prsnContractService)
            : base(context, smgr)
        {
            _conn = context.Database.GetDbConnection();
            this.persistenceService = persistenceService;
            this.rtnModelService = rtnModelService;
            this.hospContractTypeService = hospContractTypeService;
            this.hospContractService = hospContractService;
            this.prsnContractService = prsnContractService;
        }
        /// <summary>
        /// 取得醫事機構基本資料
        /// </summary>
        /// <param name="hospId"></param>
        /// <param name="hospSeqNo"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<HospBasicViewModel>> getHospBasicByOrgId(string hospId)
        {
            LogicRtnModel<HospBasicViewModel> model = new LogicRtnModel<HospBasicViewModel>()
            {
                IsSuccess = true,
                Data = new HospBasicViewModel()
            };
            var data = await context.HospBscAll.FindAsync(hospId);
            if (data == null)
            {
                model.IsSuccess = false;
                model.ErrMsg = "查無此健保合約院所";
                return model;
            }
            if (!string.IsNullOrEmpty(data.HospEndDate.Trim()))
            {
                model.IsSuccess = false;
                model.ErrMsg = "此健保合約院所已解約";
                return model;
            }
            if (await context.HospBasic.AnyAsync(p => p.HospId == hospId))
            {
                model.IsSuccess = false;
                model.ErrMsg = "該機構已建檔";
                return model;
            }
            HospBasicViewModel hospBasicViewModel = new HospBasicViewModel()
            {
                HospId = data.HospId,
                HospSeqNo = "00",
                HospName = data.HospName,
                HospAddress = data.HospAddress,
                BranchNo = data.BranchNo,
                HospTel = $"{data.HospTelArea.Trim()}-{data.HospTel.Trim()}"
            };
            model.Data = hospBasicViewModel;

            return model;
        }


        /// <summary>
        /// 取得已建檔醫事機構資料
        /// </summary>
        /// <param name="hospId"></param>
        /// <param name="hospSeqNo"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<HospBasicViewModel>> getHospBasic(string hospId, string hospseqno)
        {
            LogicRtnModel<HospBasicViewModel> model = new LogicRtnModel<HospBasicViewModel>()
            {
                IsSuccess = true,
                Data = new HospBasicViewModel()
            };
            var data = await context.HospBasic.FindAsync(new[] { hospId, hospseqno });
            if (data == null)
            {
                model.IsSuccess = false;
                model.ErrMsg = "查無建檔資料";
                return model;
            }

            var contract = await context.HospContract
                                    .Where(p => p.HospId == hospId && p.HospSeqNo == hospseqno)
                                    .OrderBy(p => p.CreateDate).ThenByDescending(p => p.HospStartDate)
                                    .Select(p => new HospContractViewModel(p))
                                    .ToListAsync();

            var contrctMainContractType = contract.OrderByDescending(x => x.SmkcontractType == "01").ThenByDescending(x => x.Id).FirstOrDefault();
            //新增預設參數，讓系統可以穩定運作
            if (contrctMainContractType == null)
            {
                contrctMainContractType = new HospContractViewModel
                {
                    HospStartDate = "20230801"
                };
            }
            if (data.LastContType == null)
            {
                data.LastContType = "00";
            }


            var HospCont = await context.GenHospCont
                                        .Where(p => p.HospContType == data.LastContType)
                                        .FirstOrDefaultAsync();
            var year = context.QsQuota.Where(x => x.HOSP_ID == data.HospId && x.HOSP_SEQ_NO == data.HospSeqNo).Max(x => x.YEARS);
            int? Quota = null;
            if (await context.QsQuota.AnyAsync(x => x.HOSP_ID == data.HospId && x.HOSP_SEQ_NO == data.HospSeqNo && x.YEARS == year))
            {
                Quota = context.QsQuota.Where(x => x.HOSP_ID == data.HospId && x.HOSP_SEQ_NO == data.HospSeqNo && x.YEARS == year).Sum(x => x.QUOTA);
            }
            int? MedicationQuota = null;
            if (await context.QsQuota.AnyAsync(x => x.HOSP_ID == data.HospId && x.HOSP_SEQ_NO == data.HospSeqNo && x.CURE_TYPE == 1 && x.YEARS == year))
            {
                MedicationQuota = context.QsQuota.Where(x => x.HOSP_ID == data.HospId && x.HOSP_SEQ_NO == data.HospSeqNo && x.CURE_TYPE == 1 && x.YEARS == year).Sum(x => x.QUOTA);
            }
            int? InstructionsQuota = null;
            if (await context.QsQuota.AnyAsync(x => x.HOSP_ID == data.HospId && x.HOSP_SEQ_NO == data.HospSeqNo && x.CURE_TYPE == 2 && x.YEARS == year))
            {
                InstructionsQuota = context.QsQuota.Where(x => x.HOSP_ID == data.HospId && x.HOSP_SEQ_NO == data.HospSeqNo && x.CURE_TYPE == 2 && x.YEARS == year).Sum(x => x.QUOTA);
            }
            HospBasicViewModel hospBasicViewModel = new HospBasicViewModel()
            {
                HospId = data.HospId,
                HospName = data.HospName,
                HospTel = data.HospTel,
                HospFax = data.HospFax,
                HospEmail = data.HospEmail,
                Contact1 = data.Contact1,
                ContactTel1 = data.ContactTel1,
                ContactFax1 = data.ContactFax1,
                ContactEmail1 = data.ContactEmail1,
                Contact2 = data.Contact2,
                ContactTel2 = data.ContactTel2,
                ContactFax2 = data.ContactFax2,
                ContactEmail2 = data.ContactEmail2,
                HospOwnName = data.HospOwnName,
                HospOwnId = data.HospOwnId,
                BranchNo = data.BranchNo,
                Zip = data.Zip,
                DivisionNo = data.DivisionNo,
                SubDivisionNo = data.SubDivisionNo,
                HospAddress = data.HospAddress,
                HospStatus = data.HospStatus.ParseNoError<HospStatus>(),
                FirstHospId = data.FirstHospId,
                PrevHospID = data.PrevHospID,
                LastHospId = data.LastHospId,
                LastContType = data.LastContType,
                Remark = data.Remark,
                ChFlg1 = data.ChFlg1,
                ChFlg2 = data.ChFlg2,
                ChFlg3 = data.ChFlg3,
                PrevHospSeqNo = data.PrevHospSeqNo,
                LastHospSeqNo = data.LastHospSeqNo,
                HospSeqNo = data.HospSeqNo,
                FirstHospSeqNo = data.FirstHospSeqNo,
                HospAbbr = data.HospAbbr,
                HospContracts = contract,
                HospCont = HospCont,
                CreateDate = data.CreateDate.ToSlashTaiwanDateFromYYYYMMDD(),
                Quota = Quota,
                MedicationQuota = MedicationQuota,
                InstructionsQuota = InstructionsQuota,
                HospContMainType_StartDay = contrctMainContractType.HospStartDate.ToSlashTaiwanDateFromYYYYMMDD(),
                HospContMainType_HospContType = data.LastContType,
                HospContType = data.LastContType,
            };
            model.Data = hospBasicViewModel;

            return model;
        }

        /// <summary>
        /// 建立新機構資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<HospBasicViewModel>> CreateHospBasic(HospBasicViewModel model)
        {
            LogicRtnModel<HospBasicViewModel> logicRtnModel = new LogicRtnModel<HospBasicViewModel>()
            {
                IsSuccess = true,
                Data = model
            };
            if (!string.IsNullOrEmpty(model.HospAddress))
            {
                if (await context.GenSubDivision.AnyAsync(x => model.HospAddress.StartsWith(x.SubdivisionName)))
                {
                    var GenSubDivision = await context.GenSubDivision.FirstAsync(x => model.HospAddress.StartsWith(x.SubdivisionName));
                    model.SubDivisionNo = GenSubDivision.SubdivisionNo;
                    model.DivisionNo = GenSubDivision.DivisionNo;
                }
            }
            HospBasic hospBasic = new HospBasic()
            {
                HospId = model.HospId,
                HospName = model.HospName,
                HospTel = model.HospTel,
                HospFax = model.HospFax,
                HospEmail = model.HospEmail,
                Contact1 = model.Contact1,
                ContactTel1 = model.ContactTel1,
                ContactFax1 = model.ContactFax1,
                ContactEmail1 = model.ContactEmail1,
                Contact2 = model.Contact2,
                ContactTel2 = model.ContactTel2,
                ContactFax2 = model.ContactFax2,
                ContactEmail2 = model.ContactEmail2,
                HospOwnName = model.HospOwnName,
                HospOwnId = model.HospOwnId,
                BranchNo = model.BranchNo,
                Zip = model.Zip,
                DivisionNo = model.DivisionNo,
                SubDivisionNo = model.SubDivisionNo,
                HospAddress = model.HospAddress,
                HospStatus = HospStatus.ApplyHosp.ToString("d"),
                FirstHospId = model.FirstHospId,
                PrevHospID = model.PrevHospID,
                LastHospId = model.LastHospId,
                LastContType = model.LastContType,
                Remark = model.Remark,
                ChFlg1 = model.ChFlg1,
                ChFlg2 = model.ChFlg2,
                ChFlg3 = model.ChFlg3,
                PrevHospSeqNo = model.PrevHospSeqNo,
                LastHospSeqNo = model.LastHospSeqNo,
                HospSeqNo = model.HospSeqNo,
                FirstHospSeqNo = model.FirstHospSeqNo,
                HospAbbr = model.HospAbbr,
            };

            var createResult = await this.Create<HospBasic>(hospBasic, null, true);
            if (!createResult.IsSuccess)
            {
                logicRtnModel.extends(createResult);
            }
            return logicRtnModel;
        }

        /// <summary>
        /// 變更院所代碼
        /// </summary>
        /// <remarks>來源 clsCon1000.vb wkSaveType = "6" Then '合約生效做代碼變更</remarks>
        public async Task<LogicRtnModel<bool>> CloneHospId(string oldHospId,
            string hospId,
            string hospSDate,
            string hospName,
            string endReasonNo)
        {
            var logicRtnModel = new LogicRtnModel<bool>()
            {
                IsSuccess = true,
                Data = false
            };

            using var txn = context.GetTransactionScope();
            try
            {
                var hospStartDate = hospSDate.ToDateFromTaiwan()?.ToDate();
                // 未終止之舊機構合約自動終止，終止日期設為新機構的生效日減1
                var previousContractEndDate = hospSDate.ToDateFromTaiwan()?.AddDays(-1).ToDate();

                // '變更院所代碼時，若下有不同院區，則所有合約中的院區都複製過去
                // (0100000001有01、02、03三個院區，其中02院區已解約。變更為0100000002時，
                // 新建0100000002-01、0100000002-03兩家醫院。)
                // sql = "select HospID,HospSeqNo,HospName from HospBasic
                // where HospID='" & wkg_EHospID & "'"
                var hospBasics = await QueryHospBasiscs(oldHospId);
                foreach (var e in hospBasics)
                {
                    // '新機構代碼重複無法新增
                    // sql = "select HospID from HospBasic
                    // where HospID = '" & wkg_CHospID & "' and HospSeqNo='" & wkg_EHospSeqNo(g_count) & "'"
                    var single = await FindOne<HospBasic>(c =>
                        c.Where(b => b.HospId == hospId && b.HospSeqNo == e.HospSeqNo));
                    if (single.Data != null)
                    {
                        throw new APIException("新機構代碼重複無法新增");
                    }

                    // '生效日需大於舊機構合約之最大生效日
                    // sql = "select max(HospStartDate) as maxHospStartDate
                    // from HospContract where HospID='" & wkg_EHospID
                    // & "' and HospSeqNo='" & wkg_EHospSeqNo(g_count) & "'"
                    if (!await hospContractService.ValidateHospContractExpireDate(e.HospId, e.HospSeqNo, hospStartDate))
                    {
                        throw new APIException("生效日需大於舊機構合約之最大生效日");
                    }

                    // '取出最初機構代碼
                    // sql = "select FirstHospID,FirstHospSeqNo
                    // from HospBasic
                    // where HospID='" & wkg_EHospID
                    // & "' and HospSeqNo='" & wkg_EHospSeqNo(g_count) & "'"
                    // var firstHospBasic = await GetHospBasic(e.HospId, e.HospSeqNo);

                    // '新機構自動帶入除備註外之其餘舊機構資料，前次機構代碼設為舊機構代碼
                    // sql = "insert into HospBasic(HospID,HospName,HospTel,HospFax,HospEmail,Contact1,ContactTel1,ContactFax1,ContactEmail1,Contact2,ContactTel2,"
                    // sql += "ContactFax2,ContactEmail2,HospOwnName,HospOwnID,BranchNo,ZIP,DivisionNo,SubDivisionNo,HospAddress,HospStatus,"
                    // sql += "FirstHospID,PrevHospID,LastHospID,chFlg1,chFlg2,chFlg3,LastContType,HospSeqNo,FirstHospSeqNo,PrevHospSeqNo,LastHospSeqNo)"
                    // sql += " select '" & wkg_CHospID & "',N'" & wkg_CHospName & "',HospTel,HospFax,HospEmail,Contact1,ContactTel1,ContactFax1,ContactEmail1,Contact2,ContactTel2,"
                    // sql += "ContactFax2,ContactEmail2,HospOwnName,HospOwnID,BranchNo,ZIP,DivisionNo,SubDivisionNo,HospAddress,'2',"
                    // sql += "'" & wkFirstHospID & "','" & wkg_EHospID & "','" & wkg_CHospID & "',chFlg1,chFlg2,chFlg3,LastContType,'" & wkg_EHospSeqNo(g_count) & "','" & wkFirstHospSeqNo & "', "
                    // sql += "'" & wkg_EHospSeqNo(g_count) & "', '" & wkg_EHospSeqNo(g_count) & "'"
                    // sql += " from HospBasic
                    // where HospID='" & wkg_EHospID
                    // & "' and HospSeqNo='" & wkg_EHospSeqNo(g_count) & "'"
                    context.HospBasic.Add(new HospBasic()
                    {
                        HospId = hospId,
                        HospSeqNo = e.HospSeqNo,
                        HospName = hospName,
                        HospTel = e.HospTel,
                        HospFax = e.HospFax,
                        HospEmail = e.HospEmail,
                        HospAbbr = e.HospAbbr,
                        Contact1 = e.Contact1,
                        ContactTel1 = e.ContactTel1,
                        ContactFax1 = e.ContactFax1,
                        ContactEmail1 = e.ContactEmail1,
                        Contact2 = e.Contact2,
                        ContactTel2 = e.ContactTel2,
                        ContactFax2 = e.ContactFax2,
                        ContactEmail2 = e.ContactEmail2,
                        HospOwnName = e.HospOwnName,
                        HospOwnId = e.HospOwnId,
                        BranchNo = e.BranchNo,
                        Zip = e.Zip,
                        DivisionNo = e.DivisionNo,
                        SubDivisionNo = e.SubDivisionNo,
                        HospAddress = e.HospAddress,
                        HospStatus = HospStatus.EnabledHosp.ToString("d"),
                        FirstHospId = string.IsNullOrWhiteSpace(e.FirstHospId) ? e.HospId : e.FirstHospId,
                        FirstHospSeqNo = string.IsNullOrWhiteSpace(e.FirstHospSeqNo) ? e.HospSeqNo : e.FirstHospSeqNo,
                        PrevHospID = e.HospId,
                        PrevHospSeqNo = e.HospSeqNo,
                        LastHospId = hospId,
                        LastHospSeqNo = e.HospSeqNo,
                        ChFlg1 = e.ChFlg1,
                        ChFlg2 = e.ChFlg2,
                        ChFlg3 = e.ChFlg3,
                        LastContType = e.LastContType,
                        CreateDate = DateTime.Now.ToDate(),
                        ModifyDate = DateTime.Now.ToDate(),
                    });

                    // '新增新的機構合約(轉入原機構生效的合約)
                    // sql = "insert into HospContract(HospID,SMKContractType,HospStartDate,CreateDate,ModifyDate,ModifyPersonNo,Remark,HospSeqNo) "
                    // sql += " select '" & wkg_CHospID & "',SMKContractType,"
                    // sql += "'" & DatetoDB(wkg_CHospStartDate) & "',"
                    // sql += "'" & SysDateToDB(System.DateTime.Now.ToShortDateString) & "',"
                    // sql += "'" & SysDateToDB(System.DateTime.Now.ToShortDateString) & "',"
                    // sql += "" & G_USERID & ",Remark,'" & wkg_EHospSeqNo(g_count) & "'"
                    // sql += " from HospContract where HospID='" & wkg_EHospID & "'"
                    // sql += " and (HospEndDate is null or HospEndDate='')"
                    // sql += " and HospSeqNo='" & wkg_EHospSeqNo(g_count) & "'"
                    var hospContracts = await hospContractService.QueryHospContracts(e.HospId, e.HospSeqNo);
                    if (hospContracts != null)
                    {
                        foreach (var c in hospContracts)
                        {
                            context.HospContract.Add(new HospContract()
                            {
                                HospId = hospId,
                                HospSeqNo = e.HospSeqNo,
                                SmkcontractType = c.SmkcontractType,
                                HospStartDate = hospStartDate,
                                CreateDate = DateTime.Now.ToDate(),
                                ModifyDate = DateTime.Now.ToDate(),
                                ModifyPersonNo = null,
                                Remark = c.Remark,
                            });

                            // '未終止之舊機構合約自動終止，終止日期設為新機構的生效日減1，並帶入終止原因
                            // sql = "update HospContract set HospEndDate='" & SysDateToDB(wkCHospEndDate.ToString("yyyy/MM/dd")) & "',EndReasonNo=" & wkCEndReasonNo & ""
                            // sql += " where HospID='" & wkg_EHospID
                            // & "' and HospSeqNo='" & wkg_EHospSeqNo(g_count)
                            // & "' and (HospEndDate is null or HospEndDate='')"
                            c.HospEndDate = previousContractEndDate;
                            c.EndReasonNo = endReasonNo ?? "0";
                            context.HospContract.Update(c);
                        }
                    }

                    // '新增新的機構特約(轉入原機構特約資料)
                    // sql = "insert into HospContractType(HospID,HospContType,CntSDate,HospSeqNo) "
                    // sql += " select top 1 '" & wkg_CHospID & "',HospContType,'" & DatetoDB(wkg_CHospStartDate) & "','" & wkg_EHospSeqNo(g_count) & "' "
                    // sql += " from HospContractType
                    // where HospID='" & wkg_EHospID & "'
                    // and HospSeqNo='" & wkg_EHospSeqNo(g_count) & "' order by CntSDate desc"
                    var hospContractTypes = hospContractTypeService.QueryTop1HospContractTypes(e.HospId, e.HospSeqNo);
                    if (hospContractTypes != null)
                    {
                        foreach (var t in hospContractTypes)
                        {
                            context.HospContractType.Add(new HospContractType()
                            {
                                HospId = hospId,
                                HospSeqNo = e.HospSeqNo,
                                HospContType = t.HospContType,
                                CntSDate = hospStartDate,
                            });

                            // '舊機構特約資料寫入迄日
                            // sql = "update HospContractType set CntEDate='" & SysDateToDB(wkCHospEndDate.ToString("yyyy/MM/dd")) & "' where HospID='" & wkg_EHospID & "'"
                            // sql += " and (CntEDate is null or rtrim(CntEDate)='') and HospSeqNo='" & wkg_EHospSeqNo(g_count) & "'"
                            if (string.IsNullOrWhiteSpace(t.CntEDate))
                            {
                                t.CntEDate = previousContractEndDate;
                                context.HospContractType.Update(t);
                            }
                        }
                    }

                    // '更新舊機構特約
                    // sql = "select top 1 HospID,HospContType,CntSDate,HospSeqNo from HospContractType where HospID='" & wkg_EHospID & "' and HospSeqNo='" & wkg_EHospSeqNo(g_count) & "' order by CntSDate desc"
                    // sql = "update HospContractType set CntEDate='" & SysDateToDB(wkCHospEndDate.ToString("yyyy/MM/dd")) & "'"
                    // sql += " where HospID='" & wkCHospID & "' and HospContType='" & wkCHospContType & "' and CntSDate='" & wkCCntSDate & "' and HospSeqNo='" & wkCHospSeqNo & "'"

                    // sql = "update HospBasic set HospStatus='3' where HospID='" & wkg_EHospID & "' and HospSeqNo='" & wkg_EHospSeqNo(g_count) & "'"
                    e.HospStatus = HospStatus.TerminationHosp.ToString("d");
                    e.FirstHospId = string.IsNullOrWhiteSpace(e.FirstHospId) ? e.HospId : e.FirstHospId;
                    e.FirstHospSeqNo = string.IsNullOrWhiteSpace(e.FirstHospSeqNo) ? e.HospSeqNo : e.FirstHospSeqNo;
                    e.LastHospId = hospId;
                    e.LastHospSeqNo = e.HospSeqNo;
                    context.HospBasic.Update(e);

                    // '舊機構及最後機構ID為舊機構者，最後機構ID自動設為新機構ID
                    // sql = "update HospBasic set LastHospID='" & wkg_CHospID & "',LastHospSeqNo='" & wkg_EHospSeqNo(g_count) & "'
                    // where FirstHospID='" & wkFirstHospID & "' and FirstHospSeqno='" & wkFirstHospSeqNo & "'"
                    var firsts = await QueryFirstHospBasic(e.FirstHospId, e.FirstHospSeqNo);
                    if (firsts != null)
                    {
                        foreach (var first in firsts)
                        {
                            first.LastHospId = e.LastHospId;
                            first.LastHospSeqNo = e.LastHospSeqNo;
                            context.HospBasic.Update(first);
                        }
                    }

                    // '自動將未終止之醫事人員合約名單新增至新機構，生效日同新機構生效日
                    // sql = "insert into PrsnContract(HospID,PrsnID,SMKContractType,PrsnStartDate,CreateDate,ModifyDate,ModifyPersonNo,Remark,HospSeqNo,CouldTreat,CouldInstruct) "
                    // sql += "select '" & wkg_CHospID & "',PrsnID,SMKContractType,"
                    // sql += "'" & DatetoDB(wkg_CHospStartDate) & "',"
                    // sql += "'" & SysDateToDB(System.DateTime.Now.ToShortDateString) & "',"
                    // sql += "'" & SysDateToDB(System.DateTime.Now.ToShortDateString) & "',"
                    // sql += "" & G_USERID & ",Remark,"
                    // sql += "'" & wkg_EHospSeqNo(g_count) & "',CouldTreat,CouldInstruct"
                    // sql += " from PrsnContract where HospID='" & wkg_EHospID & "'"
                    // sql += " and (PrsnEndDate is null or rtrim(PrsnEndDate) = '')"
                    // sql += " and HospSeqNo='" & wkg_EHospSeqNo(g_count) & "'"
                    var prsnContracts = await prsnContractService.QueryPrsnContracts(e.HospId, e.HospSeqNo);
                    if (prsnContracts != null)
                    {
                        foreach (var c in prsnContracts)
                        {
                            context.PrsnContract.Add(new PrsnContract()
                            {
                                HospId = hospId,
                                PrsnId = c.PrsnId,
                                SmkcontractType = c.SmkcontractType,
                                PrsnStartDate = hospStartDate,
                                CreateDate = DateTime.Now.ToDate(),
                                ModifyDate = DateTime.Now.ToDate(),
                                ModifyPersonNo = null,
                                Remark = c.Remark,
                                HospSeqNo = e.HospSeqNo,
                                CouldTreat = c.CouldTreat,
                                CouldInstruct = c.CouldInstruct,
                            });

                            // '舊機構未終止之醫事人員合約自動終止，終止日期設為新機構的生效日減1
                            // sql = "update PrsnContract set PrsnEndDate='" & SysDateToDB(wkCHospEndDate.ToString("yyyy/MM/dd"))
                            // & "' where HospID='" & wkg_EHospID
                            // & "' and HospSeqNo='" & wkg_EHospSeqNo(g_count) & "'"
                            // sql += " and (PrsnEndDate is null or rtrim(PrsnEndDate) = '')"
                            c.EndReasonNo = endReasonNo;
                            c.PrsnEndDate = previousContractEndDate;
                            context.PrsnContract.Update(c);
                        }
                    }

                    // ''將醫事人員Email資料新增至新機構，僅限於舊機構狀態為合約中醫事人員
                    // 'sql = "insert into PrsnEmail(PrsnID,PEmail,HospID,HospSeqNo) "
                    // 'sql += "select PrsnID,PEmail,'" & wkg_CHospID & "','" & wkg_EHospSeqNo(g_count) & "' from PrsnEmail"
                    // 'sql += " where HospID='" & wkg_EHospID & "' and HospSeqNo='" & wkg_EHospSeqNo(g_count) & "'"
                    // 'sql += " and PrsnID in (select PrsnID from PrsnContract where HospID='" & wkg_EHospID & "' and HospSeqNo='" & wkg_EHospSeqNo(g_count) & "'"
                    // 'sql += " and (PrsnEndDate is null or rtrim(PrsnEndDate) = ''))"
                    // 舊程式已停用
                }

                await context.SaveChangesAsync();
                await txn.CommitAsync();
                return logicRtnModel;
            }
            catch (Exception ex)
            {
                await txn.RollbackAsync();
                return new LogicRtnModel<bool>(MsgType.SaveFail, ex.Message)
                {
                    StackTrace = ex.DumpDetail()
                };
            }
        }

        private async Task<HospBasic> GetHospBasic(string hospId, string hospSeqNo)
        {
            var single = await FindOne<HospBasic>(c =>
                c.Where(e => e.HospId == hospId && e.HospSeqNo == hospSeqNo));
            if (single.IsSuccess) return null;
            return single.Data;
        }

        public async Task<LogicRtnModel<bool>> ChangeHospId(string NewHospId, string NewHospSeqNo, string OldHospId, string OldHospSeqNo)
        {
            LogicRtnModel<bool> logicRtnModel = new LogicRtnModel<bool>()
            {
                IsSuccess = true,
                Data = false
            };

            HospBasic hospBasic = await context.HospBasic.FindAsync(OldHospId, OldHospSeqNo);
            if (hospBasic == null)
            {
                throw new Exception("查無機構建檔資料");
            }
            _conn.Open();
            using (var txn = _conn.BeginTransaction())
            {
                try
                {
                    string sqlstr = @"update hospBasic
                                      set PrevHospID = @OldHospId,PrevHospSeqNo = @OldHospSeqNo
                                      ,LastHospId = @NewHospId,LastHospSeqNo = @NewHospSeqNo
                                      ,HospId = @NewHospId,HospSeqNo = @NewHospSeqNo,
                                      ModifyDate=convert(varchar, getdate(), 112)
                                      where HospId=@OldHospId AND  HospSeqNo =@OldHospSeqNo;
                                      
                                      update HospContract
                                      set HospId = @NewHospId,HospSeqNo = @NewHospSeqNo,
                                      ModifyDate=convert(varchar, getdate(), 112)
                                      where HospId=@OldHospId AND  HospSeqNo =@OldHospSeqNo;
                                      
                                      update PrsnContract
                                      set HospId = @NewHospId,HospSeqNo = @NewHospSeqNo,
                                      ModifyDate=convert(varchar, getdate(), 112)
                                      where HospId=@OldHospId AND  HospSeqNo =@OldHospSeqNo;";

                    var data = await _conn.ExecuteAsync(sqlstr, new
                    {
                        NewHospId,
                        NewHospSeqNo,
                        OldHospId,
                        OldHospSeqNo
                    }, txn);

                    logicRtnModel.Msg = "更新成功";
                    txn.Commit();
                    return logicRtnModel;
                }
                catch (Exception err)
                {
                    txn.Rollback();
                    return new LogicRtnModel<bool>(MsgType.SaveFail, err.Message)
                    {
                        StackTrace = err.StackTrace
                    };
                }
            }
        }

        public async Task<LogicRtnModel<HospBasicViewModel>> updateHospBasic(HospBasicViewModel model)
        {
            LogicRtnModel<HospBasicViewModel> logicRtnModel = new LogicRtnModel<HospBasicViewModel>()
            {
                IsSuccess = true,
                Data = model
            };
            ValidationResult validationResult = await new HospBasicViewModelValidator().ValidateAsync(model);
            if (!validationResult.IsValid)
                return ValidationFlase<HospBasicViewModel>(MsgType.SaveFail, validationResult);
            using (var txn = context.GetTransactionScope())
            {
                try
                {
                    HospBasic hospBasic = await context.HospBasic.FindAsync(model.HospId, model.HospSeqNo);
                    if (hospBasic == null)
                    {
                        throw new Exception("查無機構建檔資料");
                    }
                    if (!string.IsNullOrEmpty(model.HospAddress))
                    {
                        if (await context.GenSubDivision.AnyAsync(x => model.HospAddress.StartsWith(x.SubdivisionName)))
                        {
                            var GenSubDivision = await context.GenSubDivision.FirstAsync(x => model.HospAddress.StartsWith(x.SubdivisionName));
                            model.SubDivisionNo = GenSubDivision.SubdivisionNo;
                            model.DivisionNo = GenSubDivision.DivisionNo;
                        }
                    }
                    hospBasic.HospId = model.HospId;
                    hospBasic.HospName = model.HospName;
                    hospBasic.HospTel = model.HospTel;
                    hospBasic.HospFax = model.HospFax;
                    hospBasic.HospEmail = model.HospEmail;
                    hospBasic.Contact1 = model.Contact1;
                    hospBasic.ContactTel1 = model.ContactTel1;
                    hospBasic.ContactFax1 = model.ContactFax1;
                    hospBasic.ContactEmail1 = model.ContactEmail1;
                    hospBasic.Contact2 = model.Contact2;
                    hospBasic.ContactTel2 = model.ContactTel2;
                    hospBasic.ContactFax2 = model.ContactFax2;
                    hospBasic.ContactEmail2 = model.ContactEmail2;
                    hospBasic.HospOwnName = model.HospOwnName;
                    hospBasic.HospOwnId = model.HospOwnId;
                    hospBasic.BranchNo = model.BranchNo;
                    hospBasic.Zip = model.Zip;
                    hospBasic.DivisionNo = model.DivisionNo;
                    hospBasic.SubDivisionNo = model.SubDivisionNo;
                    hospBasic.HospAddress = model.HospAddress;
                    hospBasic.HospStatus = model.HospStatus?.ToString("d");
                    hospBasic.FirstHospId = model.FirstHospId;
                    hospBasic.PrevHospID = model.PrevHospID;
                    hospBasic.LastHospId = model.LastHospId;
                    hospBasic.LastContType = model.LastContType;
                    hospBasic.Remark = model.Remark;
                    hospBasic.ChFlg1 = model.ChFlg1;
                    hospBasic.ChFlg2 = model.ChFlg2;
                    hospBasic.ChFlg3 = model.ChFlg3;
                    hospBasic.PrevHospSeqNo = model.PrevHospSeqNo;
                    hospBasic.LastHospSeqNo = model.LastHospSeqNo;
                    hospBasic.HospSeqNo = model.HospSeqNo;
                    hospBasic.FirstHospSeqNo = model.FirstHospSeqNo;
                    hospBasic.HospAbbr = model.HospAbbr;
                    hospBasic.ModifyDate = DateTime.Now.ToDate();
                    hospBasic.LastContType = model.HospContType;

                    context.HospBasic.Update(hospBasic);
                    foreach (var contract in model.HospContracts)
                    {
                        var rtnModel = await FindOne<HospContract>(
                            (context) => context.Where(m => m.Id == contract.Id)
                        );
                        HospContract _contract;
                        if (rtnModel.IsSuccess)
                        {
                            _contract = rtnModel.Data;
                            //update
                            _contract.SmkcontractType = contract.SmkcontractType;
                            if (!string.IsNullOrEmpty(contract.HospStartDate))
                                _contract.HospStartDate = contract.HospStartDate;
                            _contract.HospEndDate = contract.HospEndDate;
                            _contract.EndReasonNo = contract.EndReasonNo;
                            _contract.ModifyDate = DateTime.Now.ToDate();
                            //_contract.ModifyPersonNo = contract.ModifyPersonNo;
                            _contract.Remark = contract.Remark;
                            context.HospContract.Update(_contract);
                        }
                        else
                        {
                            //new 
                            _contract = new HospContract();
                            _contract.HospId = contract.HospId;
                            _contract.SmkcontractType = contract.SmkcontractType;
                            _contract.HospStartDate = contract.HospStartDate;
                            _contract.HospEndDate = contract.HospEndDate;
                            _contract.EndReasonNo = contract.EndReasonNo;
                            _contract.CreateDate = DateTime.Now.ToDate();
                            //_contract.ModifyPersonNo = contract.ModifyPersonNo;
                            _contract.Remark = contract.Remark;
                            _contract.HospSeqNo = contract.HospSeqNo;
                            context.HospContract.Add(_contract);
                        }
                    }

                    var returnModel = await Query(context => context.HospContractType
                                                .Where(e => e.HospId == model.HospId && e.HospSeqNo == model.HospSeqNo));
                    if (returnModel.IsSuccess)
                    {
                        foreach (var e in returnModel.Data)
                        {
                            await Remove(e);
                        }

                        model.HospContractTypes.ForEach(e =>
                        {
                            context.HospContractType.Add(new HospContractType
                            {
                                HospId = model.HospId,
                                HospSeqNo = model.HospSeqNo,
                                HospContType = e.HospContType,
                                CntSDate = e.CntSDate.TwDateToDateTime(),
                                CntEDate = e.CntEDate.TwDateToDateTime()
                            });
                        });
                    }

                    var result = await context.SaveChangesWithAuditAsync(identity.Account, "更新");
                    logicRtnModel.Msg = "更新成功";
                    await txn.CommitAsync();
                    return logicRtnModel;
                }
                catch (Exception err)
                {
                    await txn.RollbackAsync();
                    return new LogicRtnModel<HospBasicViewModel>(MsgType.SaveFail, err.Message)
                    {
                        Data = model,
                        StackTrace = err.StackTrace
                    };
                }
            }
        }


        public async Task<string> GetHospNameAsync(string hospId, string hospSeqNo)
        {
            var result
                = await context
                    .HospBasic
                    .Where(x => x.HospId == hospId && x.HospSeqNo == hospSeqNo)
                    .Select(x => x.HospName)
                    .FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<HospBasic>> QueryHospBasiscs(string hospId)
        {
            var rtnModel = await Query(c => c.HospBasic.Where(e => e.HospId == hospId));
            return rtnModel.IsSuccess ? rtnModel.Data : null;
        }

        public async Task<IEnumerable<HospBasic>> QueryFirstHospBasic(string hospId, string hospSeqNo)
        {
            var rtnModel = await Query<HospBasic>(c =>
                c.HospBasic.Where(e => e.FirstHospId == hospId && e.FirstHospSeqNo == hospSeqNo));
            return rtnModel.IsSuccess ? rtnModel.Data : null;
        }

        #region 機構申請
        /// <summary>
        /// 上傳excel
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<HospContractQueryModel>> UploadApplication(IFormFile formFile)
        {
            LogicRtnModel<HospContractQueryModel> logicRtnModel = new LogicRtnModel<HospContractQueryModel>();
            string success_Msg = string.Empty;
            int all_Count = 0;
            int Update_Count = 0;
            int Next_Count = 0;
            #region 新開辦戒菸服務醫事機構
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                ExcelReader excelReader = new ExcelReader();
                List<List<string>> content = excelReader.ReadAsLIstOfList(ms, "【一】新開辦戒菸服務醫事機構");
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
                            ApplyHospNew obj = new ApplyHospNew();
                            for (int i = 0; i < 12; i++)
                            {
                                logicRtnModel = FillObjByIndex_New(obj, i, row, content.IndexOf(row), "【一】新開辦戒菸服務醫事機構");
                                if (!logicRtnModel.IsSuccess)
                                {
                                    return logicRtnModel;
                                }
                            }
                            obj.UpdatedBy = identity.Account;
                            if (!context.ApplyHospNew.Any(x => x.Serial_Number == obj.Serial_Number))
                            {
                                context.ApplyHospNew.Add(obj);
                            }
                            else
                            {
                                context.ApplyHospNew.Update(obj);
                                Update_Count++;
                            }
                            all_Count++;
                        }
                        catch (Exception e)
                        {
                            logicRtnModel.IsSuccess = false;
                            logicRtnModel.Data = new HospContractQueryModel()
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
                        DataInsertLog dataInsertLog = new Services.DataInsertLogService().dataInsertLog($"新開辦戒菸服務醫事機構 - {formFile.FileName}", all_Count);
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
            logicRtnModel.Msg += $"【一】新開辦戒菸服務醫事機構，需執行 {all_Count + Next_Count} 筆資料。(寫入 {all_Count - Update_Count} 筆，更新 {Update_Count} 筆，跳過 {Next_Count} 筆)";
            success_Msg = logicRtnModel.Msg;

            #endregion

            #region 【解約】機構
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                ExcelReader excelReader = new ExcelReader();
                List<List<string>> content = excelReader.ReadAsLIstOfList(ms, "【解約】機構");
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
                            ApplyHospEnd ApplyHospEnd_obj = new ApplyHospEnd();
                            for (int i = 0; i < 18; i++)
                            {
                                logicRtnModel = FillObjByIndex_End(ApplyHospEnd_obj, i, row, content.IndexOf(row), "【解約】機構");
                                if (!logicRtnModel.IsSuccess)
                                {
                                    return logicRtnModel;
                                }
                            }
                            ApplyHospEnd_obj.UpdatedBy = identity.Account;
                            ApplyHospEnd_obj.Application_Type = ApplicationType.End_order.GetEnumDescription();
                            if (!context.ApplyHospEnd.Any(x => x.Serial_Number == ApplyHospEnd_obj.Serial_Number))
                            {
                                context.ApplyHospEnd.Add(ApplyHospEnd_obj);
                            }
                            else
                            {
                                context.ApplyHospEnd.Update(ApplyHospEnd_obj);
                                Update_Count++;
                            }
                            all_Count++;
                        }
                        catch (Exception e)
                        {
                            logicRtnModel.IsSuccess = false;
                            logicRtnModel.Data = new HospContractQueryModel()
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
                        DataInsertLog dataInsertLog = new Services.DataInsertLogService().dataInsertLog($"【解約】機構 - {formFile.FileName}", all_Count);
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
            logicRtnModel.Msg = success_Msg + $"; 【解約】機構，需執行 {all_Count + Next_Count} 筆資料。(寫入 {all_Count - Update_Count} 筆，更新 {Update_Count} 筆，跳過 {Next_Count} 筆)";
            success_Msg = logicRtnModel.Msg;
            #endregion

            #region 【變更】醫事機構(代碼、名稱、負責人、地址)
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                ExcelReader excelReader = new ExcelReader();
                List<List<string>> content = excelReader.ReadAsLIstOfList(ms, "【變更】醫事機構(代碼、名稱、負責人、地址)");
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
                            if (string.IsNullOrEmpty(row[6]))
                            {
                                Next_Count++;
                                continue;
                            }
                            string rowdata = string.Empty;
                            ApplyHospChange ApplyHospChange_obj = new ApplyHospChange();
                            for (int i = 0; i < 20; i++)
                            {
                                if (string.IsNullOrEmpty(row[6]))
                                    continue;
                                logicRtnModel = FillObjByIndex_Change(ApplyHospChange_obj, i, row, content.IndexOf(row), "【變更】醫事機構(代碼、名稱、負責人、地址)");
                                if (!logicRtnModel.IsSuccess)
                                {
                                    return logicRtnModel;
                                }
                            }
                            ApplyHospChange_obj.UpdatedBy = identity.Account;
                            ApplyHospChange_obj.Application_Type = ApplicationType.change.GetEnumDescription();
                            if (!context.ApplyHospChange.Any(x => x.Serial_Number == ApplyHospChange_obj.Serial_Number))
                            {
                                context.ApplyHospChange.Add(ApplyHospChange_obj);
                            }
                            else
                            {
                                context.ApplyHospChange.Update(ApplyHospChange_obj);
                                Update_Count++;
                            }
                            all_Count++;
                        }
                        catch (Exception e)
                        {
                            logicRtnModel.IsSuccess = false;
                            logicRtnModel.Data = new HospContractQueryModel()
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
                        DataInsertLog dataInsertLog = new Services.DataInsertLogService().dataInsertLog($"【變更】醫事機構(代碼、名稱、負責人、地址) - {formFile.FileName}", all_Count);
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
        private LogicRtnModel<HospContractQueryModel> CheckExcelHeader_New(List<string> row)
        {
            HospContractQueryModel hospContractQueryModel = new HospContractQueryModel()
            {
                err = new List<string>()
            };
            string[] header = { "序次", "收件年月", "編號", "收件日\n(格式：YYYMMDD)", "申請類型", "院所代碼", "院區別", "院所名稱", "層級", "業務組別", "SMK系統登錄日期", "備註" };
            if (row.Count != header.Length)
            {
                hospContractQueryModel.err.Add("新開辦，(欄位數量錯誤)標題列應該有" + header.Length + "欄:" + string.Join(",", header) + " ，目前傳入有 " + row.Count + "欄");
            }
            else
            {
                for (int i = 0; i < row.Count; i++)
                {
                    if (row[i].Trim() != header[i])
                    {
                        hospContractQueryModel.err.Add($"新開辦，標題列第{i + 1}({row[i].Trim()})欄名稱應該為{header[i]}");
                    }
                }
            }

            var rtnModel = new LogicRtnModel<HospContractQueryModel>();
            rtnModel.Data = hospContractQueryModel;
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

        private LogicRtnModel<HospContractQueryModel> CheckExcelHeader_End(List<string> row)
        {
            HospContractQueryModel hospContractQueryModel = new HospContractQueryModel()
            {
                err = new List<string>()
            };
            string[] header = { "序次", "收件年月", "編號", "收件日\n(格式：YYYMMDD)", "院所代碼", "院區別", "院所名稱", "層級別", "業務組別", "負責人姓名", "地址", "合約生效日\n(格式：YYYMMDD)", "合約終止日\n(格式：YYYMMDD)", "解約原因", "備註", "TTC\n合約已解約\n(以0顯示)", "修改日期", "修改備註" };
            if (row.Count != header.Length)
            {
                hospContractQueryModel.err.Add("【解約】機構，(欄位數量錯誤)標題列應該有" + header.Length + "欄:" + string.Join(",", header));
            }
            else
            {
                for (int i = 0; i < row.Count; i++)
                {
                    if (row[i].Trim() != header[i])
                    {
                        hospContractQueryModel.err.Add($"【解約】機構，標題列第{i + 1}({row[i].Trim()})欄名稱應該為{header[i]}");
                    }
                }
            }

            var rtnModel = new LogicRtnModel<HospContractQueryModel>();
            rtnModel.Data = hospContractQueryModel;
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

        private LogicRtnModel<HospContractQueryModel> CheckExcelHeader_Change(List<string> row)
        {
            HospContractQueryModel hospContractQueryModel = new HospContractQueryModel()
            {
                err = new List<string>()
            };
            string[] header = { "", "院所代碼", "院所名稱", "負責人", "機構地址", "", "", "", "", "院所代碼", "院區別", "院所名稱", "負責人", "機構地址", "院所代碼", "院區別", "院所名稱", "負責人", "機構地址", "院所代碼", "院所名稱", "負責人", "機構地址", "調劑藥局" };
            if (row.Count != header.Length)
            {
                hospContractQueryModel.err.Add("【變更】醫事機構，(欄位數量錯誤)標題列應該有" + header.Length + "欄:" + string.Join(",", header));
            }
            else
            {
                for (int i = 0; i < row.Count; i++)
                {
                    if (row[i].Trim() != header[i])
                    {
                        hospContractQueryModel.err.Add($"【變更】醫事機構，標題列第{i + 1}({row[i].Trim()})欄名稱應該為{header[i]}");
                    }
                }
            }

            var rtnModel = new LogicRtnModel<HospContractQueryModel>();
            rtnModel.Data = hospContractQueryModel;
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


        private LogicRtnModel<HospContractQueryModel> CheckExcelSheet(List<string> row, int array_Num)
        {
            HospContractQueryModel hospContractQueryModel = new HospContractQueryModel()
            {
                err = new List<string>()
            };
            string[] CheckSheetName = { "一、 新開辦戒菸服務醫事機構", "【解約】機構", "【變更】醫事機構(代碼、名稱、負責人、地址)" };
            if (CheckSheetName[array_Num] != row[0])
            {
                hospContractQueryModel.err.Add($"標題列A1為{row[0]}，名稱應該為{CheckSheetName[array_Num]}");
            }


            var rtnModel = new LogicRtnModel<HospContractQueryModel>();
            rtnModel.Data = hospContractQueryModel;
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


        private LogicRtnModel<HospContractQueryModel> FillObjByIndex_New(ApplyHospNew obj, int i, List<string> row, int rownumber, string sheetName)
        {
            HospContractQueryModel hospContractQueryModel = new HospContractQueryModel()
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
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMM，或月分異常");
                    }
                    else
                    {
                        obj.FeeYM = row[i];
                    }

                    break;
                case 2:
                    if (row[i].Trim().Length != 10)
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyy-MM-000，或長度異常");
                    }
                    else
                    {
                        obj.Serial_Number = row[i];
                    }
                    break;
                case 3:
                    if (row[i].Trim().Length == 7)
                    {
                        obj.FeeYMD = row[i].ToYYYYMMDDFromTaiwan();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMMdd，或月分、日期異常");
                    }
                    break;
                case 4:
                    obj.Application_Type = ApplicationType.new_order.GetEnumDescription();
                    break;
                case 5:
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
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為10");
                    }
                    break;
                case 6:
                    if (row[i].Length == 2 || row[i].Length == 1)
                    {
                        obj.HospSeqNo = row[i].Length == 1 ? "0" + row[i] : row[i];
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為2");
                    }

                    break;
                case 7:
                    if (row[i].Trim().Length < 50)
                    {
                        obj.HospName = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過50");
                    }

                    break;
                case 8:
                    if (row[i].Trim().Length < 10)
                    {
                        obj.LastContType = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過10");
                    }
                    break;
                case 9:
                    if (row[i].Trim().Length < 10)
                    {
                        obj.BranchName = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過10");
                    }
                    break;
                case 10:
                    if (row[i].Trim().Length == 7 || string.IsNullOrEmpty(row[i]) || row[i] == "-")
                    {
                        if (string.IsNullOrEmpty(row[i]) || row[i] == "-")
                            obj.SMKLogDate = null;
                        else
                            obj.SMKLogDate = row[i].ToYYYYMMDDFromTaiwan();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMMdd，或月分、日期異常");
                    }
                    break;
                case 11:
                    if (row[i].Trim().Length < 50)
                    {
                        obj.Note = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過50");
                    }
                    break;
                default:
                    break;
            }
            var rtnModel = new LogicRtnModel<HospContractQueryModel>();
            rtnModel.Data = hospContractQueryModel;
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

        private LogicRtnModel<HospContractQueryModel> FillObjByIndex_End(ApplyHospEnd ApplyHospEnd_obj, int i, List<string> row, int rownumber, string sheetName)
        {
            HospContractQueryModel hospContractQueryModel = new HospContractQueryModel()
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
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMM，或月分異常");
                    }
                    else
                    {
                        ApplyHospEnd_obj.FeeYM = row[i];
                    }

                    break;
                case 2:
                    if (row[i].Trim().Length != 10)
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyy-MM-000，或長度異常");
                    }
                    else
                    {
                        ApplyHospEnd_obj.Serial_Number = row[i];
                    }
                    break;
                case 3:
                    if (row[i].Trim().Length == 7)
                    {
                        ApplyHospEnd_obj.FeeYMD = row[i].ToYYYYMMDDFromTaiwan();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMMdd，或月分、日期異常");
                    }
                    break;
                case 4:
                    if (row[i].Trim().Length == 10 || row[i].Trim().Length == 9)
                    {
                        if (row[i].Trim().Length == 9)
                        {
                            row[i] = "0" + row[i];
                        }
                        ApplyHospEnd_obj.HospID = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為10");
                    }
                    break;
                case 5:
                    if (row[i].Length == 2 || row[i].Length == 1)
                    {
                        ApplyHospEnd_obj.HospSeqNo = row[i].Length == 1 ? "0" + row[i] : row[i];
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為2");
                    }

                    break;
                case 6:
                    if (row[i].Trim().Length < 50)
                    {
                        ApplyHospEnd_obj.HospName = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過50");
                    }
                    break;
                case 7:
                    if (row[i].Trim().Length < 10)
                    {
                        ApplyHospEnd_obj.LastContType = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過10");
                    }
                    break;
                case 8:
                    if (row[i].Trim().Length < 10)
                    {
                        ApplyHospEnd_obj.BranchName = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過10");
                    }
                    break;
                case 9:
                    if (row[i].Trim().Length < 50)
                    {
                        ApplyHospEnd_obj.HospUserName = row[i].Trim() == "-" ? null : row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過50");
                    }
                    break;
                case 10:
                    if (row[i].Trim().Length < 50)
                    {
                        ApplyHospEnd_obj.HospAddress = row[i].Trim() == "-" ? null : row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過50");
                    }
                    break;
                case 11:
                    if (row[i].Trim().Length == 7)
                    {
                        ApplyHospEnd_obj.MinHospStartDate = row[i].ToYYYYMMDDFromTaiwan();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMMdd，或月分、日期異常");
                    }
                    break;
                case 12:
                    if (row[i].Trim().Length == 7)
                    {
                        ApplyHospEnd_obj.MaxHospEndDate = row[i].ToYYYYMMDDFromTaiwan();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMMdd，或月分、日期異常");
                    }
                    break;
                case 13:
                    if (row[i].Trim().Length < 50 || row[i].Trim() == "" || row[i].Trim() == null)
                    {
                        ApplyHospEnd_obj.Reason = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過50");
                    }
                    break;
                case 14:
                    if (row[i].Trim().Length < 50 || row[i].Trim() == "" || row[i].Trim() == null)
                    {
                        ApplyHospEnd_obj.Note = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過50");
                    }
                    break;
                case 15:
                    if (row[i].Trim().Length < 50 || row[i].Trim() == "" || row[i].Trim() == null)
                    {
                        ApplyHospEnd_obj.TTCNote = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過50");
                    }
                    break;
                case 16:
                    if (DateTime.TryParse(row[i], out DateTime date) || string.IsNullOrEmpty(row[i]) || row[i] == "-")
                    {
                        if (string.IsNullOrEmpty(row[i]) || row[i] == "-")
                        {
                            row[i] = "";
                            ApplyHospEnd_obj.SingleChangeDate = null;
                        }
                        else
                        {
                            ApplyHospEnd_obj.SingleChangeDate = date.ToString("yyyyMMdd");
                        }
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，，格式應該為yyyy/MM/dd，或月分、日期異常");
                    }
                    break;
                case 17:
                    if (row[i].Trim().Length < 50 || row[i].Trim() == "" || row[i].Trim() == null)
                    {
                        ApplyHospEnd_obj.SingleNote = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 2}欄資料({row[i].Trim()})錯誤，長度超過50");
                    }
                    break;
                default:
                    break;
            }
            var rtnModel = new LogicRtnModel<HospContractQueryModel>();
            rtnModel.Data = hospContractQueryModel;
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

        private LogicRtnModel<HospContractQueryModel> FillObjByIndex_Change(ApplyHospChange ApplyHospChange_obj, int i, List<string> row, int rownumber, string sheetName)
        {
            HospContractQueryModel hospContractQueryModel = new HospContractQueryModel()
            {
                err = new List<string>()
            };
            string Total_number = row[0] == "" ? "此筆沒有序號" : row[0];
            switch (i)
            {
                case 0:

                    break;
                case 1:
                    ApplyHospChange_obj.ChangeHospID = row[i] == "V" ? true : false;
                    break;
                case 2:
                    ApplyHospChange_obj.ChangeHospName = row[i] == "V" ? true : false;
                    break;
                case 3:
                    ApplyHospChange_obj.ChangeHospUserName = row[i] == "V" ? true : false;
                    break;
                case 4:
                    ApplyHospChange_obj.ChangeHospAddress = row[i] == "V" ? true : false;
                    break;
                case 5:
                    if (row[i].Trim().Length != 5)
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMM，或月分異常");
                    }
                    else
                    {
                        ApplyHospChange_obj.FeeYM = row[i];
                    }
                    break;
                case 6:
                    if (row[i].Trim().Length != 10)
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyy-MM-000，或長度異常");
                    }
                    else
                    {
                        ApplyHospChange_obj.Serial_Number = row[i];
                    }
                    break;
                case 7:
                    if (row[i].Trim().Length == 7)
                    {
                        ApplyHospChange_obj.FeeYMD = row[i].ToYYYYMMDDFromTaiwan();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMMdd，或月分、日期異常");
                    }
                    break;
                case 8:
                    if (row[i].Length == 7 || row[i] == "" || row[i] == null)
                    {
                        ApplyHospChange_obj.StartYMD = row[i].ToYYYYMMDDFromTaiwan();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMMdd，或月分、日期異常");
                    }
                    break;
                case 9:
                    if (row[i].Trim().Length == 10 || row[i].Trim().Length == 9)
                    {
                        ApplyHospChange_obj.HospID = row[i].Trim().Length == 9 ? $"0{row[i]}" : row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為10");
                    }
                    break;
                case 10:
                    if (row[i].Length == 2 || row[i].Length == 1)
                    {
                        ApplyHospChange_obj.HospSeqNo = row[i].Length == 1 ? "0" + row[i] : row[i];
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為2");
                    }
                    break;
                case 11:
                    if (row[i].Trim().Length < 50)
                    {
                        ApplyHospChange_obj.HospName = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過50");
                    }
                    break;
                case 12:
                    if (row[i].Length < 50 || row[i].Trim() == "")
                    {
                        ApplyHospChange_obj.HospUserName = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過50");
                    }
                    break;
                case 13:
                    if (row[i].Length < 80)
                    {
                        ApplyHospChange_obj.HospAddress = row[i];
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過80");
                    }
                    break;
                case 14:
                    if (row[i].Trim().Length == 10 || row[i].Trim().Length == 9 || row[i] == null || row[i] == "")
                    {
                        ApplyHospChange_obj.NewHospID = row[i].Trim().Length == 9 ? $"0{row[i]}" : row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為10");
                    }
                    break;
                case 15:
                    if (row[i].Length == 2 || row[i].Length == 1 || row[i] == "0" || row[i].Trim() == "" || row[i] == null)
                    {
                        ApplyHospChange_obj.NewHospSeqNo = row[i].Length == 1 ? "0" + row[i] : row[i];
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為2");
                    }
                    break;
                case 16:
                    if (row[i].Trim().Length < 50 || row[i].Trim() == "" || row[i] == null)
                    {
                        ApplyHospChange_obj.NewHospName = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過50");
                    }
                    break;
                case 17:
                    if (row[i].Length < 50 || row[i].Trim() == "" || row[i] == null)
                    {
                        ApplyHospChange_obj.NewHospUserName = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過50");
                    }
                    break;
                case 18:
                    if (row[i].Length < 80 || row[i].Trim() == "" || row[i] == null)
                    {
                        ApplyHospChange_obj.NewHospAddress = row[i];
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過80");
                    }
                    break;
                default:
                    break;
            }
            var rtnModel = new LogicRtnModel<HospContractQueryModel>();
            rtnModel.Data = hospContractQueryModel;
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

        #endregion

        #region 戒菸服務專案申請

        /// <summary>
        /// 上傳戒菸服務excel
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<HospContractQueryModel>> UploadQuotaHosp(IFormFile formFile)
        {
            LogicRtnModel<HospContractQueryModel> logicRtnModel = new LogicRtnModel<HospContractQueryModel>();
            string success_Msg = string.Empty;
            int all_Count = 0;
            #region 戒菸服務專案申請
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                ExcelReader excelReader = new ExcelReader();
                List<List<string>> content = excelReader.ReadAsLIstOfList(ms, "戒菸服務專案申請");
                all_Count = 2;
                bool foundSecond = false;
                using (var txn = context.GetTransactionScope())
                {
                    foreach (List<string> row in content)
                    {
                        try
                        {
                            if (content.First() == row)
                            {
                                logicRtnModel = CheckExcelHeader_QuotaHosp(row);
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
                                continue;
                            }
                            string rowdata = string.Empty;
                            QuotaHosp obj = new QuotaHosp();
                            for (int i = 0; i < 21; i++)
                            {
                                logicRtnModel = FillObjByIndex_QuotaHosp(obj, i, row, content.IndexOf(row), "戒菸服務專案申請", all_Count);
                                if (!logicRtnModel.IsSuccess)
                                {
                                    return logicRtnModel;
                                }
                            }
                            obj.UpdatedBy = identity.Account;
                            if (!context.QuotaHosp.Any(x => x.Serial_Number == obj.Serial_Number))
                            {
                                context.QuotaHosp.Add(obj);
                            }
                            else
                            {
                                context.QuotaHosp.Update(obj);
                            }
                            all_Count++;
                        }
                        catch (Exception e)
                        {
                            logicRtnModel.IsSuccess = false;
                            logicRtnModel.Data = new HospContractQueryModel()
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
                        DataInsertLog dataInsertLog = new Services.DataInsertLogService().dataInsertLog($"戒菸服務專案申請 - {formFile.FileName}", all_Count - 2);
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
            logicRtnModel.Msg += "戒菸服務專案申請，共寫入" + (all_Count - 2) + "筆資料";
            success_Msg = logicRtnModel.Msg;

            #endregion


            logicRtnModel.IsSuccess = true;
            return logicRtnModel;
        }

        private LogicRtnModel<HospContractQueryModel> CheckExcelHeader_QuotaHosp(List<string> row)
        {
            HospContractQueryModel hospContractQueryModel = new HospContractQueryModel()
            {
                err = new List<string>()
            };
            string[] header = { "配額年度", "收件年月", "編號", "收件日\n(格式：YYYMMDD)", "院所代碼", "院區別", "院所名稱", "層級別", "申請戒菸服務項目及人次", "", "", "", "專設戒菸服務門診\n(每月幾個診次_半日為1個診次)", "", "署審查結果", "", "VPN異動說明", "文號", "發文日", "申請項目", "掃描檔名" };
            if (row.Count != header.Length)
            {
                hospContractQueryModel.err.Add("戒菸服務專案申請，(欄位數量錯誤)標題列應該有" + header.Length + "欄:" + string.Join(",", header));
            }
            else
            {
                for (int i = 0; i < row.Count; i++)
                {
                    if (row[i].Trim() != header[i])
                    {
                        hospContractQueryModel.err.Add($"戒菸服務專案申請，標題列第{i + 1}({row[i].Trim()})欄名稱應該為{header[i]}");
                    }
                }
            }

            var rtnModel = new LogicRtnModel<HospContractQueryModel>();
            rtnModel.Data = hospContractQueryModel;
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

        private LogicRtnModel<HospContractQueryModel> FillObjByIndex_QuotaHosp(QuotaHosp obj, int i, List<string> row, int rownumber, string sheetName, int Total_number)
        {
            HospContractQueryModel hospContractQueryModel = new HospContractQueryModel()
            {
                err = new List<string>()
            };

            switch (i)
            {
                case 0:
                    if (row[i].Trim().Length != 3)
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyy，或月份異常");
                    }
                    else
                    {
                        obj.QuotaYear = row[i];
                    }
                    break;
                case 1:

                    if (row[i].Trim().Length != 5)
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMM，或月分異常");
                    }
                    else
                    {
                        obj.FeeYM = row[i];
                    }

                    break;
                case 2:
                    if (row[i].Trim().Length != 10)
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyy-MM-000，或長度異常");
                    }
                    else
                    {
                        obj.Serial_Number = row[i];
                    }
                    break;
                case 3:
                    if (row[i].Trim().Length == 7)
                    {
                        obj.FeeYMD = row[i].ToYYYYMMDDFromTaiwan();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMMdd，或月分、日期異常");
                    }
                    break;
                case 4:
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
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為10");
                    }
                    break;
                case 5:
                    if (row[i].Length == 2 || row[i].Length == 1)
                    {
                        obj.HospSeqNo = row[i].Length == 1 ? "0" + row[i] : row[i];
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為2");
                    }

                    break;
                case 6:
                    if (row[i].Trim().Length < 50)
                    {
                        obj.HospName = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過50");
                    }

                    break;
                case 7:
                    if (row[i].Trim().Length < 10)
                    {
                        obj.LastContType = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過10");
                    }
                    break;
                case 8:
                    if (row[i].Trim().Length == 1 || row[i] == "" || row[i] == null)
                    {
                        obj.ApplyTreat = row[i].Trim() == "是" ? true : false;
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過1");
                    }
                    break;
                case 9:
                    if (int.TryParse(row[i], out int result10) || row[i] == null || row[i] == "")
                    {
                        if (row[i] == null || row[i] == "")
                        {
                            obj.ApplyTreatPeople = null;
                        }
                        else
                        {
                            obj.ApplyTreatPeople = Int16.Parse(row[i]);
                        }
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為數字");
                    }
                    break;
                case 10:
                    if (row[i].Trim().Length == 1 || row[i] == "" || row[i] == null)
                    {
                        obj.ApplyHealthEdu = row[i].Trim() == "是" ? true : false;
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過1");
                    }
                    break;
                case 11:
                    if (int.TryParse(row[i], out int results12) || row[i] == null || row[i] == "")
                    {
                        if (row[i] == null || row[i] == "")
                        {
                            obj.ApplyHealthEduPeople = null;
                        }
                        else
                        {
                            obj.ApplyHealthEduPeople = Int16.Parse(row[i]);
                        }
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為數字");
                    }
                    break;
                case 12:
                    if (row[i].Trim().Length < 100)
                    {
                        obj.DesignedTreat = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過100");
                    }
                    break;
                case 13:
                    if (row[i].Trim().Length < 100)
                    {
                        obj.DesignedTreatHealthEdu = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過100");
                    }
                    break;
                case 14:
                    if (row[i].Trim().Length < 100)
                    {
                        obj.ResultTreat = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過100");
                    }
                    break;
                case 15:
                    if (row[i].Trim().Length < 100)
                    {
                        obj.ResultHealthEdu = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過100");
                    }
                    break;
                case 16:
                    if (row[i].Trim().Length < 500)
                    {
                        obj.VPNChangeNote = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過500");
                    }
                    break;
                case 17:
                    if (row[i].Trim().Length < 100)
                    {
                        obj.DocumentNumber = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過100");
                    }
                    break;
                case 18:
                    if (row[i].Trim().Length < 100)
                    {
                        obj.IssueDate = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過100");
                    }
                    break;
                case 19:
                    if (row[i].Trim().Length < 20)
                    {
                        obj.Treatment = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過20");
                    }
                    break;
                case 20:
                    if (row[i].Trim().Length < 100)
                    {
                        obj.ScanFileName = row[i].Trim();
                    }
                    else
                    {
                        hospContractQueryModel.err.Add($"{sheetName}，序號 : {Total_number}，第{i + 1}欄資料({row[i].Trim()})錯誤，長度超過100");
                    }
                    break;
                default:
                    break;
            }
            var rtnModel = new LogicRtnModel<HospContractQueryModel>();
            rtnModel.Data = hospContractQueryModel;
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



        #endregion

    }
}
