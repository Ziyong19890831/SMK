using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.Models
{
    public class ICNotFoundQueryModel : PagedRequest
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
        [DisplayName("身份證號")]
        public string ID { get; set; }
        [DisplayName("醫院層級")]
        public string HospContName { get; set; }
    }

    /// <summary>
    /// 未過卡詳細資料
    /// </summary>
    public class ICNotFoundViewModel : ICNotFound
    {

    }
}
