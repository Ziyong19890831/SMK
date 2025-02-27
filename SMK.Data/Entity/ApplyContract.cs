using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace SMK.Data.Entity
{
    /// <summary>
    /// 主約+調製藥局
    /// </summary>
    public partial class ApplyContract
    {
        /// <summary>院所代碼</summary>
        [MaxLength(10)]
        [Required(ErrorMessage ="院所代碼不能為空值")]
        [DisplayName("院所代碼")]
        public string HOSP_ID { get; set; }

        /// <summary>分院識別號</summary>
        [MaxLength(2)]
        [Required(ErrorMessage = "分院所代碼不能為空值")]
        [DisplayName("分院識別號")]
        public string HOSP_SEQ_NO { get; set; }

        /// <summary>療程年度</summary>
        [MaxLength(5)]
        [Required(ErrorMessage = "治療類別不能為空值")]
        [DisplayName("療程年度")]
        public string Cure_Type { get; set; }

        /// <summary>日期起始</summary>
        [MaxLength(10)]
        [Required(ErrorMessage = "日期不能為空值")]
        [DisplayName("日期起始")]
        public string CONT_S_DATE { get; set; }

        /// <summary>日期迄</summary>
        [MaxLength(10)]
        [DisplayName("日期迄")]
        public string CONT_E_DATE { get; set; }

        /// <summary>建立人ID</summary>
        [MaxLength(50)]
        [DisplayName("建立人ID")]
        [Required(ErrorMessage = "建立人ID不能為空值")]
        public string CREATE_USER_ID { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        public DateTime CREATE_DATE { get; set; }

        /// <summary>修改人ID</summary>
        [MaxLength(50)]
        [DisplayName("修改人ID")]
        [Required(ErrorMessage = "修改人ID不能為空值")]
        public string TXT_USER_ID { get; set; }

        /// <summary>修改時間</summary>
        [DisplayName("建立時間")]
        public DateTime TXT_DATE { get; set; }
    }

}
