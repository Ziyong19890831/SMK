using Microsoft.EntityFrameworkCore.Internal;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Utility;
using SMK.Shared.Extensions;
using SMK.Web.Extensions;
using SMK.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;
using Yozian.Extension;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace SMK.Web.Services.Foundation
{
    /// <summary>
    /// 申報資料報表
    /// </summary>
    [ScopedService]
    public class DeclareReportService : GenericService
    {
        public DeclareReportService(SMKWEBContext context, SessionManager smgr)
            : base(context, smgr)
        {

        }

        /// <summary>
        /// 健保門診處方及治療明細資料-原始檔
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<DeclareReportViewModel>>> GetIniOpDtl(DeclareReportQueryModel model)
        {
            var YearMonthStart = model.YearMonthStart?.ToYYYYMMFromTaiwan();
            var YearMonthEnd = model.YearMonthEnd?.ToYYYYMMFromTaiwan();
            var Birthday = model.Birthday?.ToYYYYMMDDFromTaiwan();

            //var predicate = PredicateBuilder.True<IniOpDtl>();
            //if (model.Type1)
            //{
            //    predicate = predicate.Or(x => x.MedApply == "1");
            //}
            //if (model.Type2)
            //{
            //    predicate = predicate.Or(x => x.InstructApply == "1");
            //}
            //if (model.Type3)
            //{
            //    predicate = predicate.Or(x => x.TraceApply == "1" ||
            //                                  x.TraceApply == "2" ||
            //                                  x.TraceApply == "3" ||
            //                                  x.TraceApply == "4" ||
            //                                  x.TraceApply == "5");
            //}
            //if (model.Type4)
            //{
            //    predicate = predicate.Or(x => x.ReleaseApply == "1");
            //}
            var list = context.IniOpDtl
                          .WhereWhen(!string.IsNullOrEmpty(YearMonthStart),
                                     p => string.Compare(p.FeeYm, YearMonthStart) >= 0)
                          .WhereWhen(!string.IsNullOrEmpty(YearMonthEnd),
                                     p => string.Compare(YearMonthEnd, p.FeeYm) >= 0)
                          .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospId == model.HospID)
                          .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo)
                          .WhereWhen(!string.IsNullOrEmpty(model.PrsnID), x => x.PrsnId == model.PrsnID)
                          .WhereWhen(!string.IsNullOrEmpty(model.Birthday), x => x.Birthday == model.Birthday.TwDateToDateTime())
                          //.Where(predicate)
                          .Select(p => new DeclareReportViewModel()
                          {
                              DataId = p.DataId,
                              FeeYm = p.FeeYm,
                              ExamYear = p.ExamYear,
                              ExamTime = p.ExamTime,
                              FirstTreatDate = p.FirstTreatDate,
                              WeekCount = p.WeekCount,
                              InstructExamYear = p.InstructExamYear,
                              InstructExamTime = p.InstructExamTime,
                              FirstInstructDate = p.FirstInstructDate,
                              InctructSerial = p.InctructSerial,
                              MedApply = p.MedApply,
                              InstructApply = p.InstructApply,
                              TraceApply = p.TraceApply,
                              ReleaseApply = p.ReleaseApply,
                              ApplType = p.ApplType,
                              HospId = p.HospId,
                              ApplDate = p.ApplDate,
                              CaseType = p.CaseType,
                              HospSeqNo = p.HospSeqNo,
                              FuncType = p.FuncType,
                              FuncDate = p.FuncDate,
                              CureEDate = p.CureEDate,
                              Birthday = p.Birthday,
                              Id = p.Id,
                              FuncSeqNo = p.FuncSeqNo,
                              PayType = p.PayType,
                              PartCode = p.PartCode,
                              Icd9cmCode = p.Icd9cmCode,
                              Icd9cmCode1 = p.Icd9cmCode1,
                              Icd9cmCode2 = p.Icd9cmCode2,
                              DrugDays = p.DrugDays,
                              RelMode = p.RelMode,
                              PrsnId = p.PrsnId,
                              DrugPrsnId = p.DrugPrsnId,
                              DrugDot = p.DrugDot,
                              CureDot = p.CureDot,
                              DiagCode = p.DiagCode,
                              DiagDot = p.DiagDot,
                              DsvcCode = p.DsvcCode,
                              DsvcDot = p.DsvcDot,
                              ExpDot = p.ExpDot,
                              PartAmt = p.PartAmt,
                              ApplDot = p.ApplDot,
                              IdSex = p.IdSex,
                              CureItem1 = p.CureItem1,
                              CureItem2 = p.CureItem2,
                              CureItem3 = p.CureItem3,
                              CureItem4 = p.CureItem4,
                              AreaService = p.AreaService,
                              SuppArea = p.SuppArea,
                              RealHospId = p.RealHospId,
                              HospDataType = p.HospDataType,
                              AgencyPartAmt = p.AgencyPartAmt,
                              Name = p.Name,
                              ApplCauseMark = p.ApplCauseMark,
                              Icd10cmCode3 = p.Icd10cmCode3,
                              Icd10cmCode4 = p.Icd10cmCode4,
                              MetDot = p.MetDot,
                              CorrHospId = p.CorrHospId,
                              TranDate = p.TranDate
                          });

            return await QueryPaging(model.get(), list);
        }
        /// <summary>
        /// 健保門診處方及治療明細資料-原始檔
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<DeclareReportViewModel>>> GetIniDrDtl(DeclareReportQueryModel model)
        {
            var YearMonthStart = model.YearMonthStart?.ToYYYYMMFromTaiwan();
            var YearMonthEnd = model.YearMonthEnd?.ToYYYYMMFromTaiwan();
            var Birthday = model.Birthday?.ToYYYYMMDDFromTaiwan();

            //var predicate = PredicateBuilder.True<IniOpDtl>();
            //if (model.Type1)
            //{
            //    predicate = predicate.Or(x => x.MedApply == "1");
            //}
            //if (model.Type2)
            //{
            //    predicate = predicate.Or(x => x.InstructApply == "1");
            //}
            //if (model.Type3)
            //{
            //    predicate = predicate.Or(x => x.TraceApply == "1" ||
            //                                  x.TraceApply == "2" ||
            //                                  x.TraceApply == "3" ||
            //                                  x.TraceApply == "4" ||
            //                                  x.TraceApply == "5");
            //}
            //if (model.Type4)
            //{
            //    predicate = predicate.Or(x => x.ReleaseApply == "1");
            //}
            var list = context.IniDrDtl
                          .WhereWhen(!string.IsNullOrEmpty(YearMonthStart),
                                     p => string.Compare(p.FeeYm, YearMonthStart) >= 0)
                          .WhereWhen(!string.IsNullOrEmpty(YearMonthEnd),
                                     p => string.Compare(YearMonthEnd, p.FeeYm) >= 0)
                          .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospId == model.HospID)
                          .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo), x => x.HospSeqNo == model.HospSeqNo)
                          .WhereWhen(!string.IsNullOrEmpty(model.PrsnID), x => x.PrsnId == model.PrsnID)
                          .WhereWhen(!string.IsNullOrEmpty(model.Birthday), x => x.Birthday == model.Birthday.TwDateToDateTime())
                          //.Where(predicate)
                          .Select(p => new DeclareReportViewModel()
                          {
                              DataId = p.DataId,
                              FeeYm = p.FeeYm,
                              ExamYear = p.ExamYear,
                              ExamTime = p.ExamTime,
                              FirstTreatDate = p.FirstTreatDate,
                              WeekCount = p.WeekCount,
                              InstructExamYear = p.InstructExamYear,
                              InstructExamTime = p.InstructExamTime,
                              FirstInstructDate = p.FirstInstructDate,
                              InctructSerial = p.InctructSerial,
                              MedApply = p.MedApply,
                              InstructApply = p.InstructApply,
                              TraceApply = p.TraceApply,
                              ReleaseApply = p.ReleaseApply,
                              ApplType = p.ApplType,
                              HospId = p.HospId,
                              ApplDate = p.ApplDate,
                              CaseType = p.CaseType,
                              SeqNo = p.SeqNo,
                              FuncType = p.FuncType,
                              FuncDate = p.FuncDate,
                              CureEDate = string.Empty,
                              Birthday = p.Birthday,
                              Id = p.Id,
                              FuncSeqNo = p.FuncSeqNo,
                              PayType = p.PayType,
                              PartCode = p.PartCode,
                              Icd9cmCode = p.Icd9cmCode,
                              Icd9cmCode1 = p.Icd9cmCode1,
                              Icd9cmCode2 = p.Icd9cmCode2,
                              DrugDays = p.DrugDays,
                              RelMode = string.Empty,
                              PrsnId = p.PrsnId,
                              DrugPrsnId = p.DrugPrsnId,
                              DrugDot = p.DrugDot,
                              CureDot = p.CureDot,
                              DiagCode = string.Empty,
                              DiagDot = null,
                              DsvcCode = p.DsvcCode,
                              DsvcDot = p.DsvcDot,
                              ExpDot = p.ExpDot,
                              PartAmt = p.PartAmt,
                              ApplDot = p.ApplDot,
                              IdSex = p.IdSex,
                              CureItem1 = p.CureItem1,
                              CureItem2 = p.CureItem2,
                              CureItem3 = p.CureItem3,
                              CureItem4 = p.CureItem4,
                              AreaService = p.AreaService,
                              SuppArea = string.Empty,
                              RealHospId = string.Empty,
                              HospDataType = string.Empty,
                              AgencyPartAmt = null,
                              Name = p.Name,
                              ApplCauseMark = p.ApplCauseMark,
                              Icd10cmCode3 = p.Icd10cmCode3,
                              Icd10cmCode4 = p.Icd10cmCode4,
                              MetDot = null,
                              CorrHospId = p.CorrHospId,
                              TranDate = p.TranDate,
                              HospSeqNo = p.HospSeqNo
                          });
            return await QueryPaging(model.get(), list);
        }
    }
}
