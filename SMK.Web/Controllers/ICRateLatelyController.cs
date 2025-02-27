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
    public class ICRateLatelyController : BaseController
    {
        //private readonly string _folder;

        public ICRateLatelyService iCRateLatelyService { get; }

        public ICRateLatelyController(ICRateLatelyService iCRateLatelyService,
            IWebHostEnvironment env)
        {
            this.iCRateLatelyService = iCRateLatelyService;
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
        public async Task<IActionResult> QueryICRateLately(ICRateLatelyQueryModel model)
        {
            var ret = await iCRateLatelyService.GetiCRateLately(model);
            return Json(ret);
        }

        [HttpPost, ActionName("ExportICRateLately")]
        public async Task<IActionResult> ExportICRateLately(ICRateLatelyQueryModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            var logicRtnModel = await iCRateLatelyService.GetiCRateLately(model); ;
            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }
            var list = logicRtnModel.Data.Data;
            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<ICRateLatelyViewModel>(list)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.LastContType, "醫事機構層級");
                        bindder.ColumnFor(p => p.HospDataType, "醫事類別");
                        bindder.ColumnFor(p => p.HospID, "醫事機構代碼");
                        bindder.ColumnFor(p => p.HospSeqNo, "院區別");
                        bindder.ColumnFor(p => p.HospName, "醫事機構名稱");
                        bindder.ColumnFor(p => p.FeeYM, "費用年月");
                        bindder.ColumnFor(p => p.MedIC, "健保卡上傳/登錄筆數(用藥)");
                        bindder.ColumnFor(p => p.MedApply, "健保申報筆數(用藥)");
                        bindder.ColumnFor(p => p.MedRate, "健保卡上傳/登錄率(用藥)");
                        bindder.ColumnFor(p => p.InsIC, "健保卡上傳/登錄筆數(衛教)");
                        bindder.ColumnFor(p => p.InsApply, "健保申報筆數(衛教)");
                        bindder.ColumnFor(p => p.InsRate, "健保卡上傳/登錄率(衛教)");
                    })
                    .GetResult();
            });
            var fileName = $"層級別醫院別單月過卡率.{fileType.ToString()}";
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
