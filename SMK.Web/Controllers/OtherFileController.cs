using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMK.Data.Dto;
using SMK.Data.Enums;
using SMK.Web.Services.Foundation;

namespace SMK.Web.Controllers
{
    public class OtherFileController : BaseController
    {
        private readonly FileService FileService;
        private readonly OtherFileService OtherFileService;
        private readonly string _folder, _folder1, _folder2;

        public OtherFileController(FileService FileService,OtherFileService OtherFileService,  IWebHostEnvironment env)
        {
            this.OtherFileService = OtherFileService;
            this.FileService = FileService;
            _folder = $@"{env.WebRootPath}\DataImport\" + FileType.ICCardTxt;
            _folder1 = $@"{env.WebRootPath}\DataImport\" + FileType.SamplingListTxt;
            _folder2 = $@"{env.WebRootPath}\DataImport\" + FileType.HospBscAllTxt;


        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> UploadFile(IFormFile file, string types)
        {

            if (file.Length == 0)
            {
                return Json(new LogicRtnModel<bool>()
                {
                    IsSuccess = false,
                    ErrMsg = "未選擇檔案",
                });
            }
            var path = "";
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
            }

            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                fileInfo.Directory.Create();
            }
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            if (types == "0")
            {
                var result = await OtherFileService.UploadICCardData(file);
                return Json(result);
                //var result = await FileService.UploadFileLog(fileInfo.Name, FileType.ICCardTxt);
                
            }
            else if (types == "1")
            {
                var result = await FileService.UploadFileLog(fileInfo.Name, FileType.SamplingListTxt);
                return Json(result);
            }
            else
            {
                var result = await FileService.UploadFileLog(fileInfo.Name, FileType.HospBscAllTxt);
                return Json(result);
            }
        }
    }
}
