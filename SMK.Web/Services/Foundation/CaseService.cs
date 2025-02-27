using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Utility;
using SMK.Shared.Extensions;
using SMK.Web.Models;
using Yozian.DependencyInjectionPlus.Attributes;
using Yozian.Extension;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class CaseService : GenericService
    {
        public ILogger<CaseService> Logger { get; set; }

        public CaseService(SMKWEBContext context, SessionManager smgr,
            ILogger<CaseService> logger) : base(context, smgr)
        {
            Logger = logger;
        }

        public async Task<LogicRtnModel<PagedModel<CaseQueryViewModel>>> GetDataQueryData(DataQueryQueryModel model)
        {
            try
            {
                var hasChecked = model.Type1 || model.Type2 || model.Type3 || model.Type4;
                var query1 = context.IniOpDtl
                    .WhereWhen(!string.IsNullOrWhiteSpace(model.HospID), x => x.HospId == model.HospID)
                    .WhereWhen(!string.IsNullOrWhiteSpace(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo)
                    .WhereWhen(!string.IsNullOrWhiteSpace(model.Birthday), x => x.Birthday == model.Birthday.TwDateToDateTime())
                    .WhereWhen(!string.IsNullOrWhiteSpace(model.PrsnID), x => x.Id == model.PrsnID)
                    .WhereWhen(!string.IsNullOrWhiteSpace(model.FuncStartDate), x => string.Compare(x.FuncDate, model.FuncStartDate.TwDateToDateTime()) >= 0)
                    .WhereWhen(!string.IsNullOrWhiteSpace(model.FuncEndDate), x => string.Compare(x.FuncDate, model.FuncEndDate.TwDateToDateTime()) <= 0);
                if (hasChecked)
                {
                    var predicate = PredicateBuilder.False<IniOpDtl>();
                    if (model.Type1)
                    {
                        predicate = predicate.Or(x => x.MedApply == "1");
                    }
                    if (model.Type2)
                    {
                        predicate = predicate.Or(x => x.InstructApply == "1");
                    }
                    if (model.Type3)
                    {
                        predicate = predicate.Or(x => x.TraceApply == "1" ||
                                                      x.TraceApply == "2" ||
                                                      x.TraceApply == "3" ||
                                                      x.TraceApply == "4" ||
                                                      x.TraceApply == "5");
                    }
                    if (model.Type4)
                    {
                        predicate = predicate.Or(x => x.ReleaseApply == "1");
                    }
                    query1 = query1.Where(predicate);
                }
                query1 = query1.OrderBy(x => x.FuncDate);
                
                var query2 = context.IniDrDtl
                    .WhereWhen(!string.IsNullOrWhiteSpace(model.HospID), x => x.HospId == model.HospID)
                    .WhereWhen(!string.IsNullOrWhiteSpace(model.Birthday), x => x.Birthday == model.Birthday.TwDateToDateTime())
                    .WhereWhen(!string.IsNullOrWhiteSpace(model.PrsnID), x => x.Id == model.PrsnID)
                    .WhereWhen(!string.IsNullOrWhiteSpace(model.FuncStartDate), x => string.Compare(x.FuncDate, model.FuncStartDate.TwDateToDateTime()) >= 0)
                    .WhereWhen(!string.IsNullOrWhiteSpace(model.FuncEndDate), x => string.Compare(x.FuncDate, model.FuncEndDate.TwDateToDateTime()) <= 0);
                if (hasChecked)
                {
                    var predicate = PredicateBuilder.False<IniDrDtl>();
                    if (model.Type1)
                    {
                        predicate = predicate.Or(x => x.MedApply == "1");
                    }
                    if (model.Type2)
                    {
                        predicate = predicate.Or(x => x.InstructApply == "1");
                    }
                    if (model.Type3)
                    {
                        predicate = predicate.Or(x => x.TraceApply == "1" ||
                                                      x.TraceApply == "2" ||
                                                      x.TraceApply == "3" ||
                                                      x.TraceApply == "4" ||
                                                      x.TraceApply == "5");
                    }
                    if (model.Type4)
                    {
                        predicate = predicate.Or(x => x.ReleaseApply == "1");
                    }
                    query2 = query2.Where(predicate);
                }

                //var query3 = from inner in query2
                //    join outer in query1
                //        on new {inner.Id, inner.HospId, inner.Birthday, inner.FuncDate}
                //        equals new {outer.Id, outer.HospId, outer.Birthday, outer.FuncDate}
                //    orderby inner.FuncDate
                //    select inner;

                var data = ((query1.Select(x => new CaseQueryViewModel()
                        {
                            DataType = "1",
                            DataId = x.DataId,
                            FeeYm = x.FeeYm,
                            ExamYear = x.ExamYear,
                            ExamTime = x.ExamTime,
                            FirstTreatDate = x.FirstTreatDate.WestToTwDate(),
                            WeekCount = x.WeekCount,
                            InstructExamYear = x.InstructExamYear,
                            InstructExamTime = x.InstructExamTime,
                            FirstInstructDate = x.FirstInstructDate.WestToTwDate(),
                            InctructSerial = x.InctructSerial,
                            MedApply = x.MedApply,
                            InstructApply = x.InstructApply,
                            TraceApply = x.TraceApply,
                            ReleaseApply = x.ReleaseApply,
                            ApplType = x.ApplType,
                            HospId = x.HospId,
                            ApplDate = x.ApplDate.WestToTwDate(),
                            CaseType = x.CaseType,
                            SeqNo = x.SeqNo,
                            FuncType = x.FuncType,
                            FuncDate = x.FuncDate.WestToTwDate(),
                            RelDate = "",
                            Birthday = x.Birthday.WestToTwDate(),
                            Id = x.Id,
                            FuncSeqNo = x.FuncSeqNo,
                            PayType = x.PayType,
                            PartCode = x.PartCode,
                            Icd9cmCode = x.Icd9cmCode,
                            Icd9cmCode1 = x.Icd9cmCode1,
                            Icd9cmCode2 = x.Icd9cmCode2,
                            DrugDays = x.DrugDays,
                            RelMode = x.RelMode,
                            PrsnId = x.PrsnId,
                            DrugPrsnId = x.PrsnId,
                            DrugDot = x.DrugDot,
                            DiagCode = x.DiagCode,
                            DiagDot = x.DiagDot,
                            CureDot = x.CureDot,
                            DsvcCode = x.DsvcCode,
                            DsvcDot = x.DsvcDot,
                            ExpDot = x.ExpDot,
                            PartAmt = x.PartAmt,
                            ApplDot = x.ApplDot,
                            IdSex = x.IdSex,
                            Remark = "",
                            CureItem1 = x.CureItem1,
                            CureItem2 = x.CureItem2,
                            CureItem3 = x.CureItem3,
                            CureItem4 = x.CureItem4,
                            OrigHospId = "",
                            AreaService = x.AreaService,
                            SuppArea = x.SuppArea,
                            RealHospId = x.RealHospId,
                            HospDataType = x.HospDataType,
                            OrigCaseType = "",
                            AgencyPartAmt = x.AgencyPartAmt,
                            OtherPartAmt = 0,
                            Name = x.Name,
                            ApplCauseMark = x.ApplCauseMark,
                            Icd10cmCode2 = x.Icd9cmCode2,
                            Icd10cmCode3 = x.Icd10cmCode3,
                            Icd10cmCode4 = x.Icd10cmCode4,
                            MetDot = x.MetDot,
                            CorrHospId = x.CorrHospId,
                            TranDate = x.TranDate,
                            data_id = x.DataId,
                        })).AsEnumerable()
                        .Concat(query2.Select(x => new CaseQueryViewModel()
                        {
                            DataType = "2",
                            DataId = x.DataId,
                            FeeYm = x.FeeYm,
                            ExamYear = x.ExamYear,
                            ExamTime = x.ExamTime,
                            FirstTreatDate = x.FirstTreatDate.WestToTwDate(),
                            WeekCount = x.WeekCount,
                            InstructExamYear = x.InstructExamYear,
                            InstructExamTime = x.InstructExamTime,
                            FirstInstructDate = x.FirstInstructDate.WestToTwDate(),
                            InctructSerial = x.InctructSerial,
                            MedApply = x.MedApply,
                            InstructApply = x.InstructApply,
                            TraceApply = x.TraceApply,
                            ReleaseApply = x.ReleaseApply,
                            ApplType = x.ApplType,
                            HospId = x.HospId,
                            ApplDate = x.ApplDate.WestToTwDate(),
                            CaseType = x.CaseType,
                            SeqNo = x.SeqNo,
                            FuncType = x.FuncType,
                            FuncDate = x.FuncDate.WestToTwDate(),
                            RelDate = x.RelDate.WestToTwDate(),
                            Birthday = x.Birthday,
                            Id = x.Id,
                            FuncSeqNo = x.FuncSeqNo,
                            PayType = x.PayType,
                            PartCode = x.PartCode,
                            Icd9cmCode = x.Icd9cmCode,
                            Icd9cmCode1 = x.Icd9cmCode1,
                            Icd9cmCode2 = x.Icd9cmCode2,
                            DrugDays = x.DrugDays,
                            PrsnId = x.PrsnId,
                            DrugPrsnId = x.DrugPrsnId,
                            DrugDot = x.DrugDot,
                            CureDot = x.CureDot,
                            DsvcCode = x.DsvcCode,
                            DsvcDot = x.DsvcDot,
                            ExpDot = x.ExpDot,
                            PartAmt = x.PartAmt,
                            ApplDot = x.ApplDot,
                            IdSex = x.IdSex,
                            Remark = "",
                            CureItem1 = x.CureItem1,
                            CureItem2 = x.CureItem2,
                            CureItem3 = x.CureItem3,
                            CureItem4 = x.CureItem4,
                            OrigHospId = x.OrigHospId,
                            AreaService = x.AreaService,
                            SuppArea = "",
                            RealHospId = "",
                            HospDataType = "",
                            OrigCaseType = x.OrigCaseType,
                            AgencyPartAmt = 0,
                            OtherPartAmt = x.OtherPartAmt,
                            Name = x.Name,
                            ApplCauseMark = x.ApplCauseMark,
                            Icd10cmCode2 = x.Icd10cmCode2,
                            Icd10cmCode3 = x.Icd10cmCode3,
                            Icd10cmCode4 = x.Icd10cmCode4,
                            MetDot = 0,
                            CorrHospId = x.CorrHospId,
                            TranDate = x.TranDate,
                            data_id = x.DataId,
                        }).AsEnumerable()));
                    // .OrderBy(x => new { x.DataType, x.FuncDate });
                    // .Skip(model.Start)
                    // .Take(model.Length);

                var pagedData = PagedModel<CaseQueryViewModel>.Create(data.AsQueryable(), model.get());
                pagedData.RecordsTotal = query1.Count() + query2.Count();
                return new LogicRtnModel<PagedModel<CaseQueryViewModel>>()
                {
                    IsSuccess = true,
                    Data = pagedData,
                };
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                Logger.LogError(e.StackTrace);
                return new LogicRtnModel<PagedModel<CaseQueryViewModel>>()
                {
                    IsSuccess = false,
                    ErrMsg = e.Message,
                    StackTrace = e.StackTrace,
                };
            }
        }
    }
}
