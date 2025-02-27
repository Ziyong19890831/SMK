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
    public class SpecialsController : BaseController
    {
        private readonly SpecialsService specialsService;

        public SpecialsController(SpecialsService specialsService)
        {
            this.specialsService = specialsService;
        }

        // GET: Specials
        public async Task<IActionResult> Index()
        {
            var list = specialsService.Query((context) => context.GenSpecial);
            return View(await list);
        }

        // GET: Specials/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var genSpecial = await specialsService.FindOne<GenSpecial>((context) => context.Where(m => m.SpecialistNo == id));
            if (genSpecial == null)
            {
                return NotFound();
            }

            return View(genSpecial);

        }

        // GET: Specials/Create
        public IActionResult Create()
        {
            return View(new GenSpecial());
        }

        // POST: Specials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpecialistNo,SpecialistName")] GenSpecial genSpecial)
        {

            if (!ModelState.IsValid)
            {
                return View(genSpecial);
            }

            var rtnModel = await specialsService.Create(genSpecial, new GenSpecialsValidator().Validate);
            if (rtnModel.IsSuccess)
            {
                return RedirectTo(rtnModel, nameof(this.Index));
            }
            else
            {
                return View(rtnModel);
            }
        }

        // GET: Specials/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rtnModel = await specialsService.FindOne<GenSpecial>((context) => context.Where(m => m.SpecialistNo == id));
            if (rtnModel == null)
            {
                return NotFound();
            }

            return View(rtnModel);


        }

        // POST: Specials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SpecialistNo,SpecialistName")] GenSpecial genSpecial)
        {
            if (id != genSpecial.SpecialistNo)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(genSpecial);
            }
            var rtnModel = await specialsService.Update(genSpecial,
                new GenSpecialsValidator().Validate,
                false,
                x => x.SpecialistName);
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
            GenSpecial genSpecial = new GenSpecial()
            {
                SpecialistNo = id
            };

            var rtnModel = await specialsService.Remove(genSpecial);
            return Json(rtnModel);
        }
    }
}
