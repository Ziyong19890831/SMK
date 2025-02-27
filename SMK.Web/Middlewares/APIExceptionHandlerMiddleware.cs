using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SMK.Web.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SMK.Web.Middlewares
{
    public class APIExceptionHandlerMiddleware
    {

        private readonly RequestDelegate _next;

        public APIExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (context.Request.Path.StartsWithSegments(new PathString("/api")))
                {
                    await HandleExceptionAsync(context, ex);
                }
                else
                {
                    throw;
                }
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            string message = exception.Message;
            switch (exception)
            {
                case APIException apiException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    message = apiException.Message;
                    break;
                case UnauthorizedAccessException unauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    message = unauthorizedAccessException.Message;
                    break;
                default:
                    break;
            }

            var result = JsonConvert.SerializeObject(new
            {
                succeeded = false,
                message
            });
            return context.Response.WriteAsync(result);
        }
    }

    public static class APIExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder APIExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<APIExceptionHandlerMiddleware>();
        }
    }
}
