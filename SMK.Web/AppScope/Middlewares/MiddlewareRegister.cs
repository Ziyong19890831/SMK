using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.AppScope.Middlewares
{
    public static class MiddlewareRegister
    {
        public static IApplicationBuilder UseMyFilters(this IApplicationBuilder app)
        {
            app.Use(RequestTimeElapsedMiddleware.Executor);
            app.UseMiddleware(typeof(ErrorHandlerMiddleware));
            return app;
        }
    }
}