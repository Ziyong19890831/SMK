using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.AppScope.Middlewares
{
    public static class RequestTimeElapsedMiddleware
    {

        public static Func<HttpContext, Func<Task>, Task> Executor = async (context, next) =>
        {
            var watch = new Stopwatch();
            watch.Start();

            context.Response.OnStarting(() =>
            {
                watch.Stop();
                if (!context.Response.Headers.ContainsKey("Time-Elapsed"))
                {
                    context.Response.Headers.Add("Time-Elapsed", watch.ElapsedMilliseconds.ToString() + " ms");
                }
                return Task.CompletedTask;
            });

            await next();
        };
    }
}
