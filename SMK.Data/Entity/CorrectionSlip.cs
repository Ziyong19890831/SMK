using SMK.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMK.Data.Entity
{
    public partial class  CorrectionSlip
    {
        [Display(Name = "案件編號")]
        [StringLength(9)]
        [Key]
        public string CaseNo { get; set; }

        [Display(Name = "收件日期")]
        public DateTime ReceiveDate { get; set; }
        [Display(Name = "機構代碼")]
        public string HospId { get; set; }
        [DisplayName("醫事機構代碼(院區別)")]
        public string HospSeqNo { get; set; }
        [Display(Name = "機構名稱")]
        public string HospName { get; set; }
        [Display(Name = "個案姓名")]
        public string Name { get; set; }
        [Display(Name = "身分證號")]
        [StringLength(10)]
        public string ID { get; set; }
        [Display(Name = "出生日期")]
        public DateTime? Birthday { get; set; }
        [Display(Name = "更-基本")]
        public string CorrectBasic { get; set; }
        [Display(Name = "更-就醫")]
        public string CorrectHosp { get; set; }
        [Display(Name = "更-衛教")]
        public string CorrectHealth { get; set; }
        [Display(Name = "更-其他")]
        public string CorrectOther { get; set; }
        [Display(Name = "更正項目")]
        public string CorrectItems { get; set; }
        [Display(Name = "更正項目_2")]
        public string CorrectItems2 { get; set; }
        [Display(Name = "備註(資料來源)")]
        public string source { get; set; }
        [Display(Name = "註記")]
        public string Memo { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("Getdate()")]
        [Display(Name = "建立時間")]
        [Column("UpdateAt")]
        public DateTime UpdateAt { get; set; } = DateTime.Now;

        [MaxLength(127)]
        [Display(Name = "修改者")]
        [Column("UpdatedBy")]
        public string UpdatedBy { get; set; }

        [MaxLength(200)]
        [Display(Name = "申請更正原因")]
        public string CorrectionReason { get; set; }

        [MaxLength(200)]
        [Display(Name = "原因說明")]
        public string ReasonStatement { get; set; }
    }
}
