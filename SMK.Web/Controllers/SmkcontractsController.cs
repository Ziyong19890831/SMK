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
    public class SmkcontractsController : BaseController
    {
        private readonly SmkcontractsService smkcontractsService;

        public SmkcontractsController(SmkcontractsService smkcontractsService)
        {
            this.smkcontractsService = smkcontractsService;
        }

        // GET: Smkcontracts
        public async Task<IActionResult> Index()
        {

            var list = smkcontractsService.Query((context) => context.GenSmkcontract);
            return View(await list);
        }

        // GET: Smkcontracts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var genBranch = await smkcontractsService.FindOne<GenSmkcontract>((context) => context.Where(m => m.SmkcontractType == id));
            if (genBranch == null)
            {
                return NotFound();
            }

            return View(genBranch);

        }

        // GET: Smkcontracts/Create
        public IActionResult Create()
        {
            return View(new GenSmkcontract());
        }

        // POST: Smkcontracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SmkcontractType,SmkcontractName")] GenSmkcontract genSmkcontract)
        {
            if (!ModelState.IsValid)
            {
                return View(genSmkcontract);
            }

            var rtnModel = await smkcontractsService.Create(genSmkcontract, new GenSmkcontractValidator().Validate);
            if (rtnModel.IsSuccess)
            {
                return RedirectTo(rtnModel, nameof(this.Index));
            }
            else
            {
                return View(rtnModel);
            }
        }

        // GET: Smkcontracts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rtnModel = await smkcontractsService.FindOne<GenSmkcontract>((context) => context.Where(m => m.SmkcontractType == id));
            if (rtnModel == null)
            {
                return NotFound();
            }

            return View(rtnModel);

        }

        // POST: Smkcontracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SmkcontractType,SmkcontractName")] GenSmkcontract genSmkcontract)
        {
            if (id != genSmkcontract.SmkcontractType)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(genSmkcontract);
            }
            var rtnModel = await smkcontractsService.Update(genSmkcontract,
                new GenSmkcontractValidator().Validate,
                false,
                x => x.SmkcontractName);
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
            GenSmkcontract genSmkcontract = new GenSmkcontract()
            {
                SmkcontractType = id
            };

            var rtnModel = await smkcontractsService.Remove(genSmkcontract);
            return Json(rtnModel);
        }
    }
}
