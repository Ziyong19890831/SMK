using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Web.AppScope.Filters;
using SMK.Web.Models;
using SMK.Web.Services.Foundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Yozian.WebCore.Library.Utility.Excel;
using Microsoft.Net.Http.Headers;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    public class DeclareReportController : BaseController
    {
        //private readonly string _folder;

        public DeclareReportService DeclareReportService { get; }

        public DeclareReportController(DeclareReportService declareReportService,
            IWebHostEnvironment env)
        {
            this.DeclareReportService = declareReportService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 醫院查詢
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QueryIniOpDtl(DeclareReportQueryModel model)
        {
            var ret = await DeclareReportService.GetIniOpDtl(model);
            return Json(ret);
        }
        /// <summary>
        /// 藥局查詢
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QueryIniDrDtl(DeclareReportQueryModel model)
        {
            var ret = await DeclareReportService.GetIniDrDtl(model);
            return Json(ret);
        }

        /// <summary>
        /// 判斷要查詢哪個資料庫
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Query_IniOpDtl_IniDrDtl(DeclareReportQueryModel model)
        {
            //判斷機構代碼是否為null，不判斷的話會產生Bug
            string check_number = model.HospID != null ? model.HospID.ToString()[0].ToString() : string.Empty;
            //如果機構代碼第一碼是5，就執行尋找藥局，其他找醫院
            var query_IniOpDtl_IniDrDtl = check_number == "5" ? await DeclareReportService.GetIniDrDtl(model) : await DeclareReportService.GetIniOpDtl(model);
            return Json(query_IniOpDtl_IniDrDtl);
        }

        [HttpPost, ActionName("ExportIniOpDtl")]
        public async Task<IActionResult> ExportIniOpDtl(DeclareReportQueryModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            var logicRtnModel = await DeclareReportService.GetIniOpDtl(model);
            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }
            var list = logicRtnModel.Data.Data;
            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<DeclareReportViewModel>(list)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.DataId, "電腦序號");
                        bindder.ColumnFor(p => p.FeeYm, "費用年月");
                        bindder.ColumnFor(p => p.ExamYear, "療程年度");
                        bindder.ColumnFor(p => p.ExamTime, "療程次數");
                        bindder.ColumnFor(p => p.FirstTreatDate, "初診日");
                        bindder.ColumnFor(p => p.WeekCount, "週數");
                        bindder.ColumnFor(p => p.InstructExamYear, "衛教療程年度");
                        bindder.ColumnFor(p => p.InstructExamTime, "衛教療程次數");
                        bindder.ColumnFor(p => p.FirstInstructDate, "衛教初診日");
                        bindder.ColumnFor(p => p.InctructSerial, "衛教序次");
                        bindder.ColumnFor(p => p.MedApply, "藥物申報");
                        bindder.ColumnFor(p => p.InstructApply, "衛教申報");
                        bindder.ColumnFor(p => p.TraceApply, "追蹤申報");
                        bindder.ColumnFor(p => p.ReleaseApply, "釋出申報");
                        bindder.ColumnFor(p => p.ApplType, "申報類別");
                        bindder.ColumnFor(p => p.HospId, "機構代碼");
                        bindder.ColumnFor(p => p.ApplDate, "申報日期");
                        bindder.ColumnFor(p => p.CaseType, "案件類別");
                        bindder.ColumnFor(p => p.SeqNo, "流水號");
                        bindder.ColumnFor(p => p.FuncType, "就醫科別");
                        bindder.ColumnFor(p => p.FuncDate, "就醫日期");
                        bindder.ColumnFor(p => p.CureEDate, "治療結束日期");
                        bindder.ColumnFor(p => p.Birthday, "出生日期");
                        bindder.ColumnFor(p => p.Id, "身分證號");
                        bindder.ColumnFor(p => p.FuncSeqNo, "就醫序號");
                        bindder.ColumnFor(p => p.PayType, "給付類別");
                        bindder.ColumnFor(p => p.PartCode, "部分負擔代號");
                        bindder.ColumnFor(p => p.Icd9cmCode, "主診斷代碼");
                        bindder.ColumnFor(p => p.Icd9cmCode1, "次診斷代碼(一)");
                        bindder.ColumnFor(p => p.Icd9cmCode2, "次診斷代碼(二)");
                        bindder.ColumnFor(p => p.DrugDays, "給藥日份");
                        bindder.ColumnFor(p => p.RelMode, "處方調劑方式");
                        bindder.ColumnFor(p => p.PrsnId, "醫事人員身分證號");
                        bindder.ColumnFor(p => p.DrugPrsnId, "藥師身分證號");
                        bindder.ColumnFor(p => p.DrugDot, "藥費點數");
                        bindder.ColumnFor(p => p.CureDot, "診療費點數");
                        bindder.ColumnFor(p => p.DiagCode, "診察費代碼");
                        bindder.ColumnFor(p => p.DiagDot, "診察費點數");
                        bindder.ColumnFor(p => p.DsvcCode, "藥事服務費代碼");
                        bindder.ColumnFor(p => p.DsvcDot, "藥事服務費點數");
                        bindder.ColumnFor(p => p.ExpDot, "醫療費用點數");
                        bindder.ColumnFor(p => p.PartAmt, "部份負擔金額");
                        bindder.ColumnFor(p => p.ApplDot, "申請金額");
                        bindder.ColumnFor(p => p.IdSex, "性別");
                        bindder.ColumnFor(p => p.CureItem1, "特定治療項目(一)");
                        bindder.ColumnFor(p => p.CureItem2, "特定治療項目(二)");
                        bindder.ColumnFor(p => p.CureItem3, "特定治療項目(三)");
                        bindder.ColumnFor(p => p.CureItem4, "特定治療項目(四)");
                        bindder.ColumnFor(p => p.AreaService, "特定地區醫療服務");
                        bindder.ColumnFor(p => p.SuppArea, "支援區域");
                        bindder.ColumnFor(p => p.RealHospId, "實際提供醫療服務之醫事服務機構代號");
                        bindder.ColumnFor(p => p.HospDataType, "醫事類別");
                        bindder.ColumnFor(p => p.AgencyPartAmt, "代辦部分負擔金額");
                        bindder.ColumnFor(p => p.Name, "姓名");
                        bindder.ColumnFor(p => p.ApplCauseMark, "補報原因註記");
                        bindder.ColumnFor(p => p.Icd10cmCode3, "國際疾病分類碼(四)");
                        bindder.ColumnFor(p => p.Icd10cmCode4, "國際疾病分類碼(五)");
                        bindder.ColumnFor(p => p.MetDot, "特殊材料明細點數小計");
                        bindder.ColumnFor(p => p.CorrHospId, "矯正機關代號");
                        bindder.ColumnFor(p => p.TranDate, "轉檔日期");
                    })
                    .GetResult();
            });
            var fileName = $"申報資料.{fileType.ToString()}";
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
        [HttpPost, ActionName("ExportIniDrDtl")]
        public async Task<IActionResult> ExportIniDrDtl(DeclareReportQueryModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            var logicRtnModel = await DeclareReportService.GetIniDrDtl(model);
            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }
            var list = logicRtnModel.Data.Data;
            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<DeclareReportViewModel>(list)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.DataId, "電腦序號");
                        bindder.ColumnFor(p => p.FeeYm, "費用年月");
                        bindder.ColumnFor(p => p.ExamYear, "療程年度");
                        bindder.ColumnFor(p => p.ExamTime, "療程次數");
                        bindder.ColumnFor(p => p.FirstTreatDate, "初診日");
                        bindder.ColumnFor(p => p.WeekCount, "週數");
                        bindder.ColumnFor(p => p.InstructExamYear, "衛教療程年度");
                        bindder.ColumnFor(p => p.InstructExamTime, "衛教療程次數");
                        bindder.ColumnFor(p => p.FirstInstructDate, "衛教初診日");
                        bindder.ColumnFor(p => p.InctructSerial, "衛教序次");
                        bindder.ColumnFor(p => p.MedApply, "藥物申報");
                        bindder.ColumnFor(p => p.InstructApply, "衛教申報");
                        bindder.ColumnFor(p => p.TraceApply, "追蹤申報");
                        bindder.ColumnFor(p => p.ReleaseApply, "釋出申報");
                        bindder.ColumnFor(p => p.ApplType, "申報類別");
                        bindder.ColumnFor(p => p.HospId, "機構代碼");
                        bindder.ColumnFor(p => p.ApplDate, "申報日期");
                        bindder.ColumnFor(p => p.CaseType, "案件類別");
                        bindder.ColumnFor(p => p.SeqNo, "流水號");
                        bindder.ColumnFor(p => p.FuncType, "就醫科別");
                        bindder.ColumnFor(p => p.FuncDate, "就醫日期");
                        bindder.ColumnFor(p => p.CureEDate, "治療結束日期");
                        bindder.ColumnFor(p => p.Birthday, "出生日期");
                        bindder.ColumnFor(p => p.Id, "身分證號");
                        bindder.ColumnFor(p => p.FuncSeqNo, "就醫序號");
                        bindder.ColumnFor(p => p.PayType, "給付類別");
                        bindder.ColumnFor(p => p.PartCode, "部分負擔代號");
                        bindder.ColumnFor(p => p.Icd9cmCode, "主診斷代碼");
                        bindder.ColumnFor(p => p.Icd9cmCode1, "次診斷代碼(一)");
                        bindder.ColumnFor(p => p.Icd9cmCode2, "次診斷代碼(二)");
                        bindder.ColumnFor(p => p.DrugDays, "給藥日份");
                        bindder.ColumnFor(p => p.RelMode, "處方調劑方式");
                        bindder.ColumnFor(p => p.PrsnId, "醫事人員身分證號");
                        bindder.ColumnFor(p => p.DrugPrsnId, "藥師身分證號");
                        bindder.ColumnFor(p => p.DrugDot, "藥費點數");
                        bindder.ColumnFor(p => p.CureDot, "診療費點數");
                        bindder.ColumnFor(p => p.DiagCode, "診察費代碼");
                        bindder.ColumnFor(p => p.DiagDot, "診察費點數");
                        bindder.ColumnFor(p => p.DsvcCode, "藥事服務費代碼");
                        bindder.ColumnFor(p => p.DsvcDot, "藥事服務費點數");
                        bindder.ColumnFor(p => p.ExpDot, "醫療費用點數");
                        bindder.ColumnFor(p => p.PartAmt, "部份負擔金額");
                        bindder.ColumnFor(p => p.ApplDot, "申請金額");
                        bindder.ColumnFor(p => p.IdSex, "性別");
                        bindder.ColumnFor(p => p.CureItem1, "特定治療項目(一)");
                        bindder.ColumnFor(p => p.CureItem2, "特定治療項目(二)");
                        bindder.ColumnFor(p => p.CureItem3, "特定治療項目(三)");
                        bindder.ColumnFor(p => p.CureItem4, "特定治療項目(四)");
                        bindder.ColumnFor(p => p.AreaService, "特定地區醫療服務");
                        bindder.ColumnFor(p => p.SuppArea, "支援區域");
                        bindder.ColumnFor(p => p.RealHospId, "實際提供醫療服務之醫事服務機構代號");
                        bindder.ColumnFor(p => p.HospDataType, "醫事類別");
                        bindder.ColumnFor(p => p.AgencyPartAmt, "代辦部分負擔金額");
                        bindder.ColumnFor(p => p.Name, "姓名");
                        bindder.ColumnFor(p => p.ApplCauseMark, "補報原因註記");
                        bindder.ColumnFor(p => p.Icd10cmCode3, "國際疾病分類碼(四)");
                        bindder.ColumnFor(p => p.Icd10cmCode4, "國際疾病分類碼(五)");
                        bindder.ColumnFor(p => p.MetDot, "特殊材料明細點數小計");
                        bindder.ColumnFor(p => p.CorrHospId, "矯正機關代號");
                        bindder.ColumnFor(p => p.TranDate, "轉檔日期");
                    })
                    .GetResult();
            });
            var fileName = $"申報資料.{fileType.ToString()}";
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
