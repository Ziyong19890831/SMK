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
    public class BranchController : BaseController
    {
        private readonly BranchService branchService;

        public BranchController(BranchService branchService)
        {
            this.branchService = branchService;

        }

        // GET: Branch
        public async Task<IActionResult> Index()
        {
            var list=branchService.Query((context) => context.GenBranch);
            return View(await list);
        }

        // GET: Branch/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var genBranch= await branchService.FindOne<GenBranch>((context) => context.Where(m => m.BranchNo == id));
            if (genBranch == null)
            {
                return NotFound();
            }

            return View(genBranch);
        }

        // GET: Branch/Create
        public IActionResult Create()
        {
            return View(new GenBranch());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BranchNo,BranchName")] GenBranch genBranch)
        {
            if (!ModelState.IsValid)
            {
                return View(genBranch);
            }

            var rtnModel = await branchService.Create(genBranch,new GenBranchValidator().Validate);
            if (rtnModel.IsSuccess)
            {
                return RedirectTo(rtnModel, nameof(this.Index));
            }
            else
            {
                return View(rtnModel);
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rtnModel = await branchService.FindOne<GenBranch>((context) => context.Where(m => m.BranchNo == id));
            if (rtnModel == null)
            {
                return NotFound();
            }

            return View(rtnModel);
        }

        // POST: Branch/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BranchNo,BranchName")] GenBranch genBranch)
        {
            if (id != genBranch.BranchNo)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) {
                return View(genBranch);
            }
            var rtnModel = await branchService.Update(genBranch, 
                new GenBranchValidator().Validate,
                false,
                x=>x.BranchName);
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
            GenBranch genBranch = new GenBranch() {
                BranchNo = id
            };

            var rtnModel = await branchService.Remove(genBranch);
            return Json(rtnModel);
        }
    }
}
