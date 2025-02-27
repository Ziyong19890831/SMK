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
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Web.Models;
using SMK.Web.Services.Foundation;
using Yozian.WebCore.Library.Utility.Excel;

namespace SMK.Web.Controllers
{
    /// <summary>
    /// 醫事人員合約
    /// </summary>
    public class PrsnContractController : BaseController
    {
        private readonly PrsnContractService prsnContractService;

        public PrsnContractController(PrsnContractService prsnContractService)
        {
            this.prsnContractService = prsnContractService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Application_Upload(PrsnContractQueryModel query)
        {
            LogicRtnModel<PrsnContractQueryModel> logicRtnModel = await prsnContractService.UploadApplication(query.file);
            return Json(logicRtnModel);
        }


        public IActionResult Query()
        {
            return View(new PrsnContractQueryModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Query(PrsnContractQueryModel model) {
            var logicRtnModel= await prsnContractService.GetPrsnContracts(model);
            return Json(logicRtnModel);
        }

        /// <summary>
        /// 查詢申請概況
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QueryApplication(PrsnContractQueryModel model)
        {
            var logicRtnModel = await prsnContractService.GetInsertPrsnContracts(model);
            return Json(logicRtnModel);
        }

        /// <summary>
        /// 列印申請概況
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        [HttpPost, ActionName("ExportyPrsnApplication")]
        public async Task<IActionResult> ExportyPrsnApplication(PrsnContractQueryModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            var logicRtnModel = await prsnContractService.GetInsertPrsnContracts(model);
            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }
            var list = logicRtnModel.Data.Data;
            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<ApplicationPrsnContractViewModel>(list)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.FeeYM, "收件年月");
                        bindder.ColumnFor(p => p.FeeYMD, "收件日");
                        bindder.ColumnFor(p => p.Application_Type, "申請類型");
                        bindder.ColumnFor(p => p.Change_Type, "異動類型");
                        bindder.ColumnFor(p => p.HospID, "院所代碼");
                        bindder.ColumnFor(p => p.HospSeqNo, "院區別");
                        bindder.ColumnFor(p => p.HospName, "院所名稱");
                        bindder.ColumnFor(p => p.UserName, "醫事人員姓名");
                        bindder.ColumnFor(p => p.UserID, "醫事人員ID");
                        bindder.ColumnFor(p => p.UserTitle, "職稱");
                        bindder.ColumnFor(p => p.UserServise, "服務類型");
                        bindder.ColumnFor(p => p.Note, "備註");
                    })
                    .GetResult();
            });
            var fileName = $"戒菸服務人員概況.{fileType.ToString()}";
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

        /// <summary>
        /// 核准
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="hospStartDate"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Judge(List<PrsnContractViewModel> contract, string prsnStartDate)
        {
            return Json(await prsnContractService.JudgeContracts(contract, prsnStartDate));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [IgnoreAntiforgeryToken]
        public async Task<List<PrsnContractViewModel>> SMKPrsnContractToQSMS(string PrsnId)
        {
            
            var Prsnbasic = await this.prsnContractService.GetPrsnContractAPI(PrsnId);
            return Prsnbasic;
        }
    }
}
