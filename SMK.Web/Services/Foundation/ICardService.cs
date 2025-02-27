using Microsoft.EntityFrameworkCore;
using SMK.Data;
using System.Data.Common;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class ICardService : GenericService
    {
        public ICardService(SMKWEBContext context, SessionManager smgr)
           : base(context, smgr)
        {
        }
    }
}
