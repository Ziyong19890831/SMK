using SMK.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMK.Data.Entity
{
    public partial class ICRateByMonth
    {
        [Display(Name = "醫院層級")]
        [StringLength(10)]
        public string LastContType { get; set; }

        [Display(Name = "機構類別")]
        [StringLength(50)]
        public string HospDataType { get; set; }

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

        /// <summary>
        /// 198801
        /// </summary>
        [Display(Name = "療程年月")]
        [StringLength(6)]
        [Required]
        public string FeeYM { get; set; }

        /// <summary>
        /// 用藥
        /// </summary>
        [Display(Name = "治療型態")]
        [StringLength(5)]
        public string CureType { get; set; }

        /// <summary>
        /// 68/19
        /// </summary>
        [Display(Name = "樣本")]
        [StringLength(10)]
        public string samples { get; set; }

        /// <summary>
        /// 98.55
        /// </summary>
        [Display(Name = "過卡率%")]
        [StringLength(10)]
        public string Rate { get; set; }

        /// <summary>
        /// 1.45
        /// </summary>
        [Display(Name = "未過卡率%")]
        [StringLength(10)]
        public string NoRate { get; set; }
    }
}
