using Microsoft.AspNetCore.Mvc.Rendering;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class GenHospContService
    {
        private readonly PersistenceService persistenceService;
        private readonly RtnModelService rtnModelService;

        public GenHospContService(PersistenceService persistenceService,
            RtnModelService rtnModelService)
        {
            this.persistenceService = persistenceService;
            this.rtnModelService = rtnModelService;
        }

        public List<SelectListItem> GenHospContSelectListItem()
        {
            return persistenceService.GetSelectLists(context => context.GenHospCont, x => x.HospContType, x => x.HospContName);
        }

        public LogicRtnModel<List<GenHospCont>> GetGenHospConts()
        {
            return rtnModelService.Query(context => context.GenHospCont);
        }
    }
}
