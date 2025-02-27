using SMK.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMK.Data.Entity
{
    public partial class ICNotFound
    {
        [Display(Name = "健保業務組")]
        [StringLength(10)]
        public string BranchName { get; set; }

        [Display(Name = "醫院層級")]
        [StringLength(10)]
        public string LastContType { get; set; }

        [Display(Name = "醫事機構代碼")]
        [Required]
        [StringLength(10)]
        public string HospID { get; set; }
        
        [Display(Name = "院區別")]
        [Required]
        [StringLength(2)]
        public string HospSeqNo { get; set; }

        [Display(Name = "機構名稱")]
        [StringLength(50)]
        public string HospName { get; set; }   
        
        [Display(Name = "機構類別")]
        [StringLength(50)]
        public string HospDataType { get; set; }      
        
        [Display(Name = "身份證字號")]
        [StringLength(10)]
        public string ID { get; set; }        

        /// <summary>
        /// 19880101
        /// </summary>
        [Display(Name = "生日")]
        [StringLength(8)]
        public string Birthday { get; set; }    
        
        /// <summary>
        /// 19880101
        /// </summary>
        [Display(Name = "就醫日期")]
        [StringLength(8)]
        public string FuncDate { get; set; }    
        
        /// <summary>
        /// 198801
        /// </summary>
        [Display(Name = "療程年月")]
        [StringLength(6)]
        [Required]
        public string FeeYM { get; set; }

        /// <summary>
        /// 104011800141100210B700022812
        /// </summary>
        [Display(Name = "電腦序號")]
        [StringLength(28)]
        [Required]
        public string Data_id { get; set; }

        /// <summary>
        /// B7
        /// </summary>
        [Display(Name = "案件分類")]
        [StringLength(5)]
        public string CaseType { get; set; }

        /// <summary>
        /// 228
        /// </summary>
        [Display(Name = "流水號")]
        [StringLength(10)]
        public string SeqNo { get; set; }

        /// <summary>
        /// 0401180014
        /// </summary>
        [Display(Name = "真實機構代碼")]
        [StringLength(10)]
        public string Real_HospID { get; set; }

        /// <summary>
        /// 用藥
        /// </summary>
        [Display(Name = "醫治類別")]
        [StringLength(5)]
        [Required]
        public string CureType { get; set; }

        /// <summary>
        /// 3663
        /// </summary>
        [Display(Name = "醫療費用點數")]
        [StringLength(10)]
        public string ExpDot { get; set; }

        /// <summary>
        /// Xxxxxx
        /// </summary>
        [Display(Name = "備註")]
        [StringLength(50)]
        public string Note { get; set; }
    }
}
