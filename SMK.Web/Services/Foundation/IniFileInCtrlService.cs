using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Data.Utility;
using SMK.Shared.Extensions;
using SMK.Web.Models;
using SMK.Web.Extensions;
using Yozian.DependencyInjectionPlus.Attributes;
using Yozian.Extension;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class IniFileInCtrlService : GenericService
    {
        public ILogger<IniFileInCtrlService> Logger { get; set; }

        public SMKWEBContext Context { get; set; }

        public NhiFileService NhiFileService { get; set; }
        public IniFileInCtrlService(SMKWEBContext context, SessionManager smgr,
            ILogger<IniFileInCtrlService> logger,
            NhiFileService nhiFileService) : base(context, smgr)
        {
            Context = context;
            Logger = logger;
            NhiFileService = nhiFileService;
        }

        public async Task<LogicRtnModel<PagedModel<IniFileInCtrlViewModel>>> Query(IniFileInCtrlQueryModel model)
        {
            try
            {

                DateTime? startCreatedAt = null;
                DateTime? endCreatedAt = null;
                if (!string.IsNullOrWhiteSpace(model.CreatedAt))
                {
                    startCreatedAt = model.CreatedAt.TwDateToDateTime().ToDateTime();
                    endCreatedAt = model.CreatedAt.TwDateToDateTime().ToDateTime()?.AddTicks(-1).AddDays(1);
                }

                var data = context.IniFileInCtrl
                    .WhereWhen(!string.IsNullOrWhiteSpace(model.FileName), x => x.Filename.Contains(model.FileName))
                    .WhereWhen(!string.IsNullOrWhiteSpace(model.fileInStatus.ToString()), x => x.Status == model.fileInStatus)
                    //.WhereWhen(!string.IsNullOrWhiteSpace(fileDate), x => x.Filename.Contains(fileDate))
                    .WhereWhen(!string.IsNullOrWhiteSpace(model.CreatedAt), x => x.CreatedAt > startCreatedAt)
                    .WhereWhen(!string.IsNullOrWhiteSpace(model.CreatedAt), x => x.CreatedAt <= endCreatedAt)
                    .OrderByDescending(x => x.Id);

                //var pagedDate = (await QueryPaging(model.get(), data)).Data.Data;
                var result = await QueryPaging(model.get(), data.AsAsyncQueryable()
                    .Select(x => new IniFileInCtrlViewModel()
                    {
                        Id = x.Id,
                        Filename = x.Filename,
                        StartedAt = x.StartedAt,
                        Status = x.Status,
                        StatusUpdatedAt = x.StatusUpdatedAt,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt,
                        FileIsExisted = File.Exists(NhiFileService.GetFolder(NhiFileService.GetFileType(x.Filename)) + "\\" + x.Filename),
                    }));
                return result;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                Logger.LogError(e.StackTrace);
                return new LogicRtnModel<PagedModel<IniFileInCtrlViewModel>>()
                {
                    IsSuccess = false,
                    ErrMsg = e.Message,
                    StackTrace = e.StackTrace,
                };
            }
        }

        public async Task<LogicRtnModel<IEnumerable<IniFileInCtrlViewModel>>> ImportData(IniFileInCtrlRunModel model)
        {
            var ctrl = await Context.IniFileInCtrl.FindAsync(model.Id);
            using var tran = context.Database.BeginTransaction();
            try
            {
                if (ctrl != null)
                {
                    Context.IniFileInCtrl.Remove(ctrl);
                }

                var iniFileInCtrl = new IniFileInCtrl()
                {
                    Status = FileInStatus.Initialized,
                    Filename = ctrl.Filename,
                    StartedAt = DateTime.Now,
                };
                await Context.IniFileInCtrl.AddAsync(iniFileInCtrl);
                await Context.SaveChangesAsync();
                File.SetCreationTime(NhiFileService.GetPathname(ctrl.Filename), DateTime.Now);
                await tran.CommitAsync();
            }
            catch (Exception e)
            {
                await tran.RollbackAsync();
                Logger.LogError(e.Message);
                Logger.LogError(e.StackTrace);
                return new LogicRtnModel<IEnumerable<IniFileInCtrlViewModel>>()
                {
                    IsSuccess = false,
                    ErrMsg = e.Message,
                    StackTrace = e.StackTrace,
                };
            }

            var result = await Query(model);
            return new LogicRtnModel<IEnumerable<IniFileInCtrlViewModel>>()
            {
                IsSuccess = result.IsSuccess,
                Data = result.Data.Data,
            };
        }
    }
}
