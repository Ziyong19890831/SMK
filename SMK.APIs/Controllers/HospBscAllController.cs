using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SMK.APIs.Models;
using SMK.APIs.Services;
using SMK.APIs.Services.Foundation;
using SMK.Data;
using SMK.Data.Entity;

namespace SMK.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospBscAllController : ControllerBase
    {
        private readonly HospBscAllService _hospBscAllService;
        private readonly AllToken _AllToken;

        public HospBscAllController(HospBscAllService hospBscAllService, IOptions<AllToken> AllToken)
        {
            _AllToken = AllToken.Value;
            _hospBscAllService = hospBscAllService;
        }
       
        [HttpGet("HospBscAll_Get")]
        public string HospBscAll_Get(string token, [FromQuery]HospBscAll value)
        {
            var list = _hospBscAllService.QueryHospBscAll(token, _AllToken.QuickSmokingToken.ToString(), value);
            return list;
        }
    }
}
