using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Enums;
using SMK.Data.Utility;
using SMK.Web.Extensions;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Worker.Services
{
    [SingletonService]
    public class NhiScheduleService
    {
        private ILogger<NhiScheduleService> Logger { get; set; }

        private Dictionary<string, bool> Jobs { get; set; }

        public SMKWEBContext Context { get; set; }

        public NhiScheduleService(SMKWEBContext context,
            ILogger<NhiScheduleService> logger)
        {
            Context = context;
            Logger = logger;
        }

        public bool StartFinalTask()
        {
            return CheckTaskPool();
        }

        public bool StartFinalTask2()
        {
            var initasks = Context.IniFileInCtrl.Where(x => x.Status == FileInStatus.Finished && IniFileNameSuffix.Any(y => x.Filename.ToUpper().EndsWith(y)))
                .GroupBy(x => x.Filename.Substring(3, 6))
                .Select(x => new
                {
                    FileDate = x.Key,
                    Count = x.Count(),
                })
                .ToList();
            var vpntasks = Context.IniFileInCtrl.Where(x => x.Status == FileInStatus.Completed && vpnFileNameSuffix.Any(y => x.Filename.ToUpper().EndsWith(y)))
                .GroupBy(x => x.Filename.Substring(3, 6))
                .Select(x => new
                {
                    FileDate = x.Key,
                    Count = x.Count(),
                })
                .ToList();
            foreach (var task in initasks)
            {
                if (task.Count != IniFileSuccessCount)
                {
                    continue;
                }
                else if(! vpntasks.Any(x=> x.FileDate == task.FileDate && x.Count == vpnFileSuccessCount))
                {
                    continue;
                }
                try
                {
                    var results = RunPostProcessSp();
                    if (results.IsSuccess)
                    {
                        var iniFileInCtrls = Context.IniFileInCtrl.Where(x => x.Filename.Contains(task.FileDate));
                        foreach (var e in iniFileInCtrls)
                        {
                            e.Status = FileInStatus.Finished;
                        }
                        Context.SaveChanges();
                    }
                    return results.IsSuccess;
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message);
                    Logger.LogError(e.StackTrace);
                }
            }
            return true;
        }

        public int IniFileSuccessCount { get; set; } = 4;
        public int vpnFileSuccessCount { get; set; } = 4;
        public string[] IniFileNameSuffix = { "OPORD.TXT","DRDTL.TXT","DRORD.TXT","OPDTL.TXT"};
        public string[] vpnFileNameSuffix = { "CSTAGENTPATIENT.TXT", "CSTQSCURE.TXT", "CSTQSDATA.TXT", "CSTQSSTATE.TXT" };
        public bool CheckTaskPool()
        {
            var tasks = Context.IniFileInCtrl.Where(x => x.Status == FileInStatus.Completed && IniFileNameSuffix.Any(y=> x.Filename.ToUpper().EndsWith(y)))
                .GroupBy(x => x.Filename.Substring(3, 6) )
                .Select(x => new
                {
                    FileDate = x.Key,
                    Count = x.Count(),
                })
                .ToList();
            foreach (var task in tasks)
            {
                if (task.Count != IniFileSuccessCount)
                {
                    continue;
                }
                try
                {
                    UpdateFirstDateAndExamYear(task.FileDate);
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message);
                    Logger.LogError(e.StackTrace);
                }
            }
            return true;
        }
        public bool DataInsertLog(string FileName, int Count)
        {
            using var tran = Context.GetTransactionScope();
            try
            {
                Context.DataInsertLog.Add(new Data.Entity.DataInsertLog()
                {
                    FileName = FileName,
                    FinishDate = DateTime.Now,
                    RecordCount = Count
                });
                Context.SaveChanges();
                tran.Commit();
                return true;
            }
            catch (Exception e)
            {
                tran.Rollback();
                Logger.LogError(e.Message);
                Logger.LogError(e.StackTrace);
                return false;
            }
        }
        public LogicRtnModel<bool> RunPostProcessSp()
        {
            using var tran = Context.GetTransactionScope();
            try
            {
                // sp_SetDtl***
                Context.Database.ExecuteSqlRaw("exec sp_SetDtl",null );

                // sp_SetVisitsOfB7***
                Context.Database.ExecuteSqlRaw("exec sp_SetVisitsOfB7",null);

                // sp_Population_QuitMan***
                Context.Database.ExecuteSqlRaw("exec sp_Population_QuitMan", null);

                tran.Commit();
                return new LogicRtnModel<bool>()
                {
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                tran.Rollback();
                Logger.LogError(e.Message);
                Logger.LogError(e.StackTrace);
                return new LogicRtnModel<bool>()
                {
                    IsSuccess = false,
                    ErrMsg = e.Message,
                    StackTrace = e.StackTrace,
                };
            }
        }

        public LogicRtnModel<bool> UpdateFirstDateAndExamYear(string fileDate)
        {
            if (fileDate.Length != 6) throw new Exception("filename length is too short. " + fileDate);

            var startDate = fileDate.ApplyStartDate()?.ToDate();
            var endDate = fileDate.ApplyEndDate()?.ToDate();

            var results = UpdateFirstDateAndExamYear(startDate, endDate);
            if (results.IsSuccess)
            {
                var iniFileInCtrls = Context.IniFileInCtrl.Where(x => x.Filename.Contains(fileDate));
                foreach (var e in iniFileInCtrls)
                {
                    e.Status = FileInStatus.Finished;
                }
                Context.SaveChanges();
            }
            return results;
        }

        public LogicRtnModel<bool> UpdateFirstDateAndExamYear(string startTranDate, string endTranDate)
        {
            using var tran = Context.GetTransactionScope();
            try
            {
                // 更新申報類別***
                Context.Database.ExecuteSqlRaw("exec UpdateApplyWeekCount @p0, @p1",
                    new object[] { startTranDate, endTranDate });
                Context.SaveChanges();

                // 建立暫存資料表
                Context.Database.ExecuteSqlRaw("exec UpdateTempTable");
                Context.SaveChanges();

                var ids = Context.IniOpDtl.Where(x => string.Compare(x.TranDate, startTranDate) >= 0 &&
                                                            string.Compare(x.TranDate, endTranDate) <= 0)
                    .ToList()
                    .GroupBy(x => x.Id)
                    .Select(x => new
                    {
                        Id = x.Key,
                    });
                foreach (var e in ids)
                {
                    // 院所療程初診日
                    Context.Database.ExecuteSqlRaw("exec UpdateOpProcess @p0, @p1, @p2",
                        new object[] { e.Id, startTranDate, endTranDate });

                    // 院所療程次數
                    Context.Database.ExecuteSqlRaw("exec UpdateExamTimeOp @p0, @p1, @p2",
                        new object[] { e.Id, startTranDate, endTranDate });
                }
                Context.SaveChanges();

                // 更新OP
                Context.Database.ExecuteSqlRaw("exec UpdateOpData @p0, @p1",
                    new object[] { startTranDate, endTranDate });
                Context.SaveChanges();

                // 建立暫存資料表
                Context.Database.ExecuteSqlRaw("exec UpdateTempTable");
                Context.SaveChanges();

                var iniDrDtls = Context.IniDrDtl.Where(x => string.Compare(x.TranDate, startTranDate) >= 0 &&
                                                            string.Compare(x.TranDate, endTranDate) <= 0)
                    .ToList()
                    .GroupBy(x => x.Id)
                    .Select(x => new
                    {
                        Id = x.Key,
                    });
                foreach (var e in iniDrDtls)
                {
                    // 藥局療程初診日
                    Context.Database.ExecuteSqlRaw("exec UpdateDrProcess @p0, @p1, @p2",
                        new object[] { e.Id, startTranDate, endTranDate });

                    // 藥局療程次數
                    Context.Database.ExecuteSqlRaw("exec UpdateExamTimeDr @p0, @p1, @p2",
                        new object[] { e.Id, startTranDate, endTranDate });
                }
                Context.SaveChanges();

                // 更新DR
                Context.Database.ExecuteSqlRaw("exec UpdateDrData @p0, @p1",
                    new object[] { startTranDate, endTranDate });
                Context.SaveChanges();

                tran.Commit();
                return new LogicRtnModel<bool>()
                {
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                tran.Rollback();
                Logger.LogError(e.Message);
                Logger.LogError(e.StackTrace);
                return new LogicRtnModel<bool>()
                {
                    IsSuccess = false,
                    ErrMsg = e.Message,
                    StackTrace = e.StackTrace,
                };
            }
        }
    }
}
