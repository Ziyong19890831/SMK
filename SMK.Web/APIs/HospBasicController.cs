using Microsoft.AspNetCore.Mvc;
using SMK.Web.AppScope.Filters;
using SMK.Web.Services.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMK.Data.Dto;

namespace SMK.Web.APIs
{
    // [EmpAuthorized]
    public class HospBasicController : ApiController
    {
        private readonly HospBasicService hospBasicService;

        public HospBasicController(HospBasicService hospBasicService)
        {
            this.hospBasicService = hospBasicService;
        }

        [HttpGet]
        public async Task<string> GetHospName(string hospId, string hospSeqNo)
        {
            return await hospBasicService.GetHospNameAsync(hospId, hospSeqNo);
        }

        [HttpPost("~/api/HospBasic/{hospId}/clone")]
        [Consumes("application/json")]
        [IgnoreAntiforgeryToken]
        public async Task<LogicRtnModel<bool>> CloneHospBasic(string hospId, [FromBody]HospBasicViewModel model)
        {
            return await hospBasicService.CloneHospId(
                hospId,
                model.HospId,
                model.HospSDate,
                model.HospName,
                model.EndReasonNo);
        }

        public class HospBasicViewModel
        {
            public string HospId { get; set; }
            public string HospSDate { get; set; }
            public string HospName { get; set; }
            public string EndReasonNo { get; set; }
        }
    }
}
