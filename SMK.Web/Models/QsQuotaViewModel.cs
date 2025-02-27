using SMK.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMK.Web.Models
{
    public class QsQuotaViewModel
    {
           

        [Display(Name = "療程年度別")]
        public string YEARS { get; set; }
        [Display(Name = "醫事機構代碼")]
        public string HOSP_ID { get; set; }
        [Display(Name = "分院識別號")]
        public string HOSP_SEQ_NO { get; set; }
        [Display(Name = "當年度用藥/衛教配額")]   
        public int? QUOTA { get; set; }
        [Display(Name = "輸入日期")]
        public DateTime? TXT_DATE { get; set; }
        [Display(Name = "異動人員")]
        public string ADJUST_USER_ID { get; set; }
        [Display(Name = "生效起日")]
        public DateTime VALID_S_DATE { get; set; }
        [Display(Name = "生效迄日")]
        public DateTime VALID_E_DATE { get; set; }
        [Display(Name = "異動說明")]
        public string REMARK { get; set; }
        [Display(Name = "療程類別")]
        public int CURE_TYPE { get; set; }
        [Display(Name = "異動人員")]
        public string CREAT_USER_ID { get; set; }
        [Display(Name = "異動日期")]
        public DateTime CREAT_DATE { get; set; }
    }
}