using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using SMK.Data.Enums;
using SMK.Web.AppScope.Filters;
using SMK.Web.Models;
using SMK.Web.Services.Foundation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yozian.WebCore.Library.Utility.Excel;
using Microsoft.Net.Http.Headers;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    public class ICcardByMonthController : BaseController
    {
        //private readonly string _folder;

        public ICcardByMonthService ICcardByMonthService { get; }

        public ICcardByMonthController(ICcardByMonthService iCcardByMonthService,
            IWebHostEnvironment env)
        {
            this.ICcardByMonthService = iCcardByMonthService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 每月過卡資料查詢
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QueryICcardByMonth(ICcardByMonthQueryModel model)
        {
            var ret = await ICcardByMonthService.GetICcardByMonth(model);
            return Json(ret);
        }

        /// <summary>
        /// 每月過卡資料查詢
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QueryICcardByMonthForPivot(ICcardByMonthQueryModel model)
        {
            var ret = await ICcardByMonthService.GetICcardByMonthForExcelPivot(model);
            return Json(ret);
        }


        [HttpPost, ActionName("ExportICcardByMonth")]
        public async Task<IActionResult> ExportICcardByMonth(ICcardByMonthQueryModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            var logicRtnModel = await ICcardByMonthService.GetICcardByMonth(model);
            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }
            var list = logicRtnModel.Data.Data;
            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<ICcardByMonthViewModel>(list)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.LastContType, "醫事機構層級");
                        bindder.ColumnFor(p => p.HospID, "醫療院所代碼");
                        bindder.ColumnFor(p => p.HospName, "醫事機構名稱");
                        bindder.ColumnFor(p => p.ICCard_YM, "刷卡年月");
                        bindder.ColumnFor(p => p.ICCard_Times, "刷卡筆數");
                    })
                    .GetResult();
            });
            var fileName = $"醫院別刷卡次數.{fileType.ToString()}";
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

        [HttpPost, ActionName("ExportICcardByMonthForPivot")]
        public async Task<IActionResult> ExportICcardByMonthForPivot(ICcardByMonthQueryModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            var logicRtnModel = await ICcardByMonthService.GetICcardByMonthForExcelPivot(model);
            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }
            var list = logicRtnModel.Data.Data;

            #region 把資料轉換成List，與同階層資料
            Dictionary<string, object> dataDictionary = null;
            List<Dictionary<string, object>> dataDictionaryList = new List<Dictionary<string, object>>();
            foreach (var data in list)
            {
                //新增字典
                dataDictionary = new Dictionary<string, object>
                {
                    {"醫事機構層級", data.LastContType},
                    {"醫療院所代碼", data.HospID},
                    {"醫事機構名稱", data.HospName}
                };
                //將年月的字典做合併
                Dictionary<string, int> monthlySumsDictionary = data.MonthlySums.ToDictionary(pair => pair.Key, pair => pair.Value);
                dataDictionary = dataDictionary.Concat(monthlySumsDictionary.Select(kv => new KeyValuePair<string, object>(kv.Key, kv.Value)))
                                               .ToDictionary(pair => pair.Key, pair => pair.Value);
                dataDictionaryList.Add(dataDictionary);
            }
            #endregion

            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<Dictionary<string, object>>(dataDictionaryList)
                    .DefineColumns((bindder) =>
                    {
                        foreach (var keyValuePair in dataDictionaryList.First())
                        {
                            //透過迴圈，把每列資料寫入到指定欄位
                            bindder.ColumnForDictionary(model => keyValuePair.Value, keyValuePair.Key, keyValuePair.Key);
                        }
                    })
                    .GetResult();
            });
            var fileName = $"醫院別刷卡次數.{fileType.ToString()}";
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
