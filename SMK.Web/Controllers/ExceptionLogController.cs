using Microsoft.AspNetCore.Mvc;
using SMK.Data.Dto;
using SMK.Web.AppScope.Filters;
using SMK.Web.Models;
using SMK.Web.Services;
using System.Linq;
using System.Threading.Tasks;
using Yozian.Extension;

namespace SMK.Web.Controllers
{
    public class ExceptionLogController : BaseController
    {
        private readonly SessionManager smgr;
        private readonly IdentityModel identity;
        private readonly ExceptionLogService exceptionLogService;

        public ExceptionLogController(
            SessionManager sessionManager,
            ExceptionLogService exceptionLogService
            )
        {
            this.smgr = sessionManager;
            this.identity = smgr.Get<IdentityModel>();
            this.exceptionLogService = exceptionLogService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Query(ExceptionLogQueryModel model)
        {
            var ret = await exceptionLogService.Query(model);
            return Json(ret);
        }
    }
}