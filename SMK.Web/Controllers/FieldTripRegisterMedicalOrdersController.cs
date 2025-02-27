using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using SMK.Data.Dto;
using SMK.Data.Enums;
using SMK.Web.AppScope.Filters;
using SMK.Web.Models;
using SMK.Web.Services.Foundation;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Yozian.WebCore.Library.Utility.Excel;
using System.Linq;
using static SMK.Data.Enums.SmokingServicesType;
using System.Diagnostics;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    public class FieldTripRegisterMedicalOrdersController : BaseController
    {
        public FieldTripRegisterMedicalOrdersService fieldTripRegisterMedicalOrdersService { get; }
        public FieldTripRegisterMedicalOrdersController(FieldTripRegisterMedicalOrdersService fieldTripRegisterMedicalOrdersService)
        {
            this.fieldTripRegisterMedicalOrdersService = fieldTripRegisterMedicalOrdersService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 調閱醫令清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QueryRegisterMedicalOrders(FieldTripQueryViewModel model)
        {
            var ret = await fieldTripRegisterMedicalOrdersService.GetFieldRegisterMedicalOrders(model);
            return Json(ret); 
        }

        /// <summary>
        /// 健保治療下載
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        [HttpPost, ActionName("ExportRegisterMedicalOrders")]
        public async Task<IActionResult> ExportRegisterMedicalOrders(FieldTripQueryViewModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            LogicRtnModel<PagedModel<GetRegisterMedicalOrdersViewModel>> logicRtnModel = new LogicRtnModel<PagedModel<GetRegisterMedicalOrdersViewModel>>();
            logicRtnModel.IsSuccess = false;
            string SheetName = "調閱醫令清單";
            //初始化字典
            Dictionary<string, Tuple<string, bool>> DictionaryMedicalOrders = fieldTripRegisterMedicalOrdersService.DictionaryData();

            logicRtnModel = await fieldTripRegisterMedicalOrdersService.GetFieldRegisterMedicalOrders(model);

            if (model.SelectCheckBox != null && model.SelectCheckBox.Count > 0)
            {
                DictionaryMedicalOrders = fieldTripRegisterMedicalOrdersService.DictionaryDataForAll(DictionaryMedicalOrders, false);
                foreach (var item in model.SelectCheckBox)
                {
                    DictionaryMedicalOrders = fieldTripRegisterMedicalOrdersService.DictionaryDataForOutPut(DictionaryMedicalOrders, item);
                }
            }

            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }

            var list = logicRtnModel.Data.Data;

            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<GetRegisterMedicalOrdersViewModel>(list)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.HospID, "醫事機構代碼", DictionaryMedicalOrders["醫事機構代碼"].Item2);
                        bindder.ColumnFor(p => p.HospSeqNo, "院區別", DictionaryMedicalOrders["院區別"].Item2);
                        bindder.ColumnFor(p => p.HospName, "醫事機構名稱", DictionaryMedicalOrders["醫事機構名稱"].Item2);
                        bindder.ColumnFor(p => p.ID, "身分證號", DictionaryMedicalOrders["身分證號"].Item2);
                        bindder.ColumnFor(p => p.Birthday, "出生日期", DictionaryMedicalOrders["出生日期"].Item2);
                        bindder.ColumnFor(p => p.Name, "姓名", DictionaryMedicalOrders["姓名"].Item2);
                        bindder.ColumnFor(p => p.FuncDate, "就醫日期", DictionaryMedicalOrders["就醫日期"].Item2);
                        bindder.ColumnFor(p => p.OrderChiName, "醫令名稱", DictionaryMedicalOrders["醫令名稱"].Item2);
                        bindder.ColumnFor(p => p.OrderCode, "醫令代碼", DictionaryMedicalOrders["醫令代碼"].Item2);
                        bindder.ColumnFor(p => p.OrderUprice, "醫令單價", DictionaryMedicalOrders["醫令單價"].Item2);
                        bindder.ColumnFor(p => p.OrderQty, "醫令數量", DictionaryMedicalOrders["醫令數量"].Item2);
                        bindder.ColumnFor(p => p.OrderDot, "醫令金額", DictionaryMedicalOrders["醫令金額"].Item2);
                        bindder.ColumnFor(p => p.FeeYM, "費用年月", DictionaryMedicalOrders["費用年月"].Item2);
                        bindder.ColumnFor(p => p.OrderSeqNo, "醫令序號", DictionaryMedicalOrders["醫令序號"].Item2);
                        bindder.ColumnFor(p => p.MedApply, "治療申報", DictionaryMedicalOrders["治療申報"].Item2);
                        bindder.ColumnFor(p => p.InstructApply, "衛教申報", DictionaryMedicalOrders["衛教申報"].Item2);
                        bindder.ColumnFor(p => p.TraceApply, "追蹤申報", DictionaryMedicalOrders["追蹤申報"].Item2);
                        bindder.ColumnFor(p => p.ReleaseApply, "釋出申報", DictionaryMedicalOrders["釋出申報"].Item2);
                        bindder.ColumnFor(p => p.DataID, "電腦序號", DictionaryMedicalOrders["電腦序號"].Item2);
                    })
                    .GetResult(SheetName);
            });
            var fileName = $"MedicalOrders.{fileType.ToString()}";
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
