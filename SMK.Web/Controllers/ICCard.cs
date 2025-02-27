using Microsoft.AspNetCore.Mvc;
using SMK.Web.AppScope.Filters;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    public class ICCard : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
