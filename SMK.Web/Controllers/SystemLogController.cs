using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMK.Web.AppScope.Filters;
using SMK.Web.Models;
using SMK.Web.Services.Foundation;
using Yozian.Extension;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    //[PrivilegeGuard]
    public class SystemLogController : BaseController
    {
        private readonly SystemLogService logService;

        public SystemLogController(SystemLogService logService)
        {
            this.logService = logService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AuditList(AuditLogQueryModel model)
        {
            var rtnModel = await logService.QueryPaging(
                model,
                (context) => context.AuditLog
                  .Where(x => x.Id.Length > 0)
                  .WhereWhen(!string.IsNullOrEmpty(model.ActionRemark), x => x.ActionRemark.Contains(model.ActionRemark))
                  .WhereWhen(!string.IsNullOrEmpty(model.Account), x => x.Account.Contains(model.Account))
                  .WhereWhen(!string.IsNullOrEmpty(model.ActionType), x => x.ActionTypeStr.Contains(model.ActionType))
                  .WhereWhen(model.StartTime.HasValue, x => x.CreatedAt >= model.StartTime.Value)
                  .WhereWhen(model.EndTime.HasValue, x => x.CreatedAt <= model.EndTime.Value)
                  .OrderByDescending(x => x.CreatedAt)
                );

            return Json(rtnModel);
        }
    }
}
