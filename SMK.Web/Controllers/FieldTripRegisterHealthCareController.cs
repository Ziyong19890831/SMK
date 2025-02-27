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
    public class FieldTripRegisterHealthController : BaseController
    {
        public FieldTripRegisterHealthService fieldTripRegisterHealthService { get; }
        public FieldTripRegisterHealthController(FieldTripRegisterHealthService fieldTripRegisterHealthService,
            IWebHostEnvironment env)
        {
            this.fieldTripRegisterHealthService = fieldTripRegisterHealthService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登錄資料(健保)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QueryFieldTripRegisterHealth(FieldTripQueryViewModel model)
        {
            switch (model.smokingServicesTypeEnums)
            {
                case SmokingServicesTypeEnums.Treat:
                    var ret = await fieldTripRegisterHealthService.GetFieldTripRegisterHealthTreat(model);
                    return Json(ret);
                case SmokingServicesTypeEnums.HealthEducation:
                    var retTreat = await fieldTripRegisterHealthService.GetFieldTripRegisterHealthCareHealth(model);
                    return Json(retTreat);
                default:
                    return Json(new LogicRtnModel<bool>()
                    {
                        ErrMsg = "查詢失敗",
                        IsSuccess = false
                    });
            }
        }

        /// <summary>
        /// 健保治療下載
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        [HttpPost, ActionName("ExportHealthTreat")]
        public async Task<IActionResult> ExportHealthTreat(FieldTripQueryViewModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            LogicRtnModel<PagedModel<FieldTripRegisteHealthQueryViewModel>> logicRtnModel = new LogicRtnModel<PagedModel<FieldTripRegisteHealthQueryViewModel>>();
            logicRtnModel.IsSuccess = false;
            string SheetName = "健保治療";
            //初始化字典
            Dictionary<string, Tuple<string, bool>> DictionaryTreat = fieldTripRegisterHealthService.DictionaryTreatData();

            logicRtnModel = await fieldTripRegisterHealthService.GetFieldTripRegisterHealthTreat(model);

            if (model.SelectCheckBox != null && model.SelectCheckBox.Count > 0)
            {
                DictionaryTreat = fieldTripRegisterHealthService.DictionaryDataForAll(DictionaryTreat, false);
                foreach (var item in model.SelectCheckBox)
                {
                    DictionaryTreat = fieldTripRegisterHealthService.DictionaryDataForOutPut(DictionaryTreat, item);
                }
            }

            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }

            var list = logicRtnModel.Data.Data;

            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<FieldTripRegisteHealthQueryViewModel>(list)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.DataType, "類型", DictionaryTreat["類型"].Item2);
                        bindder.ColumnFor(p => p.HospId, "醫事機構代碼", DictionaryTreat["醫事機構代碼"].Item2);
                        bindder.ColumnFor(p => p.HospSeqNo, "院區別", DictionaryTreat["院區別"].Item2);
                        bindder.ColumnFor(p => p.HospName, "醫事機構名稱", DictionaryTreat["醫事機構名稱"].Item2);
                        bindder.ColumnFor(p => p.Id, "身分證號", DictionaryTreat["身分證號"].Item2);
                        bindder.ColumnFor(p => p.Birthday, "出生日期", DictionaryTreat["出生日期"].Item2);
                        bindder.ColumnFor(p => p.Name, "姓名", DictionaryTreat["姓名"].Item2);
                        bindder.ColumnFor(p => p.Sex, "性別", DictionaryTreat["性別"].Item2);
                        bindder.ColumnFor(p => p.TelD, "電話(日)", DictionaryTreat["電話(日)"].Item2);
                        bindder.ColumnFor(p => p.TelM, "手機", DictionaryTreat["手機"].Item2);
                        bindder.ColumnFor(p => p.TownName, "縣市鄉鎮", DictionaryTreat["縣市鄉鎮"].Item2);
                        bindder.ColumnFor(p => p.InformADDR, "通訊地址", DictionaryTreat["通訊地址"].Item2);
                        bindder.ColumnFor(p => p.ExamYearTW, "療程年度", DictionaryTreat["療程年度"].Item2);
                        bindder.ColumnFor(p => p.WeekCount, "用藥週數", DictionaryTreat["用藥週數"].Item2);
                        bindder.ColumnFor(p => p.FeeYm, "費用年月", DictionaryTreat["費用年月"].Item2);
                        bindder.ColumnFor(p => p.FuncDate, "就醫日期", DictionaryTreat["就醫日期"].Item2);
                        bindder.ColumnFor(p => p.ApplDate, "申報日期", DictionaryTreat["申報日期"].Item2);
                        bindder.ColumnFor(p => p.Order_code1, "醫令代碼1", DictionaryTreat["醫令代碼1"].Item2);
                        bindder.ColumnFor(p => p.OrderChiName1, "代碼名稱1(處方藥名)", DictionaryTreat["代碼名稱1(處方藥名)"].Item2);
                        bindder.ColumnFor(p => p.Order_qty1, "量1(藥量)", DictionaryTreat["量1(藥量)"].Item2);
                        bindder.ColumnFor(p => p.Order_code2, "醫令代碼2", DictionaryTreat["醫令代碼2"].Item2);
                        bindder.ColumnFor(p => p.OrderChiName2, "代碼名稱2(處方藥名)", DictionaryTreat["代碼名稱2(處方藥名)"].Item2);
                        bindder.ColumnFor(p => p.Order_qty2, "量2(藥量)", DictionaryTreat["量2(藥量)"].Item2);
                        bindder.ColumnFor(p => p.Order_code3, "醫令代碼3", DictionaryTreat["醫令代碼3"].Item2);
                        bindder.ColumnFor(p => p.OrderChiName3, "代碼名稱3(處方藥名)", DictionaryTreat["代碼名稱3(處方藥名)"].Item2);
                        bindder.ColumnFor(p => p.Order_qty3, "量3(藥量)", DictionaryTreat["量3(藥量)"].Item2);
                        bindder.ColumnFor(p => p.Order_code4, "醫令代碼4", DictionaryTreat["醫令代碼4"].Item2);
                        bindder.ColumnFor(p => p.OrderChiName4, "代碼名稱4(處方藥名)", DictionaryTreat["代碼名稱4(處方藥名)"].Item2);
                        bindder.ColumnFor(p => p.Order_qty4, "量4(藥量)", DictionaryTreat["量4(藥量)"].Item2);
                        bindder.ColumnFor(p => p.DataId, "電腦序號", DictionaryTreat["電腦序號"].Item2);
                    })
                    .GetResult(SheetName);
            });
            var fileName = $"HealthCare_Treate.{fileType.ToString()}";
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

        /// <summary>
        /// 健保衛教下載
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        [HttpPost, ActionName("ExportHealthHealth")]
        public async Task<IActionResult> ExportHealthHealth(FieldTripQueryViewModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            LogicRtnModel<PagedModel<FieldTripRegisteHealthQueryViewModel>> logicRtnModel = new LogicRtnModel<PagedModel<FieldTripRegisteHealthQueryViewModel>>();
            logicRtnModel.IsSuccess = false;
            string SheetName = "健保衛教";
            //初始化字典
            Dictionary<string, Tuple<string, bool>> DictionaryTreat = fieldTripRegisterHealthService.DictionaryHealthData();

            logicRtnModel = await fieldTripRegisterHealthService.GetFieldTripRegisterHealthCareHealth(model);

            if (model.SelectCheckBox != null && model.SelectCheckBox.Count > 0)
            {
                DictionaryTreat = fieldTripRegisterHealthService.DictionaryDataForAll(DictionaryTreat, false);
                foreach (var item in model.SelectCheckBox)
                {
                    DictionaryTreat = fieldTripRegisterHealthService.DictionaryDataForOutPut(DictionaryTreat, item);
                }
            }

            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }

            var list = logicRtnModel.Data.Data;

            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<FieldTripRegisteHealthQueryViewModel>(list)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.DataType, "類型", DictionaryTreat["類型"].Item2);
                        bindder.ColumnFor(p => p.HospId, "醫事機構代碼", DictionaryTreat["醫事機構代碼"].Item2);
                        bindder.ColumnFor(p => p.HospSeqNo, "院區別", DictionaryTreat["院區別"].Item2);
                        bindder.ColumnFor(p => p.HospName, "醫事機構名稱", DictionaryTreat["醫事機構名稱"].Item2);
                        bindder.ColumnFor(p => p.Id, "身分證號", DictionaryTreat["身分證號"].Item2);
                        bindder.ColumnFor(p => p.Birthday, "出生日期", DictionaryTreat["出生日期"].Item2);
                        bindder.ColumnFor(p => p.Name, "姓名", DictionaryTreat["姓名"].Item2);
                        bindder.ColumnFor(p => p.Sex, "性別", DictionaryTreat["性別"].Item2);
                        bindder.ColumnFor(p => p.TelD, "電話(日)", DictionaryTreat["電話(日)"].Item2);
                        bindder.ColumnFor(p => p.TelM, "手機", DictionaryTreat["手機"].Item2);
                        bindder.ColumnFor(p => p.TownName, "縣市鄉鎮", DictionaryTreat["縣市鄉鎮"].Item2);
                        bindder.ColumnFor(p => p.InformADDR, "通訊地址", DictionaryTreat["通訊地址"].Item2);
                        bindder.ColumnFor(p => p.ExamYearTW, "療程年度", DictionaryTreat["療程年度"].Item2);
                        bindder.ColumnFor(p => p.InctructSerial, "療程序號", DictionaryTreat["療程序號"].Item2);
                        bindder.ColumnFor(p => p.FeeYm, "費用年月", DictionaryTreat["費用年月"].Item2);
                        bindder.ColumnFor(p => p.FuncDate, "就醫日期", DictionaryTreat["就醫日期"].Item2);
                        bindder.ColumnFor(p => p.ApplDate, "申報日期", DictionaryTreat["申報日期"].Item2);
                        bindder.ColumnFor(p => p.Order_code1, "醫令代碼1", DictionaryTreat["醫令代碼1"].Item2);
                        bindder.ColumnFor(p => p.OrderChiName1, "代碼名稱1(處方藥名)", DictionaryTreat["代碼名稱1(處方藥名)"].Item2);
                        bindder.ColumnFor(p => p.Order_qty1, "量1(藥量)", DictionaryTreat["量1(藥量)"].Item2);
                        bindder.ColumnFor(p => p.Order_code2, "醫令代碼2", DictionaryTreat["醫令代碼2"].Item2);
                        bindder.ColumnFor(p => p.OrderChiName2, "代碼名稱2(處方藥名)", DictionaryTreat["代碼名稱2(處方藥名)"].Item2);
                        bindder.ColumnFor(p => p.Order_qty2, "量2(藥量)", DictionaryTreat["量2(藥量)"].Item2);
                        bindder.ColumnFor(p => p.DataId, "電腦序號", DictionaryTreat["電腦序號"].Item2);
                    })
                    .GetResult(SheetName);
            });
            var fileName = $"HealthCare_Health.{fileType.ToString()}";
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
