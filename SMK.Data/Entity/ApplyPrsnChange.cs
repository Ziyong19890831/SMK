using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMK.Data.Entity
{
    public partial class ApplyPrsnChange
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

        [Display(Name = "更正ID")]
        public bool ChangeID { get; set; }      
        
        [Display(Name = "更正姓名")]
        public bool ChangeName { get; set; }       
        
        [Display(Name = "生效日")]
        [StringLength(8)]
        public string StartYMD { get; set; }

        [Display(Name = "申請類型")]
        [StringLength(10)]
        [Required]
        public string Application_Type { get; set; }


        [Display(Name = "ID")]
        [StringLength(10)]
        public string ID { get; set; }

        [Display(Name = "姓名")]
        [StringLength(80)]
        public string Name { get; set; }

        [Display(Name = "ID")]
        [StringLength(10)]
        public string NewID { get; set; }

        [Display(Name = "姓名")]
        [StringLength(80)]
        public string NewName { get; set; }

        [Display(Name = "單機版修改日期")]
        [StringLength(8)]
        public string SingleChangeDate { get; set; }

        [Display(Name = "單機版修改備註")]
        [StringLength(80)]
        public string SingleNote { get; set; }

        [MaxLength(127)]
        [Display(Name = "修改者")]
        [Column("UpdatedBy")]
        public string UpdatedBy { get; set; }

    }
}