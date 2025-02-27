using SMK.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class DataInsertLogService : GenericService
    {
        public DataInsertLogService(SMKWEBContext context, SessionManager smgr)
            : base(context, smgr)
        {

        }
    }
}
