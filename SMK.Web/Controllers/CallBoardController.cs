using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Utility;
using SMK.Web.AppScope.Filters;
using SMK.Web.Models;
using SMK.Web.Services;
using SMK.Web.Services.Foundation;
using SMK.Web.Validator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    public class CallBoardController : BaseController
    {
        private readonly CallBoardService callBoardService;
        private readonly string _folder;
        private readonly string _Deletefolder;
        public CallBoardController(CallBoardService callBoardServices, IWebHostEnvironment env)
        {
            callBoardService = callBoardServices;
            _folder = $@"{env.WebRootPath}\CallBoardFolder";
            _Deletefolder = $@"{env.WebRootPath}\CallBoardFolder\DeleteBackUp";
        }

        [HttpGet]
        public IActionResult Index()
        {
            var rtnModel = callBoardService.QueryCallBoard();
            if (rtnModel.IsSuccess)
            {
                List<CallBoardViewModel> bModel = new List<CallBoardViewModel>();
                string json = string.Empty;
                foreach (var item in rtnModel.Data)
                {
                    json = JsonConvert.SerializeObject(item);
                    CallBoardViewModel callBoardViewModel = JsonConvert.DeserializeObject<CallBoardViewModel>(json);
                    callBoardViewModel.Files = callBoardService.GetFilesData(_folder, item.SysNo);
                    bModel.Add(callBoardViewModel);
                }
                rtnModel.Data = bModel;
            }

            return View(rtnModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CallBoard model, List<IFormFile> files = null)
        {
            var rtnModel = await callBoardService.CreateCallBoard(model, files);
            if (rtnModel.IsSuccess)
            {
                if (files != null)
                {
                    var fileServices = await callBoardService.UpdateFiles(rtnModel.Data.SysNo, files, _folder);
                    if (!fileServices.IsSuccess)
                    {
                        rtnModel.IsSuccess = false;
                        rtnModel.ErrMsg = "檔案上傳失敗，但資料創建成功";
                        return RedirectTo(rtnModel, nameof(CallBoardController.Index));
                    }
                    rtnModel.Msg += fileServices.Msg;
                }
                return RedirectTo(rtnModel, nameof(CallBoardController.Index));
            }
            else
            {
                return View(rtnModel);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var rtnModel = await callBoardService
                .FindOne<CallBoard>((callBoards) => callBoards.Where(x => x.SysNo.Equals(id)).AsNoTracking());
            if (rtnModel.IsSuccess)
            {
                CallBoardViewModel bModel = new CallBoardViewModel();
                string json = JsonConvert.SerializeObject(rtnModel.Data);
                bModel = JsonConvert.DeserializeObject<CallBoardViewModel>(json);
                bModel.Files = callBoardService.GetFilesData(_folder, id);
                rtnModel.Data = bModel;
            }

            return View(rtnModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CallBoard model, List<IFormFile> files = null)
        {
            var rtnModel = await callBoardService.UpdateCallBoard(model, files);
            if (rtnModel.IsSuccess)
            {
                if (files != null)
                {
                    var fileServices = await callBoardService.UpdateFiles(rtnModel.Data.SysNo, files, _folder);
                    if (!fileServices.IsSuccess)
                    {
                        rtnModel.IsSuccess = false;
                        rtnModel.ErrMsg = "檔案上傳失敗，但資料創建成功";
                        return RedirectTo(rtnModel, nameof(CallBoardController.Index));
                    }
                    rtnModel.Msg += fileServices.Msg;
                }
                return RedirectTo(rtnModel, nameof(this.Index));
            }
            else
            {
                return View(rtnModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletFiles(int Sysno, string FileNames)
        {
            var rtnModel = await callBoardService.RemoveFile(Sysno, FileNames, _folder, _Deletefolder);
            return Json(rtnModel);// return RedirectToAction(nameof(RoleController.List));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCallBoard(int id)
        {
            var rtnModel = await callBoardService.DeleteOrConditionCallBoard(id, true, false);
            return Json(rtnModel);
        }

        [HttpPost]
        public async Task<IActionResult> ConditionCallBoard(int id, bool ChangeCondition)
        {
            var rtnModel = await callBoardService.DeleteOrConditionCallBoard(id, false, ChangeCondition);
            return RedirectTo(rtnModel, nameof(this.Index));
        }

        /// <summary>
        /// 下載檔案
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public IActionResult CallBoardGetFiles(string filePath)
        {
            filePath = Path.Combine(_folder, filePath);
            FileInfo info = new FileInfo(filePath);

            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(info.Name, out contentType))
            {
                contentType = "application/octet-stream";
            }
            var contentDisposition = new ContentDispositionHeaderValue("attachment");
            contentDisposition.SetHttpFileName(info.Name);
            Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

            return new FileContentResult(System.IO.File.ReadAllBytes(info.FullName), contentType);
        }
    }
}
