using Microsoft.AspNetCore.Mvc;
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
    public class SamplingExamineController : ApiController
    {
        private readonly SamplingExamineService _samplingExamineService;

        public SamplingExamineController(SamplingExamineService samplingExamineService)
        {
            _samplingExamineService = samplingExamineService;
        }

        [HttpGet]
        public async Task<IList<SamplingExamineCreateData>> GetSamplingExamineCreateDatas([FromQuery] SamplingExamineQueryModel request)
        {
            return await _samplingExamineService.GetSamplingExamineCreateDatasAsync(request);
        }

        [HttpGet]
        public async Task<IList<string>> GetAccessnos(string feeStart, string feeEnd)
        {
            return await _samplingExamineService.GetAccessnosAsync(feeStart, feeEnd);
        }

        [HttpPost]
        public async Task SaveSamplingCreateData([FromBody] SamplingCreateRequest request)
        {
            await _samplingExamineService.SaveSamplingCreateDataAsync(request);
        }

        [HttpPost]
        public async Task<IList<SamplingResultQueryData>> GetWithCreateSamplingResultQueryDatas([FromBody] SamplingResultQueryRequest request)
        {
            return await _samplingExamineService.GetWithCreateSamplingResultQueryDatasAsync(request);
        }

        [HttpPost]
        public async Task SaveSamplingResultData([FromBody] SaveSamplingResultDataRequest request)
        {
            await _samplingExamineService.SaveSamplingResultDataAsync(request);
        }

        /// <summary>
        /// 門診療服務點數及醫令清單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult> ExportRew1300([FromForm] ExportRewRequest request)
        {
            var vm = await _samplingExamineService.ExportRew1300Async(request);
            return File(vm.Stream, "application/octet-stream", vm.FileName);
        }

        [HttpGet]
        public async Task<IList<SamplingExamineQueryData>> GetSamplingExamineQueryDatas([FromQuery] SamplingExamineQueryRequest request)
        {
            return await _samplingExamineService.GetSamplingExamineQueryDatasAsync(request);
        }

        [HttpGet]
        public async Task<IList<SamplingExamineQueryDetailData>> GetSamplingExamineQueryDetailDatas(string fee_ym, string data_id)
        {
            return await _samplingExamineService.GetSamplingExamineQueryDetailDatasAsync(fee_ym, data_id);
        }

        /// <summary>
        /// 取得專審/申復收件狀況資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IList<SamplingExamineReceiveData>> GetSamplingExamineReceiveDatas([FromQuery] SamplingExamineReceiveRequest request)
        {
            return await _samplingExamineService.GetSamplingExamineReceiveDatasAsync(request);
        }

        [HttpGet]
        public async Task SaveSamplingExamineReceiveData(SaveSamplingExamineReceiveRequest request)
        {
            await _samplingExamineService.SaveSamplingExamineReceiveDataAsync(request);
        }

        /// <summary>
        /// 專業審查追扣核定總表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult> ExportRew1600([FromForm] ExportRewRequest request)
        {
            var vm = await _samplingExamineService.ExportRew1600Async(request);
            return File(vm.Stream, "application/octet-stream", vm.FileName);
        }

        /// <summary>
        /// 戒菸服務費用專業審查追扣捕付核定總表(申復)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult> ExportRew1800([FromForm] ExportRewRequest request)
        {
            var vm = await _samplingExamineService.ExportRew1800Async(request);
            return File(vm.Stream, "application/octet-stream", vm.FileName);
        }
    }

}
