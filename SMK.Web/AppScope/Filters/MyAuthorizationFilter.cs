using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Web.Controllers;
using SMK.Web.Services;
using System;
using System.Linq;

namespace SMK.Web.AppScope.Filters
{
    public class MyAuthorizationFilter : IAuthorizationFilter
    {
        private readonly SMKWEBContext context;
        private readonly SessionManager smgr;


        public MyAuthorizationFilter(SMKWEBContext context, SessionManager smgr)
        {
            this.context = context;
            this.smgr = smgr;
        }

        public void OnAuthorization(AuthorizationFilterContext authContext)
        {
             var descriptor = (ControllerActionDescriptor)authContext.ActionDescriptor;
            var shouldCheck = descriptor
                    .ControllerTypeInfo
                    .CustomAttributes
                    .Concat(descriptor.MethodInfo.CustomAttributes)
                    .Any(a => a.AttributeType == typeof(EmpAuthorizedAttribute));

            if (!shouldCheck)
            {
                return;
            }

            var identity = smgr.Get<IdentityModel>();

            if (identity == null || !identity.Authorized)
            {
                var pathString = authContext.HttpContext.Request.Path;
                var path = pathString.Value;
                if (path.ToLower().Contains("home/login"))
                {
                    path = string.Empty;
                }

                if (pathString.StartsWithSegments(new PathString("/api")))
                {
                    throw new UnauthorizedAccessException("尚未授權，請先登入!");
                }

                authContext.Result = new RedirectToActionResult("Login", nameof(HomeController).Replace("Controller", ""), new { redirect = path });
                return;
            }

            // privilege check

        }


    }

    public class EmpAuthorizedAttribute : Attribute
    {

    }
}



