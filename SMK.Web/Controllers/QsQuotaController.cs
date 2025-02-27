using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Web.AppScope.Filters;
using SMK.Web.Models;
using SMK.Web.Services.Foundation;
using System.Threading.Tasks;
using Yozian.WebCore.Library.Utility.Excel;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    public class QsQuotaController : BaseController
    {
        private readonly QsQuotaService qsQuotaService;

        public QsQuotaController(QsQuotaService qsQuotaService)
        {
            this.qsQuotaService = qsQuotaService;
        }

        public IActionResult Index()
        {
            return View(new QsQuotaQueryModel());
        }

        [HttpPost]
        public async Task<IActionResult> Upload(QsQuotaQueryModel query)
        {
            LogicRtnModel<QsQuotaQueryModel> logicRtnModel = await qsQuotaService.UploadQsQuota(query.file);
            return Json(logicRtnModel);
        }
        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Query(QsQuotaQueryModel query)
        {
            var list = await qsQuotaService.QueryQsQuota(query);
            return Json(list);
        }
        

       
    }
}

