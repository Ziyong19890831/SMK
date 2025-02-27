using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMK.Data.Entity
{
    public partial class ApplyPrsnNew
    {
        [Display(Name = "收件年月")]
        [StringLength(6)]
        [Required]
        public string FeeYM { get; set; }       
        
        [Display(Name = "編號")]
        [StringLength(11)]
        [Required]
        [Key]
        public string Serial_Number { get; set; }

        [Display(Name = "收件年月日")]
        [StringLength(8)]
        [Required]
        public string FeeYMD { get; set; }

        [Display(Name = "合約生效日期")]
        [StringLength(8)]
        public string Contract_Effective_Date { get; set; }

        [Display(Name = "申請類型")]
        [StringLength(10)]
        [Required]
        public string Application_Type { get; set; }

        [Display(Name = "醫事機構代碼")]
        [StringLength(10)]
        [Required]
        public string HospID { get; set; }

        [Display(Name = "院區別")]
        [Required]
        [StringLength(2)]
        public string HospSeqNo { get; set; }

        [Display(Name = "醫事機構名稱")]
        [StringLength(50)]
        public string HospName { get; set; }

        [Display(Name = "醫院層級")]
        [StringLength(10)]
        public string LastContType { get; set; }

        [Display(Name = "健保業務組")]
        [StringLength(10)]
        public string BranchName { get; set; }

        [Display(Name = "申請醫事人員姓名")]
        [StringLength(80)]
        public string MedicalName { get; set; }

        [Display(Name = "醫事人員ID")]
        [StringLength(10)]
        public string MedicalID { get; set; }

        [Display(Name = "出生年月日")]
        [StringLength(8)]
        public string Birthday { get; set; }

        [Display(Name = "戒菸證書有效日期")]
        [StringLength(8)]
        public string Smoking_Expiration_Date { get; set; }

        [Display(Name = "職稱")]
        [StringLength(10)]
        [Required]
        public string Professional_Title { get; set; }

        [Display(Name = "負責人姓名")]
        [StringLength(50)]
        public string HospUserName { get; set; }

        [Display(Name = "負責人ID")]
        [StringLength(10)]
        public string HospUserID { get; set; }

        [Display(Name = "負責人Email")]
        [StringLength(80)]
        public string HospUserMail { get; set; }

        [Display(Name = "醫事人員證書")]
        [StringLength(80)]
        public string Medical_Certificate { get; set; }

        [Display(Name = "戒菸證書字號")]
        [StringLength(80)]
        public string Smoking_Cessation_Certificate { get; set; }

        [Display(Name = "治療或衛教")]
        [StringLength(20)]
        public string Treatment { get; set; }

        [Display(Name = "SMK系統登錄日期")]
        public DateTime? SMKLogDate { get; set; }

        [Display(Name = "SMK登錄註記")]
        [StringLength(80)]
        public string SMKLogNote { get; set; }

        [MaxLength(127)]
        [Display(Name = "修改者")]
        [Column("UpdatedBy")]
        public string UpdatedBy { get; set; }
    }
}