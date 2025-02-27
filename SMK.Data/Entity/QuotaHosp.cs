using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMK.Data.Entity
{
    public partial class QuotaHosp
    {
        [Display(Name = "配額年度")]
        [StringLength(4)]
        [Required]
        public string QuotaYear { get; set; }    
        
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

        [Display(Name = "院所代碼")]
        [StringLength(10)]
        [Required]        
        public string HospID { get; set; }

        [Display(Name = "院區別")]
        [Required]
        [StringLength(2)]
        public string HospSeqNo { get; set; }

        [Display(Name = "院所名稱")]
        [StringLength(80)]
        public string HospName { get; set; }

        [Display(Name = "醫院層級")]
        [StringLength(10)]
        public string LastContType { get; set; }
        
        [Display(Name = "申請戒菸(治療)")]
        public bool ApplyTreat { get; set; }        
        [Display(Name = "申請戒菸(治療人數)")]
        public int? ApplyTreatPeople { get; set; }
        
        [Display(Name = "申請戒菸(衛教)")]
        public bool ApplyHealthEdu { get; set; }        
        [Display(Name = "申請戒菸(衛教人數)")]
        public int? ApplyHealthEduPeople { get; set; }
        
        [Display(Name = "專設戒菸(治療)")]
        [StringLength(100)]
        public string DesignedTreat { get; set; }        
        [Display(Name = "專設戒菸(衛教)")]
        [StringLength(100)]
        public string DesignedTreatHealthEdu { get; set; }
        
        [Display(Name = "署審查結果(治療)")]
        [StringLength(100)]
        public string ResultTreat { get; set; }        
        [Display(Name = "署審查結果(衛教)")]
        [StringLength(100)]
        public string ResultHealthEdu { get; set; }    

        [Display(Name = "VPN異動說明")]
        [StringLength(500)]
        public string VPNChangeNote { get; set; }

        [Display(Name = "文號")]
        [StringLength(100)]
        public string DocumentNumber { get; set; }   
        
        [Display(Name = "發文日")]
        [StringLength(100)]
        public string IssueDate { get; set; }

        [Display(Name = "申請項目(治療或衛教)")]
        [StringLength(20)]
        public string Treatment { get; set; }

        [Display(Name = "掃描檔名")]
        [StringLength(100)]
        public string ScanFileName { get; set; }

        [MaxLength(127)]
        [Display(Name = "修改者")]
        [Column("UpdatedBy")]
        public string UpdatedBy { get; set; }
    }
}