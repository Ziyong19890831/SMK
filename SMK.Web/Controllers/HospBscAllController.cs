using Microsoft.AspNetCore.Mvc;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Web.AppScope.Filters;
using SMK.Web.Models;
using SMK.Web.Services.Foundation;
using System.Threading.Tasks;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    public class HospBscAllController : BaseController
    {
        private readonly HospBscAllService hospBscAllService;

        public HospBscAllController(HospBscAllService hospBscAllService)
        {
            this.hospBscAllService = hospBscAllService;
        }

        public IActionResult Index()
        {
            return View(new HospBscAllQueryModel());
        }

        [HttpPost]
        public async Task<IActionResult> Upload(HospBscAllQueryModel query)
        {
            LogicRtnModel<HospBscAllQueryModel> logicRtnModel = await hospBscAllService.UploadHospBscAll(query.file);
            return Json(logicRtnModel);
        }
        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Query(HospBscAllQueryModel query)
        {
            var list = await hospBscAllService.QueryHospBscAll(query);
            return Json(list);
        }
        
        // GET: Branch/Create
        public IActionResult Create()
        {
            return View(new CorrectionSlip());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( CorrectionSlip correctionSlip)
        {
            if (!ModelState.IsValid)
            {
                return View(correctionSlip);
            }
            correctionSlip.UpdatedBy = User.Identity.Name;
            var rtnModel = await hospBscAllService.Create(correctionSlip, null);
            if (rtnModel.IsSuccess)
            {
                return RedirectTo(rtnModel, nameof(this.Index));
            }
            else
            {
                return View(rtnModel);
            }
        }
    }
}