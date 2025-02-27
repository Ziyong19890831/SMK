using System.Linq;
using SMK.Data;
using Yozian.DependencyInjectionPlus.Attributes;
using EFCore.BulkExtensions;

namespace SMK.Worker.Services
{
    [SingletonService]
    public class IniDrDtlService
    {
        private SMKWEBContext Context { get; set; }

        public IniDrDtlService(SMKWEBContext context)
        {
            Context = context;
        }

        public void DeleteIniDrDtl(string startDate, string endDate)
        {
            Context.IniDrDtl
                .Where(e => e.TranDate.CompareTo(startDate) >= 0 && e.TranDate.CompareTo(endDate) <= 0)
                .BatchDelete();
        }
    }
}