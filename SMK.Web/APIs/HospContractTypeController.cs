using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    [Route("api/[controller]/[action]")]
    public class HospContractTypeController : ControllerBase
    {
        private readonly HospContractTypeService hospContractTypeService;

        public HospContractTypeController(HospContractTypeService hospContractTypeService)
        {
            this.hospContractTypeService = hospContractTypeService;
        }

        [HttpGet]
        public LogicRtnModel<List<HospContractType>> Get(string hospId, string hospSeqNo)
        {
            return hospContractTypeService.GetHospContractTypes(hospId, hospSeqNo);
        }   
    }
}
