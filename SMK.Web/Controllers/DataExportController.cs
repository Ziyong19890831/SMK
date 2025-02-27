using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using SMK.Data.Dto;
using SMK.Data.Enums;
using SMK.Web.AppScope.Filters;
using SMK.Web.Models;
using SMK.Web.Repositories;
using SMK.Web.Services.Foundation;
using Yozian.WebCore.Library.Utility.Excel;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    public class DataExportController : BaseController
    {
        private readonly HospBasicExportService hospBasicExportService;

        public DataExportController(HospBasicExportService hospBasicExportService)
        {
            this.hospBasicExportService = hospBasicExportService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> HospBasic(HospBasicExportQueryModel query, ExcelType fileType)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            var result = await hospBasicExportService.Query(query);
            if (!result.IsSuccess)
            {
                return View("Index");
            }
            
            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<HospBasicExportModel>(result.Data)
                    .DefineColumns((binder) =>
                    {
                        binder.ColumnFor(p => p.HospId, "機構代碼");
                        binder.ColumnFor(p => p.HospName, "機構名稱");
                        binder.ColumnFor(p => p.HospTel, "機構電話");
                        binder.ColumnFor(p => p.HospFax, "機構傳真");
                        binder.ColumnFor(p => p.HospOwnName, "負責人");
                        binder.ColumnFor(p => p.HospOwnID, "負責人ID");
                        binder.ColumnFor(p => p.ZIP, "郵遞區號");
                        binder.ColumnFor(p => p.HospAddress, "地址");
                        binder.ColumnFor(p => p.HospContName, "層級");
                        binder.ColumnFor(p => p.BranchName, "分局別");
                        binder.ColumnFor(p => p.Contact1, "聯絡人1");
                        binder.ColumnFor(p => p.ContactTel1, "聯絡人1電話");
                        binder.ColumnFor(p => p.Bcnt, "可給藥");
                        binder.ColumnFor(p => p.Ccnt, "可衛教");
                        binder.ColumnFor(p => p.Icnt, "治療品質改善");
                        binder.ColumnFor(p => p.Jcnt, "衛教品質改善");
                        binder.ColumnFor(p => p.MinHospStartDate, "合約生效日");
                        binder.ColumnFor(p => p.MaxHospEndDate, "合約終止日");
                        binder.ColumnFor(p => p.HospSeqNo, "院區別");
                    })
                    .GetResult();
            });
            var fileName = $"醫事機構清單.{fileType.ToString()}";
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

        [HttpPost]
        public async Task<IActionResult> PrsnContract(HospBasicExportQueryModel query, ExcelType fileType)
        {
             if (!ModelState.IsValid)
            {
                return View("Index");
            }
            var result = await hospBasicExportService.QueryPrsnContracts(query);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("errMsg", result.ErrMsg);
                return View("Index");
            }
            
            var excel = await Task.Run(() =>
            {
                return new MyExcelExporter<PrsnContractExportModel>(result.Data)
                    .DefineColumns((binder) =>
                    {
                        binder.ColumnFor(p => p.HospID, "機構代碼");
                        binder.ColumnFor(p => p.HospName, "機構名稱");
                        binder.ColumnFor(p => p.ZIP, "郵遞區號");
                        binder.ColumnFor(p => p.HospAddress, "地址");
                        binder.ColumnFor(p => p.PrsnName, "醫事人員姓名");
                        binder.ColumnFor(p => p.PrsnID, "醫事人員ID");
                        binder.ColumnFor(p => p.PrsnBirthday, "醫事人員出生日期");
                        binder.ColumnFor(p => p.PrsnTypeName, "醫事人員類別");
                        binder.ColumnFor(p => p.CouldTreat, "可給藥");
                        binder.ColumnFor(p => p.CouldInstruct, "可衛教");
                        binder.ColumnFor(p => p.MinPrsnStartDate, "合約生效日");
                        binder.ColumnFor(p => p.MaxPrsnEndDate, "合約終止日");
                        binder.ColumnFor(p => p.PEmail, "醫師E-mail");
                        binder.ColumnFor(p => p.HospEmail, "院所E-mail");
                        binder.ColumnFor(p => p.ContactEmail1, "聯絡人1 E-mail");
                        binder.ColumnFor(p => p.ContactEmail2, "聯絡人2 E-mail");
                        binder.ColumnFor(p => p.HospSeqNo, "院區別");
                    })
                    .GetResult();
            });
            var fileName = $"醫事人員合約清單.{fileType.ToString()}";
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

        [HttpPost]
        public async Task<IActionResult> Email(HospBasicExportQueryModel query, ExcelType fileType)
        {
             if (!ModelState.IsValid)
            {
                return View("Index");
            }
            var result = await hospBasicExportService.Query(query);
            if (!result.IsSuccess)
            {
                return View("Index");
            }
            
            var excel = new byte[0];
            var fileName = $"e-mail清單.{fileType.ToString()}";
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