using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Data.Utility;
using SMK.Web.Helpers;
using SMK.Web.Models;
using SMK.Web.Services;
using SMK.Web.Services.Foundation;
using SMK.Web.Validator;
using Yozian.WebCore.Library.Utility.Excel;

namespace SMK.Web.Controllers
{
    /// <summary>
    /// 帳號管理
    /// </summary>
    public class EmpLogController : BaseController
    {
        private readonly EmpLogService empLogService;

        public EmpLogController(EmpLogService empLogService)
        {
            this.empLogService = empLogService;
        }

        // GET: EmpLog
        public IActionResult Index()
        {
            List<string> LoginLog_enum = new List<string>();
            foreach (LoginLog log in Enum.GetValues(typeof(LoginLog)))
            {
                LoginLog_enum.Add(log.GetEnumDescription());
            }
            ViewBag.LoginLog_enum = LoginLog_enum;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Query(EmpLoginLogViewModel model)
        {
            return Json(await empLogService.Query(model));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExportEmpLog(EmpLoginLogViewModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            var logicRtnModel = await empLogService.GetEmpLoginLog(model);
            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }
            var list = logicRtnModel.Data.Data;
            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<EmpLoginLogViewModel>(list)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.User_Account, "使用者帳號");
                        bindder.ColumnFor(p => p.User_Name, "使用者名字");
                        bindder.ColumnFor(p => p.LoginTime, "登入時間");
                        bindder.ColumnFor(p => p.LoginMsg, "登入狀態");
                        bindder.ColumnFor(p => p.Enable, "帳號狀態");
                    })
                    .GetResult();
            });
            var fileName = $"帳號登入紀錄.{fileType.ToString()}";
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            var contentDisposition = new ContentDispositionHeaderValue("attachment");
            contentDisposition.SetHttpFileName(fileName);
            Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

            return new FileContentResult(excel, contentType);
        }


    }
}
