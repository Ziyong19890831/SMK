using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Web.AppScope.Filters;
using SMK.Web.Services.Foundation;
using SMK.Web.Validator;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    public class PrsnLicenceController : BaseController
    {
        private readonly PrsnLicenceService prsnLicenceService;

        public PrsnLicenceController(PrsnLicenceService prsnLicenceService)
        {
            this.prsnLicenceService = prsnLicenceService;
        }

        public async Task<IActionResult> GetPrsnLicence(string prsnId,bool IsSync=false)
        {
            if (string.IsNullOrEmpty(prsnId)) {
                return Json(new LogicRtnModel<bool>("請輸入身分證號"));
            }
            var data = await prsnLicenceService.GetPrsnLicence(prsnId, IsSync);
            return Json(data);
        }
    }
}
