using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using EFCore.BulkExtensions;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Data.Utility;
using SMK.Web.Extensions;
using SMK.Worker.FileProcess.Handler;
using SMK.Worker.Services;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Worker.FileProcess
{
    [TransientService]
    public class IniOpDtlProcessor : FileInProcessor<IniOpDtl>, IIniFileInCtrlWriter
    {
        public SMKWEBContext Context { get; set; }
        public IniFileInCtrlService IniFileInCtrlService { get; set; }
        public IniOpDtlService IniOpDtlService { get; set; }
        public NhiScheduleService NhiScheduleService { get; set; }

        public IniOpDtlProcessor(SMKWEBContext context,
            IniFileInCtrlService iniFileInCtrlService,
            IniOpDtlService iniOpDtlService,
            NhiScheduleService nhiScheduleService)
        {
            IniOpDtlService = iniOpDtlService;
            IniFileInCtrlService = iniFileInCtrlService;
            Context = context;
            NhiScheduleService = nhiScheduleService;
            PrepareEnvironment = x =>
            {
                // wkDate = Format(CDate(Mid(wkYYYYMM, 1, 4) & "/" & Mid(wkYYYYMM, 5, 2) & "/19"), "yyyy/MM/dd")
                // wkTranDateS = DateFormat(Mid(DateAdd(DateInterval.Month, 1, DateAdd(DateInterval.Day, 1, wkDate)).ToString, 1, 10).Trim(), "1")
                // wkTranDateE = DateFormat(Mid(DateAdd(DateInterval.Month, 2, wkDate).ToString, 1, 10).Trim(), "1")
                var pat = FileInHandler.FilenamePattern;
                // Instantiate the regular expression object.
                var r = new Regex(pat, RegexOptions.IgnoreCase);

                // Match the regular expression pattern against a text string.
                var m = r.Match(FileName);
                var applyDate = (m.Groups[1].Value + "19").ToDateTime();
                var startDate = applyDate?.AddDays(1).AddMonths(1).ToDate();
                var endDate = applyDate?.AddMonths(2).ToDate();
                IniOpDtlService.Delete(startDate, endDate);
            };
            Loader = x =>
            {
                using var txn = context.Database.BeginTransaction();
                try
                {
                    context.BulkInsertOrUpdate(x.ToList());
                    txn.Commit();
                }
                catch (Exception e)
                {
                    WriteExceptionLog(context, e);
                    txn.Rollback();
                    throw;
                }
            };
            PostProcess = x =>
            {
                File.Delete(FileName);
            };
            OnProcessCompleted = x =>
            {
                NhiScheduleService.StartFinalTask();
                NhiScheduleService.StartFinalTask2();
                NhiScheduleService.DataInsertLog(FileName, x.Count());
            };
            FileInHandler = new IniOpDtlHandler();
        }

        public void Write(int id, FileInStatus status)
        {
            IniFileInCtrlService.ChangeIniDrOrdStatus(id, status);
        }

        public int Initialize(string filename)
        {
            return IniFileInCtrlService.Initialize(filename);
        }
    }
}
