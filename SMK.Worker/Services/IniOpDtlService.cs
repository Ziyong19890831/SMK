using System.Linq;
using EFCore.BulkExtensions;
using SMK.Data;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Worker.Services
{
    [SingletonService]
    public class IniOpDtlService
    {
        private SMKWEBContext Context { get; set; }

        public IniOpDtlService(SMKWEBContext context)
        {
            Context = context;
        }

        public void Delete(string startDate, string endDate)
        {
            Context.IniOpDtl
                .Where(e => e.TranDate.CompareTo(startDate) >= 0 && e.TranDate.CompareTo(endDate) <= 0)
                .BatchDelete();
        }
    }
}
