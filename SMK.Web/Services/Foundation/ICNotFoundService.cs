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
    public class ICNotFoundService : GenericService
    {
        public ICNotFoundService(SMKWEBContext context, SessionManager smgr)
            : base(context, smgr)
        {

        }

        /// <summary>
        /// 健保卡過卡-未過卡詳細資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<ICNotFoundViewModel>>> GetICNotFound(ICNotFoundQueryModel model)
        {
            var YearMonthStart = model.YearMonthStart?.ToYYYYMMFromTaiwan();
            var YearMonthEnd = model.YearMonthEnd?.ToYYYYMMFromTaiwan();

            var list = context.ICNotFound
                          .WhereWhen(!string.IsNullOrEmpty(YearMonthStart),
                                     p => string.Compare(p.FeeYM, YearMonthStart) >= 0)
                          .WhereWhen(!string.IsNullOrEmpty(YearMonthEnd),
                                     p => string.Compare(YearMonthEnd, p.FeeYM) >= 0)
                          .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospID == model.HospID)
                          .WhereWhen(!string.IsNullOrEmpty(model.HospName), x => x.HospName.Contains(model.HospName))
                          .WhereWhen(!string.IsNullOrEmpty(model.ID), x => x.ID == model.ID)
                          .WhereWhen(!string.IsNullOrEmpty(model.HospContName), x => x.LastContType.Contains(model.HospContName))
                          .OrderBy(x => x.LastContType)     // 按照 LastContType 欄位降序排序
                          .ThenBy(x => x.HospID)     // 按照 HospID 欄位降序排序
                          .ThenBy(x => x.HospSeqNo)     // 按照 HospSeqNo 欄位降序排序
                          .ThenBy(x => x.FeeYM)  // 按照 FeeYM 欄位降序排序
                          .ThenBy(x => x.FuncDate)  // 按照 FuncDate 欄位降序排序
                          .ThenBy(x => x.ID)  // 按照 ID 欄位降序排序
                          .Select(p => new ICNotFoundViewModel()
                          {
                              BranchName = p.BranchName,
                              LastContType = p.LastContType,
                              HospID = p.HospID,
                              HospSeqNo = p.HospSeqNo,
                              HospName = p.HospName,
                              HospDataType = p.HospDataType,
                              ID = p.ID,
                              Birthday = p.Birthday,
                              FuncDate = p.FuncDate,
                              FeeYM = p.FeeYM,
                              Data_id = p.Data_id,
                              CaseType = p.CaseType,
                              SeqNo = p.SeqNo,
                              Real_HospID = p.Real_HospID,
                              CureType = p.CureType,
                              ExpDot = p.ExpDot,
                              Note = p.Note
                          });

            return await QueryPaging(model.get(), list);
        }


    }
}
