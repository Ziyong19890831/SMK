using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMK.Web.Services.Foundation;
using SMK.Web.Models;
using Yozian.WebCore.Library.Utility.Excel;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using SMK.Data.Enums;

namespace SMK.Web.Controllers
{
    public class RegularMonthlyReportController : BaseController
    {
        private readonly RegularMonthlyReportService _regularMonthlyReportService;
        public RegularMonthlyReportController(RegularMonthlyReportService regularMonthlyReportService)
        {
            this._regularMonthlyReportService = regularMonthlyReportService;
        }
        public IActionResult Index()
        {
            RegularMonthlyReportQueryModel regularMonthlyReportQueryModel = new RegularMonthlyReportQueryModel();
            return View(regularMonthlyReportQueryModel);
        }

        [HttpPost]
        public async Task<IActionResult> ExportTotalTable(RegularMonthlyReportQueryModel model, ExcelType fileType = ExcelType.xlsx)
        {
            model.Start = 0;
            model.Length = 999999;
            var logicRtnModel = await _regularMonthlyReportService.ExportTotalTable(model);
            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }
            var list = logicRtnModel.Data.Data;
            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<ExportTotalTableResult>(list)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.年度, "年度");
                        bindder.ColumnFor(p => p.合約機構數_年底, "合約機構數_排除解約機構");
                        bindder.ColumnFor(p => p.合約人員數_年度, "合約機構數_累計(含解約機構)");
                        bindder.ColumnFor(p => p.執行機構數, "執行機構數_累計(未排除解約人員)");
                        bindder.ColumnFor(p => p.合約人員數_年底, "合約人員數_排除解約人員");
                        bindder.ColumnFor(p => p.合約人員數_年度, "合約人員數_累計(含解約人員)");
                        bindder.ColumnFor(p => p.執行人員數_年度, "執行人員數_累計(未排除解約人員)");
                        bindder.ColumnFor(p => p.人次, "總計人次(用藥 + 衛教)");
                        bindder.ColumnFor(p => p.用藥人次, "用藥人次");
                        bindder.ColumnFor(p => p.衛教人次, "衛教人次");
                        bindder.ColumnFor(p => p.人數, "總計人數(用藥 + 衛教)");
                        bindder.ColumnFor(p => p.用藥人數, "用藥人數");
                        bindder.ColumnFor(p => p.衛教人數, "衛教人數");
                        bindder.ColumnFor(p => p.用藥_衛教人數, "用藥+衛教人數");
                        bindder.ColumnFor(p => p.給藥週數, "給藥週數");
                        
                    })
                    .GetResult();
            });
            var fileName = $"總計表.{fileType.ToString()}";
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

        [HttpPost]
        public async Task<IActionResult> ExportCategoryTable(RegularMonthlyReportQueryModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            var logicRtnModel = await _regularMonthlyReportService.ExportCategoryList(model);
            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }
            var list = logicRtnModel.Data;
            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<ExportCategoryList>(list)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.類別, "類別");
                        bindder.ColumnFor(p => p.用藥人次, "用藥人次");
                        bindder.ColumnFor(p => p.衛教人次, "衛教人次");
                        bindder.ColumnFor(p => p.申報人次, "申報人次");
                        bindder.ColumnFor(p => p.用藥人數, "用藥人數");
                        bindder.ColumnFor(p => p.衛教人數, "衛教人數");

                        bindder.ColumnFor(p => p.平均每人給藥次數, "平均每人給藥次數(人次 / 人數)");
                        bindder.ColumnFor(p => p.平均每人衛教次數, "平均每人衛教次數(人次 / 人數)");
                        bindder.ColumnFor(p => p.用藥_衛教人數, "用藥加衛教人數");
                        bindder.ColumnFor(p => p.申報人數, "申報人數");

                        bindder.ColumnFor(p => p.用藥週數, "用藥週數");
                        bindder.ColumnFor(p => p.平均每人用藥週數, "平均用藥週數(人數 / 週數)");
                        bindder.ColumnFor(p => p.申報金額, "申報金額");
                        //N
                        bindder.ColumnFor(p => p.合約機構數_年底, "合約機構數_排除解約機構");
                        //O
                        bindder.ColumnFor(p => p.合約機構數_年度, "合約機構數_累計(含解約機構)");
                        //P
                        bindder.ColumnFor(p => p.執行機構數, "執行機構數");
                        //Q
                        bindder.ColumnFor(p => p.合約人員數_年底, "合約人員數_排除解約人員");
                        //R
                        bindder.ColumnFor(p => p.合約人員數_年度, "合約人員數_累計(含解約人員)");
                        //S
                        bindder.ColumnFor(p => p.執行人員數, "執行人員數");
                        


                    })
                    .GetResult();
            });
            var fileName = $"類別表.{fileType.ToString()}";
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
