using SMK.Data;
using SMK.Data.Dto;
using SMK.Web.Extensions;
using SMK.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;
using Yozian.Extension;

namespace SMK.Web.Services.Foundation
{
    /// <summary>
    /// 健保卡過卡-醫院別刷卡次數 
    /// </summary>
    [ScopedService]
    public class ICcardByMonthService : GenericService
    {
        public ICcardByMonthService(SMKWEBContext context, SessionManager smgr)
            : base(context, smgr)
        {

        }

        /// <summary>
        /// 健保卡過卡-層級別醫院別每月過卡率
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<ICcardByMonthViewModel>>> GetICcardByMonth(ICcardByMonthQueryModel model)
        {
            var search_YM = get_Range_YM(model.YearMonthStart, model.YearMonthEnd);

            var list = context.ICcardByMonth
                          .WhereWhen(search_YM[0] != null, x => search_YM.Contains(x.ICCard_YM))
                          .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospID == model.HospID)
                          .WhereWhen(!string.IsNullOrEmpty(model.HospName), x => x.HospName.Contains(model.HospName))
                          .WhereWhen(!string.IsNullOrEmpty(model.HospContName), x => x.LastContType.Contains(model.HospContName))
                          //.Where(predicate)
                          .OrderBy(x => x.LastContType)     // 按照 LastContType 欄位降序排序
                          .ThenBy(x => x.HospID)     // 按照 HospID 欄位降序排序
                          .ThenByDescending(x => x.ICCard_YM)  // 按照 ICCard_YM 欄位降序排序
                          .Select(p => new ICcardByMonthViewModel()
                          {
                              LastContType = p.LastContType,
                              HospID = p.HospID,
                              HospName = p.HospName,
                              ICCard_YM = p.ICCard_YM,
                              ICCard_Times = p.ICCard_Times,
                          });

            return await QueryPaging(model.get(), list);
        }

        /// <summary>
        /// 健保卡過卡-層級別醫院別每月過卡率
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<PagedModel<ICcardByMonthViewModel>>> GetICcardByMonthForExcelPivot(ICcardByMonthQueryModel model)
        {
            var search_YM = get_Range_YM(model.YearMonthStart, model.YearMonthEnd);

            var list = context.ICcardByMonth
                          .WhereWhen(search_YM[0] != null, x => search_YM.Contains(x.ICCard_YM))
                          .WhereWhen(!string.IsNullOrEmpty(model.HospID), x => x.HospID == model.HospID)
                          .WhereWhen(!string.IsNullOrEmpty(model.HospName), x => x.HospName.Contains(model.HospName))
                          .WhereWhen(!string.IsNullOrEmpty(model.HospContName), x => x.LastContType.Contains(model.HospContName))
                          //.Where(predicate)
                          .OrderBy(x => x.LastContType)     // 按照 LastContType 欄位降序排序
                          .ThenBy(x => x.HospID)     // 按照 HospID 欄位降序排序
                          .ThenByDescending(x => x.ICCard_YM)  // 按照 ICCard_YM 欄位降序排序
                          .Select(p => new ICcardByMonthViewModel()
                          {
                              LastContType = p.LastContType,
                              HospID = p.HospID,
                              HospName = p.HospName,
                              ICCard_YM = p.ICCard_YM,
                              ICCard_Times = p.ICCard_Times,
                          });

            // 取得所有不重複的月份
            var allMonths = list.Select(record => record.ICCard_YM).Distinct().OrderByDescending(x => x).ToList();

            // 使用 LINQ 進行資料轉換
            var results = list
                .AsEnumerable()
                .OrderByDescending(x => x.ICCard_YM)
                .GroupBy(record => new { record.LastContType, record.HospID, record.HospName })
                .Select(group =>
                {
                    var recordWithMonths = new ICcardByMonthViewModel
                    {
                        LastContType = group.Key.LastContType,
                        HospID = group.Key.HospID,
                        HospName = group.Key.HospName
                    };

                    foreach (var month in allMonths)
                    {
                        recordWithMonths.MonthlySums[month] = group
                            .Where(record => record.ICCard_YM == month)
                            .Sum(record => int.Parse(record.ICCard_Times));
                    }
                    return recordWithMonths;
                }).ToList();

            var pagedData = PagedModel<ICcardByMonthViewModel>.Create(results.AsQueryable(), model.get());
            pagedData.RecordsTotal = results.Count();

            return new LogicRtnModel<PagedModel<ICcardByMonthViewModel>>()
            {
                IsSuccess = true,
                Data = pagedData,
            };
        }


        /// <summary>
        /// 回傳時間的Range
        /// </summary>
        /// <param name="YearMonthStart">傳入起始年月11101</param>
        /// <param name="YearMonthEnd">傳入最後年月11101</param>
        /// <returns></returns>
        public List<string> get_Range_YM(string YearMonthStart, string YearMonthEnd)
        {
            List<string> result = new List<string>();
            List<int> year = new List<int>();

            if (YearMonthStart != null && YearMonthEnd != null)
            {
                YearMonthStart = YearMonthStart?.ToYYYY_MMFromTaiwan();
                YearMonthEnd = YearMonthEnd?.ToYYYY_MMFromTaiwan();
                var first_YM = YearMonthStart?.Split("/"); //[0] 2023 [1] 01
                var end_YM = YearMonthEnd?.Split("/"); //[0] 2023 [1] 01

                int end_YM_Year = Int16.Parse(end_YM[0]);
                int first_YM_Year = Int16.Parse(first_YM[0]);

                var need_year = end_YM_Year - first_YM_Year;

                var first_month = Int16.Parse(first_YM[1]);
                var end_month = Int16.Parse(end_YM[1]);
                var onput_month = string.Empty;
                var onput_year = string.Empty;

                if (YearMonthStart == null && YearMonthEnd == null || need_year < 0) { result.Add(null); return result; } //防呆

                for (var i = 0; i <= need_year; i++)
                {
                    year.Add(end_YM_Year - i);
                    for (var j = 12; j > 0; j--)
                    {
                        if (year[i] == first_YM_Year && first_month > j) continue;  //判斷初始年月
                        if (year[i] == end_YM_Year && j - 1 >= end_month) continue; //如果在這個月份以外的，全部都要剔除
                        onput_month = j < 10 ? "0" + j.ToString() : j.ToString();
                        onput_year = year[i].ToString();
                        result.Add((onput_year + onput_month).ToSlashTaiwanDateFromYYYYMMTW());
                    }
                }
            }
            else if (YearMonthStart != null)
            {
                result.Add(YearMonthStart.ToYYYYMMFromTaiwan().ToSlashTaiwanDateFromYYYYMMTW());
            }
            else
            {
                result.Add(YearMonthEnd.ToYYYYMMFromTaiwan().ToSlashTaiwanDateFromYYYYMMTW());
            }

            return result;
        }

    }
}
