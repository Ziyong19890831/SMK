using System;
using System.IO;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Worker.FileProcess.Handler;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SMK.Shared.Utilities;
using SMK.Worker.Extension;
using Yozian.DependencyInjectionPlus.Attributes;
using System.Linq;
using System.Collections.Generic;
using Yozian.Extension;
using Microsoft.Data.SqlClient;
using System.Data;
using SMK.Worker.Services;

namespace SMK.Worker.FileProcess
{
    [TransientService]
    public class QuitDataAllProcessor : FileInProcessor<QuitDataAll>
    {
        public string TempTableName { get; set; }
        public SMKWEBContext Context { get; set; }
        List<QuitDataAll> update_Before_DataTable = new List<QuitDataAll>();//防止出Bug，還有機會回版
        public QuitDataAllProcessor(SMKWEBContext context)
        {
            var first = context.QuitDataAll;
            Context = context;
            PrepareEnvironment = x =>
            {
                //TempTableName = Context.CreateTempTable("QuitDataAll");
            };
            Loader = x =>
            {
                //context.BulkCopy(TempTableName, x);

                Update_IniFileInCtrl_Log(context,Data.Enums.FileInStatus.Running, "QuitDataAll");

                var check_Data = (QuitDataAll)null;
                using var txn = context.Database.BeginTransaction();
                try
                {
                    List<QuitDataAll> update_DataTable = new List<QuitDataAll>();
                    
                    List<QuitDataAll> Insert_DataTable = new List<QuitDataAll>();
                    x.ForEach(x =>
                    {
                        //判斷是否要新增
                        check_Data = first.Where(b => b.CaseNo == x.CaseNo && b.TimeSpan == x.TimeSpan && b.FirstMonth == x.FirstMonth).FirstOrDefault();
                        if (check_Data == null)
                        {
                            Insert_DataTable.Add(x);
                        }
                        else
                        {
                            update_DataTable.Add(x);
                            this.update_Before_DataTable.Add(check_Data);
                        }
                    });

                    var log_Name = context.IniFileInCtrl.Where(x => x.Filename.Contains("QuitDataAll")).OrderByDescending(d => d.Id).FirstOrDefault().Filename;
                    log_Name = log_Name.Substring(0, log_Name.IndexOf("."));

                    update_QuitDataAll(update_DataTable, context);//資料更新
                    context.BulkInsert(Insert_DataTable);//做資料新增
                    context.BulkInsert(dataInsertLogs(update_DataTable.Count+ Insert_DataTable.Count, log_Name));//做資料上傳的Log
                    txn.Commit();
                    Update_IniFileInCtrl_Log(context, Data.Enums.FileInStatus.Finished, "QuitDataAll");
                    Console.WriteLine("goyes");
                }
                catch (Exception e)
                {
                    update_QuitDataAll(this.update_Before_DataTable, context);//如果失敗的話，把已經更新的資料做回復
                    Update_IniFileInCtrl_Log(context, Data.Enums.FileInStatus.Failed, "QuitDataAll");
                    WriteExceptionLog(context, e);
                    txn.Rollback();
                    throw;
                }
            };
            PostProcess = x =>
            {
                File.Delete(FileName);
                //using var txn = context.Database.BeginTransaction();
                //try
                //{
                //    var toTableName = "QuitDataAll_" + KeyGenerator.GetUniqueKey(5);
                //    var fromTableName = TempTableName;
                //    context.RenameTable("QuitDataAll", toTableName, context.Database.GetDbConnection());
                //    context.RenameTable(fromTableName, "QuitDataAll", context.Database.GetDbConnection());
                //    context.DropTable(toTableName, context.Database.GetDbConnection());
                //    txn.Commit();
                //    File.Delete(FileName);
                //}
                //catch (Exception e)
                //{
                //    WriteExceptionLog(context, e);
                //    txn.Rollback();
                //    throw;
                //}
            };

            FileInHandler = new QuitDataAllHandler();
        }
        public void update_QuitDataAll(List<QuitDataAll> list_data, SMKWEBContext context)
        {
            for (int i = 0; i < list_data.Count; i++)
            {
                QuitDataAll quitDataAll = context.QuitDataAll.Where(b => b.CaseNo == list_data[i].CaseNo && b.TimeSpan == list_data[i].TimeSpan && b.FirstMonth == list_data[i].FirstMonth).FirstOrDefault();

                quitDataAll.HospID = list_data[i].HospID;
                quitDataAll.HospSeqNo = list_data[i].HospSeqNo;
                quitDataAll.ID = list_data[i].ID;
                quitDataAll.Birthday = list_data[i].Birthday;
                quitDataAll.Edition = list_data[i].Edition;
                quitDataAll.VisitDate = list_data[i].VisitDate;
                quitDataAll.Result = list_data[i].Result;
                quitDataAll.QuitPnt = list_data[i].QuitPnt;
                quitDataAll.QuitCtn = list_data[i].QuitCtn;
                quitDataAll.Edu = list_data[i].Edu;
                quitDataAll.Job = list_data[i].Job;
                context.Entry(quitDataAll).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

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