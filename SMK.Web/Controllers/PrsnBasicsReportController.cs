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
    public class PrsnBasicsReportController : BaseController
    {
        private readonly PrsnBasicsService prsnBasicsService;

        public PrsnBasicsReportController(PrsnBasicsService prsnBasicsService)
        {
            this.prsnBasicsService = prsnBasicsService;
        }

        /// <summary>
        /// 查詢醫事人員清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Query(PrsnBasicQueryModel model)
        {
            var list = await prsnBasicsService.QueryPrsnContactList(model);
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
        public async Task<IActionResult> Export(PrsnBasicQueryModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            var logicRtnModel = await prsnBasicsService.QueryPrsnContactList(model);
            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }
            var list = logicRtnModel.Data.Data;
            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<PrsnContactReportViewModel>(list)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.身分證號, "身分證號");
                        bindder.ColumnFor(p => p.姓名, "姓名");
                        bindder.ColumnFor(p => p.出生日期, "出生日期");
                        bindder.ColumnFor(p => p.人員類別, "人員類別");
                        bindder.ColumnFor(p => p.醫事機構代碼, "醫事機構代碼");
                        bindder.ColumnFor(p => p.院區別, "院區別");
                        bindder.ColumnFor(p => p.機構名稱, "機構名稱");
                        bindder.ColumnFor(p => p.機構狀態, "機構狀態");
                        bindder.ColumnFor(p => p.服務類型, "服務類型");
                        bindder.ColumnFor(p => p.人員合約起日, "人員合約起日");
                        bindder.ColumnFor(p => p.人員合約迄日, "人員合約迄日");
                    })
                    .GetResult();
            });
            var fileName = $"醫事人員名冊.{fileType.ToString()}";
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
