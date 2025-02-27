using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Web.AppScope.Filters;
using SMK.Web.Models;
using SMK.Web.Services.Foundation;
using SMK.Web.Validator;
using Yozian.WebCore.Library.Utility.Excel;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    public class ServiceReportController : BaseController
    {
        private readonly ServiceReportService serviceReportService;

        public ServiceReportController(ServiceReportService serviceReportService)
        {
            this.serviceReportService = serviceReportService;
        }


        /// <summary>
        /// 查詢醫事人員清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Query(ServiceReportQueryModel model)
        {
            var list = await serviceReportService.QueryServiceReportData(model);
            return Json(list);
        }

        /// <summary>
        /// 醫事人員清單
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("Export")]
        public async Task<IActionResult> Export(ServiceReportQueryModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            var logicRtnModel = await serviceReportService.QueryServiceReportData(model);
            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }
            var list = logicRtnModel.Data.Data;
            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<ServiceReportViewModel>(list)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.HospID, "機構代碼");
                        bindder.ColumnFor(p => p.HospSeqNo, "院區別");
                        bindder.ColumnFor(p => p.HospName, "機構名稱");
                        bindder.ColumnFor(p => p.HospContName, "機構層級");
                        bindder.ColumnFor(p => p.Year, "年度");
                        bindder.ColumnFor(p => p.TopTreatCount, "治療人次上限");
                        bindder.ColumnFor(p => p.TopInstructCount, "衛教人次上限");
                        bindder.ColumnFor(p => p.TreatReal, "實際治療人次");
                        bindder.ColumnFor(p => p.InstructReal, "實際衛教人次");         
                        bindder.ColumnFor(p => p.TreatSussueRate, "治療服務達成率(%)");
                        bindder.ColumnFor(p => p.InstructSussueRate, "衛教服務達成率(%)");
                    })
                    .GetResult();
            });
            var fileName = $"服務人次名冊.{fileType.ToString()}";
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
