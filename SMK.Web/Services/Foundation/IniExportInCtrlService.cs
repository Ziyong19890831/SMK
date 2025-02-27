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
    public class IniExportInCtrlService : GenericService
    {
        public ILogger<IniExportInCtrlService> Logger { get; set; }

        public SMKWEBContext Context { get; set; }

        public NhiFileService NhiFileService { get; set; }
        public IniExportInCtrlService(SMKWEBContext context, SessionManager smgr,
            ILogger<IniExportInCtrlService> logger,
            NhiFileService nhiFileService) : base(context, smgr)
        {
            Context = context;
            Logger = logger;
            NhiFileService = nhiFileService;
        }

        public async Task<LogicRtnModel<PagedModel<IniExportInCtrlViewModel>>> Query(IniExportInCtrlQueryModel model)
        {
            try
            {
                var fileDate = "";
                if (!string.IsNullOrWhiteSpace(model.FileDate))
                {
                    fileDate = (Convert.ToInt32(model.FileDate.Replace("/", "")) + 191100).ToString();
                }

                DateTime? startCreatedAt = null;
                DateTime? endCreatedAt = null;
                if (!string.IsNullOrWhiteSpace(model.CreatedAt))
                {
                    startCreatedAt = model.CreatedAt.TwDateToDateTime().ToDateTime();
                    endCreatedAt = model.CreatedAt.TwDateToDateTime().ToDateTime()?.AddTicks(-1).AddDays(1);
                }

                var data = context.IniExportInCtrl
                    .WhereWhen(!string.IsNullOrWhiteSpace(model.fee_ym), x => x.fee_ym == model.fee_ym)
                    .WhereWhen(!string.IsNullOrWhiteSpace(model.CreatedAt), x => x.CreatedAt > startCreatedAt)
                    .WhereWhen(!string.IsNullOrWhiteSpace(model.CreatedAt), x => x.CreatedAt <= endCreatedAt)
                    .OrderByDescending(x => x.Id);

                var pagedDate = (await QueryPaging(model.get(), data)).Data.Data;
                var result = await QueryPaging(model.get(), pagedDate.AsAsyncQueryable()
                    .Select(x => new IniExportInCtrlViewModel()
                    {
                        Id = x.Id,
                        fee_ym = x.fee_ym,
                        StartedAt = x.StartedAt,
                        Status = x.Status,
                        StatusUpdatedAt = x.StatusUpdatedAt,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt
                    }));
                return result;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                Logger.LogError(e.StackTrace);
                return new LogicRtnModel<PagedModel<IniExportInCtrlViewModel>>()
                {
                    IsSuccess = false,
                    ErrMsg = e.Message,
                    StackTrace = e.StackTrace,
                };
            }
        }

        public async Task<LogicRtnModel<IEnumerable<IniExportInCtrlViewModel>>> ImportData(IniExportInCtrlQueryModel model)
        {
            using var tran = context.Database.BeginTransaction();
            try
            {

                var iniExportInCtrl = new IniExportInCtrl()
                {
                    Status = FileInStatus.Initialized,
                    fee_ym = model.fee_ym,
                    CreatedAt = DateTime.Now
                };
                await Context.IniExportInCtrl.AddAsync(iniExportInCtrl);
                await Context.SaveChangesAsync();
                await tran.CommitAsync();
            }
            catch (Exception e)
            {
                await tran.RollbackAsync();
                Logger.LogError(e.Message);
                Logger.LogError(e.StackTrace);
                return new LogicRtnModel<IEnumerable<IniExportInCtrlViewModel>>()
                {
                    IsSuccess = false,
                    ErrMsg = e.Message,
                    StackTrace = e.StackTrace,
                };
            }

            var result = await Query(model);
            return new LogicRtnModel<IEnumerable<IniExportInCtrlViewModel>>()
            {
                IsSuccess = result.IsSuccess,
                Data = result.Data.Data,
            };
        }
    }
}
