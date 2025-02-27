using SMK.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;


namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class PrsnTypesService : GenericService 
    {
        public PrsnTypesService(SMKWEBContext context, SessionManager smgr)
          : base(context, smgr)
        {

        }
    }
}
