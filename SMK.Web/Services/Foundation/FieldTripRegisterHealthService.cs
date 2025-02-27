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
    public class FieldTripRegisterHealthService : GenericService
    {
        public FieldTripRegisterHealthService(SMKWEBContext context, SessionManager smgr)
        : base(context, smgr)
        {
        }

        /// <summary>
        /// 實地訪查-健保資料-查詢
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<FieldTripRegisteHealthQueryViewModel>>> GetFieldTripRegisterHealthTreat(FieldTripQueryViewModel model)
        {
            #region 測試參數
            //model.HospID = "593106A646";
            //model.HospSeqNo = "00";
            //model.FuncStartDate = "1110101";
            //model.FuncEndDate = "1111231";
            #endregion
            var YearMonthStart = model.FuncStartDate?.ToYYYYMMDDFromTaiwan();
            var YearMonthEnd = model.FuncEndDate?.ToYYYYMMDDFromTaiwan();
            # region 1.篩選費用年月範圍之戒菸治療健保資料 (MedApply='1')
            List<FieldTripRegisterIniOpDtlViewModel> A = new List<FieldTripRegisterIniOpDtlViewModel>();

            var AIniOpDtl = context.IniOpDtl
                        .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospId == model.HospID)
                        .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo)
                        .WhereWhen(!string.IsNullOrEmpty(YearMonthStart),
                                   p => string.Compare(p.FuncDate, YearMonthStart) >= 0)
                        .WhereWhen(!string.IsNullOrEmpty(YearMonthEnd),
                                   p => string.Compare(YearMonthEnd, p.FuncDate) >= 0)
                        .Where(x => x.MedApply == "1")
                        .Select(d => new FieldTripRegisterIniOpDtlViewModel()
                        {
                            HospId = (new List<string> { "1101100011", "1132070011", "1517061032" }.Contains(d.HospId) ? d.RealHospId : d.HospId),
                            HospSeqNo = (string.Compare("20150101", d.FuncDate) >= 0 && d.HospId.StartsWith("0101090517") ?
                                            (d.SeqNo <= 100000 ? "10" : ((int)d.SeqNo).ToString("0000000").Substring(0, 2)) : "00"),
                            DTL = d.DataId.TrimStart().TrimEnd() + d.FeeYm.TrimStart().TrimEnd(),
                            DataId = d.DataId.TrimStart().TrimEnd(),
                            FeeYm = d.FeeYm.TrimStart().TrimEnd(),
                            ExamYear = d.ExamYear.TrimStart().TrimEnd(),
                            ExamTime = d.ExamTime,
                            MedApply = d.MedApply.TrimStart().TrimEnd(),
                            InstructApply = d.InstructApply.TrimStart().TrimEnd(),
                            TraceApply = d.TraceApply.TrimStart().TrimEnd(),
                            ReleaseApply = d.ReleaseApply.TrimStart().TrimEnd(),
                            CaseType = d.CaseType.TrimStart().TrimEnd(),
                            HospDataType = d.HospDataType.TrimStart().TrimEnd(),
                            SeqNo = d.SeqNo,
                            FuncType = d.FuncType.TrimStart().TrimEnd(),
                            FuncDate = d.FuncDate.TrimStart().TrimEnd(),
                            Birthday = d.Birthday.TrimStart().TrimEnd(),
                            Id = d.Id.TrimStart().TrimEnd(),
                            WeekCount = d.WeekCount,
                            DrugDays = d.DrugDays,
                            ApplDot = d.ApplDot,
                            Name = d.Name.TrimStart().TrimEnd(),
                            ExpDot = d.ExpDot,
                            PartAmt = d.PartAmt,
                            ApplDate = d.ApplDate.TrimStart().TrimEnd()
                        })
                    .Distinct().AsNoTracking().ToList();

            var AIniDrDtl = context.IniDrDtl
                     .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospId == model.HospID)
                     .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo)
                     .WhereWhen(!string.IsNullOrEmpty(YearMonthStart),
                             p => string.Compare(p.FuncDate, YearMonthStart) >= 0)
                     .WhereWhen(!string.IsNullOrEmpty(YearMonthEnd),
                                 p => string.Compare(YearMonthEnd, p.FuncDate) >= 0)
                     .Where(x => x.MedApply == "1")
                     .Select(d => new FieldTripRegisterIniOpDtlViewModel()
                     {
                         HospId = d.HospId.TrimStart().TrimEnd(),
                         HospSeqNo = "00",
                         DTL = d.DataId.TrimStart().TrimEnd() + d.FeeYm.TrimStart().TrimEnd(),
                         DataId = d.DataId.TrimStart().TrimEnd(),
                         FeeYm = d.FeeYm.TrimStart().TrimEnd(),
                         ExamYear = d.ExamYear.TrimStart().TrimEnd(),
                         ExamTime = d.ExamTime,
                         MedApply = d.MedApply.TrimStart().TrimEnd(),
                         InstructApply = d.InstructApply.TrimStart().TrimEnd(),
                         TraceApply = d.TraceApply.TrimStart().TrimEnd(),
                         ReleaseApply = d.ReleaseApply.TrimStart().TrimEnd(),
                         CaseType = d.CaseType.TrimStart().TrimEnd(),
                         HospDataType = "30",
                         SeqNo = d.SeqNo,
                         FuncType = d.FuncType.TrimStart().TrimEnd(),
                         FuncDate = d.FuncDate.TrimStart().TrimEnd(),
                         Birthday = d.Birthday.TrimStart().TrimEnd(),
                         Id = d.Id.TrimStart().TrimEnd(),
                         WeekCount = d.WeekCount,
                         DrugDays = d.DrugDays,
                         ApplDot = d.ApplDot,
                         Name = d.Name.TrimStart().TrimEnd(),
                         ExpDot = d.ExpDot,
                         PartAmt = d.PartAmt,
                         ApplDate = d.ApplDate.TrimStart().TrimEnd()
                     })
                     .Distinct().AsNoTracking().ToList();
            A = AIniOpDtl.Union(AIniDrDtl).ToList();
            #endregion

            #region 2.Ord 戒菸藥品資料彙整 【產製報表前，先確定Row_ID序號最大值，20230101~最新：Row_ID=4】
            List<FieldTripRegisteIniDrOrdViewModel> B0 = new List<FieldTripRegisteIniDrOrdViewModel>();

            var BIniDrOrd = context.IniDrOrd
                .WhereWhen(!string.IsNullOrEmpty(YearMonthStart),
                            p => string.Compare(p.FeeYm, YearMonthStart.Substring(0, 6)) >= 0)
                .WhereWhen(!string.IsNullOrEmpty(YearMonthEnd),
                            p => string.Compare(YearMonthEnd.Substring(0, 6), p.FeeYm) >= 0)
                    .Where(o => o.OrderCode.StartsWith("A") || o.OrderCode.StartsWith("B"))
                .Select(o => new FieldTripRegisteIniDrOrdViewModel()
                {
                    DataId = o.DataId,
                    FeeYm = o.FeeYm,
                    OrderSeqNo = o.OrderSeqNo,
                    OrderCode = o.OrderCode,
                    OrderQty = o.OrderQty,
                    OrderDot = o.OrderDot
                }).AsNoTracking().ToList();

            var BIniOpOrd = context.IniOpOrd
                    .WhereWhen(!string.IsNullOrEmpty(YearMonthStart),
                                p => string.Compare(p.FeeYm, YearMonthStart.Substring(0, 6)) >= 0)
                    .WhereWhen(!string.IsNullOrEmpty(YearMonthEnd),
                                p => string.Compare(YearMonthEnd.Substring(0, 6), p.FeeYm) >= 0)
                    .Where(o => o.OrderCode.StartsWith("A") || o.OrderCode.StartsWith("B"))
                    .Select(o => new FieldTripRegisteIniDrOrdViewModel()
                    {
                        DataId = o.DataId,
                        FeeYm = o.FeeYm,
                        OrderSeqNo = o.OrderSeqNo,
                        OrderCode = o.OrderCode,
                        OrderQty = o.OrderQty,
                        OrderDot = o.OrderDot
                    }).AsNoTracking().ToList();
            B0 = BIniDrOrd.Union(BIniOpOrd).ToList();
            B0 = B0
                .GroupBy(row => new { row.DataId, row.FeeYm })
                .SelectMany(g => g.OrderBy(row => row.OrderSeqNo)
                                    .Select((item, index) => new { Item = item, Index = index + 1 }))
                .Select(o => new FieldTripRegisteIniDrOrdViewModel()
                {
                    DataId = o.Item.DataId,
                    FeeYm = o.Item.FeeYm,
                    OrderSeqNo = o.Item.OrderSeqNo,
                    OrderCode = o.Item.OrderCode,
                    OrderQty = o.Item.OrderQty,
                    OrderDot = o.Item.OrderDot,
                    Row_ID = o.Index
                }).ToList();
            #endregion

            #region 2-1做藥品資料彙整分類
            var B1 = B0.Where(x => x.Row_ID == 1).ToList();
            var B2 = B0.Where(x => x.Row_ID == 2).ToList();
            var B3 = B0.Where(x => x.Row_ID == 3).ToList();
            var B4 = B0.Where(x => x.Row_ID == 4).ToList();
            #endregion

            #region 3.彙整vpn治療清單
            var GenOrderCode = context.GenOrderCode.AsNoTracking().ToList();
            var HospBasic = context.HospBasic
                            .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospId == model.HospID)
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

            var result = from a in A
                         join m in MhbtAgentPatient on new { HospID = a.HospId, ID = a.Id } equals new { m.HospID, m.ID } into mJoin
                         from m in mJoin.DefaultIfEmpty()
                         join b1 in B1 on new { a.FeeYm, a.DataId } equals new { b1.FeeYm, b1.DataId } into b1Join
                         from b1 in b1Join.DefaultIfEmpty()
                         join c1 in GenOrderCode on b1?.OrderCode.Trim() equals c1.OrderCode into c1Join
                         from c1 in c1Join.DefaultIfEmpty()
                         join b2 in B2 on new { a.FeeYm, a.DataId } equals new { b2.FeeYm, b2.DataId } into b2Join
                         from b2 in b2Join.DefaultIfEmpty()
                         join c2 in GenOrderCode on b2?.OrderCode.Trim() equals c2.OrderCode into c2Join
                         from c2 in c2Join.DefaultIfEmpty()
                         join b3 in B3 on new { a.FeeYm, a.DataId } equals new { b3.FeeYm, b3.DataId } into b3Join
                         from b3 in b3Join.DefaultIfEmpty()
                         join c3 in GenOrderCode on b3?.OrderCode.Trim() equals c3.OrderCode into c3Join
                         from c3 in c3Join.DefaultIfEmpty()
                         join b4 in B4 on new { a.FeeYm, a.DataId } equals new { b4.FeeYm, b4.DataId } into b4Join
                         from b4 in b4Join.DefaultIfEmpty()
                         join c4 in GenOrderCode on b4?.OrderCode.Trim() equals c4.OrderCode into c4Join
                         from c4 in c4Join.DefaultIfEmpty()
                         join d in HospBasic on new { HospID = a.HospId, a.HospSeqNo } equals new { HospID = d?.HospId, d?.HospSeqNo } into dJoin
                         from d in dJoin.DefaultIfEmpty()
                         select new FieldTripRegisteHealthQueryViewModel()
                         {
                             DataType = "治療",
                             HospId = a.HospId.TrimStart().TrimEnd(),
                             HospSeqNo = a.HospSeqNo.TrimStart().TrimEnd(),
                             HospName = d != null ? d.HospName.TrimStart().TrimEnd() : null,
                             Id = a.Id.TrimStart().TrimEnd(),
                             Birthday = a.Birthday.TrimStart().TrimEnd() != null ? ((DateTime)a.Birthday.ToDateTime()).ToString("yyyy/MM/dd") : null,
                             Name = a.Name.TrimStart().TrimEnd(),
                             Sex = m != null ? (m.Sex.TrimStart().TrimEnd() == "M" ? "男" : "女") : null,
                             TelD = m != null ? m.TelD.TrimStart().TrimEnd() : null,
                             TelM = m != null ? m.TelM.TrimStart().TrimEnd() : null,
                             TownName = m != null ? m.TownName.TrimStart().TrimEnd() : null,
                             InformADDR = m != null ? m.InformADDR.TrimStart().TrimEnd() : null,
                             ExamYear = a.ExamYear.TrimStart().TrimEnd(),
                             WeekCount = a.WeekCount.ToString(),
                             FeeYm = a.FeeYm.TrimStart().TrimEnd().ToSlashTaiwanDateFromYYYYMM(),
                             DataId = a.DataId.TrimStart().TrimEnd(),
                             FuncDate = a.FuncDate.TrimStart().TrimEnd() != null ? ((DateTime)a.FuncDate.ToDateTime()).ToString("yyyy/MM/dd") : null,
                             ApplDate = a.ApplDate.TrimStart().TrimEnd() != null ? ((DateTime)a.ApplDate.ToDateTime()).ToString("yyyy/MM/dd") : null,
                             Order_code1 = b1 != null ? b1.OrderCode.TrimStart().TrimEnd() : null,
                             OrderChiName1 = c1 != null ? c1.OrderChiName.TrimStart().TrimEnd() : null,
                             Order_qty1 = b1 != null ? b1.OrderQty.ToString() : null,
                             Order_code2 = b2 != null ? b2.OrderCode.TrimStart().TrimEnd() : null,
                             OrderChiName2 = c2 != null ? c2.OrderChiName.TrimStart().TrimEnd() : null,
                             Order_qty2 = b2 != null ? b2.OrderQty.ToString() : null,
                             Order_code3 = b3 != null ? b3.OrderCode.TrimStart().TrimEnd() : null,
                             OrderChiName3 = c3 != null ? c3.OrderChiName.TrimStart().TrimEnd() : null,
                             Order_qty3 = b3 != null ? b3.OrderQty.ToString() : null,
                             Order_code4 = b4 != null ? b4.OrderCode.TrimStart().TrimEnd() : null,
                             OrderChiName4 = c4 != null ? c4.OrderChiName.TrimStart().TrimEnd() : null,
                             Order_qty4 = b4 != null ? b4.OrderQty.ToString() : null
                         };
            result = result.AsQueryable()
                    .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospId == model.HospID)
                    .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo)
                    .Distinct()
                    .OrderBy(x => x.Id).ThenBy(x => x.FuncDate).ToList();

            #endregion
            var pagedData = PagedModel<FieldTripRegisteHealthQueryViewModel>.Create(result.AsQueryable(), model.get());
            pagedData.RecordsTotal = result.Count();
            return new LogicRtnModel<PagedModel<FieldTripRegisteHealthQueryViewModel>>()
            {
                IsSuccess = true,
                Data = pagedData,
            };
        }



        /// <summary>
        /// 實地訪查-健保資料-衛教
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<FieldTripRegisteHealthQueryViewModel>>> GetFieldTripRegisterHealthCareHealth(FieldTripQueryViewModel model)
        {
            #region 測試參數
            //model.HospID = "593106A646";
            //model.HospSeqNo = "00";
            //model.FuncStartDate = "1110101";
            //model.FuncEndDate = "1121231";
            #endregion
            var YearMonthStart = model.FuncStartDate?.ToYYYYMMDDFromTaiwan();
            var YearMonthEnd = model.FuncEndDate?.ToYYYYMMDDFromTaiwan();

            #region 1.篩選費用年月範圍之戒菸治療健保資料 (InstructApply='1')
            List<FieldTripRegisterIniOpDtlViewModel> A = new List<FieldTripRegisterIniOpDtlViewModel>();

            var AIniOpDtl = context.IniOpDtl
                        .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospId == model.HospID)
                        .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo)
                        .WhereWhen(!string.IsNullOrEmpty(YearMonthStart),
                                   p => string.Compare(p.FuncDate, YearMonthStart) >= 0)
                        .WhereWhen(!string.IsNullOrEmpty(YearMonthEnd),
                                   p => string.Compare(YearMonthEnd, p.FuncDate) >= 0)
                        .Where(x => x.InstructApply == "1")
                        .Select(d => new FieldTripRegisterIniOpDtlViewModel()
                        {
                            HospId = (new List<string> { "1101100011", "1132070011", "1517061032" }.Contains(d.HospId) ? d.RealHospId : d.HospId),
                            HospSeqNo = (string.Compare("20150101", d.FuncDate) >= 0 && d.HospId.StartsWith("0101090517") ?
                                            (d.SeqNo <= 100000 ? "10" : ((int)d.SeqNo).ToString("0000000").Substring(0, 2)) : "00"),
                            DTL = d.DataId.TrimStart().TrimEnd() + d.FeeYm.TrimStart().TrimEnd(),
                            DataId = d.DataId.TrimStart().TrimEnd(),
                            FeeYm = d.FeeYm.TrimStart().TrimEnd(),
                            ExamYear = d.ExamYear.TrimStart().TrimEnd(),
                            ExamTime = d.ExamTime,
                            MedApply = d.MedApply.TrimStart().TrimEnd(),
                            InstructApply = d.InstructApply.TrimStart().TrimEnd(),
                            InctructSerial = d.InctructSerial,
                            TraceApply = d.TraceApply.TrimStart().TrimEnd(),
                            ReleaseApply = d.ReleaseApply.TrimStart().TrimEnd(),
                            CaseType = d.CaseType.TrimStart().TrimEnd(),
                            HospDataType = d.HospDataType.TrimStart().TrimEnd(),
                            SeqNo = d.SeqNo,
                            FuncType = d.FuncType.TrimStart().TrimEnd(),
                            FuncDate = d.FuncDate.TrimStart().TrimEnd(),
                            Birthday = d.Birthday.TrimStart().TrimEnd(),
                            Id = d.Id.TrimStart().TrimEnd(),
                            WeekCount = d.WeekCount,
                            DrugDays = d.DrugDays,
                            ApplDot = d.ApplDot,
                            Name = d.Name.TrimStart().TrimEnd(),
                            ExpDot = d.ExpDot,
                            PartAmt = d.PartAmt,
                            ApplDate = d.ApplDate.TrimStart().TrimEnd()
                        })
                    .Distinct().AsNoTracking().ToList();

            var AIniDrDtl = context.IniDrDtl
                     .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospId == model.HospID)
                     .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo)
                     .WhereWhen(!string.IsNullOrEmpty(YearMonthStart),
                             p => string.Compare(p.FuncDate, YearMonthStart) >= 0)
                     .WhereWhen(!string.IsNullOrEmpty(YearMonthEnd),
                                 p => string.Compare(YearMonthEnd, p.FuncDate) >= 0)
                     .Where(x => x.InstructApply == "1")
                     .Select(d => new FieldTripRegisterIniOpDtlViewModel()
                     {
                         HospId = d.HospId.TrimStart().TrimEnd(),
                         HospSeqNo = "00",
                         DTL = d.DataId.TrimStart().TrimEnd() + d.FeeYm.TrimStart().TrimEnd(),
                         DataId = d.DataId.TrimStart().TrimEnd(),
                         FeeYm = d.FeeYm.TrimStart().TrimEnd(),
                         ExamYear = d.ExamYear.TrimStart().TrimEnd(),
                         ExamTime = d.ExamTime,
                         MedApply = d.MedApply.TrimStart().TrimEnd(),
                         InstructApply = d.InstructApply.TrimStart().TrimEnd(),
                         InctructSerial = d.InctructSerial,
                         TraceApply = d.TraceApply.TrimStart().TrimEnd(),
                         ReleaseApply = d.ReleaseApply.TrimStart().TrimEnd(),
                         CaseType = d.CaseType.TrimStart().TrimEnd(),
                         HospDataType = "30",
                         SeqNo = d.SeqNo,
                         FuncType = d.FuncType.TrimStart().TrimEnd(),
                         FuncDate = d.FuncDate.TrimStart().TrimEnd(),
                         Birthday = d.Birthday.TrimStart().TrimEnd(),
                         Id = d.Id.TrimStart().TrimEnd(),
                         WeekCount = d.WeekCount,
                         DrugDays = d.DrugDays,
                         ApplDot = d.ApplDot,
                         Name = d.Name.TrimStart().TrimEnd(),
                         ExpDot = d.ExpDot,
                         PartAmt = d.PartAmt,
                         ApplDate = d.ApplDate.TrimStart().TrimEnd()
                     })
                     .Distinct().AsNoTracking().ToList();
            A = AIniOpDtl.Union(AIniDrDtl).ToList();
            #endregion

            #region 2.0rd 衛教代碼申報彙整 【產製報表前，先確定Row_ID序號最大值，20230101~最新：Row_ID=2】
            List<FieldTripRegisteIniDrOrdViewModel> B0 = new List<FieldTripRegisteIniDrOrdViewModel>();

            var BIniDrOrd = context.IniDrOrd
                .WhereWhen(!string.IsNullOrEmpty(YearMonthStart),
                            p => string.Compare(p.FeeYm, YearMonthStart.Substring(0, 6)) >= 0)
                .WhereWhen(!string.IsNullOrEmpty(YearMonthEnd),
                            p => string.Compare(YearMonthEnd.Substring(0, 6), p.FeeYm) >= 0)
                .Where(o => o.OrderCode == "E1022C")
                .Select(o => new FieldTripRegisteIniDrOrdViewModel()
                {
                    DataId = o.DataId,
                    FeeYm = o.FeeYm,
                    OrderSeqNo = o.OrderSeqNo,
                    OrderCode = o.OrderCode,
                    OrderQty = o.OrderQty,
                    OrderDot = o.OrderDot
                }).AsNoTracking().ToList();

            var BIniOpOrd = context.IniOpOrd
                    .WhereWhen(!string.IsNullOrEmpty(YearMonthStart),
                                p => string.Compare(p.FeeYm, YearMonthStart.Substring(0, 6)) >= 0)
                    .WhereWhen(!string.IsNullOrEmpty(YearMonthEnd),
                                p => string.Compare(YearMonthEnd.Substring(0, 6), p.FeeYm) >= 0)
                    .Where(o => o.OrderCode == "E1022C")
                    .Select(o => new FieldTripRegisteIniDrOrdViewModel()
                    {
                        DataId = o.DataId,
                        FeeYm = o.FeeYm,
                        OrderSeqNo = o.OrderSeqNo,
                        OrderCode = o.OrderCode,
                        OrderQty = o.OrderQty,
                        OrderDot = o.OrderDot
                    }).AsNoTracking().ToList();
            B0 = BIniDrOrd.Union(BIniOpOrd).ToList();
            B0 = B0
                .GroupBy(row => new { row.DataId, row.FeeYm })
                .SelectMany(g => g.OrderBy(row => row.OrderSeqNo)
                                    .Select((item, index) => new { Item = item, Index = index + 1 }))
                .Select(o => new FieldTripRegisteIniDrOrdViewModel()
                {
                    DataId = o.Item.DataId,
                    FeeYm = o.Item.FeeYm,
                    OrderSeqNo = o.Item.OrderSeqNo,
                    OrderCode = o.Item.OrderCode,
                    OrderQty = o.Item.OrderQty,
                    OrderDot = o.Item.OrderDot,
                    Row_ID = o.Index
                }).ToList();
            #endregion

            #region 2-1做藥品資料彙整分類
            var B1 = B0.Where(x => x.Row_ID == 1).ToList();
            var B2 = B0.Where(x => x.Row_ID == 2).ToList();
            #endregion

            #region 3.彙整vpn治療清單
            var GenOrderCode = context.GenOrderCode.AsNoTracking().ToList();
            var HospBasic = context.HospBasic
                            .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospId == model.HospID)
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

            var result = from a in A
                         join m in MhbtAgentPatient on new { HospID = a.HospId, ID = a.Id } equals new { m.HospID, m.ID } into mJoin
                         from m in mJoin.DefaultIfEmpty()
                         join b1 in B1 on new { a.FeeYm, a.DataId } equals new { b1.FeeYm, b1.DataId } into b1Join
                         from b1 in b1Join.DefaultIfEmpty()
                         join c1 in GenOrderCode on b1?.OrderCode.Trim() equals c1.OrderCode into c1Join
                         from c1 in c1Join.DefaultIfEmpty()
                         join b2 in B2 on new { a.FeeYm, a.DataId } equals new { b2.FeeYm, b2.DataId } into b2Join
                         from b2 in b2Join.DefaultIfEmpty()
                         join c2 in GenOrderCode on b2?.OrderCode.Trim() equals c2.OrderCode into c2Join
                         from c2 in c2Join.DefaultIfEmpty()
                         join d in HospBasic on new { HospID = a.HospId, a.HospSeqNo } equals new { HospID = d?.HospId, d?.HospSeqNo } into dJoin
                         from d in dJoin.DefaultIfEmpty()
                         select new FieldTripRegisteHealthQueryViewModel()
                         {
                             DataType = "衛教",
                             HospId = a.HospId.TrimStart().TrimEnd(),
                             HospSeqNo = a.HospSeqNo.TrimStart().TrimEnd(),
                             HospName = d != null ? d.HospName.ToString() : null,
                             Id = a.Id.TrimStart().TrimEnd(),
                             Birthday = a.Birthday.TrimStart().TrimEnd() != null ? ((DateTime)a.Birthday.ToDateTime()).ToString("yyyy/MM/dd") : null,
                             Name = a.Name.TrimStart().TrimEnd(),
                             Sex = m != null ? (m.Sex.TrimStart().TrimEnd() == "M" ? "男" : "女") : null,
                             TelD = m != null ? m.TelD.TrimStart().TrimEnd() : null,
                             TelM = m != null ? m.TelM.TrimStart().TrimEnd() : null,
                             TownName = m != null ? m.TownName.TrimStart().TrimEnd() : null,
                             InformADDR = m != null ? m.InformADDR.TrimStart().TrimEnd() : null,
                             ExamYear = a.ExamYear.TrimStart().TrimEnd(),
                             InctructSerial = a.InctructSerial.ToString(),
                             FeeYm = a.FeeYm.ToSlashTaiwanDateFromYYYYMM(),
                             DataId = a.DataId.TrimStart().TrimEnd(),
                             FuncDate = a.FuncDate.TrimStart().TrimEnd() != null ? ((DateTime)a.FuncDate.ToDateTime()).ToString("yyyy/MM/dd") : null,
                             ApplDate = a.ApplDate.TrimStart().TrimEnd() != null ? ((DateTime)a.ApplDate.ToDateTime()).ToString("yyyy/MM/dd") : null,
                             Order_code1 = b1 != null ? b1.OrderCode.TrimStart().TrimEnd() : null,
                             OrderChiName1 = c1 != null ? c1.OrderChiName.TrimStart().TrimEnd() : null,
                             Order_qty1 = b1 != null ? b1.OrderQty.ToString() : null,
                             Order_code2 = b2 != null ? b2.OrderCode.TrimStart().TrimEnd() : null,
                             OrderChiName2 = c2 != null ? c2.OrderChiName.TrimStart().TrimEnd() : null,
                             Order_qty2 = b2 != null ? b2.OrderQty.ToString() : null,
                         };
            result = result.AsQueryable()
                    .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospId == model.HospID)
                    .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo)
                    .Distinct()
                    .OrderBy(x => x.Id).ThenBy(x => x.FuncDate).ToList();
            #endregion

            var pagedData = PagedModel<FieldTripRegisteHealthQueryViewModel>.Create(result.AsQueryable(), model.get());
            pagedData.RecordsTotal = result.Count();
            return new LogicRtnModel<PagedModel<FieldTripRegisteHealthQueryViewModel>>()
            {
                IsSuccess = true,
                Data = pagedData,
            };
        }

        /// <summary>
        /// 健保治療字典初始化
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
                {"身分證號", new Tuple<string, bool>("Id", true)},
                {"出生日期", new Tuple<string, bool>("Birthday", true)},
                {"姓名", new Tuple<string, bool>("Name", true)},
                {"性別", new Tuple<string, bool>("Sex", true)},
                {"電話(日)", new Tuple<string, bool>("TelD", true)},
                {"手機", new Tuple<string, bool>("TelM", true)},
                {"縣市鄉鎮", new Tuple<string, bool>("TownName", true)},
                {"通訊地址", new Tuple<string, bool>("InformADDR", true)},
                {"療程年度", new Tuple<string, bool>("ExamYearTW", true)},
                {"用藥週數", new Tuple<string, bool>("WeekCount", true)},
                {"費用年月", new Tuple<string, bool>("FeeYm", true)},
                {"就醫日期", new Tuple<string, bool>("FuncDate", true)},
                {"申報日期", new Tuple<string, bool>("ApplDate", true)},
                {"醫令代碼1", new Tuple<string, bool>("Order_code1", true)},
                {"代碼名稱1(處方藥名)", new Tuple<string, bool>("OrderChiName1", true)},
                {"量1(藥量)", new Tuple<string, bool>("Order_qty1", true)},
                {"醫令代碼2", new Tuple<string, bool>("Order_code2", true)},
                {"代碼名稱2(處方藥名)", new Tuple<string, bool>("OrderChiName2", true)},
                {"量2(藥量)", new Tuple<string, bool>("Order_qty2", true)},
                {"醫令代碼3", new Tuple<string, bool>("Order_code3", true)},
                {"代碼名稱3(處方藥名)", new Tuple<string, bool>("OrderChiName3", true)},
                {"量3(藥量)", new Tuple<string, bool>("Order_qty3", true)},
                {"醫令代碼4", new Tuple<string, bool>("Order_code4", true)},
                {"代碼名稱4(處方藥名)", new Tuple<string, bool>("OrderChiName4", true)},
                {"量4(藥量)", new Tuple<string, bool>("Order_qty4", true)},
                {"電腦序號", new Tuple<string, bool>("DataId", true)}
            };
            return DictionaryForTreat;
        }
        /// <summary>
        /// 健保衛教字典初始化
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
                {"身分證號", new Tuple<string, bool>("Id", true)},
                {"出生日期", new Tuple<string, bool>("Birthday", true)},
                {"姓名", new Tuple<string, bool>("Name", true)},
                {"性別", new Tuple<string, bool>("Sex", true)},
                {"電話(日)", new Tuple<string, bool>("TelD", true)},
                {"手機", new Tuple<string, bool>("TelM", true)},
                {"縣市鄉鎮", new Tuple<string, bool>("TownName", true)},
                {"通訊地址", new Tuple<string, bool>("InformADDR", true)},
                {"療程年度", new Tuple<string, bool>("ExamYearTW", true)},
                {"療程序號", new Tuple<string, bool>("InctructSerial", true)},
                {"費用年月", new Tuple<string, bool>("FeeYm", true)},
                {"就醫日期", new Tuple<string, bool>("FuncDate", true)},
                {"申報日期", new Tuple<string, bool>("ApplDate", true)},
                {"醫令代碼1", new Tuple<string, bool>("Order_code1", true)},
                {"代碼名稱1(處方藥名)", new Tuple<string, bool>("OrderChiName1", true)},
                {"量1(藥量)", new Tuple<string, bool>("Order_qty1", true)},
                {"醫令代碼2", new Tuple<string, bool>("Order_code2", true)},
                {"代碼名稱2(處方藥名)", new Tuple<string, bool>("OrderChiName2", true)},
                {"量2(藥量)", new Tuple<string, bool>("Order_qty2", true)},
                {"電腦序號", new Tuple<string, bool>("DataId", true)}
            };
            return DictionaryForTreat;
        }
    }
}
