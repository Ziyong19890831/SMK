using System;
using System.IO;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Worker.FileProcess.Handler;
using Microsoft.EntityFrameworkCore;
using SMK.Shared.Utilities;
using SMK.Worker.Extension;
using Yozian.DependencyInjectionPlus.Attributes;
using SMK.Worker.Services;
using SMK.Data.Enums;

namespace SMK.Worker.FileProcess
{
    [TransientService]
    public class CstQsCureProcessor : FileInProcessor<MhbtQsCure>, IIniFileInCtrlWriter
    {
        public SMKWEBContext Context { get; set; }
        public IniFileInCtrlService IniFileInCtrlService { get; set; }
        public NhiScheduleService NhiScheduleService { get; set; }
        public string TempTableName { get; set; }
        public string TableName { get; set; } = "MhbtQsCure";
        public CstQsCureProcessor(SMKWEBContext context,
            IniFileInCtrlService iniFileInCtrlService,
            NhiScheduleService nhiScheduleService)
        {
            Context = context;
            IniFileInCtrlService = iniFileInCtrlService;
            NhiScheduleService = nhiScheduleService;
            PrepareEnvironment = args =>
            {
                TempTableName = Context.CreateTempTable(TableName);
            };
            Loader = x =>
            {
                context.BulkCopy(TempTableName, x);
            };
            PostProcess = x =>
            {
                using var txn = context.Database.BeginTransaction();
                try
                {
                    var toTableName = TableName + "_" + KeyGenerator.GetUniqueKey(5);
                    var fromTableName = TempTableName;
                    context.RenameTable(TableName, toTableName, context.Database.GetDbConnection());
                    context.RenameTable(fromTableName, TableName, context.Database.GetDbConnection());
                    context.DropTable(toTableName, context.Database.GetDbConnection());
                    txn.Commit();
                    File.Delete(FileName);
                }
                catch (Exception e)
                {
                    WriteExceptionLog(context, e);
                    txn.Rollback();
                    throw;
                }
            };
            OnProcessCompleted = x =>
            {
                NhiScheduleService.StartFinalTask();
                NhiScheduleService.StartFinalTask2();
                NhiScheduleService.DataInsertLog(FileName, x.Count);
            };
            FileInHandler = new CstQsCureHandler();
        }
        public void Write(int id, FileInStatus status)
        {
            IniFileInCtrlService.ChangeIniDrDtlStatus(id, status);
        }

        public int Initialize(string filename)
        {
            return IniFileInCtrlService.Initialize(filename);
        }
    }
}