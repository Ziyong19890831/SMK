using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using OfficeOpenXml;
using SMK.Data.Enums;
using SMK.Web.Services.Foundation;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.Controllers
{
    public class PhoneSurveySatisfactionReportController : BaseController
    {
        private readonly FileService FileService;
        private readonly string _folder;

        public PhoneSurveySatisfactionReportController(FileService FileService, IWebHostEnvironment env)
        {
            this.FileService = FileService;
            _folder = $@"{env.WebRootPath}\MedicalOrderData\電訪前端抽樣\";
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
        /// PhoneSurveySatisfactionReport
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
