using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Web.AppScope.Filters;
using SMK.Web.Services.Foundation;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    public class QuitDataAllController : BaseController
    {
        private const long MaxFileSize = 1000000000000;
        public IWebHostEnvironment Env { get; set; }
        public FileService FileService { get; set; }

        protected readonly SMKWEBContext context;

        public QuitDataAllController(
            SMKWEBContext context,
            IWebHostEnvironment env,
            FileService fileService)
        {
            this.context = context;
            Env = env;
            FileService = fileService;
        }

        // GET
        public IActionResult Index()
        {
            return View();
        }
        [DisableRequestSizeLimit]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = MaxFileSize)]
        public async Task<IActionResult> UploadFile(IFormFile file, string type)
        {
            type = "11";//對應(FileType)戒菸率調查檔
            List<string> check_File_Name = new List<string>() {
                "QuitDataAll_",
                "_6M.txt",
                "_1Y.txt",
                "QuitDataAll_yyyyMM_6M.txt",
                "QuitDataAll.txt"
            };

            if (file.Length == 0 || file.FileName.Length != check_File_Name[3].Length)
            {
                return Json(new LogicRtnModel<bool>()
                {
                    IsSuccess = false,
                    ErrMsg = "未選擇檔案，或上傳檔案名稱錯誤",
                });
            }

            //QuitDataAll_202301_6M.txt
            var fileType = (FileType)(Convert.ToInt32(type));
            var file_FileName_title = file.FileName.ToString().Substring(0, check_File_Name[0].Length);
            var file_FileName_footer = file.FileName.ToString().Substring(check_File_Name[0].Length + 6 , check_File_Name[1].Length); 

            if (check_File_Name.IndexOf(file_FileName_title.ToString()) < 0 && check_File_Name.IndexOf(file_FileName_footer.ToString()) < 0)
            {
                return Json(new LogicRtnModel<bool>()
                {
                    IsSuccess = false,
                    ErrMsg = "上傳檔案名稱錯誤",
                });
            }

            //給worker操作用
            var path = $@"{Env.WebRootPath}\DataImport\{fileType.ToString()}\{check_File_Name[4]}";
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                fileInfo.Directory.Create();
            }

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }       
            
            //做文件備份處理
            var backup_path = $@"{Env.WebRootPath}\DataImport\Backup_txt\{fileType.ToString()}\{file.FileName}";
            var backup_fileInfo = new FileInfo(backup_path);
            if (!backup_fileInfo.Exists)
            {
                backup_fileInfo.Directory.Create();
            }

            if (System.IO.File.Exists(backup_path))
            {
                System.IO.File.Delete(backup_path);
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }       

            using (var stream = new FileStream(backup_path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            IniFileInCtrl _IniFileInCtrl = new Services.DataInsertLogService().Insert_IniFileInCtrl_Log(file.FileName, Data.Enums.FileInStatus.Initialized);
            await context.IniFileInCtrl.AddRangeAsync(_IniFileInCtrl);
            await context.SaveChangesAsync();

            return Json(await FileService.UploadFileLog(fileInfo.Name, fileType));
        }
        

    }
}