using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Web.AppScope.Filters;
using SMK.Web.Services.Foundation;
using SMK.Web.Validator;

namespace SMK.Web.Controllers
{
    /// <summary>
    /// 醫令自動化電腦審核-自訂參數
    /// </summary>
    [EmpAuthorized]
    public class MedicalOrderController : BaseController
    {
        public MedicalOrderController()
        {
        }

        // GET: MedicalOrder
        public IActionResult Index()
        {
            return View();
        }
    }
}
