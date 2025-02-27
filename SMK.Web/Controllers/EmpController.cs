using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Data.Utility;
using SMK.Web.Services;
using SMK.Web.Validator;

namespace SMK.Web.Controllers
{
    /// <summary>
    /// 帳號管理
    /// </summary>
    public class EmpController : BaseController
    {
        private readonly EmpService empService;

        public EmpController( EmpService empService)
        {
            this.empService = empService;
        }

        // GET: Emp
        public async Task<IActionResult> Index()
        {
            var list = empService.Query((context) => context.GenEmpData);
            return View(await list);
        }

        // GET: Emp/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var genEmpData = await empService.FindOne<GenEmpData>((context) => context.Where(m => m.Id == id));
            if (genEmpData == null)
            {
                return NotFound();
            }

            return View(genEmpData);
        }

        // GET: Emp/Create
        public IActionResult Create()
        {
            return View(new GenEmpData());
        }

        // POST: Emp/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Account,Pwd,Name")] GenEmpData genEmpData,string RoleId)
        {
            if (!ModelState.IsValid)
            {
                return View(genEmpData);
            }

            var rtnModel =await empService.CreateEmp(genEmpData, RoleId);
            if (rtnModel.IsSuccess)
            {
                return RedirectTo(rtnModel, nameof(this.Index));
            }
            else
            {
                return View(rtnModel);
            }
        }

        // GET: Emp/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rtnModel = await empService.FindOne<GenEmpData>((context) => context.Where(m => m.Id == id));
            if (rtnModel == null)
            {
                return NotFound();
            }

            return View(rtnModel);
        }

        // POST: Emp/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Account,Pwd,Name,Enable")] GenEmpData genEmpData)
        {
            if (id != genEmpData.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(genEmpData);
            }
            var rtnModel = await empService.UpdateEmp(genEmpData);
            if (rtnModel.IsSuccess)
            {
                return RedirectTo(rtnModel, nameof(this.Index));
            }
            else
            {
                return View(rtnModel);
            }
        }

        // POST: Emp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            GenEmpData genEmpData = new GenEmpData()
            {
                Id=id
            };
            var rtnModel= await empService.Remove(genEmpData);
            return Json(rtnModel);
        }

        public async Task<IActionResult> Release(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ret = await empService.Release(id);
            if (ret == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(EmpController.Index), GetControllerName<EmpController>());
        }
    }
}
