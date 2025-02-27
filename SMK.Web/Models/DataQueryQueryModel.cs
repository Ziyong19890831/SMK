using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SMK.Data.Dto;

namespace SMK.Web.Models
{
    public class DataQueryQueryModel : PagedRequest
    {
        [DisplayName("醫事機構代碼")]
        [Required(ErrorMessage ="請填寫 {0}")]
        public string HospID { get; set; }
        [DisplayName("醫事機構代碼")]
        [Required(ErrorMessage ="請填寫 {0}")]
        [MaxLength(2)]
        public string HospSeqNo { get; set; }
        [DisplayName("醫事機構名稱")]
        [Required(ErrorMessage ="請填寫 {0}")]
        public string HospName { get; set; }
        [DisplayName("起訖年月日")]
        [Required(ErrorMessage ="請填寫 {0}")]
        public string FuncStartDate { get; set; }
        [DisplayName("起訖年月")]
        [Required(ErrorMessage ="請填寫 {0}")]
        public string FuncEndDate { get; set; }
        [DisplayName("出生日期")]
        [Required(ErrorMessage ="請填寫 {0}")]
        [MaxLength(10)]
        public string Birthday { get; set; }
        [DisplayName("身份證號")]
        [Required(ErrorMessage ="請填寫 {0}")]
        [MaxLength(10)]
        public string PrsnID { get; set; }
        [DisplayName("個案編號")]
        [Required(ErrorMessage ="請填寫 {0}")]
        [MaxLength(10)]
        public string CaseNo { get; set; }
        [DisplayName("開藥")]
        public bool Type1 { get; set; }
        [DisplayName("衛教")]
        public bool Type2 { get; set; }
        [DisplayName("追蹤")]
        public bool Type3 { get; set; }
        [DisplayName("釋出")]
        public bool Type4 { get; set; }
        [DisplayName("下載明細檔")]
        public bool customSwitch { get; set; } = false;
        [DisplayName("自訂義下載")]
        public List<string> SelectCheckBox { get; set; }
        [DisplayName("自訂義下載Ord")]
        public List<string> OrdCheckbox { get; set; }
    }

    public class DataQueryExcelViewModel: CaseQueryViewModel
    {
        #region 從這邊過來的SMK.Data\Entity\IniOpOrd.cs

        /// <summary>
        /// 流水號
        /// </summary>
        public int OrderSeqNo { get; set; }

        /// <summary>
        /// 醫令類別
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        /// 醫令代碼
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 調劑方式
        /// </summary>
        public string Ord_RelMode { get; set; }

        /// <summary>
        /// 連續處方註記
        /// </summary>
        public string ChrMark { get; set; }

        /// <summary>
        /// 藥品用量
        /// </summary>
        public string DrugNum { get; set; }

        /// <summary>
        /// 藥品使用頻率
        /// </summary>
        public string DrugFre { get; set; }

        /// <summary>
        /// 用藥途徑/作用部位
        /// </summary>
        public string DrugPath { get; set; }

        /// <summary>
        /// 醫令單價
        /// </summary>
        public decimal? OrderUprice { get; set; }

        /// <summary>
        /// 醫令數量
        /// </summary>
        public decimal? OrderQty { get; set; }

        /// <summary>
        /// 醫令點數
        /// </summary>
        public int? OrderDot { get; set; }

        /// <summary>
        /// 執行醫事人員代號
        /// </summary>
        public string ExePrsnId { get; set; }

        /// <summary>
        /// 診療部位
        /// </summary>
        public string CurePath { get; set; }

        /// <summary>
        /// 醫令給藥日份
        /// </summary>
        public int? OrderDrugDay { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Note { get; set; }

        #endregion
    }
}