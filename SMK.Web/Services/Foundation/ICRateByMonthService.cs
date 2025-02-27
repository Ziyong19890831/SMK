using Microsoft.EntityFrameworkCore.Internal;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Utility;
using SMK.Shared.Extensions;
using SMK.Web.Extensions;
using SMK.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;
using Yozian.Extension;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace SMK.Web.Services.Foundation
{
    /// <summary>
    /// 申報資料報表
    /// </summary>
    [ScopedService]
    public class ICRateByMonthService : GenericService
    {
        public ICRateByMonthService(SMKWEBContext context, SessionManager smgr)
            : base(context, smgr)
        {

        }

        /// <summary>
        /// 健保卡過卡-層級別醫院別單月過卡率
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<ICRateByMonthViewModel>>> GetICRateByMonth(ICRateByMonthQueryModel model)
        {
            var YearMonthStart = model.YearMonthStart?.ToYYYYMMFromTaiwan();
            var YearMonthEnd = model.YearMonthEnd?.ToYYYYMMFromTaiwan();

            var list = context.ICRateByMonth
                          .WhereWhen(!string.IsNullOrEmpty(YearMonthStart),
                                     p => string.Compare(p.FeeYM, YearMonthStart) >= 0)
                          .WhereWhen(!string.IsNullOrEmpty(YearMonthEnd),
                                     p => string.Compare(YearMonthEnd, p.FeeYM) >= 0)
                          .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospID == model.HospID)
                          .WhereWhen(!string.IsNullOrEmpty(model.HospName), x => x.HospName.Contains(model.HospName))
                          .WhereWhen(!string.IsNullOrEmpty(model.HospContName), x => x.LastContType.Contains(model.HospContName))
                          .WhereWhen(model.RateFor90Percent, x => Convert.ToDouble(x.Rate) < 90)
                          .OrderBy(x => x.LastContType)     // 按照 LastContType 欄位升序排序
                          .ThenBy(x => x.HospID)     // 按照 HospID 欄位升序排序
                          .ThenBy(x => x.HospSeqNo)     // 按照 HospSeqNo 欄位升序排序
                          .ThenBy(x => x.FeeYM)  // 按照 FeeYM 欄位升序排序
                          .Select(p => new ICRateByMonthViewModel()
                          {
                              LastContType = p.LastContType,
                              HospDataType = p.HospDataType,
                              HospID = p.HospID,
                              HospSeqNo = p.HospSeqNo,
                              HospName = p.HospName,
                              FeeYM = p.FeeYM,
                              CureType = p.CureType,
                              samples = p.samples,
                              Rate = p.Rate,
                              NoRate = p.NoRate
                          });

            return await QueryPaging(model.get(), list);
        }


    }
}
