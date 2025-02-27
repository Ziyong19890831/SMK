using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Web.AppScope.Filters;
using SMK.Web.Models;
using SMK.Web.Services.Foundation;
using System;
using System.Threading.Tasks;
using Yozian.WebCore.Library.Utility.Excel;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    public class CorrectionSlipController : BaseController
    {
        private readonly CorrectionSlipService correctionSlipService;

        public CorrectionSlipController(CorrectionSlipService correctionSlipService)
        {
            this.correctionSlipService = correctionSlipService;
        }

        public IActionResult Index()
        {
            return View(new CorrectionSlipQueryModel());
        }

        [HttpPost]
        public async Task<IActionResult> Upload(CorrectionSlipQueryModel query)
        {
            LogicRtnModel<CorrectionSlipQueryModel> logicRtnModel = await correctionSlipService.UploadCorrectionSlip(query.file);
            return Json(logicRtnModel);
        }
        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Query(CorrectionSlipQueryModel query)
        {
            var list = await correctionSlipService.QueryCorrectionSlip(query);
            return Json(list);
        }
        [HttpPost, ActionName("Export")]
        public async Task<IActionResult> downloadMonthlyReport(CorrectionSlipQueryModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            var logicRtnModel = await correctionSlipService.Export(model);
            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }
            var list = logicRtnModel.Data.Data;
            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<CorrectionSlipExport>(list)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.HospId, "機構代碼");
                        bindder.ColumnFor(p => p.HospSeqNo, "院區別");
                        bindder.ColumnFor(p => p.HospName, "機構名稱");
                        bindder.ColumnFor(p => p.Jan2, "1月次數");
                        bindder.ColumnFor(p => p.Jan, "1月筆數");
                        bindder.ColumnFor(p => p.Feb2, "2月次數");
                        bindder.ColumnFor(p => p.Feb, "2月筆數");
                        bindder.ColumnFor(p => p.Mar2, "3月次數");
                        bindder.ColumnFor(p => p.Mar, "3月筆數");
                        bindder.ColumnFor(p => p.Apr2, "4月次數");
                        bindder.ColumnFor(p => p.Apr, "4月筆數");
                        bindder.ColumnFor(p => p.May2, "5月次數");
                        bindder.ColumnFor(p => p.May, "5月筆數");
                        bindder.ColumnFor(p => p.Jun2, "6月次數");
                        bindder.ColumnFor(p => p.Jun, "6月筆數");
                        bindder.ColumnFor(p => p.Jul2, "7月次數");
                        bindder.ColumnFor(p => p.Jul, "7月筆數");
                        bindder.ColumnFor(p => p.Aug2, "8月次數");
                        bindder.ColumnFor(p => p.Aug, "8月筆數");
                        bindder.ColumnFor(p => p.Sep2, "9月次數");
                        bindder.ColumnFor(p => p.Sep, "9月筆數");
                        bindder.ColumnFor(p => p.Oct2, "10月次數");
                        bindder.ColumnFor(p => p.Oct, "10月筆數");
                        bindder.ColumnFor(p => p.Nov2, "11月次數");
                        bindder.ColumnFor(p => p.Nov, "11月筆數");
                        bindder.ColumnFor(p => p.Dec2, "12月次數");
                        bindder.ColumnFor(p => p.Dec, "12月筆數");
                    })
                    .GetResult();
            });
            var fileName = $"CorrectionSlip.{fileType.ToString()}";
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
        // GET: Branch/Create
        public IActionResult Create()
        {
            return View(new CorrectionSlip());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( CorrectionSlip correctionSlip)
        {
            if (!ModelState.IsValid)
            {
                return View(correctionSlip);
            }
            correctionSlip.UpdatedBy = User.Identity.Name;
            var rtnModel = await correctionSlipService.Create(correctionSlip, null);
            if (rtnModel.IsSuccess)
            {
                return RedirectTo(rtnModel, nameof(this.Index));
            }
            else
            {
                return View(rtnModel);
            }
        }
    }
}