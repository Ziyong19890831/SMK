using System.Linq;
using EFCore.BulkExtensions;
using SMK.Data;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Worker.Services
{
    [SingletonService]
    public class IniOpOrdService
    {
        private SMKWEBContext Context { get; set; }

        public IniOpOrdService(SMKWEBContext context)
        {
            Context = context;
        }

        public void Delete(string startDate, string endDate)
        {
            Context.IniOpOrd
                .Where(e => e.TranDate.CompareTo(startDate) >= 0 && e.TranDate.CompareTo(endDate) <= 0)
                .BatchDelete();
        }
    }
}
