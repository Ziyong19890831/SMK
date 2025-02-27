using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using SMK.Data;
using SMK.Data.Attributes;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Shared.Extensions;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class NhiFileService : GenericService
    {
        public SMKWEBContext Context { get; set; }
        public FileService FileService { get; set; }
        public IWebHostEnvironment Env { get; set; }
        public NhiFileService(SMKWEBContext context, SessionManager smgr,
            FileService fileService,
            IWebHostEnvironment env)
            : base(context, smgr)
        {
            Context = context;
            FileService = fileService;
            Env = env;
        }

        public async Task<LogicRtnModel<FileUploadLog>> UploadFile(string fullName, FileType fileType)
        {
            try
            {
                var fileName = Path.GetFileName(fullName);
                var result = await FileService.UploadFileLog(fullName, fileType);
                var iniFileInCtrls = Context.IniFileInCtrl.Where(x => x.Filename == fileName);
                Context.IniFileInCtrl.RemoveRange(iniFileInCtrls);
                await Context.SaveChangesAsync();

                var iniFileInCtrl = new IniFileInCtrl()
                {
                    Status = FileInStatus.Initialized,
                    Filename = fileName,
                    StartedAt = DateTime.Now,
                };
                // await Create(iniFileInCtrl, null, true);
                await Context.IniFileInCtrl.AddAsync(iniFileInCtrl);
                await Context.SaveChangesAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new LogicRtnModel<FileUploadLog>()
                {
                    IsSuccess = false,
                    ErrMsg = e.Message  
                };
            }
        }

        public string GetPathname(string filename)
        {
            return Path.Join(GetFolder(filename), filename);
        }

        public string GetFolder(string fileName)
        {
            var values = Enum.GetValues(typeof(FileType)).Cast<FileType>();
            foreach (var value in values)
            {
                var pattern = value.GetAttribute<FilenamePatternAttribute>();
                if (pattern != null)
                {
                    if (fileName.Contains(pattern.FilenamePattern))
                        return GetFolder(value);
                }
            }

            return string.Empty;
        }

        public FileType GetFileType(string fileName)
        {
            var values = Enum.GetValues(typeof(FileType)).Cast<FileType>();
            foreach (var value in values)
            {
                var pattern = value.GetAttribute<FilenamePatternAttribute>();
                if (pattern != null)
                {
                    if (fileName.Contains(pattern.FilenamePattern))
                        return value;
                }
            }

            return FileType.iniDrDtlTxt;
        }

        public string GetFolder(FileType fileType)
        {
            return $@"{Env.WebRootPath}\DataImport\" + fileType.ToString();
        }
    }
}
