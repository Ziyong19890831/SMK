using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMK.Data.Entity
{
    public partial class ApplyHospEnd
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

        [Display(Name = "負責人姓名")]
        [StringLength(50)]
        public string HospUserName { get; set; }   
        
        [Display(Name = "院區地址")]
        [StringLength(80)]
        public string HospAddress { get; set; }

        [Display(Name = "合約生效日")]
        [StringLength(8)]
        public string MinHospStartDate { get; set; }    
        
        [Display(Name = "合約終止日")]
        [StringLength(8)]
        public string MaxHospEndDate { get; set; }

        /// <summary>
        /// Xxxxxx
        /// </summary>
        [Display(Name = "解約原因")]
        [StringLength(50)]
        public string Reason { get; set; }   
        
        /// <summary>
        /// Xxxxxx
        /// </summary>
        [Display(Name = "備註")]
        [StringLength(50)]
        public string Note { get; set; }
        
        /// <summary>
        /// Xxxxxx
        /// </summary>
        [Display(Name = "TTC合約已解約(以0顯示)")]
        [StringLength(50)]
        public string TTCNote { get; set; }

        [Display(Name = "單機版修改日期")]
        [StringLength(8)]
        public string SingleChangeDate { get; set; }

        [Display(Name = "單機版修改備註")]
        [StringLength(50)]
        public string SingleNote { get; set; }

        [MaxLength(127)]
        [Display(Name = "修改者")]
        [Column("UpdatedBy")]
        public string UpdatedBy { get; set; }

    }
}