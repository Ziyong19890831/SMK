using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMK.Data.Dto;
using SMK.Data.Enums;
using SMK.Web.AppScope.Filters;
using SMK.Web.Services.Foundation;

namespace SMK.Web.Controllers
{
    /// <summary>
    /// VPN上傳
    /// </summary>
    [EmpAuthorized]
    public class SmkVPNFileController : BaseController
    {
        private const long MaxFileSize = 1000000000000;
        private readonly FileService FileService;
        private readonly string _folder, _folder1, _folder2, _folder3, _folder4;

        public SmkVPNFileController(FileService FileService, IWebHostEnvironment env)
        {
            this.FileService = FileService;
            _folder = $@"{env.WebRootPath}\DataImport\" + FileType.AgentPatientTxt;
            _folder1 = $@"{env.WebRootPath}\DataImport\" + FileType.QsDataTxt;
            _folder2 = $@"{env.WebRootPath}\DataImport\" + FileType.QsCureTxt;
            _folder3 = $@"{env.WebRootPath}\DataImport\" + FileType.QsStateTxt;
            _folder4 = $@"{env.WebRootPath}\DataImport\" + FileType.QsData2Txt;
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
                case "3":
                    path = $@"{_folder3}\{file.FileName}";
                    break;
                case "4":
                    path = $@"{_folder4}\{file.FileName}";
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
                var result = await FileService.UploadFileLog(fileInfo.Name, FileType.AgentPatientTxt);
                return Json(result);
            }
            else if (types == "1")
            {
                var result = await FileService.UploadFileLog(fileInfo.Name, FileType.QsCureTxt);
                return Json(result);
            }
            else if (types == "2")
            {
                var result = await FileService.UploadFileLog(fileInfo.Name, FileType.QsDataTxt);
                return Json(result);
            }
            else if (types == "3")
            {
                var result = await FileService.UploadFileLog(fileInfo.Name, FileType.QsStateTxt);
                return Json(result);
            }
            else //if(types == "3")
            {
                var result = await FileService.UploadFileLog(fileInfo.Name, FileType.QsData2Txt);
                return Json(result);
            }
        }
    }
}
