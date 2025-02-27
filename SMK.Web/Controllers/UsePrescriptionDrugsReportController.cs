using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using SMK.Data.Enums;
using SMK.Web.Services.Foundation;
using System.Threading.Tasks;

namespace SMK.Web.Controllers
{
    public class UsePrescriptionDrugsReportController : BaseController
    {
        private readonly UsePrescriptionDrugsReportService usePrescriptionDrugsReportService;
        public UsePrescriptionDrugsReportController(UsePrescriptionDrugsReportService usePrescriptionDrugsReportService)
        {
            this.usePrescriptionDrugsReportService = usePrescriptionDrugsReportService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Export(int syear,int eyear, ExcelType fileType)
        {
            var excel =  await usePrescriptionDrugsReportService.Export(syear, eyear);
            var fileName = $"戒菸藥品使用及處方情形_"+syear+"-"+eyear+ "年度."+fileType.ToString();
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
