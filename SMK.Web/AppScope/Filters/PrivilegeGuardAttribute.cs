using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SMK.Data.Dto;
using SMK.Web.Services;
using System;
using System.Linq;

namespace SMK.Web.AppScope.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PrivilegeGuardAttribute : ActionFilterAttribute
    {
        public PrivilegeGuardAttribute()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var smgr = new SessionManager(context.HttpContext);
            var identity = smgr.Get<IdentityModel>();
            var privileges = identity.Privileges;

            var descriptor = (ControllerActionDescriptor)context.ActionDescriptor;

            var privilege = privileges
                    .Where(x => descriptor.ControllerName.Equals(x.ControllerName)
                                && descriptor.ActionName.Equals(x.ActionName))
                    .FirstOrDefault();

            if (privilege == null || !privilege.EnableEntry)
            {
                string referer = context.HttpContext.Request.Headers["Referer"].ToString();
                if (referer == null)
                {
                    referer = "/Home/Index";
                }
                context.Result = new RedirectResult(referer);
                return;
            }

            var controller = context.Controller as Controller;
            // binding info
            controller.ViewBag.PageName = privilege.Name;
            controller.ViewBag.ControllerName = privilege.ControllerName;
            controller.ViewBag.ActionName = privilege.ActionName;

            base.OnActionExecuting(context);
        }


    }


}



