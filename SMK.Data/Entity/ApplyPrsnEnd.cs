using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMK.Data.Entity
{
    public partial class ApplyPrsnEnd
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

        [Display(Name = "申請類型")]
        [StringLength(10)]
        [Required]
        public string Application_Type { get; set; }

        [Display(Name = "院所代碼")]
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

        [Display(Name = "醫事人員ID")]
        [StringLength(10)]
        public string MedicalID { get; set; }

        [Display(Name = "解除醫事人員姓名")]
        [StringLength(80)]
        public string MedicalName { get; set; }

        [Display(Name = "職稱")]
        [StringLength(10)]
        public string Professional_Title { get; set; }

        [Display(Name = "單機版修改日期")]
        public DateTime? SingleChangeDate { get; set; }

        [Display(Name = "單機版修改備註")]
        [StringLength(80)]
        public string SingleNote { get; set; }

        [MaxLength(127)]
        [Display(Name = "修改者")]
        [Column("UpdatedBy")]
        public string UpdatedBy { get; set; }

    }
}