using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace SMK.Web.Extensions
{
    public static class StringExtensions
    {
        public static string TruncateString(this string str, int maxLength)
        {
            if (str == null) return default;

            if (str.Length > maxLength)
            {
                return str.Substring(0, maxLength);
            }

            return str;
        }

        /// <summary>
        /// 例如 107/12 或 10712 轉成 201812
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToYYYYMMFromTaiwan(this string value)
        {
            if (value == null) return default;

            var digitText = ToDigitText(value).TruncateString(5);

            var date = ToDateFromTaiwan(digitText);

            if (!date.HasValue) return default;

            return date.Value.ToString("yyyyMM");
        }      
        
        /// <summary>
        /// 例如 107/12 或 10712 轉成 2018/12
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToYYYY_MMFromTaiwan(this string value)
        {
            if (value == null) return default;

            var digitText = ToDigitText(value).TruncateString(5);

            var date = ToDateFromTaiwan(digitText);

            if (!date.HasValue) return default;

            return date.Value.ToString("yyyy/MM");
        }

        /// <summary>
        /// 例如 107/12/20 或 1071220 轉成 20181220
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToYYYYMMDDFromTaiwan(this string value)
        {
            if (value == null) return default;

            var digitText = ToDigitText(value).TruncateString(7);

            var date = ToDateFromTaiwan(digitText);

            if (!date.HasValue) return default;

            if (date.Value.Year < 1911) return default;

            return date.Value.ToString("yyyyMMdd");
        }


        private static string ToDigitText(string value)
        {
            if (value.All(char.IsDigit))
            {
                return value.Trim();
            }

            var result = string.Join(null, Regex.Matches(value, @"\d+").Cast<Match>().Select(m => m.Value));
            return result;
        }

        /// <summary>
        /// 例如 10712 或 107/12 轉 2018/12/01 DateTime 格式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? ToDateFromTaiwan(this string value)
        {
            if (value == null)
            {
                return default;
            }

            value = ToDigitText(value);

            var culture = GetTaiwanCultureInfo();

            var format = (value.Length < 6) ? "yyyyMM" : "yyyyMMdd";
            
            DateTime.TryParseExact(value.PadLeft(format.Length, '0'), format, culture, DateTimeStyles.None, out var result);
            return result;
        }

        public static DateTime? ToDateTime(this string value)
        {
            if (value == null)
            {
                return default;
            }
            value = ToDigitText(value);
            if (!DateTime.TryParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var result))
                return null;
            return result;
        }

        /// <summary>
        /// 例如 107/01 轉成 107/01
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSlashTaiwanDateFromTaiwanYYYMM(this string value)
        {
            if (value == null) return default;

            var digitText = ToDigitText(value).TruncateString(5);

            var date = ToDateFromTaiwan(digitText);

            if (!date.HasValue) return default;

            var taiwanCalendar = GetTaiwanCalendar();

            return string.Format("{0:000}/{1:00}",
                taiwanCalendar.GetYear(date.Value),
                date.Value.Month);
        }

        /// <summary>
        /// 例如 201801 轉成 107/01
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSlashTaiwanDateFromYYYYMM(this string value)
        {
            if (value == null)
                return null;

            if (!DateTime.TryParseExact(value.PadLeft(6, '0'), "yyyyMM", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                return value;

            var taiwanCalendar = GetTaiwanCalendar();

            return string.Format("{0:000}/{1:00}",
                taiwanCalendar.GetYear(result),
                result.Month);
        }    

        /// <summary>
        /// 例如 201801 轉成 107年01月
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSlashTaiwanDateFromYYYYMMTW(this string value)
        {
            if (value == null)
                return null;

            if (!DateTime.TryParseExact(value.PadLeft(6, '0'), "yyyyMM", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                return value;

            var taiwanCalendar = GetTaiwanCalendar();

            return string.Format("{0:000}年{1:00}月",
                taiwanCalendar.GetYear(result),
                result.Month);
        }     
        

        /// <summary>
        /// 例如 20180103 轉成 107/01/03
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSlashTaiwanDateFromYYYYMMDD(this string value)
        {
            if (value == null)
                return null;

            if (!DateTime.TryParseExact(value.PadLeft(8, '0'), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                return value;

            var taiwanCalendar = GetTaiwanCalendar();

            if (result.Year < 1911)
            {
                return null;
            }

            return string.Format("{0:000}/{1:00}/{2:00}",
                taiwanCalendar.GetYear(result),
                result.Month,
                result.Day);
        }

        public static string ToTaiwanDateFromYYYYMMDD(this string value)
        {
            if (value == null)
                return null;

            if (!DateTime.TryParseExact(value.PadLeft(8, '0'), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                return value;

            var taiwanCalendar = GetTaiwanCalendar();

            if (result.Year < 1911)
            {
                return null;
            }

            return string.Format("{0:000}{1:00}{2:00}",
                taiwanCalendar.GetYear(result),
                result.Month,
                result.Day);
        }

        /// <summary>
        /// 例如 2020/10/25 轉成民國 109/10/25
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns></returns>
        public static string ToSimpleTaiwanDate(this DateTime datetime)
        {
            var taiwanCalendar = GetTaiwanCalendar();

            return string.Format("{0:000}/{1:00}/{2:00}",
                taiwanCalendar.GetYear(datetime),
                datetime.Month,
                datetime.Day);
        }

        /// <summary>
        /// 例如從 西元2020/10/25 轉成民國 109/10/25
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns></returns>
        public static string ToSimpleTaiwanDateFromAD(this string value)
        {
            if (!DateTime.TryParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                return null;

            return ToSimpleTaiwanDate(result);
        }

        private static CultureInfo GetTaiwanCultureInfo()
        {
            CultureInfo culture = new CultureInfo("zh-TW");
            culture.DateTimeFormat.Calendar = GetTaiwanCalendar();
            return culture;
        }

        private static Calendar GetTaiwanCalendar()
        {
            return new TaiwanCalendar();
        }

        /// <summary>
        /// 遮罩文字
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maskChar"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string MaskText(this string text, char maskChar, int startIndex, int count)
        {
            if (text == null)
                return null;

            var start = new string(text.Take(startIndex).ToArray());
            var middle = string.Empty.PadLeft(count, maskChar);
            var end = new string(text.Skip(startIndex + count).Take(startIndex).ToArray());

            return start + middle + end;
        }

        public static string VbStrConvNarrow(this string text)
        {
            if (text == null) return "";
            return Strings.StrConv(text.Trim(), VbStrConv.Narrow);
        }

        public static decimal? CheckZero(this string wkString)
        {
            if (string.IsNullOrEmpty(wkString) || Strings.Trim(wkString) == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(Strings.Trim(wkString));
            }
        }
        
        public static string ToGender(this string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                var id = str.Substring(1, 1).ToUpper();
                switch (id)
                {
                    case "1":
                        return "M";
                    case "A":
                        return "M";
                    case "C":
                        return "M";
                    case "8":
                        return "M";
                    case "2":
                        return "F";
                    case "B":
                        return "F";
                    case "D":
                        return "F";
                    case "9":
                        return "F";
                }
            }
            return "U";
        }

        public static decimal ToDecimal(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            return Convert.ToDecimal(text);
        } 
        public static int ToInt32(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            return Convert.ToInt32(text);
        }

        public static string ToTelPhone(string a, string b, string c)
        {
            if (string.IsNullOrWhiteSpace(a) &&
                string.IsNullOrWhiteSpace(b) &&
                string.IsNullOrWhiteSpace(c))
                return "";
            var phone = a;
            if (!string.IsNullOrWhiteSpace(b))
            {
                phone += "-" + b;
            }

            if (!string.IsNullOrWhiteSpace(c))
            {
                phone += "#" + c;
            }

            return phone;
        }

        public static string TelFormat(this string wkString)
        {
            string[] wkStrArray, wkStrArray1;
            string wkTel_d_ac, wkTel_d, wkTel_d_extn;

            wkStrArray = Strings.Split(wkString, "-");
            wkTel_d_ac = wkStrArray[0].ToString().Trim();
            wkStrArray1 = Strings.Split(wkStrArray[1].ToString().Trim(), "#");
            wkTel_d = wkStrArray1[0].ToString().Trim();
            wkTel_d_extn = wkStrArray1[1].ToString().Trim();

            if (wkTel_d_ac != "" & wkTel_d != "" & wkTel_d_extn != "")
                return wkTel_d_ac + "-" + wkTel_d + "#" + wkTel_d_extn;        
            else if (wkTel_d_ac == "" & wkTel_d != "" & wkTel_d_extn != "")
                return wkTel_d + "#" + wkTel_d_extn;
            else if (wkTel_d_ac != "" & wkTel_d != "" & wkTel_d_extn == "")
                return wkTel_d_ac + "-" + wkTel_d;
            else if (wkTel_d_ac == "" & wkTel_d != "" & wkTel_d_extn == "")
                return wkTel_d;
            else if (wkTel_d_ac != "" & wkTel_d == "" & wkTel_d_extn != "")
                return wkTel_d_ac + "-#" + wkTel_d_extn;
            else if (wkTel_d_ac != "" & wkTel_d == "" & wkTel_d_extn == "")
                return wkTel_d_ac + "-";
            else if (wkTel_d_ac == "" & wkTel_d == "" & wkTel_d_extn != "")
                return "#" + wkTel_d_extn;
            else
                return "";
        }

        public static string SafeSubstring(this string text, int start, int length)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";
            return text.Trim().Length >= length ? text.Trim().Substring(start, length) : text;
        }

        public static DateTime? ApplyStartDate(this string text)
        {
            var applyDate = (text + "19").ToDateTime();
            return applyDate?.AddDays(1).AddMonths(1);
        }
        public static DateTime? ApplyEndDate(this string text)
        {
            var applyDate = (text + "19").ToDateTime();
            return applyDate?.AddMonths(2);
        }
    }
}
