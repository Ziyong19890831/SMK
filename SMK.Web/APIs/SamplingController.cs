using Microsoft.AspNetCore.Mvc;
using SMK.Data.Enums;
using SMK.Web.AppScope.Filters;
using SMK.Web.Models;
using SMK.Web.Services.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.APIs
{
    [EmpAuthorized]
    public class SamplingController : ApiController
    {
        private readonly SamplingService _samplingService;

        public SamplingController(SamplingService SamplingService)
        {
            this._samplingService = SamplingService;

        }

        /// <summary>
        /// 抽樣查詢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<SamplingItemDto>> GetSamplingItems([FromQuery] SamplingQueryModel request)
        {
            var items = await _samplingService.GetSamplingItemsAsync(request);
            return items;
        }

        /// <summary>
        /// 戒菸服務專業審查調閱清單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ExportRew1500(string FeeStart, string FeeEnd, ExcelType fileType)
        {
            var vm = await _samplingService.ExportRew1500Async(FeeStart, FeeEnd, fileType.ToString());
            return File(vm.Stream, "application/octet-stream", vm.FileName);
        }
    }
}
