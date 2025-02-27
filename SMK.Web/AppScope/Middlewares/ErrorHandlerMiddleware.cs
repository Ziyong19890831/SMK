using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Data.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Yozian.Extension;
using Yozian.WebCore.Library.Utility;

namespace SMK.Web.AppScope.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(
               HttpContext context,
               ILogger<ErrorHandlerMiddleware> logger
            )
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Global");
                await handleExceptionAsync(context, ex);
                // just throw out !
                throw ex;
            }
        }

        private static async Task handleExceptionAsync(HttpContext context, Exception exception)
        {
            // MyValidationException validtionEx
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            var stackTrace = DumpDetail(exception);
            var result = $"Status={code},{ stackTrace }";

            var loggerFactory = context.RequestServices.GetService<ILoggerFactory>();

            var logger = loggerFactory.CreateLogger("InternalServerError");

            logger.LogError(result);

            // save to db
            var dbContext = context.RequestServices.GetService<SMKWEBContext>();

            var errLog = new ExceptionLog()
            {
                Id = MyGuid.NewGuid(),
                Category = "Global",
                Message = exception.Message,
                Source = exception.Source,
                StackTrace = stackTrace
            };

            dbContext.ExceptionLog.Add(errLog);

            await dbContext.SaveChangesAsync();
        }
        private static string DumpDetail(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            var st = new StackTrace(ex, true);
            var innerExeption = ex;
            while (innerExeption != null)
            {
                sb.AppendLine(innerExeption.Message);
                innerExeption = innerExeption.InnerException;
            }
            sb.AppendLine("--------Data----------");

            sb.AppendLine(JsonConvert.SerializeObject(ex.Data));

            sb.AppendLine("--------Stacktrace----------");

            st.GetFrames()
                          .Select(frame => new
                          {
                              FileName = frame.GetFileName(),
                              LineNumber = frame.GetFileLineNumber(),
                              ColumnNumber = frame.GetFileColumnNumber(),
                              Method = frame.GetMethod(),
                              Class = frame.GetMethod().DeclaringType
                          })
                          //.Where(x => !x.Class.Namespace.StartsWith("System") && !x.Class.Namespace.StartsWith("Microsoft"))
                          .Reverse()
                          .ForEach((info) =>
                          {
                              sb.AppendLine($"class:{info.Class}");
                              sb.AppendLine($"method:{info.Method}");
                              sb.AppendLine($"line:{info.LineNumber}");
                              sb.AppendLine($"column:{info.ColumnNumber}");
                              sb.AppendLine($"file:{info.FileName}");
                              sb.AppendLine("----------Next-----------");
                          });


            return sb.ToString();
        }
    }
}