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
    public class ICRateByMonthController : BaseController
    {
        //private readonly string _folder;

        public ICRateByMonthService iCRateByMonthService { get; }

        public ICRateByMonthController(ICRateByMonthService iCRateByMonthService,
            IWebHostEnvironment env)
        {
            this.iCRateByMonthService = iCRateByMonthService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 未過卡詳細資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QueryICRateByMonth(ICRateByMonthQueryModel model)
        {
            var ret = await iCRateByMonthService.GetICRateByMonth(model);
            return Json(ret);
        }

        [HttpPost, ActionName("ExportICRateByMonth")]
        public async Task<IActionResult> ExportICRateByMonth(ICRateByMonthQueryModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            var logicRtnModel = await iCRateByMonthService.GetICRateByMonth(model); ;
            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }
            var list = logicRtnModel.Data.Data;
            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<ICRateByMonthViewModel>(list)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.LastContType, "醫事層級");
                        bindder.ColumnFor(p => p.HospDataType, "醫事類別");
                        bindder.ColumnFor(p => p.HospID, "醫事機構代碼");
                        bindder.ColumnFor(p => p.HospSeqNo, "院區別");
                        bindder.ColumnFor(p => p.HospName, "醫事機構名稱");
                        bindder.ColumnFor(p => p.FeeYM, "費用年月");
                        bindder.ColumnFor(p => p.CureType, "治療型態");
                        bindder.ColumnFor(p => p.samples, "樣本");
                        bindder.ColumnFor(p => p.Rate, "過卡率(%)");
                        bindder.ColumnFor(p => p.NoRate, "未過卡率(%)");
                    })
                    .GetResult();
            });
            var fileName = $"層級別醫院別每月過卡率.{fileType.ToString()}";
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
