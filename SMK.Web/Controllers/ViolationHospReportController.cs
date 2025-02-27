using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using OfficeOpenXml;
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
    /// <summary>
    /// 違規機構
    /// </summary>
    [EmpAuthorized]
    public class ViolationHospReportController : BaseController
    {
        private readonly HospContractService hospContractService;
        private readonly FileService FileService;
        private readonly string _folder;
        public ViolationHospReportController(HospContractService hospContractService,IWebHostEnvironment env, FileService FileService)
        {
            this.hospContractService = hospContractService;
            this.FileService = FileService;
            _folder = $@"{env.WebRootPath}\違規名單\";
        }

        ///// <summary>
        ///// AuditDeduction
        ///// </summary>
        ///// <returns></returns>
        public IActionResult Index()
        {
            DirectoryInfo list = new DirectoryInfo(_folder);
            var filelist = list.EnumerateFiles()
                               .Select(p => (
                                   Title: Path.GetFileNameWithoutExtension(p.FullName),
                                   FileName: p.Name
                               ))
                               .ToList();
            return View(filelist);
        }
        public IActionResult DownloadFile(string filename, ExcelType fileType)
        {
            using (var package = new ExcelPackage(new FileInfo(_folder + filename)))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets[0];
                string pwd = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString();
                sheet.Protection.SetPassword(pwd);
                sheet.Protection.IsProtected = true;
                if (fileType.ToString() == "ods")
                {
                    filename = filename.Split('.').First() + ".ods";
                }
                return File(package.GetAsByteArray(pwd), "application/octet-stream", filename);
            }
        }
        //public IActionResult Index()
        //{
        //    //歷年戒菸服務違規醫事機構案件清單_1091015.xlsx
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Query(HospContractQueryModel model)
        {
            model.IsViolation = true;//抓出違規機構
            var logicRtnModel = await hospContractService.GetHospContracts(model);
            return Json(logicRtnModel);
        }

        [HttpGet, ActionName("Export")]
        public async Task<IActionResult> Export(HospContractQueryModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            model.IsViolation = true;//抓出違規機構
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
                        bindder.ColumnFor(p => p.ContractStatusStr, "合約類別");
                        bindder.ColumnFor(p => p.SMKContractTypeNam, "合約類別");
                        bindder.ColumnFor(p => p.HospStartDate, "生效日期");
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
