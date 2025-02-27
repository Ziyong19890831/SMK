using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Utility;
using SMK.Shared.Extensions;
using SMK.Web.Extensions;
using SMK.Web.Models;
using Yozian.DependencyInjectionPlus.Attributes;
using Yozian.Extension;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class IniDrDtlService : GenericService
    {
        public ILogger<IniDrDtlService> Logger { get; set; }

        public IniDrDtlService(SMKWEBContext context, SessionManager smgr,
            ILogger<IniDrDtlService> logger) : base(context, smgr)
        {
            Logger = logger;
        }

        public async Task<LogicRtnModel<PagedModel<IniDrDtlViewModel>>> GetIniDrDtls(IniDrOrdQueryModel model)
        {
            try
            {
                var data = context.IniDrDtl
                    .Where(x => x.DataId == model.DataId && x.FeeYm == model.FeeYm)
                    .Select(x => x.CopyProperties<IniDrDtl, IniDrDtlViewModel>());
                return await QueryPaging(model.get(), data);
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                Logger.LogError(e.StackTrace);
                return new LogicRtnModel<PagedModel<IniDrDtlViewModel>>()
                {
                    IsSuccess = false,
                    ErrMsg = e.Message,
                    StackTrace = e.StackTrace,
                };
            }
        }
    }
}
