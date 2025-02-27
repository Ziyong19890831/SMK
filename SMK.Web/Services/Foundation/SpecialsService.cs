using SMK.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class SpecialsService : GenericService
    {
        public SpecialsService(SMKWEBContext context, SessionManager smgr)
            : base(context, smgr)
        {

        }

    }
}
