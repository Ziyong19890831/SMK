using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMK.Web.Services.Foundation;
using SMK.Web.Models;

namespace SMK.Web.Controllers
{
    public class DataInsertLogController : BaseController
    {
        private readonly DataInsertLogService DataInsertLogService;
        public DataInsertLogController(DataInsertLogService DataInsertLogService)
        {
            this.DataInsertLogService = DataInsertLogService;

        }
        public async Task <IActionResult> Index()
        {
            var list = DataInsertLogService.Query((context) => context.DataInsertLog.OrderByDescending(d=>d.ISNO));
            return View(await list);
        }
    }
}
