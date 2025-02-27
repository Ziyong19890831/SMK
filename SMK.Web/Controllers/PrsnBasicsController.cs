using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Web.AppScope.Filters;
using SMK.Web.Models;
using SMK.Web.Services.Foundation;
using SMK.Web.Validator;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    public class PrsnBasicsController : BaseController
    {
        private readonly PrsnBasicsService prsnBasicsService;

        public PrsnBasicsController(PrsnBasicsService prsnBasicsService)
        {
            this.prsnBasicsService = prsnBasicsService;
        }

        /// <summary>
        /// 查詢醫事人員清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Query(PrsnBasicQueryModel model)
        {
            var list = await prsnBasicsService.QueryPrsnBasicList(model);
            return Json(list);
        }

        /// <summary>
        /// 醫事人員清單
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增醫事人員
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View(new PrsnBasicViewModel());
        }

        /// <summary>
        /// 新增存檔
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrsnBasicViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var rtnModel = await prsnBasicsService.CreatePrsn(model);
            if (rtnModel.IsSuccess)
            {
                return RedirectToAction(nameof(this.Edit), new { id = model.PrsnId });
            }
            else
            {
                rtnModel.Data = model;
                return View(rtnModel);
            }
        }

        /// <summary>
        /// 新增醫事人員合約
        /// </summary>
        /// <returns></returns>
        ///
        public async Task<IActionResult> InsertPrsnContractView(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rtnModel = await prsnBasicsService.QueryPrsnBasic(id);
            if (rtnModel.IsSuccess)
            {
                return View(rtnModel);
            }
            return NotFound();
        }

        /// <summary>
        /// 編輯醫事人員
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rtnModel = await prsnBasicsService.QueryPrsnBasic(id);
            if (rtnModel.IsSuccess)
            {
                return View(rtnModel);
            }
            return NotFound();
        }

        /// <summary>
        /// 更新機構醫事人員合約
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdatePrsnContract(PrsnContractViewModel model)
        {
            var result = await prsnBasicsService.UpdatePrsnContract(model);
            return Json(result);
        }

        /// <summary>
        /// 做新增合約
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> InsertPrsnContract(PrsnContractViewModel model)
        {
            return Json(await prsnBasicsService.AddPrsnContract(model));
        }

        public async Task<IActionResult> UpdatePrsnLicence(PrsnLicenceViewModel model)
        {
            var result = await prsnBasicsService.UpdatePrsnLicense(model);
            return Json(result);
        }
        public async Task<IActionResult> InsertPrsnLicence(PrsnLicenceViewModel model)
        {
            return Json(await prsnBasicsService.AddPrsnLicense(model));
        }
        /// <summary>
        /// 更新機構合約
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdatePrsnBasic(PrsnBasicViewModel model)
        {
            var result = await prsnBasicsService.UpdatePrsn(model);
            return Json(result);
        }
        /// <summary>
        /// 刪除醫事人員合約
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeletePrsnContract(PrsnContractViewModel model)
        {
            var logicRtnModel = await prsnBasicsService.FindOne<PrsnContract>(x => x.Where(p => p.Id == model.Id));
            if (!logicRtnModel.IsSuccess)
            {
                return Json(logicRtnModel);
            }
            var result = await prsnBasicsService.Remove(logicRtnModel.Data);
            return Json(result);
        }

        /// <summary>
        /// 刪除醫事人員資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> RemovePrsnBasic(string prsnId)
        {
            var logicRtnModel = await prsnBasicsService.FindOne<PrsnBasic>(x => x.Where(p => p.PrsnId == prsnId));
            if (!logicRtnModel.IsSuccess)
            {
                return Json(logicRtnModel);
            }
            var result = await prsnBasicsService.Remove(logicRtnModel.Data);
            return Json(result);
        }
    }
}
