using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Web.Services.Foundation;
using SMK.Web.Validator;

namespace SMK.Web.Controllers
{
    public class EndReasonsController : BaseController
    {


        private readonly EndReasonsService endReasonsService;

        public EndReasonsController(EndReasonsService endReasonsService)
        {
            this.endReasonsService = endReasonsService;
        }

        // GET: EndReasons
        public async Task<IActionResult> Index()
        {
            var list = endReasonsService.Query((context) => context.GenEndReason);
            return View(await list);
        }

        // GET: EndReasons/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var genEndReason = await endReasonsService.FindOne<GenEndReason>((context) => context.Where(m => m.EndReasonNo == id));
            if (genEndReason == null)
            {
                return NotFound();
            }

            return View(genEndReason);
        }

        // GET: EndReasons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EndReasons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EndReasonNo,EndReasonName")] GenEndReason genEndReason)
        {
            if (!ModelState.IsValid)
            {
                return View(genEndReason);
            }

            var rtnModel = await endReasonsService.Create(genEndReason, new GenEndReasonValidator().Validate);
            if (rtnModel.IsSuccess)
            {
                return RedirectTo(rtnModel, nameof(this.Index));
            }
            else
            {
                return View(rtnModel);
            }
        }

        // GET: EndReasons/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rtnModel = await endReasonsService.FindOne<GenEndReason>((context) => context.Where(m => m.EndReasonNo == id));
            if (rtnModel == null)
            {
                return NotFound();
            }


            return View(rtnModel);

        }

        // POST: EndReasons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("EndReasonNo,EndReasonName")] GenEndReason genEndReason)
        {
            if (id != genEndReason.EndReasonNo)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(genEndReason);
            }
            var rtnModel = await endReasonsService.Update(genEndReason,
                new GenEndReasonValidator().Validate,
                false,
                x => x.EndReasonName);
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
            GenEndReason genEndReason = new GenEndReason()
            {
                EndReasonNo = id
            };

            var rtnModel = await endReasonsService.Remove(genEndReason);
            return Json(rtnModel);
        }
    }
}
