using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Web.AppScope.Filters;
using SMK.Web.Services.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.APIs
{
    //[EmpAuthorized]
    [ApiController]
    [Route("api/[controller]")]
    public class GenHospContController : ControllerBase
    {
        private readonly GenHospContService genHospContService;

        public GenHospContController(GenHospContService genHospContService)
        {
            this.genHospContService = genHospContService;
        }

        [HttpGet]
        public LogicRtnModel<List<GenHospCont>> Get()
        {
            return genHospContService.GetGenHospConts();
        }
    }
}
