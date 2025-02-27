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
    public class DeclareReportQueryModel : PagedRequest
    {
        [DisplayName("費用年月")]
        public string YearMonthStart { get; set; }

        [DisplayName("費用年月")]
        public string YearMonthEnd { get; set; }

        [DisplayName("醫事機構代碼")]
        public string HospID { get; set; }
        public string HospSeqNo { get; set; }
        [DisplayName("醫療機構名稱")]
        public string HospName { get; set; }
        [DisplayName("身份證字號")]
        public string PrsnID { get; set; }
        [DisplayName("生日")]
        public string Birthday { get; set; }
        [DisplayName("開藥")]
        public bool Type1 { get; set; }
        [DisplayName("衛教")]
        public bool Type2 { get; set; }
        [DisplayName("追蹤")]
        public bool Type3 { get; set; }
        [DisplayName("釋出")]
        public bool Type4 { get; set; }
        public DeclareReportQueryModel()
        {
            Type1 = false;
            Type2 = false;
            Type3 = false;
            Type4 = false;
        }
    }

    /// <summary>
    /// 申報資料
    /// </summary>
    public class DeclareReportViewModel : IniOpDtl
    {

    }
}
