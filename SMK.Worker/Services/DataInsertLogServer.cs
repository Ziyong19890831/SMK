using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Data.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMK.Worker.Services
{
    public class DataInsertLogServer
    {
        public DataInsertLogServer(SMKWEBContext context)
        {
            Context = context;
        }

        public SMKWEBContext Context { get; set; }

        /// <summary>
        /// 上傳資料後的Log檔案
        /// </summary>
        /// <param name="Count">確認上傳幾筆資料</param>
        /// <param name="Update_Name">上傳的檔案名稱</param>
        /// <returns></returns>
        public List<DataInsertLog> dataInsertLogs(int Count, string Update_Name)
        {
            List<DataInsertLog> Insert_DataTable_Log = new List<DataInsertLog>();
            Insert_DataTable_Log.Add(new DataInsertLog
            {
                FileName = Update_Name,
                FinishDate = DateTime.Now,
                RecordCount = Count
            });

            return Insert_DataTable_Log;
        }


        public void Update_IniFileInCtrl_Log(SMKWEBContext context, Data.Enums.FileInStatus fileInStatus, string Update_Name)
        {
            IniFileInCtrl iniFileInCtrl = context.IniFileInCtrl.Where(x => x.Filename.Contains(Update_Name)).OrderByDescending(d => d.Id).FirstOrDefault();

            iniFileInCtrl.Filename = iniFileInCtrl.Filename;
            iniFileInCtrl.StartedAt = iniFileInCtrl.StartedAt;
            iniFileInCtrl.Status = fileInStatus;
            iniFileInCtrl.StatusUpdatedAt = DateTime.Now;
            iniFileInCtrl.UpdatedAt = DateTime.Now;

            context.Entry(iniFileInCtrl).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
