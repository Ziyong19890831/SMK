using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.AppScope.Filters
{
    public static class FilterRegister
    {

        public static MvcOptions AddMyFilters(this MvcOptions config)
        {
            config.Filters.Add(new AuthorizationFilterFactory());

            return config;
        }
    }
}
