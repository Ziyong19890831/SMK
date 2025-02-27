using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Web.AppScope.Filters;
using SMK.Web.Services.Foundation;
using SMK.Web.Validator;

namespace SMK.Web.Controllers
{
    /// <summary>
    /// 健保檔上傳
    /// </summary>
    [EmpAuthorized]
    public class NhiFileController : BaseController
    {
        private const long MaxFileSize = 1000000000000;
        private readonly FileService FileService;
        private readonly string _folder, _folder1, _folder2, _folder3;
        public NhiFileService NhiFileService { get; set; }
       
        public NhiFileController(FileService FileService, IWebHostEnvironment env,
            NhiFileService nhiFileService)
        {
            
            this.FileService = FileService;
            _folder = $@"{env.WebRootPath}\DataImport\"+FileType.iniDrDtlTxt;
            _folder1 = $@"{env.WebRootPath}\DataImport\" + FileType.iniDrOrdTxt;
            _folder2 = $@"{env.WebRootPath}\DataImport\" + FileType.iniOpDtlTxt;
            _folder3 = $@"{env.WebRootPath}\DataImport\" + FileType.iniOpOrdTxt;
            NhiFileService = nhiFileService;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <returns></returns>
        [DisableRequestSizeLimit]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = MaxFileSize)]
        public async Task<IActionResult> UploadFile(IFormFile file,string types)
        {
            if (file.Length == 0)
            {
                return Json(new LogicRtnModel<bool>()
                {
                    IsSuccess = false,
                    ErrMsg = "未選擇檔案",
                });
            }
            var path="";
            switch (types)
            {
                case "0":
                     path = $@"{_folder}\{file.FileName}";
                    break;
                case "1":
                    path = $@"{_folder1}\{file.FileName}";
                    break;
                case "2":
                    path = $@"{_folder2}\{file.FileName}";
                    break;
                case "3":
                    path = $@"{_folder3}\{file.FileName}";
                    break;
            }
           
            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                fileInfo.Directory.Create();
            }

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            if (types == "0")
            {
                var result = await NhiFileService.UploadFile(fileInfo.Name, FileType.iniDrDtlTxt);
                return Json(result);
            }
            else if(types == "1")
            {
                var result = await NhiFileService.UploadFile(fileInfo.Name, FileType.iniDrDtlTxt);
                return Json(result);
            }
            else if (types == "2")
            {
                var result = await NhiFileService.UploadFile(fileInfo.Name, FileType.iniDrDtlTxt);
                return Json(result);
            }
            else
            {
                var result = await NhiFileService.UploadFile(fileInfo.Name, FileType.iniDrDtlTxt);
                return Json(result);
            }
        }
    }
}
