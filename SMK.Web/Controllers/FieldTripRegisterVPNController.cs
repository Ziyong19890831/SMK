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
    public class FieldTripRegisterVPNController : BaseController
    {
        public FieldTripRegisterService fieldTripRegisterService { get; }
        public FieldTripRegisterVPNController(FieldTripRegisterService fieldTripRegisterService,
            IWebHostEnvironment env)
        {
            this.fieldTripRegisterService = fieldTripRegisterService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登錄資料(VPN)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QueryFieldTripRegisterVPN(FieldTripQueryViewModel model)
        {
            switch (model.smokingServicesTypeEnums)
            {
                case SmokingServicesTypeEnums.Treat:
                    var ret = await fieldTripRegisterService.GetFieldTripRegisterVPNTreat(model);
                    return Json(ret);
                case SmokingServicesTypeEnums.HealthEducation:
                    var retTreat = await fieldTripRegisterService.GetFieldTripRegisterVPNHealth(model);
                    return Json(retTreat);
                default:
                    return Json(new LogicRtnModel<bool>()
                    {
                        ErrMsg = "查詢失敗",
                        IsSuccess = false
                    });
            }
        }

        [HttpPost, ActionName("ExportVpnTreat")]
        public async Task<IActionResult> ExportVpnTreat(FieldTripQueryViewModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            LogicRtnModel<PagedModel<TreatmentViewModel>> logicRtnModel = new LogicRtnModel<PagedModel<TreatmentViewModel>>();
            logicRtnModel.IsSuccess = false;
            string SheetName = "VPN治療";
            //初始化字典
            Dictionary<string, Tuple<string, bool>> DictionaryTreat = fieldTripRegisterService.DictionaryTreatData();

            logicRtnModel = await fieldTripRegisterService.GetFieldTripRegisterVPNTreat(model);

            if (model.SelectCheckBox != null && model.SelectCheckBox.Count > 0)
            {
                DictionaryTreat = fieldTripRegisterService.DictionaryDataForAll(DictionaryTreat, false);
                foreach (var item in model.SelectCheckBox)
                {
                    DictionaryTreat = fieldTripRegisterService.DictionaryDataForOutPut(DictionaryTreat, item);
                }
            }

            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }

            var list = logicRtnModel.Data.Data;
            List<TreatmentViewModel> Data = logicRtnModel.Data.Data.Select(x => new TreatmentViewModel()
            {
                DataType = x.DataType,
                HospId = x.HospId,
                HospSeqNo = x.HospSeqNo,
                HospName = x.HospName,
                ID = x.ID,
                Birthday = x.Birthday,
                Name = x.Name,
                Sex = x.Sex,
                TelD = x.TelD,
                TelM = x.TelM,
                TownName = x.TownName,
                InformADDR = x.InformADDR,
                ExamYear = x.ExamYear,
                CureStage = x.CureStage,
                CureWeek = x.CureWeek,
                FuncDate = x.FuncDate,
                CureItem1 = x.CureItem1,
                OrderChiName1 = x.OrderChiName1,
                CureNum1 = x.CureNum1,
                CureItem2 = x.CureItem2,
                OrderChiName2 = x.OrderChiName2,
                CureNum2 = x.CureNum2,
                CureItem3 = x.CureItem3,
                OrderChiName3 = x.OrderChiName3,
                CureNum3 = x.CureNum3,
                CureItem4 = x.CureItem4,
                OrderChiName4 = x.OrderChiName4,
                CureNum4 = x.CureNum4
            }).ToList();

            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<TreatmentViewModel>(Data)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.DataType, "類型", DictionaryTreat["類型"].Item2);
                        bindder.ColumnFor(p => p.HospId, "醫事機構代碼", DictionaryTreat["醫事機構代碼"].Item2);
                        bindder.ColumnFor(p => p.HospSeqNo, "院區別", DictionaryTreat["院區別"].Item2);
                        bindder.ColumnFor(p => p.HospName, "醫事機構名稱", DictionaryTreat["醫事機構名稱"].Item2);
                        bindder.ColumnFor(p => p.ID, "身分證號", DictionaryTreat["身分證號"].Item2);
                        bindder.ColumnFor(p => p.Birthday, "出生日期", DictionaryTreat["出生日期"].Item2);
                        bindder.ColumnFor(p => p.Name, "姓名", DictionaryTreat["姓名"].Item2);
                        bindder.ColumnFor(p => p.Sex, "性別", DictionaryTreat["性別"].Item2);
                        bindder.ColumnFor(p => p.TelD, "電話(日)", DictionaryTreat["電話(日)"].Item2);
                        bindder.ColumnFor(p => p.TelM, "手機", DictionaryTreat["手機"].Item2);
                        bindder.ColumnFor(p => p.TownName, "縣市鄉鎮", DictionaryTreat["縣市鄉鎮"].Item2);
                        bindder.ColumnFor(p => p.InformADDR, "通訊地址", DictionaryTreat["通訊地址"].Item2);
                        bindder.ColumnFor(p => p.ExamYear, "療程年度", DictionaryTreat["療程年度"].Item2);
                        bindder.ColumnFor(p => p.CureStage, "療程次數", DictionaryTreat["療程次數"].Item2);
                        bindder.ColumnFor(p => p.CureWeek, "用藥週數", DictionaryTreat["用藥週數"].Item2);
                        bindder.ColumnFor(p => p.FuncDate, "就醫日期", DictionaryTreat["就醫日期"].Item2);
                        bindder.ColumnFor(p => p.CureItem1, "醫令代碼1", DictionaryTreat["醫令代碼1"].Item2);
                        bindder.ColumnFor(p => p.OrderChiName1, "代碼名稱1(處方藥名)", DictionaryTreat["代碼名稱1(處方藥名)"].Item2);
                        bindder.ColumnFor(p => p.CureNum1, "量1(藥量)", DictionaryTreat["量1(藥量)"].Item2);
                        bindder.ColumnFor(p => p.CureItem2, "醫令代碼2", DictionaryTreat["醫令代碼2"].Item2);
                        bindder.ColumnFor(p => p.OrderChiName2, "代碼名稱2(處方藥名)", DictionaryTreat["代碼名稱2(處方藥名)"].Item2);
                        bindder.ColumnFor(p => p.CureNum2, "量2(藥量)", DictionaryTreat["量2(藥量)"].Item2);
                        bindder.ColumnFor(p => p.CureItem3, "醫令代碼3", DictionaryTreat["醫令代碼3"].Item2);
                        bindder.ColumnFor(p => p.OrderChiName3, "代碼名稱3(處方藥名)", DictionaryTreat["代碼名稱3(處方藥名)"].Item2);
                        bindder.ColumnFor(p => p.CureNum3, "量3(藥量)", DictionaryTreat["量3(藥量)"].Item2);
                        bindder.ColumnFor(p => p.CureItem4, "醫令代碼4", DictionaryTreat["醫令代碼4"].Item2);
                        bindder.ColumnFor(p => p.OrderChiName4, "代碼名稱4(處方藥名)", DictionaryTreat["代碼名稱4(處方藥名)"].Item2);
                        bindder.ColumnFor(p => p.CureNum4, "量4(藥量)", DictionaryTreat["量4(藥量)"].Item2);

                    })
                    .GetResult(SheetName);
            });
            var fileName = $"VPN_Treate.{fileType.ToString()}";
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

        [HttpPost, ActionName("ExportVpnHealth")]
        public async Task<IActionResult> ExportVpnHealth(FieldTripQueryViewModel model, ExcelType fileType)
        {
            model.Start = 0;
            model.Length = 999999;
            LogicRtnModel<PagedModel<HealthMentViewModel>> logicRtnModel = new LogicRtnModel<PagedModel<HealthMentViewModel>>();
            logicRtnModel.IsSuccess = false;
            string SheetName = "VPN衛教";
            //初始化字典
            Dictionary<string, Tuple<string, bool>> DictionaryTreat = fieldTripRegisterService.DictionaryHealthData();

            logicRtnModel = await fieldTripRegisterService.GetFieldTripRegisterVPNHealth(model);

            if (model.SelectCheckBox != null && model.SelectCheckBox.Count > 0)
            {
                DictionaryTreat = fieldTripRegisterService.DictionaryDataForAll(DictionaryTreat, false);
                foreach (var item in model.SelectCheckBox)
                {
                    DictionaryTreat = fieldTripRegisterService.DictionaryDataForOutPut(DictionaryTreat, item);
                }
            }

            if (!logicRtnModel.IsSuccess)
            {
                return RedirectTo(logicRtnModel, nameof(this.Index));
            }

            var list = logicRtnModel.Data.Data;
            List<HealthMentViewModel> Data = logicRtnModel.Data.Data.Select(x => new HealthMentViewModel()
            {
                DataType = x.DataType,
                HospId = x.HospId,
                HospSeqNo = x.HospSeqNo,
                HospName = x.HospName,
                ID = x.ID,
                Birthday = x.Birthday,
                Name = x.Name,
                Sex = x.Sex,
                TelD = x.TelD,
                TelM = x.TelM,
                TownName = x.TownName,
                InformADDR = x.InformADDR,
                ExamYear = x.ExamYear,
                CureStage = x.CureStage,
                CureWeek = x.CureWeek,
                FuncDate = x.FuncDate,
            }).ToList();

            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<HealthMentViewModel>(Data)
                    .DefineColumns((bindder) =>
                    {
                        bindder.ColumnFor(p => p.DataType, "類型", DictionaryTreat["類型"].Item2);
                        bindder.ColumnFor(p => p.HospId, "醫事機構代碼", DictionaryTreat["醫事機構代碼"].Item2);
                        bindder.ColumnFor(p => p.HospSeqNo, "院區別", DictionaryTreat["院區別"].Item2);
                        bindder.ColumnFor(p => p.HospName, "醫事機構名稱", DictionaryTreat["醫事機構名稱"].Item2);
                        bindder.ColumnFor(p => p.ID, "身分證號", DictionaryTreat["身分證號"].Item2);
                        bindder.ColumnFor(p => p.Birthday, "出生日期", DictionaryTreat["出生日期"].Item2);
                        bindder.ColumnFor(p => p.Name, "姓名", DictionaryTreat["姓名"].Item2);
                        bindder.ColumnFor(p => p.Sex, "性別", DictionaryTreat["性別"].Item2);
                        bindder.ColumnFor(p => p.TelD, "電話(日)", DictionaryTreat["電話(日)"].Item2);
                        bindder.ColumnFor(p => p.TelM, "手機", DictionaryTreat["手機"].Item2);
                        bindder.ColumnFor(p => p.TownName, "縣市鄉鎮", DictionaryTreat["縣市鄉鎮"].Item2);
                        bindder.ColumnFor(p => p.InformADDR, "通訊地址", DictionaryTreat["通訊地址"].Item2);
                        bindder.ColumnFor(p => p.ExamYear, "療程年度", DictionaryTreat["療程年度"].Item2);
                        bindder.ColumnFor(p => p.CureStage, "療程次數", DictionaryTreat["療程次數"].Item2);
                        bindder.ColumnFor(p => p.CureWeek, "用藥週數", DictionaryTreat["用藥週數"].Item2);
                        bindder.ColumnFor(p => p.FuncDate, "就醫日期", DictionaryTreat["就醫日期"].Item2);

                    })
                    .GetResult(SheetName);
            });
            var fileName = $"VPN_Health.{fileType.ToString()}";
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
