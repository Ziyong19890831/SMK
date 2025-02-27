using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMK.Web.AppScope.Filters;
using SMK.Web.Models;
using SMK.Web.Services.Foundation;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    public class IniFileInCtrlController : BaseController
    {
        public IniFileInCtrlService IniFileInCtrlService { get; set; }
        public IniFileInCtrlController(IniFileInCtrlService iniFileInCtrlService)
        {
            IniFileInCtrlService = iniFileInCtrlService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Query(IniFileInCtrlQueryModel model)
        {
            return Json(await IniFileInCtrlService.Query(model));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IniFileInCtrlRunModel model)
        {
            return Json(await IniFileInCtrlService.ImportData(model));
        }
    }
}
