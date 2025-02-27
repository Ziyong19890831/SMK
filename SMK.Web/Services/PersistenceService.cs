using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMK.Data;
using SMK.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;
using Yozian.Extension;

namespace SMK.Web.Services
{
    [ScopedService]
    public class PersistenceService
    {
        private readonly SessionManager smgr;
        private readonly IdentityModel identity;
        private SMKWEBContext context { get; set; }

        public PersistenceService(SMKWEBContext context, SessionManager smgr)
        {
            this.context = context;
            this.smgr = smgr;
            this.identity = smgr.Get<IdentityModel>();
        }

        public TEntity FindOne<TEntity>(
                Func<DbSet<TEntity>,
                IQueryable<TEntity>> query
            )
            where TEntity : class, new()
        {
            return query(context.Set<TEntity>()).First();
        }

        public virtual async Task<LogicRtnModel<TEntity>> FindOne<TEntity, TExtend>(
               Func<DbSet<TEntity>,
               IQueryable<TEntity>> query,
               Func<SMKWEBContext, Task<TExtend>> fetchExtendData
           )
           where TEntity : class, new()
        {
            try
            {
                var rtnModel = new LogicRtnModel<TEntity>();
                // if not found always throw ex
                rtnModel.Data = await query(context.Set<TEntity>()).FirstAsync();

                rtnModel.ExtendData = await fetchExtendData(context);

                return rtnModel;
            }
            catch (Exception ex)
            {
                return new LogicRtnModel<TEntity>(MsgType.QueryFail, ex.Message)
                {
                    StackTrace = ex.DumpDetail()
                };
            }

        }

        public List<TEntity> Query<TEntity>(Func<SMKWEBContext, IQueryable<TEntity>> preparedQuery)
        {
            return preparedQuery(context).ToList();
        }

        public List<SelectListItem> GetSelectLists<TEntity>(Func<SMKWEBContext, IQueryable<TEntity>> entity,
            Expression<Func<TEntity, string>> value,
            Expression<Func<TEntity, string>> text)
        {
            return entity(context)
                .Select(p => new SelectListItem(
                    text.Compile().Invoke(p),
                    value.Compile().Invoke(p)))
                .ToList();
        }
    }
}
