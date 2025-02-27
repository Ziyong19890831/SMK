using Microsoft.AspNetCore.Mvc;
using SMK.Web.AppScope.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.Controllers
{
    /// <summary>
    /// 審查管理 - 專業審查
    /// </summary>
    [EmpAuthorized]
    public class SamplingExamineController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
