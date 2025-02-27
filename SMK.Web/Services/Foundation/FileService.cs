using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Data.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class FileService : GenericService
    {
        public FileService(SMKWEBContext context, SessionManager smgr)
            : base(context, smgr)
        {

        }

        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<FileUploadLog>> UploadFileLog(string fileName, FileType fileType) {
            FileUploadLog log = new FileUploadLog()
            {
                Id=MyGuid.NewGuid(),
                FileName = fileName,
                FileStatus = FileStatus.上傳,
                FileType = fileType,
                CreatedBy= identity.Account,
                UpdatedBy= identity.Account
            };
            return await base.Create(log, null, true);
        }
    }
}
