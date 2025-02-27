using Microsoft.AspNetCore.Mvc;
using SMK.Web.Services.Foundation;
using SMK.Web.Models;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using SMK.Data.Enums;

namespace SMK.Web.Controllers
{
    public class HighReportingAgencyReportController : BaseController
    {
        private HighReportingAgencyReportService highReportingAgencyReportService;
        public HighReportingAgencyReportController(HighReportingAgencyReportService highReportingAgencyReportService)
        {
            this.highReportingAgencyReportService = highReportingAgencyReportService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Export(HighReportingAgencyReportQueryModel highReportingAgencyReportQueryModel, ExcelType fileType)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", highReportingAgencyReportQueryModel);
            }
            byte[] ms =await highReportingAgencyReportService.Export(highReportingAgencyReportQueryModel);
            var fileName = $"高申報量機構報表.{fileType.ToString()}";
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            var contentDisposition = new ContentDispositionHeaderValue("attachment");
            contentDisposition.SetHttpFileName(fileName);
            Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
            return new FileContentResult(ms, contentType);

        }
    }
}
