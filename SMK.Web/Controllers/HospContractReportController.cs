using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Yozian.WebCore.Library.Utility.Excel;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    public class HospContractReportController : BaseController
    {
        private readonly HospContractService hospContractService;
        //private readonly string _folder;
        public HospContractReportController(HospContractService hospContractService,
            IWebHostEnvironment env)
        {
            this.hospContractService = hospContractService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("Export")]
        public async Task<IActionResult> Export(HospContractQueryModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            var logicRtnModel = await hospContractService.GetHospContracts(model);
            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }
            var list = logicRtnModel.Data.Data;
            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<HospBasicContractViewModel>(list)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.HospID, "機構代碼");
                        bindder.ColumnFor(p => p.HospName, "機構名稱");
                        bindder.ColumnFor(p => p.HospStatusStr, "機構狀態");
                      /*   bindder.ColumnFor(p => p.ContractStatusStr, "合約類別"); */
                        bindder.ColumnFor(p => p.SMKContractTypeNam, "合約類別");
                        bindder.ColumnFor(p => p.HospStartDate, "生效日期");
                        bindder.ColumnFor(p => p.HospEndDate, "終止日期");

                    })
                    .GetResult();
            });
            var fileName = $"機構名冊.{fileType.ToString()}";
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
