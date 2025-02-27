using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualBasic.FileIO;
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
    /// 專審名冊
    /// </summary>
    [EmpAuthorized]
    public class ProReviewController : BaseController
    {
        private readonly FileService FileService;
        private readonly string _folder;

        public ProReviewController(FileService FileService, IWebHostEnvironment env)
        {
            this.FileService = FileService;
            _folder = $@"{env.WebRootPath}\MedicalOrderData\專審名冊\";
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
        /// <summary>
        /// ProReview
        /// </summary>
        /// <returns></returns>
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

    }
}
