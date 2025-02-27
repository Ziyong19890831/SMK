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
    public class IniReportService : GenericService
    {
        public ILogger<IniReportService> Logger { get; set; }

        public IniReportService(SMKWEBContext context, SessionManager smgr,
            ILogger<IniReportService> logger) : base(context, smgr)
        {
            Logger = logger;
        }

        public async Task<LogicRtnModel<PagedModel<IniReportViewModel>>> GetIniReports(IniReportQueryModel model)
        {
            var ContractYMStart = model.ContractYmS?.ToYYYYMMFromTaiwan();
            var ContractYMStartYear = ContractYMStart.Substring(0, 4);
            var ContractYMEnd = model.ContractYmE?.ToYYYYMMFromTaiwan();
            var NhiYMStart = model.NhiYmS?.ToYYYYMMFromTaiwan();
            var NhiYMStartYear = NhiYMStart.Substring(0, 4);
            var NhiYMEnd = model.NhiYmE?.ToYYYYMMFromTaiwan();
            var curYear = model.ContractYmE.TwDateToDateTime().Substring(0, 4);
            try
            {
                var data = context.IniMonthDetail
                   .WhereWhen(!string.IsNullOrEmpty(ContractYMStart), p => string.Compare(p.ContractYM, ContractYMStartYear) >= 0)
                   .WhereWhen(!string.IsNullOrEmpty(ContractYMEnd), p => string.Compare(ContractYMEnd, p.ContractYM) >= 0)
                   .WhereWhen(!string.IsNullOrEmpty(NhiYMStart), p => string.Compare(p.NhiYM, NhiYMStartYear) >= 0)
                   .WhereWhen(!string.IsNullOrEmpty(NhiYMEnd), p => string.Compare(NhiYMEnd, p.NhiYM) >= 0)
                   .Select(x => x.CopyProperties<IniMonthDetail, IniReportViewModel>());
                //篩選非本年度的要groupby在一起取最大日期的資料
                var list = await data.ToListAsync();
                var result = list.Where(p => curYear != p.ContractYM.Substring(0, 4))
                                 .GroupBy(p => p.ContractYM.Substring(0, 4))
                                 .Select(p =>
                                 {
                                     var _data = p.OrderBy(a => a.ContractYM).Last();
                                     _data.ContractYM = p.Key;
                                     _data.NhiYM = _data.NhiYM.Substring(0, 4);
                                     return _data;
                                 })
                                 .ToList()
                                 .Concat(list.Where(p => curYear == p.ContractYM.Substring(0, 4)))
                                 .AsAsyncQueryable();
                return await QueryPaging(model.get(), result);
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                Logger.LogError(e.StackTrace);
                return new LogicRtnModel<PagedModel<IniReportViewModel>>()
                {
                    IsSuccess = false,
                    ErrMsg = e.Message,
                    StackTrace = e.StackTrace,
                };
            }
        }
    }
}
