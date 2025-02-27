using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Web.AppScope.Filters;
using SMK.Web.Services.Foundation;
using SMK.Web.Validator;

namespace SMK.Web.Controllers
{
    /// <summary>
    /// 譯碼簿
    /// </summary>
    [EmpAuthorized]
    public class SchemaReportController : BaseController
    {
        private readonly IWebHostEnvironment env;

        public SchemaReportController(IWebHostEnvironment env)
        {
            this.env = env;
        }

        /// <summary>
        /// 下載譯碼簿
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            string file = $@"{env.WebRootPath}\OtherFile\SMK系統譯碼簿-109年版.pdf";
            FileInfo info = new FileInfo(file);
            
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(info.Name, out contentType))
            {
                contentType = "application/octet-stream";
            }
            var contentDisposition = new ContentDispositionHeaderValue("attachment");
            contentDisposition.SetHttpFileName(info.Name);
            Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

            return new FileContentResult(System.IO.File.ReadAllBytes(info.FullName), contentType);
        }
    }
}
