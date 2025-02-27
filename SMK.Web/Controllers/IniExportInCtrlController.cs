using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMK.Web.AppScope.Filters;
using SMK.Web.Models;
using SMK.Web.Services.Foundation;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    public class IniExportInCtrlController : BaseController
    {
        public IniExportInCtrlService IniExportInCtrlService { get; set; }
        public IniExportInCtrlController(IniExportInCtrlService iniExportInCtrlService)
        {
            IniExportInCtrlService = iniExportInCtrlService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Query(IniExportInCtrlQueryModel model)
        {
            return Json(await IniExportInCtrlService.Query(model));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IniExportInCtrlRunModel model)
        {
            return Json(await IniExportInCtrlService.ImportData(model));
        }
    }
}
