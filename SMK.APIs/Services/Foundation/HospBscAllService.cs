using Newtonsoft.Json;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Utility;
using System.Collections.Generic;

namespace SMK.APIs.Services.Foundation
{
    public class HospBscAllService
    {
        private readonly SMKWEBContext context;

        public HospBscAllService(SMKWEBContext context)
        {
            this.context = context;
        }

        public string QueryHospBscAll(string token, string check_Token, HospBscAll hospBscAll_Data)
        {
            if (token != check_Token)
            {
                return JsonConvert.SerializeObject(null);
            }
            if (hospBscAll_Data.HospId != null)
            {
                var list = JsonConvert.SerializeObject(context.HospBscAll.Where(p=>p.HospId == hospBscAll_Data.HospId));
                return list;
            }
            else
            {
                var list = JsonConvert.SerializeObject(context.HospBscAll);
                return list;
            }
        }
}
}
