using SMK.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMK.Data.Entity
{
    public partial class ICRateLately
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
        /// 68
        /// </summary>
        [Display(Name = "藥物信息")]
        [StringLength(10)]
        public string MedIC { get; set; }

        /// <summary>
        /// 69
        /// </summary>
        [Display(Name = "藥物申報")]
        [StringLength(10)]
        public string MedApply { get; set; }

        /// <summary>
        /// 68.69
        /// </summary>
        [Display(Name = "用藥機率")]
        [StringLength(10)]
        public string MedRate { get; set; }

        /// <summary>
        /// 126
        /// </summary>
        [Display(Name = "健保卡上傳/登入筆數(用藥)")]
        [StringLength(10)]
        public string InsIC { get; set; }

        /// <summary>
        /// 126
        /// </summary>
        [Display(Name = "健保申報筆數(用藥)")]
        [StringLength(10)]
        public string InsApply { get; set; }

        /// <summary>
        /// 126
        /// </summary>
        [Display(Name = "健保卡上傳/登錄率(用藥)")]
        [StringLength(10)]
        public string InsRate { get; set; }
    }
}
