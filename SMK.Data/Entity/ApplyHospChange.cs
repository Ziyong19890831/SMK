using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMK.Data.Entity
{
    public partial class ApplyHospChange
    {
        [Display(Name = "更正院所代碼")]
        public bool ChangeHospID { get; set; }      
        
        [Display(Name = "更正院所名稱")]
        public bool ChangeHospName { get; set; }      
        
        [Display(Name = "更正負責人")]
        public bool ChangeHospUserName { get; set; }  
        
        [Display(Name = "更正機構地址")]
        public bool ChangeHospAddress { get; set; }

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
        
        [Display(Name = "生效日")]
        [StringLength(8)]
        public string StartYMD { get; set; }

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

        [Display(Name = "負責人姓名")]
        [StringLength(50)]
        public string HospUserName { get; set; }   
        
        [Display(Name = "院區地址")]
        [StringLength(80)]
        public string HospAddress { get; set; }



        [Display(Name = "醫事機構代碼")]
        [StringLength(10)]
        public string NewHospID { get; set; }

        [Display(Name = "院區別")]
        [StringLength(2)]
        public string NewHospSeqNo { get; set; }

        [Display(Name = "醫事機構名稱")]
        [StringLength(50)]
        public string NewHospName { get; set; }

        [Display(Name = "負責人姓名")]
        [StringLength(50)]
        public string NewHospUserName { get; set; }

        [Display(Name = "院區地址")]
        [StringLength(80)]
        public string NewHospAddress { get; set; }

        [MaxLength(127)]
        [Display(Name = "修改者")]
        [Column("UpdatedBy")]
        public string UpdatedBy { get; set; }

    }
}