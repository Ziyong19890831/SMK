using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using SMK.Data.Enums;
using SMK.Web.AppScope.Filters;
using SMK.Web.Extensions;
using SMK.Web.Models;
using SMK.Web.Services.Foundation;
using System;
using System.Threading.Tasks;
using Yozian.WebCore.Library.Utility.Excel;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    public class IniReportController : BaseController
    {
        //private readonly string _folder;

        public IniReportService IniReportService { get; }

        public IniReportController(IniReportService declareReportService,
            IWebHostEnvironment env)
        {
            this.IniReportService = declareReportService;
        }

        public IActionResult Index()
        {
            return View(new IniReportQueryModel()
            {
                ContractYmS = "091/09",
                ContractYmE = DateTime.Now.AddMonths(-1).ToString("yyyyMM").ToSlashTaiwanDateFromYYYYMM(),
                NhiYmS = "091/09",
                NhiYmE = DateTime.Now.AddMonths(-3).ToString("yyyyMM").ToSlashTaiwanDateFromYYYYMM()
            });

        }

        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Query(IniReportQueryModel model)
        {
            var list = await IniReportService.GetIniReports(model);
            return Json(list);
        }

        [HttpPost, ActionName("Export")]
        public async Task<IActionResult> Export(IniReportQueryModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            var logicRtnModel = await IniReportService.GetIniReports(model);
            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }
            var list = logicRtnModel.Data.Data;
            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<IniReportViewModel>(list)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.ContractYM, "合約年度");
                        bindder.ColumnFor(p => p.ContractTotal, "合約機構數_排除解約機構");
                        bindder.ColumnFor(p => p.ContractAllTotal, "合約機構數_累計(含解約機構)");
                        bindder.ColumnFor(p => p.RunTimeContractAllTotal, "執行機構數_累計(未排除解約人員)");
                        bindder.ColumnFor(p => p.ContractPersonTotal, "合約人員數_排除解約人員");
                        bindder.ColumnFor(p => p.ContractPersonAllTotal, "合約人員數_累計(含解約人員)");
                        bindder.ColumnFor(p => p.RunTimeContractPersonAllTotal, "執行人員數_累計(未排除解約人員)");
                        bindder.ColumnFor(p => p.NhiYM, "健保年度");
                        bindder.ColumnFor(p => p.TreatInstructCnt, "總計人次(用藥+衛教)");
                        bindder.ColumnFor(p => p.TreatCnt, "用藥人次");
                        bindder.ColumnFor(p => p.InstructCnt, "衛教人次");
                        bindder.ColumnFor(p => p.TreatInstructSum, "總計人數(用藥+衛教)");
                        bindder.ColumnFor(p => p.TreatSum, "用藥人數");
                        bindder.ColumnFor(p => p.InstructSum, "衛教人數");
                        bindder.ColumnFor(p => p.TreatAddInstruct, "用藥 + 衛教人數");
                        bindder.ColumnFor(p => p.TreatWeek, "給藥週數");
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
