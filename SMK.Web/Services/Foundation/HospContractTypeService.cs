using Microsoft.AspNetCore.Mvc.Rendering;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class HospContractTypeService : GenericService
    {
        private readonly PersistenceService persistenceService;
        private readonly RtnModelService rtnModelService;
        private readonly SMKWEBContext smkwebContext;

        public HospContractTypeService(SMKWEBContext context, SessionManager smgr,
            PersistenceService persistenceService,
            RtnModelService rtnModelService,
            SMKWEBContext smkwebContext): base(context, smgr)
        {
            this.persistenceService = persistenceService;
            this.rtnModelService = rtnModelService;
            this.smkwebContext = smkwebContext;
        }

        public LogicRtnModel<List<HospContractType>> GetHospContractTypes(string hospid, string hospseqno)
        {
            var ret = rtnModelService.Query(context =>
                    context.HospContractType.Where(p => p.HospId == hospid &&
                                                   p.HospSeqNo == hospseqno)
                    .Select(c => new HospContractType
                    {
                        HospId = c.HospId,
                        HospContType = c.HospContType,
                        CntSDate = c.CntSDate.WestToTwDate(),
                        CntEDate = c.CntEDate.WestToTwDate(),
                        HospSeqNo = c.HospSeqNo
                    }));
                ;
            return ret;
        }

        /// <summary>
        /// 査詢原機構特約資料，取最大起日效期
        /// </summary>
        /// <param name="hospId"></param>
        /// <param name="hospSeqNo"></param>
        /// <returns></returns>
        public IEnumerable<HospContractType> QueryTop1HospContractTypes(string hospId, string hospSeqNo)
        {
            // return await (from h in context.HospContractType
            //         group h by new {h.HospId, h.HospSeqNo}
            //         into groups
            //         select groups.OrderBy(p => p.CntSDate).First())
            //     .ToListAsync();

            // input.GroupBy(x => x.F1, (key,g) => g.OrderBy(e => e.F2).First());

            return context.HospContractType.Where(e => e.HospId == hospId)
                    .AsEnumerable()
                    .Where(e => e.HospSeqNo == hospSeqNo)
                    .GroupBy(x => new {x.HospId, x.HospSeqNo}, (key, g) => g.OrderByDescending(e => e.CntSDate).First())
                    .ToList();
            //
            //
            // return hospContractTypes
            //     .GroupBy(x => new {x.HospId, x.HospSeqNo}, (key, g) => g.OrderByDescending(e => e.CntSDate).First())
            //     .ToList();
            // '新增新的機構特約(轉入原機構特約資料)
            // sql = "insert into HospContractType(HospID,HospContType,CntSDate,HospSeqNo) "
            // sql += " select top 1 '" & wkg_CHospID & "',HospContType,'" & DatetoDB(wkg_CHospStartDate) & "','" & wkg_EHospSeqNo(g_count) & "' "
            // sql += " from HospContractType where HospID='" & wkg_EHospID & "' and HospSeqNo='" & wkg_EHospSeqNo(g_count) & "' order by CntSDate desc"
        }
    }
}
