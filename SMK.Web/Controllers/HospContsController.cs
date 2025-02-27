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
    public class HospContsController : BaseController
    {
        private readonly HospContsService hospContsService;



        public HospContsController(HospContsService hospContsService)
        {
            this.hospContsService = hospContsService;
        }

        // GET: HospConts
        public async Task<IActionResult> Index()
        {
            var list = hospContsService.Query((context) => context.GenHospCont);
            return View(await list);
        }

        // GET: HospConts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var genBranch = await hospContsService.FindOne<GenHospCont>((context) => context.Where(m => m.HospContType == id));
            if (genBranch == null)
            {
                return NotFound();
            }

            return View(genBranch);
        }

        // GET: HospConts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HospConts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HospContType,HospContName")] GenHospCont genHospCont)
        {
            if (!ModelState.IsValid)
            {
                return View(genHospCont);
            }

            var rtnModel = await hospContsService.Create(genHospCont, new GenHospContValidator().Validate);
            if (rtnModel.IsSuccess)
            {
                return RedirectTo(rtnModel, nameof(this.Index));
            }
            else
            {
                return View(rtnModel);
            }
        }

        // GET: HospConts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rtnModel = await hospContsService.FindOne<GenHospCont>((context) => context.Where(m => m.HospContType == id));
            if (rtnModel == null)
            {
                return NotFound();
            }

            return View(rtnModel);

        }

        // POST: HospConts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("HospContType,HospContName")] GenHospCont genHospCont)
        {
            if (id != genHospCont.HospContType)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(genHospCont);
            }
            
            var rtnModel = await hospContsService.Update(genHospCont,
                new GenHospContValidator().Validate,
                false,
                x => x.HospContName);
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
            GenHospCont genHospCont = new GenHospCont()
            {
                HospContType = id
            };

            var rtnModel = await hospContsService.Remove(genHospCont);
            return Json(rtnModel);
        }
    }
}
