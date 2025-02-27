using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMK.Data.Entity
{
    public partial class ApplyHospNew
    {
        [Display(Name = "收件年月")]
        [StringLength(6)]
        [Required]
        public string FeeYM { get; set; }       
        
        [Display(Name = "編號")]
        [StringLength(10)]
        [Required]
        [Key]
        public string Serial_Number { get; set; }

        [Display(Name = "收件年月日")]
        [StringLength(8)]
        [Required]
        public string FeeYMD { get; set; }

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

        [Display(Name = "SMK系統登錄日期")]
        [StringLength(8)]
        public string SMKLogDate { get; set; }

        /// <summary>
        /// Xxxxxx
        /// </summary>
        [Display(Name = "備註")]
        [StringLength(50)]
        public string Note { get; set; }

        [MaxLength(127)]
        [Display(Name = "修改者")]
        [Column("UpdatedBy")]
        public string UpdatedBy { get; set; }
    }
}