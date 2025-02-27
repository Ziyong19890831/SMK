using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Web.Models;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class PrsnEmailService : GenericService
    {
        public PrsnEmailService(SMKWEBContext context, SessionManager smgr) : base(context, smgr)
        {
        }

        public async Task<LogicRtnModel<List<PrsnEmailViewModel>>> QueryPrsnEmails(string prsnId)
        {
            var result = await Query(c => c.PrsnEmail.Where(e => e.PrsnId == prsnId));
            if (result.IsSuccess)
            {
                return new LogicRtnModel<List<PrsnEmailViewModel>>()
                {
                    Data = result.Data.Select(e =>
                    {
                        return new PrsnEmailViewModel(e);
                    }).ToList()
                };
            }

            return new LogicRtnModel<List<PrsnEmailViewModel>>()
            {
                ErrMsg = result.ErrMsg,
            };
        }
    }
}