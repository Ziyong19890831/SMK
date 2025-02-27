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
    public class FieldTripRegisterMedicalOrdersService : GenericService
    {
        public FieldTripRegisterMedicalOrdersService(SMKWEBContext context, SessionManager smgr)
        : base(context, smgr)
        {
        }

        /// <summary>
        /// 實地訪查-2.調閱醫令清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<GetRegisterMedicalOrdersViewModel>>> GetFieldRegisterMedicalOrders(FieldTripQueryViewModel model)
        {
            #region 測試參數
            //model.HospID = "593106A646";
            //model.HospSeqNo = "00";
            //model.FuncStartDate = "1110101";
            //model.FuncEndDate = "1111231";
            #endregion
            var YearMonthStart = model.FuncStartDate?.ToYYYYMMDDFromTaiwan();
            var YearMonthEnd = model.FuncEndDate?.ToYYYYMMDDFromTaiwan();
            # region 1.篩選費用年月範圍
            List<FieldTripRegisterIniOpDtlViewModel> A = new List<FieldTripRegisterIniOpDtlViewModel>();

            var AIniOpDtl = context.IniOpDtl
                        .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospId == model.HospID)
                        .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo)
                        .WhereWhen(!string.IsNullOrEmpty(YearMonthStart),
                                   p => string.Compare(p.FuncDate, YearMonthStart) >= 0)
                        .WhereWhen(!string.IsNullOrEmpty(YearMonthEnd),
                                   p => string.Compare(YearMonthEnd, p.FuncDate) >= 0)
                        .Select(d => new FieldTripRegisterIniOpDtlViewModel()
                        {
                            HospId = (new List<string> { "1101100011", "1132070011", "1517061032" }.Contains(d.HospId) ? d.RealHospId : d.HospId),
                            HospSeqNo = (string.Compare("20150101", d.FuncDate) >= 0 && d.HospId.StartsWith("0101090517") ?
                                            (d.SeqNo <= 100000 ? "10" : ((int)d.SeqNo).ToString("0000000").Substring(0, 2)) : "00"),
                            DTL = d.DataId.TrimStart().TrimEnd() + d.FeeYm.TrimStart().TrimEnd(),
                            DataId = d.DataId.TrimStart().TrimEnd(),
                            FeeYm = d.FeeYm.TrimStart().TrimEnd(),
                            InstructExamYear = d.InstructExamYear.TrimStart().TrimEnd(),
                            InstructExamTime = d.InstructExamTime,
                            InctructSerial = d.InctructSerial,
                            MedApply = d.MedApply.TrimStart().TrimEnd(),
                            InstructApply = d.InstructApply.TrimStart().TrimEnd(),
                            TraceApply = d.TraceApply.TrimStart().TrimEnd(),
                            ReleaseApply = d.ReleaseApply.TrimStart().TrimEnd(),
                            CaseType = d.CaseType.TrimStart().TrimEnd(),
                            HospDataType = d.HospDataType.TrimStart().TrimEnd(),
                            SeqNo = d.SeqNo,
                            FuncType = d.FuncType.TrimStart().TrimEnd(),
                            FuncDate = d.FuncDate.TrimStart().TrimEnd(),
                            ExamYear = d.ExamYear.TrimStart().TrimEnd(),
                            ExamTime = d.ExamTime,
                            Birthday = d.Birthday.TrimStart().TrimEnd(),
                            Id = d.Id.TrimStart().TrimEnd(),
                            WeekCount = d.WeekCount,
                            DrugDays = d.DrugDays,
                            ApplDot = d.ApplDot,
                            Name = d.Name.TrimStart().TrimEnd(),
                            ExpDot = d.ExpDot,
                            PartAmt = d.PartAmt,
                            ApplDate = d.ApplDate.TrimStart().TrimEnd(),
                        })
                    .Distinct().AsNoTracking().ToList();

            var AIniDrDtl = context.IniDrDtl
                     .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospId == model.HospID)
                     .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo)
                     .WhereWhen(!string.IsNullOrEmpty(YearMonthStart),
                             p => string.Compare(p.FuncDate, YearMonthStart) >= 0)
                     .WhereWhen(!string.IsNullOrEmpty(YearMonthEnd),
                                 p => string.Compare(YearMonthEnd, p.FuncDate) >= 0)
                     .Select(d => new FieldTripRegisterIniOpDtlViewModel()
                     {
                         HospId = d.HospId.TrimStart().TrimEnd(),
                         HospSeqNo = "00",
                         DTL = d.DataId.TrimStart().TrimEnd() + d.FeeYm.TrimStart().TrimEnd(),
                         DataId = d.DataId.TrimStart().TrimEnd(),
                         FeeYm = d.FeeYm.TrimStart().TrimEnd(),
                         InstructExamYear = d.InstructExamYear.TrimStart().TrimEnd(),
                         InstructExamTime = d.InstructExamTime,
                         InctructSerial = d.InctructSerial,
                         MedApply = d.MedApply.TrimStart().TrimEnd(),
                         InstructApply = d.InstructApply.TrimStart().TrimEnd(),
                         TraceApply = d.TraceApply.TrimStart().TrimEnd(),
                         ReleaseApply = d.ReleaseApply.TrimStart().TrimEnd(),
                         CaseType = d.CaseType.TrimStart().TrimEnd(),
                         HospDataType = "30",
                         SeqNo = d.SeqNo,
                         FuncType = d.FuncType.TrimStart().TrimEnd(),
                         FuncDate = d.FuncDate.TrimStart().TrimEnd(),
                         ExamYear = d.ExamYear.TrimStart().TrimEnd(),
                         ExamTime = d.ExamTime,
                         Birthday = d.Birthday.TrimStart().TrimEnd(),
                         Id = d.Id.TrimStart().TrimEnd(),
                         WeekCount = d.WeekCount,
                         DrugDays = d.DrugDays,
                         ApplDot = d.ApplDot,
                         Name = d.Name.TrimStart().TrimEnd(),
                         ExpDot = d.ExpDot,
                         PartAmt = d.PartAmt,
                         ApplDate = d.ApplDate.TrimStart().TrimEnd(),
                     })
                     .Distinct().AsNoTracking().ToList();
            A = AIniOpDtl.Union(AIniDrDtl).ToList();
            #endregion

            #region 2.Ord 戒菸藥品資料彙整
            List<FieldTripRegisteIniDrOrdViewModel> B0 = new List<FieldTripRegisteIniDrOrdViewModel>();

            var BIniDrOrd = context.IniDrOrd
                .WhereWhen(!string.IsNullOrEmpty(YearMonthStart),
                            p => string.Compare(p.FeeYm, YearMonthStart.Substring(0, 6)) >= 0)
                .WhereWhen(!string.IsNullOrEmpty(YearMonthEnd),
                            p => string.Compare(YearMonthEnd.Substring(0, 6), p.FeeYm) >= 0)
                .Select(o => new FieldTripRegisteIniDrOrdViewModel()
                {
                    DataId = o.DataId,
                    FeeYm = o.FeeYm,
                    OrderSeqNo = o.OrderSeqNo,
                    OrderCode = o.OrderCode,
                    OrderQty = o.OrderQty,
                    OrderDot = o.OrderDot,
                    OrderUprice = o.OrderUprice
                }).AsNoTracking().ToList();

            var BIniOpOrd = context.IniOpOrd
                    .WhereWhen(!string.IsNullOrEmpty(YearMonthStart),
                                p => string.Compare(p.FeeYm, YearMonthStart.Substring(0, 6)) >= 0)
                    .WhereWhen(!string.IsNullOrEmpty(YearMonthEnd),
                                p => string.Compare(YearMonthEnd.Substring(0, 6), p.FeeYm) >= 0)
                    .Select(o => new FieldTripRegisteIniDrOrdViewModel()
                    {
                        DataId = o.DataId,
                        FeeYm = o.FeeYm,
                        OrderSeqNo = o.OrderSeqNo,
                        OrderCode = o.OrderCode,
                        OrderQty = o.OrderQty,
                        OrderDot = o.OrderDot,
                        OrderUprice = o.OrderUprice
                    }).AsNoTracking().ToList();
            B0 = BIniDrOrd.Union(BIniOpOrd).ToList();
            #endregion

            #region 3.彙整清單
            var GenOrderCode = context.GenOrderCode.AsNoTracking().ToList();
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

            var result = from a in A
                         join b in B0 on new { a.FeeYm, a.DataId } equals new { b.FeeYm, b.DataId } into abJoin
                         from ab in abJoin.DefaultIfEmpty()
                         join c in GenOrderCode on ab?.OrderCode.Trim() equals c.OrderCode.Trim() into abcJoin
                         from abc in abcJoin.DefaultIfEmpty()
                         join d in HospBasic on new { HospId = a.HospId, a.HospSeqNo } equals new { d?.HospId, d?.HospSeqNo } into abcdJoin
                         from abcd in abcdJoin.DefaultIfEmpty()
                         select new GetRegisterMedicalOrdersViewModel()
                         {
                             HospID = a != null ? a.HospId.ToString().TrimStart().TrimEnd() : null,
                             HospSeqNo = a != null ? a.HospSeqNo.ToString().TrimStart().TrimEnd() : null,
                             HospName = abcd != null ? abcd.HospName.ToString().TrimStart().TrimEnd() : null,
                             ID = a != null ? a.Id.ToString().TrimStart().TrimEnd() : null,
                             Birthday = a.Birthday != null ? ((DateTime)a.Birthday.ToDateTime()).ToString("yyyy/MM/dd") : null,
                             Name = a != null ? a.Name != null ? a.Name.TrimStart().TrimEnd() : "" : null,
                             FuncDate = a.FuncDate != null ? ((DateTime)a.FuncDate.ToDateTime()).ToString("yyyy/MM/dd") : null,
                             OrderChiName = abc != null ? abc.OrderChiName != null ? abc.OrderChiName.ToString().TrimStart().TrimEnd() : "" : null,
                             OrderCode = abc != null ? abc.OrderCode != null ? abc.OrderCode.ToString().TrimStart().TrimEnd() : "" : null,
                             OrderUprice = ab != null ? ab.OrderUprice.ToString().TrimStart().TrimEnd() : null,
                             OrderQty = ab != null ? ab.OrderQty.ToString().TrimStart().TrimEnd() : null,
                             OrderDot = ab != null ? ab.OrderDot.ToString().TrimStart().TrimEnd() : null,
                             DataID = a != null ? a.DataId.ToString().TrimStart().TrimEnd() : null,
                             FeeYM = a != null ? a.FeeYm.ToSlashTaiwanDateFromYYYYMM() : null,
                             OrderSeqNo = ab != null ? ab.OrderSeqNo.ToString().TrimStart().TrimEnd() : null,
                             MedApply = a != null ? a.MedApply != null ? a.MedApply.TrimStart().TrimEnd() : "" : null,
                             InstructApply = a != null ? a.InstructApply != null ? a.InstructApply.TrimStart().TrimEnd() : "" : null,
                             TraceApply = a != null ? a.TraceApply != null ? a.TraceApply.TrimStart().TrimEnd() : "" : null,
                             ReleaseApply = a != null ? a.ReleaseApply != null ? a.ReleaseApply.TrimStart().TrimEnd() : "" : null,
                         };
            result = result.AsQueryable()
                    .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospID == model.HospID)
                    .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo)
                    .Distinct()
                    .OrderBy(x => x.ID).ThenBy(x => x.FuncDate).ThenBy(x => x.OrderSeqNo).ToList();

            #endregion
            var pagedData = PagedModel<GetRegisterMedicalOrdersViewModel>.Create(result.AsQueryable(), model.get());
            pagedData.RecordsTotal = result.Count();
            return new LogicRtnModel<PagedModel<GetRegisterMedicalOrdersViewModel>>()
            {
                IsSuccess = true,
                Data = pagedData,
            };
        }

        /// <summary>
        /// 醫令清單字典初始化
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Tuple<string, bool>> DictionaryData()
        {
            Dictionary<string, Tuple<string, bool>> Dictionary = new Dictionary<string, Tuple<string, bool>>()
            {
                {"醫事機構代碼", new Tuple<string, bool>("HospId", true)},
                {"院區別", new Tuple<string, bool>("HospSeqNo", true)},
                {"醫事機構名稱", new Tuple<string, bool>("HospName", true)},
                {"身分證號", new Tuple<string, bool>("Id", true)},
                {"出生日期", new Tuple<string, bool>("Birthday", true)},
                {"姓名", new Tuple<string, bool>("Name", true)},
                {"就醫日期", new Tuple<string, bool>("FuncDate", true)},
                {"醫令名稱", new Tuple<string, bool>("OrderChiName", true)},
                {"醫令代碼", new Tuple<string, bool>("OrderCode", true)},
                {"醫令單價", new Tuple<string, bool>("OrderUprice", true)},
                {"醫令數量", new Tuple<string, bool>("OrderQty", true)},
                {"醫令金額", new Tuple<string, bool>("OrderDot", true)},
                {"費用年月", new Tuple<string, bool>("FeeYM", true)},
                {"醫令序號", new Tuple<string, bool>("OrderSeqNo", true)},
                {"治療申報", new Tuple<string, bool>("MedApply", true)},
                {"衛教申報", new Tuple<string, bool>("InstructApply", true)},
                {"追蹤申報", new Tuple<string, bool>("TraceApply", true)},
                {"釋出申報", new Tuple<string, bool>("ReleaseApply", true)},
                {"電腦序號", new Tuple<string, bool>("DataID", true)},
            };
            return Dictionary;
        }
    }
}
