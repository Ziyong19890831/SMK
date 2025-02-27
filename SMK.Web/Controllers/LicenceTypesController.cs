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
    public class LicenceTypesController : BaseController
    {
        private readonly LicenceTypesService licenceTypesService;

        public LicenceTypesController(LicenceTypesService licenceTypesService)
        {
            this.licenceTypesService = licenceTypesService;
        }

        // GET: LicenceTypes
        public async Task<IActionResult> Index()
        {
            var list = licenceTypesService.Query((context) => context.GenLicenceType);
            return View(await list);
        }

        // GET: LicenceTypes/Details/5
        public async Task<IActionResult> Details(string id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var genBranch = await licenceTypesService.FindOne<GenLicenceType>((context) => context.Where(m => m.LicenceType == id));
            if (genBranch == null)
            {
                return NotFound();
            }

            return View(genBranch);
        }

        // GET: LicenceTypes/Create
        public IActionResult Create()
        {
            return View(new GenLicenceType());
        }

        // POST: LicenceTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LicenceType,LicenceName")] GenLicenceType genLicenceType)
        {
            if (!ModelState.IsValid)
            {
                return View(genLicenceType);
            }

            var rtnModel = await licenceTypesService.Create(genLicenceType, new GenLicenceTypeValidator().Validate);
            if (rtnModel.IsSuccess)
            {
                return RedirectTo(rtnModel, nameof(this.Index));
            }
            else
            {
                return View(rtnModel);
            }

        }

        // GET: LicenceTypes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rtnModel = await licenceTypesService.FindOne<GenLicenceType>((context) => context.Where(m => m.LicenceType == id));
            if (rtnModel == null)
            {
                return NotFound();
            }

            return View(rtnModel);

           
        }

        // POST: LicenceTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("LicenceType,LicenceName")] GenLicenceType genLicenceType)
        {
            if (id != genLicenceType.LicenceType)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(genLicenceType);
            }
            var rtnModel = await licenceTypesService.Update(genLicenceType,
                new GenLicenceTypeValidator().Validate,
                false,
                x => x.LicenceName);
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
            GenLicenceType genLicenceType = new GenLicenceType()
            {
                LicenceType = id
            };

            var rtnModel = await licenceTypesService.Remove(genLicenceType);
            return Json(rtnModel);
        }
    }
}
