using SMK.Data.Dto;
using SMK.Data.Entity;
using System.ComponentModel;

namespace SMK.Web.Models
{
    public class ICRateByMonthQueryModel : PagedRequest
    {
        [DisplayName("費用年月(起/迄)")]
        public string YearMonthStart { get; set; }

        [DisplayName("費用年月(起/迄)")]
        public string YearMonthEnd { get; set; }

        [DisplayName("醫事機構代碼")]
        public string HospID { get; set; }
        [DisplayName("醫療機構名稱")]
        public string HospName { get; set; }
        public string HospSeqNo { get; set; }
        [DisplayName("過卡率未達90%")]
        public bool RateFor90Percent { get; set; }
        [DisplayName("醫院層級")]
        public string HospContName { get; set; }
    }

    /// <summary>
    /// 層級別醫院別單月過卡率
    /// </summary>
    public class ICRateByMonthViewModel : ICRateByMonth
    {

    }
}
