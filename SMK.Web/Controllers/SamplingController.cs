using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    /// <summary>
    /// 抽樣作業
    /// </summary>
    [EmpAuthorized]
    public class SamplingController : BaseController
    {
        private readonly SamplingService SamplingService;

        public SamplingController(SamplingService SamplingService)
        {
            this.SamplingService = SamplingService;

        }

        // GET: Sampling
        public IActionResult Index()
        {

            //var list=SamplingService.Query((context) => context.SamplingList);

            return View();
        }
        public async Task<IActionResult> Upload(IFormFile file,string year)
        {
            var ret = await SamplingService.Upload(file,year);
            return Json(ret);
           
        }

        //// GET: Sampling/Details/5
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var genSampling= await SamplingService.FindOne<GenSampling>((context) => context.Where(m => m.SamplingNo == id));
        //    if (genSampling == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(genSampling);
        //}

        //// GET: Sampling/Create
        //public IActionResult Create()
        //{
        //    return View(new GenSampling());
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("SamplingNo,SamplingName")] GenSampling genSampling)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(genSampling);
        //    }

        //    var rtnModel = await SamplingService.Create(genSampling,new GenSamplingValidator().Validate);
        //    if (rtnModel.IsSuccess)
        //    {
        //        return RedirectTo(rtnModel, nameof(this.Index));
        //    }
        //    else
        //    {
        //        return View(rtnModel);
        //    }
        //}

        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var rtnModel = await SamplingService.FindOne<GenSampling>((context) => context.Where(m => m.SamplingNo == id));
        //    if (rtnModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(rtnModel);
        //}

        //// POST: Sampling/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("SamplingNo,SamplingName")] GenSampling genSampling)
        //{
        //    if (id != genSampling.SamplingNo)
        //    {
        //        return NotFound();
        //    }

        //    if (!ModelState.IsValid) {
        //        return View(genSampling);
        //    }
        //    var rtnModel = await SamplingService.Update(genSampling, 
        //        new GenSamplingValidator().Validate,
        //        false,
        //        x=>x.SamplingName);
        //    if (rtnModel.IsSuccess)
        //    {
        //        return RedirectTo(rtnModel, nameof(this.Index));
        //    }
        //    else
        //    {
        //        return View(rtnModel);
        //    }
        //}

        //// POST: Sampling/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    GenSampling genSampling = new GenSampling() {
        //        SamplingNo = id
        //    };

        //    var rtnModel = await SamplingService.Remove(genSampling);
        //    return Json(rtnModel);
        //}
    }
}
