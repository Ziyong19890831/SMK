using Microsoft.EntityFrameworkCore;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Web.Extensions;
using SMK.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;
using Yozian.Extension;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class FieldTripRegisterService : GenericService
    {
        public FieldTripRegisterService(SMKWEBContext context, SessionManager smgr)
        : base(context, smgr)
        {
        }

        /// <summary>
        /// 實地訪查-登錄資料(VPN)-查詢
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<TreatmentViewModel>>> GetFieldTripRegisterVPNTreat(FieldTripQueryViewModel model)
        {
            #region 測試參數
            //model.HospID = "593106A646";
            //model.HospSeqNo = "00";
            //model.FuncStartDate = "1120101";
            //model.FuncEndDate = "1121231";
            #endregion
            var YearMonthStart = model.FuncStartDate?.ToYYYYMMDDFromTaiwan();
            var YearMonthEnd = model.FuncEndDate?.ToYYYYMMDDFromTaiwan();
            # region 1.篩選就醫日期起訖範圍之戒菸服務資料-治療
            var A = context.MhbtQsData
                    .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospId == model.HospID)
                    .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo)
                    .Where(x => x.CureType == "1")
                    .WhereWhen(!string.IsNullOrEmpty(YearMonthStart),
                               p => string.Compare(p.FuncDate, YearMonthStart) >= 0)
                    .WhereWhen(!string.IsNullOrEmpty(YearMonthEnd),
                               p => string.Compare(YearMonthEnd, p.FuncDate) >= 0)
                    .Select(x => new MhbtQsDataViewModel()
                    {
                        HospId = x.HospId,
                        HospSeqNo = x.HospSeqNo,
                        CureType = x.CureType,
                        ExamYear = x.ExamYear,
                        CureStage = x.CureStage,
                        ID = x.ID,
                        FuncDate = x.FuncDate,
                        Birthday = x.Birthday,
                        StringCureWeek = x.CureWeek.ToString(),
                    }).AsNoTracking().ToList();
            #endregion

            #region 2.戒菸藥品資料彙整 【產製報表前，先確定Row_ID序號最大值，20230101~最新：Row_ID=4】
            var queryB = context.MhbtQsCure
                    .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospID == model.HospID)
                    .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo)
                    .WhereWhen(!string.IsNullOrEmpty(YearMonthStart),
                                                  p => string.Compare(p.FuncDate, YearMonthStart) >= 0)
                    .WhereWhen(!string.IsNullOrEmpty(YearMonthEnd),
                                                  p => string.Compare(YearMonthEnd, p.FuncDate) >= 0)
                    .Select(x => new MhbtQsCureViewModel()
                    {
                        HospID = x.HospID,
                        HospSeqNo = x.HospSeqNo,
                        FuncDate = x.FuncDate,
                        ID = x.ID,
                        CureItem = x.CureItem,
                        CureNumString = x.CureNum.ToString(),
                    }).AsNoTracking().ToList();
            var B = queryB
                        .GroupBy(row => new { row.HospID, row.HospSeqNo, row.ID, row.FuncDate })
                        .SelectMany(g => g.OrderBy(row => row.CureItem)
                                           .Select((item, index) => new { Item = item, Index = index + 1 }))
                        .Select(x => new MhbtQsCureViewModel
                        {
                            HospID = x.Item.HospID,
                            HospSeqNo = x.Item.HospSeqNo,
                            ID = x.Item.ID,
                            FuncDate = x.Item.FuncDate,
                            CureItem = x.Item.CureItem,
                            CureNumString = x.Item.CureNumString,
                            Row_ID = x.Index
                        }).ToList();
            #endregion

            #region 2-1做藥品資料彙整分類
            var GenOrderCode = context.GenOrderCode.AsNoTracking().ToList();
            var B1 = B.Where(x => x.Row_ID == 1).ToList();
            var B2 = B.Where(x => x.Row_ID == 2).ToList();
            var B3 = B.Where(x => x.Row_ID == 3).ToList();
            var B4 = B.Where(x => x.Row_ID == 4).ToList();
            #endregion

            #region 3.彙整vpn治療清單
            var HospBasic = context.HospBasic
                            .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospId == model.HospID)
                            .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo)
                            .AsNoTracking().ToList();

            var MhbtAgentPatient = context.MhbtAgentPatient
                                .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospID == model.HospID)
                                .AsNoTracking().DistinctBy(x => x.ID).ToList()
                                //強制轉為大寫
                                .Select(x =>
                                {
                                    x.ID = x.ID.ToUpper();
                                    return x;
                                }).ToList();


            var query = from a in A
                            // Left Joining MhbtAgentPatient table
                        join m in MhbtAgentPatient on new { HospID = a.HospId, a.ID } equals new { m.HospID, m.ID } into mJoin
                        from m in mJoin.DefaultIfEmpty()
                            // Left Joining B1 table
                        join b1 in B1 on new { HospID = a.HospId, a.HospSeqNo, a.ID, a.FuncDate } equals new { b1.HospID, b1.HospSeqNo, b1.ID, b1.FuncDate } into b1Join
                        from b1 in b1Join.DefaultIfEmpty()
                            // Left Joining B2 table
                        join b2 in B2 on new { HospID = a.HospId, a.HospSeqNo, a.ID, a.FuncDate } equals new { b2.HospID, b2.HospSeqNo, b2.ID, b2.FuncDate } into b2Join
                        from b2 in b2Join.DefaultIfEmpty()
                            // Left Joining B3 table
                        join b3 in B3 on new { HospID = a.HospId, a.HospSeqNo, a.ID, a.FuncDate } equals new { b3.HospID, b3.HospSeqNo, b3.ID, b3.FuncDate } into b3Join
                        from b3 in b3Join.DefaultIfEmpty()
                            // Left Joining B4 table
                        join b4 in B4 on new { HospID = a.HospId, a.HospSeqNo, a.ID, a.FuncDate } equals new { b4.HospID, b4.HospSeqNo, b4.ID, b4.FuncDate } into b4Join
                        from b4 in b4Join.DefaultIfEmpty()
                            // Left Joining GenOrderCode table for each B table
                        join c1 in GenOrderCode on b1?.CureItem.Trim() equals c1.OrderCode into c1Join
                        from c1 in c1Join.DefaultIfEmpty()
                        join c2 in GenOrderCode on b2?.CureItem.Trim() equals c2.OrderCode into c2Join
                        from c2 in c2Join.DefaultIfEmpty()
                        join c3 in GenOrderCode on b3?.CureItem.Trim() equals c3.OrderCode into c3Join
                        from c3 in c3Join.DefaultIfEmpty()
                        join c4 in GenOrderCode on b4?.CureItem.Trim() equals c4.OrderCode into c4Join
                        from c4 in c4Join.DefaultIfEmpty()
                            // Left Joining HospBASic table
                        join d in HospBasic on new { a.HospId, a.HospSeqNo } equals new { d?.HospId, d?.HospSeqNo } into dJoin
                        from d in dJoin.DefaultIfEmpty()
                        select new TreatmentViewModel
                        {
                            DataType = "治療",
                            HospId = a?.HospId.TrimStart().TrimEnd(),
                            HospSeqNo = a?.HospSeqNo.TrimStart().TrimEnd(),
                            HospName = d?.HospName.TrimStart().TrimEnd(),
                            ID = a?.ID.TrimStart().TrimEnd(),
                            Birthday = a != null ? a.Birthday.TrimStart().TrimEnd() != null ? ((DateTime)a.Birthday.ToDateTime()).ToString("yyyy/MM/dd") : null : null,
                            Name = m?.Name.TrimStart().TrimEnd(),
                            Sex = m?.Sex.TrimStart().TrimEnd(),
                            TelD = m?.TelD.TrimStart().TrimEnd(),
                            TelM = m?.TelM.TrimStart().TrimEnd(),
                            TownName = m?.TownName.TrimStart().TrimEnd(),
                            InformADDR = m?.InformADDR.TrimStart().TrimEnd(),
                            ExamYear = a?.ExamYear.TrimStart().TrimEnd(),
                            CureStage = a?.CureStage.TrimStart().TrimEnd(),
                            CureWeek = a?.StringCureWeek.TrimStart().TrimEnd(),
                            FuncDate = a != null ? a.FuncDate.TrimStart().TrimEnd() != null ? ((DateTime)a.FuncDate.ToDateTime()).ToString("yyyy/MM/dd") : null : null,
                            CureItem1 = b1?.CureItem.TrimStart().TrimEnd(),
                            OrderChiName1 = c1?.OrderChiName.TrimStart().TrimEnd(),
                            CureNum1 = b1?.CureNumString.TrimStart().TrimEnd(),
                            CureItem2 = b2?.CureItem.TrimStart().TrimEnd(),
                            OrderChiName2 = c2?.OrderChiName.TrimStart().TrimEnd(),
                            CureNum2 = b2?.CureNumString.TrimStart().TrimEnd(),
                            CureItem3 = b3?.CureItem.TrimStart().TrimEnd(),
                            OrderChiName3 = c3?.OrderChiName.TrimStart().TrimEnd(),
                            CureNum3 = b3?.CureNumString.TrimStart().TrimEnd(),
                            CureItem4 = b4?.CureItem.TrimStart().TrimEnd(),
                            OrderChiName4 = c4?.OrderChiName.TrimStart().TrimEnd(),
                            CureNum4 = b4?.CureNumString.TrimStart().TrimEnd()
                        };
            query = query.AsQueryable()
                    .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospId == model.HospID)
                    .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo)
                    .Distinct()
                    .OrderBy(x => x.ID).ThenBy(x => x.FuncDate);
            #endregion
            var pagedData = PagedModel<TreatmentViewModel>.Create(query.AsQueryable(), model.get());
            pagedData.RecordsTotal = query.Count();
            return new LogicRtnModel<PagedModel<TreatmentViewModel>>()
            {
                IsSuccess = true,
                Data = pagedData,
            };
        }



        /// <summary>
        /// 實地訪查-登錄資料(VPN)-衛教
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<HealthMentViewModel>>> GetFieldTripRegisterVPNHealth(FieldTripQueryViewModel model)
        {
            #region 測試參數
            //model.HospID = "593106A646";
            //model.HospSeqNo = "00";
            //model.FuncStartDate = "1120101";
            //model.FuncEndDate = "1121231";
            #endregion
            var YearMonthStart = model.FuncStartDate?.ToYYYYMMDDFromTaiwan();
            var YearMonthEnd = model.FuncEndDate?.ToYYYYMMDDFromTaiwan();
            # region 1.篩選就醫日期起訖範圍之戒菸服務資料-衛教
            var A = context.MhbtQsData
                    .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospId == model.HospID)
                    .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo)
                    .Where(x => x.CureType == "2")
                    .WhereWhen(!string.IsNullOrEmpty(YearMonthStart),
                               p => string.Compare(p.FuncDate, YearMonthStart) >= 0)
                    .WhereWhen(!string.IsNullOrEmpty(YearMonthEnd),
                               p => string.Compare(YearMonthEnd, p.FuncDate) >= 0)
                    .Select(x => new MhbtQsDataViewModel()
                    {
                        HospId = x.HospId,
                        HospSeqNo = x.HospSeqNo,
                        CureType = x.CureType,
                        ExamYear = x.ExamYear,
                        CureStage = x.CureStage,
                        ID = x.ID,
                        FuncDate = x.FuncDate,
                        Birthday = x.Birthday,
                        StringCureWeek = x.CureWeek.ToString(),
                    }).AsNoTracking().ToList();
            #endregion

            #region 彙整vpn衛教清單

            var HospBasic = context.HospBasic
                .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospId == model.HospID)
                .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo)
                .AsNoTracking().ToList();

            var MhbtAgentPatient = context.MhbtAgentPatient
                                .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospID == model.HospID)
                                .AsNoTracking().DistinctBy(x => x.ID).ToList()
                                //強制轉為大寫
                                .Select(x =>
                                {
                                    x.ID = x.ID.ToUpper();
                                    return x;
                                }).ToList();
            var query = A
                        .GroupJoin(
                           MhbtAgentPatient,
                            a => new { HospID = a.HospId, a.ID },
                            m => new { m.HospID, m.ID },
                            (a, mj) => new { a, mj }
                        )
                        .SelectMany(
                            x => x.mj.DefaultIfEmpty(),
                            (x, m) => new { x.a, m }
                        )
                        .GroupJoin(
                            HospBasic,
                            x => new { x.a.HospId, x.a.HospSeqNo },
                            d => new { d?.HospId, d?.HospSeqNo },
                            (x, dj) => new { x.a, x.m, dj }
                        )
                        .SelectMany(
                            x => x.dj.DefaultIfEmpty(),
                            (x, d) => new HealthMentViewModel
                            {
                                DataType = "衛教",
                                HospId = x.a?.HospId.TrimStart().TrimEnd(),
                                HospSeqNo = x.a?.HospSeqNo.TrimStart().TrimEnd(),
                                HospName = d?.HospName.TrimStart().TrimEnd(),
                                ID = x.a?.ID.TrimStart().TrimEnd(),
                                Birthday = x.a?.Birthday.TrimStart().TrimEnd() != null ? ((DateTime)x.a.Birthday.ToDateTime()).ToString("yyyy/MM/dd") : null,
                                Name = x.m?.Name.TrimStart().TrimEnd(),
                                Sex = x.m?.Sex.TrimStart().TrimEnd() == "M" ? "男" : "女",
                                TelD = x.m?.TelD.TrimStart().TrimEnd(),
                                TelM = x.m?.TelM.TrimStart().TrimEnd(),
                                TownName = x.m?.TownName.TrimStart().TrimEnd(),
                                InformADDR = x.m?.InformADDR.TrimStart().TrimEnd(),
                                ExamYear = x.a?.ExamYear.TrimStart().TrimEnd(),
                                FuncDate = x.a?.FuncDate.TrimStart().TrimEnd() != null ? ((DateTime)x.a.FuncDate.ToDateTime()).ToString("yyyy/MM/dd") : null,
                                CureStage = x.a?.CureStage.TrimStart().TrimEnd(),
                                CureWeek = x.a?.StringCureWeek.TrimStart().TrimEnd(),
                            }
                        );
            query = query.AsQueryable()
                    .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospId == model.HospID)
                    .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo)
                    .Distinct()
                    .OrderBy(x => x.ID).ThenBy(x => x.FuncDate).ToList();
            #endregion

            var pagedData = PagedModel<HealthMentViewModel>.Create(query.AsQueryable(), model.get());
            pagedData.RecordsTotal = query.Count();
            return new LogicRtnModel<PagedModel<HealthMentViewModel>>()
            {
                IsSuccess = true,
                Data = pagedData,
            };
        }

        /// <summary>
        /// VPN治療字典初始化
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Tuple<string, bool>> DictionaryTreatData()
        {
            Dictionary<string, Tuple<string, bool>> DictionaryForTreat = new Dictionary<string, Tuple<string, bool>>()
            {
                {"類型", new Tuple<string, bool>("DataType", true)},
                {"醫事機構代碼", new Tuple<string, bool>("HospId", true)},
                {"院區別", new Tuple<string, bool>("HospSeqNo", true)},
                {"醫事機構名稱", new Tuple<string, bool>("HospName", true)},
                {"身分證號", new Tuple<string, bool>("ID", true)},
                {"出生日期", new Tuple<string, bool>("Birthday", true)},
                {"姓名", new Tuple<string, bool>("Name", true)},
                {"性別", new Tuple<string, bool>("Sex", true)},
                {"電話(日)", new Tuple<string, bool>("TelD", true)},
                {"手機", new Tuple<string, bool>("TelM", true)},
                {"縣市鄉鎮", new Tuple<string, bool>("TownName", true)},
                {"通訊地址", new Tuple<string, bool>("InformADDR", true)},
                {"療程年度", new Tuple<string, bool>("ExamYear", true)},
                {"療程次數", new Tuple<string, bool>("CureStage", true)},
                {"用藥週數", new Tuple<string, bool>("CureWeek", true)},
                {"就醫日期", new Tuple<string, bool>("FuncDate", true)},
                {"醫令代碼1", new Tuple<string, bool>("CureItem1", true)},
                {"代碼名稱1(處方藥名)", new Tuple<string, bool>("OrderChiName1", true)},
                {"量1(藥量)", new Tuple<string, bool>("CureNum1", true)},
                {"醫令代碼2", new Tuple<string, bool>("CureItem2", true)},
                {"代碼名稱2(處方藥名)", new Tuple<string, bool>("OrderChiName2", true)},
                {"量2(藥量)", new Tuple<string, bool>("CureNum2", true)},
                {"醫令代碼3", new Tuple<string, bool>("CureItem3", true)},
                {"代碼名稱3(處方藥名)", new Tuple<string, bool>("OrderChiName3", true)},
                {"量3(藥量)", new Tuple<string, bool>("CureNum3", true)},
                {"醫令代碼4", new Tuple<string, bool>("CureItem4", true)},
                {"代碼名稱4(處方藥名)", new Tuple<string, bool>("OrderChiName4", true)},
                {"量4(藥量)", new Tuple<string, bool>("CureNum4", true)},
            };
            return DictionaryForTreat;
        }
        /// <summary>
        /// VPN衛教字典初始化
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Tuple<string, bool>> DictionaryHealthData()
        {
            Dictionary<string, Tuple<string, bool>> DictionaryForTreat = new Dictionary<string, Tuple<string, bool>>()
            {
                {"類型", new Tuple<string, bool>("DataType", true)},
                {"醫事機構代碼", new Tuple<string, bool>("HospId", true)},
                {"院區別", new Tuple<string, bool>("HospSeqNo", true)},
                {"醫事機構名稱", new Tuple<string, bool>("HospName", true)},
                {"身分證號", new Tuple<string, bool>("ID", true)},
                {"出生日期", new Tuple<string, bool>("Birthday", true)},
                {"姓名", new Tuple<string, bool>("Name", true)},
                {"性別", new Tuple<string, bool>("Sex", true)},
                {"電話(日)", new Tuple<string, bool>("TelD", true)},
                {"手機", new Tuple<string, bool>("TelM", true)},
                {"縣市鄉鎮", new Tuple<string, bool>("TownName", true)},
                {"通訊地址", new Tuple<string, bool>("InformADDR", true)},
                {"療程年度", new Tuple<string, bool>("ExamYear", true)},
                {"療程次數", new Tuple<string, bool>("CureStage", true)},
                {"用藥週數", new Tuple<string, bool>("CureWeek", true)},
                {"就醫日期", new Tuple<string, bool>("FuncDate", true)},
            };
            return DictionaryForTreat;
        }
    }
}
