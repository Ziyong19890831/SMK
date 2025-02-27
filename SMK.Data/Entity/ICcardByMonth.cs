using SMK.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMK.Data.Entity
{
    public partial class ICcardByMonth
    {
        [Display(Name = "醫院層級")]
        [StringLength(10)]        
        public string LastContType { get; set; }

        [Display(Name = "醫事機構代碼")]
        [StringLength(10)]
        [Required]        
        public string HospID { get; set; }

        [Display(Name = "醫事機構名稱")]
        [StringLength(50)]
        public string HospName { get; set; }
        
        [Display(Name = "刷卡年月")]
        [StringLength(7)]
        [Required]
        public string ICCard_YM { get; set; }

        [Display(Name = "刷卡筆數")]
        [StringLength(8)]
        public string ICCard_Times { get; set; }
    }
}
