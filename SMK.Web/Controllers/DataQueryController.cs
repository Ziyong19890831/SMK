using System;
using System.Collections;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using SMK.Data.Dto;
using SMK.Data.Enums;
using SMK.Data.Migrations;
using SMK.Web.AppScope.Filters;
using SMK.Web.Models;
using SMK.Web.Services.Foundation;
using Yozian.WebCore.Library.Utility.Excel;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    public class DataQueryController : BaseController
    {
        public IniOpOrdService IniOpOrdService { get; set; }

        public IniDrOrdService IniDrOrdService { get; set; }

        public CaseService CaseService { get; set; }
        public SamplingExamineService SamplingExamineService { get; set; }
        public DataQueryService DataQueryService { get; set; }

        public DataQueryController(CaseService caseService,
            IniDrOrdService iniDrDtlService,
            IniOpOrdService iniOpDtlService,
            SamplingExamineService samplingExamineService,
            DataQueryService dataQueryService)
        {
            CaseService = caseService;
            IniDrOrdService = iniDrDtlService;
            IniOpOrdService = iniOpDtlService;
            SamplingExamineService = samplingExamineService;
            DataQueryService = dataQueryService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Query(DataQueryQueryModel model)
        {
            var logicRtnModel = await CaseService.GetDataQueryData(model);
            return Json(logicRtnModel);
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QueryIniOpOrds(IniOpOrdQueryModel model)
        {
            return Json(await IniOpOrdService.Query(model));
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QueryIniDrOrds(IniDrOrdQueryModel model)
        {
            return Json(await IniDrOrdService.Query(model));
        }
        [HttpPost]
        public async Task<IActionResult> ExportMedicineReport([FromForm] ExportMedicineReportRequest request)
        {
            var vm = await DataQueryService.ExportMedicineReportAsync(request);
            return File(vm.Stream, "application/octet-stream", vm.FileName);
        }

        [HttpPost, ActionName("ExportDtlOrd")]
        public async Task<IActionResult> ExportIniOpDtl(DataQueryQueryModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            LogicRtnModel<PagedModel<CaseQueryViewModel>> logicRtnModel = new LogicRtnModel<PagedModel<CaseQueryViewModel>>();
            logicRtnModel.IsSuccess = false;
            string SheetName = string.Empty;
            //初始化字典
            Dictionary<string, Tuple<string, bool>> DictionaryDtl = DataQueryService.DictionaryDtlData();
            Dictionary<string, Tuple<string, bool>> DictionaryOrd = DataQueryService.DictionaryOrdData();

            logicRtnModel = await CaseService.GetDataQueryData(model);

            if (model.SelectCheckBox != null && model.SelectCheckBox.Count > 0)
            {
                DictionaryDtl = DataQueryService.DictionaryDataForAll(DictionaryDtl, false);
                DictionaryOrd = DataQueryService.DictionaryDataForAll(DictionaryOrd, false);
                foreach (var item in model.SelectCheckBox)
                {
                    DictionaryDtl = DataQueryService.DictionaryDataForOutPut(DictionaryDtl, item);
                    DictionaryOrd = DataQueryService.DictionaryDataForOutPut(DictionaryOrd, item);
                }
            }

            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }

            var list = logicRtnModel.Data.Data;
            List<DataQueryExcelViewModel> Data = logicRtnModel.Data.Data.Select(x => new DataQueryExcelViewModel()
            {
                DataType = x.DataType,
                DataId = x.DataId,
                HospId = x.HospId,
                FeeYm = x.FeeYm,
                ExamYear = x.ExamYear,
                ExamTime = x.ExamTime,
                FirstTreatDate = x.FirstTreatDate,
                WeekCount = x.WeekCount,
                InstructExamYear = x.InstructExamYear,
                InstructExamTime = x.InstructExamTime,
                FirstInstructDate = x.FirstInstructDate,
                InctructSerial = x.InctructSerial,
                MedApply = x.MedApply,
                InstructApply = x.InstructApply,
                TraceApply = x.TraceApply,
                ReleaseApply = x.ReleaseApply,
                ApplType = x.ApplType,
                ApplDate = x.ApplDate,
                CaseType = x.CaseType,
                SeqNo = x.SeqNo,
                FuncType = x.FuncType,
                FuncDate = x.FuncDate,
                RelDate = x.RelDate,
                Birthday = x.Birthday,
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
                DrugPrsnId = x.DrugPrsnId,
                DrugDot = x.DrugDot,
                DiagCode = x.DiagCode,
                DiagDot = x.DiagDot,
                CureDot = x.CureDot,
                DsvcCode = x.DsvcCode,
                DsvcDot = x.DsvcDot,
                ExpDot = x.ExpDot,
                PartAmt = x.PartAmt,
                ApplDot = x.ApplDot,
                OrigHospId = x.OrigHospId,
                IdSex = x.IdSex,
                Remark = x.Remark,
                CureItem1 = x.CureItem1,
                CureItem2 = x.CureItem2,
                CureItem3 = x.CureItem3,
                CureItem4 = x.CureItem4,
                OrigCaseType = x.OrigCaseType,
                OtherPartAmt = x.OtherPartAmt,
                ApplCauseMark = x.ApplCauseMark,
                Icd10cmCode2 = x.Icd10cmCode2,
                Icd10cmCode3 = x.Icd10cmCode3,
                Icd10cmCode4 = x.Icd10cmCode4,
                CorrHospId = x.CorrHospId,
                AreaService = x.AreaService,
                SuppArea = x.SuppArea,
                RealHospId = x.RealHospId,
                HospDataType = x.HospDataType,
                AgencyPartAmt = x.AgencyPartAmt,
                TranDate = x.TranDate,
                Name = x.Name,
                MetDot = x.MetDot,
                data_id = x.data_id,
            }).ToList();

            List<DataQueryExcelViewModel> dataQueryExcelViewModel = new List<DataQueryExcelViewModel>();
            bool DictionaryOrdHasTrueValue = DictionaryOrd.Any(item => item.Value.Item2 == true);
            if (model.customSwitch || !DictionaryOrdHasTrueValue)
            {
                DictionaryOrd = DataQueryService.DictionaryDataForAll(DictionaryOrd, false);
                dataQueryExcelViewModel.AddRange(Data);
                SheetName = "清單個案";
            }
            else
            {
                SheetName = "清單醫令別";

                IniDrOrdQueryModel iniDrOrdQueryModel = new IniDrOrdQueryModel();
                List<IniOpOrdViewModel> IniOpOrdViewModel = new List<IniOpOrdViewModel>();
                List<IniDrOrdViewModel> iniDrOrdViewModel = new List<IniDrOrdViewModel>();
                //這邊加入ord
                foreach (var dtl in Data)
                {
                    iniDrOrdQueryModel.DataId = dtl.DataId;
                    iniDrOrdQueryModel.FeeYm = dtl.FeeYm;
                    if (dtl.HospId.Substring(0, Math.Min(dtl.HospId.Length,2)) != "59")
                    {
                        IniOpOrdViewModel = IniOpOrdService.QueryOrdToDrOrd(iniDrOrdQueryModel);
                        foreach (var x in IniOpOrdViewModel)
                        {
                            dataQueryExcelViewModel.Add(new DataQueryExcelViewModel()
                            {
                                DataType = dtl.DataType,
                                DataId = dtl.DataId,
                                HospId = dtl.HospId,
                                FeeYm = dtl.FeeYm,
                                ExamYear = dtl.ExamYear,
                                ExamTime = dtl.ExamTime,
                                FirstTreatDate = dtl.FirstTreatDate,
                                WeekCount = dtl.WeekCount,
                                InstructExamYear = dtl.InstructExamYear,
                                InstructExamTime = dtl.InstructExamTime,
                                FirstInstructDate = dtl.FirstInstructDate,
                                InctructSerial = dtl.InctructSerial,
                                MedApply = dtl.MedApply,
                                InstructApply = dtl.InstructApply,
                                TraceApply = dtl.TraceApply,
                                ReleaseApply = dtl.ReleaseApply,
                                ApplType = dtl.ApplType,
                                ApplDate = dtl.ApplDate,
                                CaseType = dtl.CaseType,
                                SeqNo = dtl.SeqNo,
                                FuncType = dtl.FuncType,
                                FuncDate = dtl.FuncDate,
                                RelDate = dtl.RelDate,
                                Birthday = dtl.Birthday,
                                Id = dtl.Id,
                                FuncSeqNo = dtl.FuncSeqNo,
                                PayType = dtl.PayType,
                                PartCode = dtl.PartCode,
                                Icd9cmCode = dtl.Icd9cmCode,
                                Icd9cmCode1 = dtl.Icd9cmCode1,
                                Icd9cmCode2 = dtl.Icd9cmCode2,
                                DrugDays = dtl.DrugDays,
                                RelMode = dtl.RelMode,
                                PrsnId = dtl.PrsnId,
                                DrugPrsnId = dtl.DrugPrsnId,
                                DrugDot = dtl.DrugDot,
                                DiagCode = dtl.DiagCode,
                                DiagDot = dtl.DiagDot,
                                CureDot = dtl.CureDot,
                                DsvcCode = dtl.DsvcCode,
                                DsvcDot = dtl.DsvcDot,
                                ExpDot = dtl.ExpDot,
                                PartAmt = dtl.PartAmt,
                                ApplDot = dtl.ApplDot,
                                OrigHospId = dtl.OrigHospId,
                                IdSex = dtl.IdSex,
                                Remark = dtl.Remark,
                                CureItem1 = dtl.CureItem1,
                                CureItem2 = dtl.CureItem2,
                                CureItem3 = dtl.CureItem3,
                                CureItem4 = dtl.CureItem4,
                                OrigCaseType = dtl.OrigCaseType,
                                OtherPartAmt = dtl.OtherPartAmt,
                                ApplCauseMark = dtl.ApplCauseMark,
                                Icd10cmCode2 = dtl.Icd10cmCode2,
                                Icd10cmCode3 = dtl.Icd10cmCode3,
                                Icd10cmCode4 = dtl.Icd10cmCode4,
                                CorrHospId = dtl.CorrHospId,
                                AreaService = dtl.AreaService,
                                SuppArea = dtl.SuppArea,
                                RealHospId = dtl.RealHospId,
                                HospDataType = dtl.HospDataType,
                                AgencyPartAmt = dtl.AgencyPartAmt,
                                TranDate = dtl.TranDate,
                                Name = dtl.Name,
                                MetDot = dtl.MetDot,
                                OrderType = x.OrderType,
                                OrderCode = x.OrderCode,
                                Ord_RelMode = x.RelMode,
                                ChrMark = x.ChrMark,
                                DrugNum = x.DrugNum,
                                DrugFre = x.DrugFre,
                                DrugPath = x.DrugPath,
                                OrderUprice = x.OrderUprice,
                                OrderQty = x.OrderQty,
                                OrderDot = x.OrderDot,
                                OrderDrugDay = x.OrderDrugDay,
                                ExePrsnId = x.ExePrsnId,
                                CurePath = x.CurePath,
                                data_id = x.DataId,
                            });
                        }
                    }
                    else
                    {
                        iniDrOrdViewModel = IniDrOrdService.QueryIniDrOrd(iniDrOrdQueryModel);
                        foreach (var x in iniDrOrdViewModel)
                        {
                            dataQueryExcelViewModel.Add(new DataQueryExcelViewModel()
                            {
                                DataType = dtl.DataType,
                                DataId = dtl.DataId,
                                HospId = dtl.HospId,
                                FeeYm = dtl.FeeYm,
                                ExamYear = dtl.ExamYear,
                                ExamTime = dtl.ExamTime,
                                FirstTreatDate = dtl.FirstTreatDate,
                                WeekCount = dtl.WeekCount,
                                InstructExamYear = dtl.InstructExamYear,
                                InstructExamTime = dtl.InstructExamTime,
                                FirstInstructDate = dtl.FirstInstructDate,
                                InctructSerial = dtl.InctructSerial,
                                MedApply = dtl.MedApply,
                                InstructApply = dtl.InstructApply,
                                TraceApply = dtl.TraceApply,
                                ReleaseApply = dtl.ReleaseApply,
                                ApplType = dtl.ApplType,
                                ApplDate = dtl.ApplDate,
                                CaseType = dtl.CaseType,
                                SeqNo = dtl.SeqNo,
                                FuncType = dtl.FuncType,
                                FuncDate = dtl.FuncDate,
                                RelDate = dtl.RelDate,
                                Birthday = dtl.Birthday,
                                Id = dtl.Id,
                                FuncSeqNo = dtl.FuncSeqNo,
                                PayType = dtl.PayType,
                                PartCode = dtl.PartCode,
                                Icd9cmCode = dtl.Icd9cmCode,
                                Icd9cmCode1 = dtl.Icd9cmCode1,
                                Icd9cmCode2 = dtl.Icd9cmCode2,
                                DrugDays = dtl.DrugDays,
                                RelMode = dtl.RelMode,
                                PrsnId = dtl.PrsnId,
                                DrugPrsnId = dtl.DrugPrsnId,
                                DrugDot = dtl.DrugDot,
                                DiagCode = dtl.DiagCode,
                                DiagDot = dtl.DiagDot,
                                CureDot = dtl.CureDot,
                                DsvcCode = dtl.DsvcCode,
                                DsvcDot = dtl.DsvcDot,
                                ExpDot = dtl.ExpDot,
                                PartAmt = dtl.PartAmt,
                                ApplDot = dtl.ApplDot,
                                OrigHospId = dtl.OrigHospId,
                                IdSex = dtl.IdSex,
                                Remark = dtl.Remark,
                                CureItem1 = dtl.CureItem1,
                                CureItem2 = dtl.CureItem2,
                                CureItem3 = dtl.CureItem3,
                                CureItem4 = dtl.CureItem4,
                                OrigCaseType = dtl.OrigCaseType,
                                OtherPartAmt = dtl.OtherPartAmt,
                                ApplCauseMark = dtl.ApplCauseMark,
                                Icd10cmCode2 = dtl.Icd10cmCode2,
                                Icd10cmCode3 = dtl.Icd10cmCode3,
                                Icd10cmCode4 = dtl.Icd10cmCode4,
                                CorrHospId = dtl.CorrHospId,
                                AreaService = dtl.AreaService,
                                SuppArea = dtl.SuppArea,
                                RealHospId = dtl.RealHospId,
                                HospDataType = dtl.HospDataType,
                                AgencyPartAmt = dtl.AgencyPartAmt,
                                TranDate = dtl.TranDate,
                                Name = dtl.Name,
                                MetDot = dtl.MetDot,
                                OrderType = x.OrderType,
                                OrderCode = x.OrderCode,
                                Ord_RelMode = "",
                                ChrMark = "",
                                DrugNum = x.DrugNum,
                                DrugFre = x.DrugFre,
                                DrugPath = x.DrugPath,
                                OrderUprice = x.OrderUprice,
                                OrderQty = x.OrderQty,
                                OrderDot = x.OrderDot,
                                OrderDrugDay = x.OrderDrugDay,
                                ExePrsnId = x.ExePrsnId,
                                CurePath = "",
                                data_id = x.DataId,
                            });
                        }
                    }
                }
            }

            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<DataQueryExcelViewModel>(dataQueryExcelViewModel)
                    .DefineColumns((bindder) =>
                    {
                        //Dtl
                        bindder.ColumnFor(p => p.FeeYm, "費用年月", DictionaryDtl["費用年月"].Item2);
                        bindder.ColumnFor(p => p.ExamYear, "療程年度", DictionaryDtl["療程年度"].Item2);
                        bindder.ColumnFor(p => p.ExamTime, "療程次數", DictionaryDtl["療程次數"].Item2);
                        bindder.ColumnFor(p => p.WeekCount, "週數", DictionaryDtl["週數"].Item2);
                        bindder.ColumnFor(p => p.FirstTreatDate, "初診日", DictionaryDtl["初診日"].Item2);
                        bindder.ColumnFor(p => p.InstructExamYear, "療程年度(衛)", DictionaryDtl["療程年度(衛)"].Item2);
                        bindder.ColumnFor(p => p.InstructExamTime, "療程次數(衛)", DictionaryDtl["療程次數(衛)"].Item2);
                        bindder.ColumnFor(p => p.FirstInstructDate, "初診日(衛)", DictionaryDtl["初診日(衛)"].Item2);
                        bindder.ColumnFor(p => p.InctructSerial, "衛教序次", DictionaryDtl["衛教序次"].Item2);
                        bindder.ColumnFor(p => p.MedApply, "藥物申報", DictionaryDtl["藥物申報"].Item2);
                        bindder.ColumnFor(p => p.InstructApply, "衛教申報", DictionaryDtl["衛教申報"].Item2);
                        bindder.ColumnFor(p => p.TraceApply, "追蹤申報", DictionaryDtl["追蹤申報"].Item2);
                        bindder.ColumnFor(p => p.ReleaseApply, "釋出申報", DictionaryDtl["釋出申報"].Item2);
                        bindder.ColumnFor(p => p.ApplType, "申報類別", DictionaryDtl["申報類別"].Item2);
                        bindder.ColumnFor(p => p.HospId, "機構代碼", DictionaryDtl["機構代碼"].Item2);
                        bindder.ColumnFor(p => p.ApplDate, "申報日期", DictionaryDtl["申報日期"].Item2);
                        bindder.ColumnFor(p => p.CaseType, "案件類別", DictionaryDtl["案件類別"].Item2);
                        bindder.ColumnFor(p => p.SeqNo, "流水號", DictionaryDtl["流水號"].Item2);
                        bindder.ColumnFor(p => p.FuncType, "就醫科別", DictionaryDtl["就醫科別"].Item2);
                        bindder.ColumnFor(p => p.FuncDate, "就醫日期", DictionaryDtl["就醫日期"].Item2);
                        bindder.ColumnFor(p => p.RelDate, "調劑日期", DictionaryDtl["調劑日期"].Item2);
                        bindder.ColumnFor(p => p.Birthday, "出生日期", DictionaryDtl["出生日期"].Item2);
                        bindder.ColumnFor(p => p.Id, "身分證號", DictionaryDtl["身分證號"].Item2);
                        bindder.ColumnFor(p => p.FuncSeqNo, "就醫序號", DictionaryDtl["就醫序號"].Item2);
                        bindder.ColumnFor(p => p.PayType, "給付類別", DictionaryDtl["給付類別"].Item2);
                        bindder.ColumnFor(p => p.PartCode, "部分負擔代號", DictionaryDtl["部分負擔代號"].Item2);
                        bindder.ColumnFor(p => p.Icd9cmCode, "主診斷代碼", DictionaryDtl["主診斷代碼"].Item2);
                        bindder.ColumnFor(p => p.Icd9cmCode1, "次診斷代碼(一)", DictionaryDtl["次診斷代碼(一)"].Item2);
                        bindder.ColumnFor(p => p.Icd9cmCode2, "次診斷代碼(二)", DictionaryDtl["次診斷代碼(二)"].Item2);
                        bindder.ColumnFor(p => p.DrugDays, "給藥日份", DictionaryDtl["給藥日份"].Item2);
                        bindder.ColumnFor(p => p.RelMode, "調劑方式", DictionaryDtl["調劑方式"].Item2);
                        bindder.ColumnFor(p => p.PrsnId, "醫事人員身分證號", DictionaryDtl["醫事人員身分證號"].Item2);
                        bindder.ColumnFor(p => p.DrugPrsnId, "藥師身分證號", DictionaryDtl["藥師身分證號"].Item2);
                        bindder.ColumnFor(p => p.DrugDot, "藥費點數", DictionaryDtl["藥費點數"].Item2);
                        bindder.ColumnFor(p => p.CureDot, "診療費點數", DictionaryDtl["診療費點數"].Item2);
                        bindder.ColumnFor(p => p.DiagCode, "診察費代碼", DictionaryDtl["診察費代碼"].Item2);
                        bindder.ColumnFor(p => p.DiagDot, "診察費點數", DictionaryDtl["診察費點數"].Item2);
                        bindder.ColumnFor(p => p.DsvcCode, "藥事服務費代碼", DictionaryDtl["藥事服務費代碼"].Item2);
                        bindder.ColumnFor(p => p.DsvcDot, "藥事服務費點數", DictionaryDtl["藥事服務費點數"].Item2);
                        bindder.ColumnFor(p => p.ExpDot, "醫療費用點數", DictionaryDtl["醫療費用點數"].Item2);
                        bindder.ColumnFor(p => p.PartAmt, "部份負擔金額", DictionaryDtl["部份負擔金額"].Item2);
                        bindder.ColumnFor(p => p.ApplDot, "申請金額", DictionaryDtl["申請金額"].Item2);
                        bindder.ColumnFor(p => p.IdSex, "性別", DictionaryDtl["性別"].Item2);
                        bindder.ColumnFor(p => p.Remark, "修改備註", DictionaryDtl["修改備註"].Item2);
                        bindder.ColumnFor(p => p.CureItem1, "特定治療項目(一)", DictionaryDtl["特定治療項目(一)"].Item2);
                        bindder.ColumnFor(p => p.CureItem2, "特定治療項目(二)", DictionaryDtl["特定治療項目(二)"].Item2);
                        bindder.ColumnFor(p => p.CureItem3, "特定治療項目(三)", DictionaryDtl["特定治療項目(三)"].Item2);
                        bindder.ColumnFor(p => p.CureItem4, "特定治療項目(四)", DictionaryDtl["特定治療項目(四)"].Item2);
                        bindder.ColumnFor(p => p.OrigHospId, "釋出院所", DictionaryDtl["釋出院所"].Item2);
                        bindder.ColumnFor(p => p.AreaService, "特定地區醫療服務", DictionaryDtl["特定地區醫療服務"].Item2);
                        bindder.ColumnFor(p => p.SuppArea, "支援區域", DictionaryDtl["支援區域"].Item2);
                        bindder.ColumnFor(p => p.RealHospId, "實際提供醫療服務之醫事服務機構", DictionaryDtl["實際提供醫療服務之醫事服務機構"].Item2);
                        bindder.ColumnFor(p => p.HospDataType, "醫事類別", DictionaryDtl["醫事類別"].Item2);
                        bindder.ColumnFor(p => p.OrigCaseType, "原案件分類", DictionaryDtl["原案件分類"].Item2);
                        bindder.ColumnFor(p => p.AgencyPartAmt, "代辦部分負擔金額", DictionaryDtl["代辦部分負擔金額"].Item2);
                        bindder.ColumnFor(p => p.OtherPartAmt, "行政協助項目部分負擔點數", DictionaryDtl["行政協助項目部分負擔點數"].Item2);
                        bindder.ColumnFor(p => p.Name, "姓名", DictionaryDtl["姓名"].Item2);
                        bindder.ColumnFor(p => p.ApplCauseMark, "補報原因註記", DictionaryDtl["補報原因註記"].Item2);
                        bindder.ColumnFor(p => p.Icd10cmCode2, "國際疾病分類碼(三)", DictionaryDtl["國際疾病分類碼(三)"].Item2);
                        bindder.ColumnFor(p => p.Icd10cmCode3, "國際疾病分類碼(四)", DictionaryDtl["國際疾病分類碼(四)"].Item2);
                        bindder.ColumnFor(p => p.Icd10cmCode4, "國際疾病分類碼(五)", DictionaryDtl["國際疾病分類碼(五)"].Item2);
                        bindder.ColumnFor(p => p.MetDot, "特殊材料明細點數小計", DictionaryDtl["特殊材料明細點數小計"].Item2);
                        bindder.ColumnFor(p => p.CorrHospId, "矯正機關代號", DictionaryDtl["矯正機關代號"].Item2);
                        bindder.ColumnFor(p => p.data_id, "電腦序號", DictionaryDtl["電腦序號"].Item2);

                        //Ord
                        bindder.ColumnFor(p => p.OrderType, "醫令類別", DictionaryOrd["醫令類別"].Item2);
                        bindder.ColumnFor(p => p.OrderCode, "醫令代碼", DictionaryOrd["醫令代碼"].Item2);
                        bindder.ColumnFor(p => p.Ord_RelMode, "調劑方式", DictionaryOrd["調劑方式"].Item2);
                        bindder.ColumnFor(p => p.ChrMark, "連續處方註記", DictionaryOrd["連續處方註記"].Item2);
                        bindder.ColumnFor(p => p.DrugNum, "藥品用量", DictionaryOrd["藥品用量"].Item2);
                        bindder.ColumnFor(p => p.DrugFre, "藥品使用頻率", DictionaryOrd["藥品使用頻率"].Item2);
                        bindder.ColumnFor(p => p.DrugPath, "用藥途徑/作用部位", DictionaryOrd["用藥途徑/作用部位"].Item2);
                        bindder.ColumnFor(p => p.OrderUprice, "醫令單價", DictionaryOrd["醫令單價"].Item2);
                        bindder.ColumnFor(p => p.OrderQty, "醫令數量", DictionaryOrd["醫令數量"].Item2);
                        bindder.ColumnFor(p => p.OrderDot, "醫令點數", DictionaryOrd["醫令點數"].Item2);
                        bindder.ColumnFor(p => p.ExePrsnId, "執行醫事人員代號", DictionaryOrd["執行醫事人員代號"].Item2);
                        bindder.ColumnFor(p => p.Note, "備註", DictionaryOrd["備註"].Item2);
                        bindder.ColumnFor(p => p.CurePath, "診療部位", DictionaryOrd["診療部位"].Item2);
                        bindder.ColumnFor(p => p.OrderDrugDay, "給藥日份", DictionaryOrd["給藥日份"].Item2);
                        bindder.ColumnFor(p => p.DataId, "電腦序號", DictionaryOrd["電腦序號"].Item2);
                    })
                    .GetResult(SheetName);
            });
            var fileName = $"All_Medical_Orders_and_Points.{fileType.ToString()}";
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            var contentDisposition = new ContentDispositionHeaderValue("attachment");
            contentDisposition.SetHttpFileName(fileName);
            Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

            return new FileContentResult(excel, contentType);
        }
    }
}