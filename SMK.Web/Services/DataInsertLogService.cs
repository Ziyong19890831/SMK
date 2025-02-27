using SMK.Data.Entity;
using System;
using System.Collections.Generic;

namespace SMK.Web.Services
{
    public class DataInsertLogService
    {
        /// <summary>
        /// 儲存上傳檔案後的Log數據
        /// </summary>
        /// <param name="FileName">檔案名稱</param>
        /// <param name="All_Count">總共幾筆資料</param>
        /// <returns></returns>
        public DataInsertLog dataInsertLog(string FileName, int All_Count)
        {
            DataInsertLog dataInsertLog = new DataInsertLog
            {
                FileName = FileName.Substring(0, FileName.IndexOf(".")),
                FinishDate = DateTime.Now,
                RecordCount = All_Count,
            };
            return dataInsertLog;
        }

        /// <summary>
        /// 上傳資料後，新增資料到指定資料庫
        /// </summary>
        /// <param name="Update_Name">檔案名稱</param>
        /// <param name="fileInStatus">enums</param>
        /// <returns></returns>
        public IniFileInCtrl Insert_IniFileInCtrl_Log(string Update_Name, Data.Enums.FileInStatus fileInStatus)
        {

            IniFileInCtrl _Insert_IniFileInCtrl_Log = new IniFileInCtrl
            {
                Filename = Update_Name,
                StartedAt = DateTime.Now,
                Status = fileInStatus,
                StatusUpdatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            return _Insert_IniFileInCtrl_Log;
        }
    }
}
