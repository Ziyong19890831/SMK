using System;
using System.Linq;
using System.Threading.Tasks;
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
    public class IniOpDtlService : GenericService
    {
        public ILogger<IniOpDtlService> Logger { get; set; }

        public IniOpDtlService(SMKWEBContext context, SessionManager smgr,
            ILogger<IniOpDtlService> logger) : base(context, smgr)
        {
            Logger = logger;
        }
        
        public async Task<LogicRtnModel<PagedModel<IniOpDtlViewModel>>> GetIniOpDtls(IniOpDtlQueryModel model)
        {
            try
            {
                var data = context.IniOpDtl
                    .Where(x => x.DataId == model.DataId && x.FeeYm == model.FeeYm)
                    .Select(x => x.CopyProperties<IniOpDtl, IniOpDtlViewModel>());
                return await QueryPaging(model.get(), data);
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                Logger.LogError(e.StackTrace);
                return new LogicRtnModel<PagedModel<IniOpDtlViewModel>>()
                {
                    IsSuccess = false,
                    ErrMsg = e.Message,
                    StackTrace = e.StackTrace,
                };
            }
        }
    }
}
