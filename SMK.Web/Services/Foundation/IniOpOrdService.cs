using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Shared.Extensions;
using SMK.Web.Models;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class IniOpOrdService : GenericService
    {
        public ILogger<IniDrOrdService> Logger { get; set; }
        public IniOpOrdService(SMKWEBContext context, SessionManager smgr,
            ILogger<IniDrOrdService> logger) : base(context, smgr)
        {
            Logger = logger;
        }
        
        public async Task<LogicRtnModel<IEnumerable<IniOpOrdViewModel>>> Query(IniOpOrdQueryModel model)
        {
            try
            {
                var data = context.IniOpOrd
                    .Where(x => x.DataId == model.DataId && x.FeeYm == model.FeeYm)
                    .Select(x => x.CopyProperties<IniOpOrd, IniOpOrdViewModel>());
                return new LogicRtnModel<IEnumerable<IniOpOrdViewModel>>()
                {
                    IsSuccess = true,
                    Data = await data.ToListAsync(),
                };
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                Logger.LogError(e.StackTrace);
                return new LogicRtnModel<IEnumerable<IniOpOrdViewModel>>()
                {
                    IsSuccess = false,
                    ErrMsg = e.Message,
                    StackTrace = e.StackTrace,
                };
            }
        }
        public List<IniOpOrdViewModel> QueryOrdToDrOrd(IniDrOrdQueryModel model)
        {
            var data = context.IniOpOrd
                .Where(x => x.DataId == model.DataId && x.FeeYm == model.FeeYm)
                .Select(x => x.CopyProperties<IniOpOrd, IniOpOrdViewModel>()).ToList();
            return data.ToList();
        }
    }
}