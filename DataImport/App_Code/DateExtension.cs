using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SMK.Data.Utility
{
    /// <summary>
    /// 與資料庫日期轉換
    /// </summary>
    public static class DateExtension
    {
        /// <summary>
        /// 設定文化
        /// </summary>
        /// <returns></returns>
        private static CultureInfo getCulture() {
            CultureInfo culture = new CultureInfo("zh-TW");
            culture.DateTimeFormat.Calendar = new TaiwanCalendar();
            return culture;
        }

        /// <summary>
        /// 民國年轉DB西元年不含符號(yyyyMMdd)
        /// </summary>
        /// <param name="twDate"></param>
        /// <returns></returns>
        public static string TwDateToDateTime(this string twDate) {
            if (string.IsNullOrEmpty(twDate) || string.IsNullOrEmpty(twDate.Trim())) return string.Empty;
            return DateTime.Parse(twDate, getCulture()).ToString("yyyyMMdd");
        }

        /// <summary>
        /// 日期轉DB西元年不含符號
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToDate(this DateTime dateTime) { 
            return dateTime.ToString("yyyyMMdd");
        }

        /// <summary>
        /// DB西元年轉民國年顯示
        /// </summary>
        /// <returns></returns>
        public static string WestToTwDate(this string date) {
            if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(date.Trim())) return string.Empty;
            var _date=DateTime.ParseExact(date, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            return ToSimpleTaiwanDate(_date);
        }


        /// <summary>
        /// To the simple taiwan date.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns></returns>
        public static string ToSimpleTaiwanDate(this DateTime datetime)
        {
            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            return string.Format("{0:000}/{1:00}/{2:00}",
                taiwanCalendar.GetYear(datetime),
                datetime.Month,
                datetime.Day);
        }
    }
}
