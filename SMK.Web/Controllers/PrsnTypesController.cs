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
using SMK.Web.Services.Foundation;
using SMK.Web.Validator;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    public class PrsnTypesController : BaseController
    {
        private readonly PrsnTypesService prsnTypesService;

        public PrsnTypesController(PrsnTypesService prsnTypesService)
        {
            this.prsnTypesService = prsnTypesService;
        }

        // GET: PrsnTypes
        public async Task<IActionResult> Index()
        {
            var list = prsnTypesService.Query((context) => context.GenPrsnType);
            return View(await list);
        }

        // GET: PrsnTypes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var genPrsnType = await prsnTypesService.FindOne<GenPrsnType>((context) => context.Where(m => m.PrsnType == id));
            if (genPrsnType == null)
            {
                return NotFound();
            }

            return View(genPrsnType);

        }

        // GET: PrsnTypes/Create
        public IActionResult Create()
        {
            return View(new GenPrsnType());
        }

        // POST: PrsnTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PrsnType,PrsnTypeName")] GenPrsnType genPrsnType)
        {
            if (!ModelState.IsValid)
            {
                return View(genPrsnType);
            }

            var rtnModel = await prsnTypesService.Create(genPrsnType, new GenPrsnTypeValidator().Validate);
            if (rtnModel.IsSuccess)
            {
                return RedirectTo(rtnModel, nameof(this.Index));
            }
            else
            {
                return View(rtnModel);
            }
        }

        // GET: PrsnTypes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rtnModel = await prsnTypesService.FindOne<GenPrsnType>((context) => context.Where(m => m.PrsnType == id));
            if (rtnModel == null)
            {
                return NotFound();
            }

            return View(rtnModel);

        }

        // POST: PrsnTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PrsnType,PrsnTypeName")] GenPrsnType genPrsnType)
        {
            if (id != genPrsnType.PrsnType)
            {
                return NotFound();
            }
            
            if (!ModelState.IsValid)
            {
                return View(genPrsnType);
            }
            var rtnModel = await prsnTypesService.Update(genPrsnType,
                new GenPrsnTypeValidator().Validate,
                false,
                x => x.PrsnTypeName);
            if (rtnModel.IsSuccess)
            {
                return RedirectTo(rtnModel, nameof(this.Index));
            }
            else
            {
                return View(rtnModel);
            }

        }

        // POST: Branch/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            GenPrsnType genPrsnType = new GenPrsnType()
            {
                PrsnType = id
            };

            var rtnModel = await prsnTypesService.Remove(genPrsnType);
            return Json(rtnModel);
        }
    }
}
