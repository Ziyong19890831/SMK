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
    public class ICNotFoundController : BaseController
    {
        //private readonly string _folder;

        public ICNotFoundService iCNotFoundService { get; }

        public ICNotFoundController(ICNotFoundService iCNotFoundService,
            IWebHostEnvironment env)
        {
            this.iCNotFoundService = iCNotFoundService;
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
        public async Task<IActionResult> QueryICNotFound(ICNotFoundQueryModel model)
        {
            var ret = await iCNotFoundService.GetICNotFound(model);
            return Json(ret);
        }

        [HttpPost, ActionName("ExportICNotFound")]
        public async Task<IActionResult> ExportICNotFound(ICNotFoundQueryModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            var logicRtnModel = await iCNotFoundService.GetICNotFound(model); ;
            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }
            var list = logicRtnModel.Data.Data;
            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<ICNotFoundViewModel>(list)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.BranchName, "健保業務組");
                        bindder.ColumnFor(p => p.LastContType, "醫事機構層級");
                        bindder.ColumnFor(p => p.HospID, "醫事機構代碼");
                        bindder.ColumnFor(p => p.HospSeqNo, "院區別");
                        bindder.ColumnFor(p => p.HospName, "醫事機構名稱");
                        bindder.ColumnFor(p => p.HospDataType, "醫事類別2");
                        bindder.ColumnFor(p => p.ID, "身分證號");
                        bindder.ColumnFor(p => p.Birthday, "出生日期");
                        bindder.ColumnFor(p => p.FuncDate, "就醫日期");
                        bindder.ColumnFor(p => p.FeeYM, "費用年月");
                        bindder.ColumnFor(p => p.Data_id, "電腦序號");
                        bindder.ColumnFor(p => p.CaseType, "案件分類");
                        bindder.ColumnFor(p => p.SeqNo, "流水號");
                        bindder.ColumnFor(p => p.Real_HospID, "實際執行醫院代碼");
                        bindder.ColumnFor(p => p.CureType, "治療型態");
                        bindder.ColumnFor(p => p.ExpDot, "醫令點數總和");
                        bindder.ColumnFor(p => p.Note, "Note");
                    })
                    .GetResult();
            });
            var fileName = $"未過卡詳細資料.{fileType.ToString()}";
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
