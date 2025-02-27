using Microsoft.AspNetCore.Mvc.Filters;
using SMK.Data;
using SMK.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.AppScope.Filters
{
    public class AuthorizationFilterFactory : IFilterFactory
    {
        // no cross request
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService(typeof(SMKWEBContext)) as SMKWEBContext;
            var smgr = serviceProvider.GetService(typeof(SessionManager)) as SessionManager;

            return new MyAuthorizationFilter(context, smgr);
        }
    }
}
