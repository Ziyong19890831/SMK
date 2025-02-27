using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Web.Models;
using SMK.Web.Services.Foundation;

namespace SMK.Web.Controllers
{
    /// <summary>
    /// 機構合約
    /// </summary>
    public class HospContractController : Controller
    {
        private readonly HospContractService hospContractService;

        public HospContractController(HospContractService hospContractService)
        {
            this.hospContractService = hospContractService;
        }

        public IActionResult Query()
        {
            return View(new HospContractQueryModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Query(HospContractQueryModel model) {
            var logicRtnModel= await hospContractService.GetHospContracts(model);
            return Json(logicRtnModel);
        }
    }
}
