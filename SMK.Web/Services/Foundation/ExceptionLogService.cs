using Microsoft.EntityFrameworkCore;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;
using Yozian.Extension;

namespace SMK.Web.Services
{
    [ScopedService]
    public class ExceptionLogService : GenericService
    {
        private readonly SMKWEBContext _context;
        public ExceptionLogService(
            SMKWEBContext context,
            SessionManager smgr
          )
            : base(context, smgr)
        {
            _context = context;
        }
        public async Task< LogicRtnModel<PagedModel<ExceptionLog>>> Query(ExceptionLogQueryModel model)
        {
            var data = _context.ExceptionLog
                  .WhereWhen(!string.IsNullOrEmpty(model.Id), x => x.Id.Contains(model.Id))
                  .WhereWhen(!string.IsNullOrEmpty(model.Category), x => x.Category.Contains(model.Category))
                  .WhereWhen(model.StartTime.Ticks > 0, x => x.CreatedAt >= model.StartTime)
                  .WhereWhen(model.StopTime.Ticks > 0, x => x.CreatedAt <= model.StopTime)
                  .OrderByDescending(x => x.CreatedAt);
            return await QueryPaging(model.get(), data);
        }
    }
}