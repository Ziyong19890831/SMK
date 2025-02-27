using SMK.Data;
using SMK.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;
using Yozian.Extension;

namespace SMK.Web.Services
{
    [ScopedService]
    public class RtnModelService
    {
        private readonly PersistenceService persistenceService;
        private readonly SessionManager smgr;
        private readonly IdentityModel identity;
        private SMKWEBContext context { get; set; }

        public RtnModelService(SMKWEBContext context, SessionManager smgr, PersistenceService persistenceService)
        {
            this.persistenceService = persistenceService;
            this.context = context;
            this.smgr = smgr;
            this.identity = smgr.Get<IdentityModel>();
        }

        public LogicRtnModel<List<TEntity>> Query<TEntity>(Func<SMKWEBContext, IQueryable<TEntity>> preparedQuery)
        {
            try
            {
                return new LogicRtnModel<List<TEntity>>
                {
                    Data = persistenceService.Query(preparedQuery)
                };
            }
            catch (Exception ex)
            {
                return new LogicRtnModel<List<TEntity>>(MsgType.QueryFail, ex.Message)
                {
                    StackTrace = ex.DumpDetail()
                };
            }
        }
    }
}
