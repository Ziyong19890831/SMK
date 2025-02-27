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
    public class CstQsData2Processor : FileInProcessor<MhbtQsData2>, IIniFileInCtrlWriter
    {
        public SMKWEBContext Context { get; set; }
        public IniFileInCtrlService IniFileInCtrlService { get; set; }
        public NhiScheduleService NhiScheduleService { get; set; }
        public string TempTableName { get; set; }
        public string TableName { get; set; } = "MhbtQsData2";
        public int IniFileInCtrlId;
        public CstQsData2Processor(SMKWEBContext context,
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
                    //因目前沒有要做後續處理，所以先套上完成的功能
                    IniFileInCtrlService.ChangeMhbtQsData2Status(IniFileInCtrlId, FileInStatus.Finished);
                }
                catch (Exception e)
                {
                    WriteExceptionLog(context, e);
                    txn.Rollback();
                    IniFileInCtrlService.ChangeMhbtQsData2Status(IniFileInCtrlId, FileInStatus.Failed);
                    throw;
                }
            };
            OnProcessCompleted = x =>
            {
                NhiScheduleService.StartFinalTask();
                NhiScheduleService.StartFinalTask2();
                NhiScheduleService.DataInsertLog(FileName, x.Count);
            };
            FileInHandler = new CstQsData2Handler();
        }
        public void Write(int id, FileInStatus status)
        {
            IniFileInCtrlId = id;
            IniFileInCtrlService.ChangeIniDrDtlStatus(id, status);
        }

        public int Initialize(string filename)
        {
            return IniFileInCtrlService.Initialize(filename);
        }
    }
}
