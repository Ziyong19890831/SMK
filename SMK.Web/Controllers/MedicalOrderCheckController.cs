using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Web.AppScope.Filters;
using SMK.Web.Services.Foundation;
using SMK.Web.Validator;

namespace SMK.Web.Controllers
{
    /// <summary>
    /// 醫令自動化電腦審核-檢核結果下載
    /// </summary>
    [EmpAuthorized]
    public class MedicalOrderCheckController : BaseController
    {
        private readonly FileService FileService;
        private readonly string _folder;
        public MedicalOrderCheckController(FileService FileService, IWebHostEnvironment env)
        {
            this.FileService = FileService;
            _folder = $@"{env.WebRootPath}\MedicalOrderData\MedicalOrderCheck\";
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

        // GET: MedicalOrderCheck
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
