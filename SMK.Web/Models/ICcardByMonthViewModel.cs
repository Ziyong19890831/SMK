using SMK.Data.Dto;
using SMK.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel;

namespace SMK.Web.Models
{
    public class ICcardByMonthQueryModel : PagedRequest
    {
        [DisplayName("費用年月(民國年)")]
        public string YearMonthStart { get; set; }

        [DisplayName("費用年月(民國年)")]
        public string YearMonthEnd { get; set; }

        [DisplayName("醫事機構代碼")]
        public string HospID { get; set; }
        [DisplayName("醫療機構名稱")]
        public string HospName { get; set; }
        [DisplayName("醫院層級")]
        public string HospContName { get; set; }
    }

    /// <summary>
    /// 醫院別刷卡次數
    /// </summary>
    public class ICcardByMonthViewModel : ICcardByMonth
    {
       public Dictionary<string, int> MonthlySums = new Dictionary<string, int>();
    }
}
