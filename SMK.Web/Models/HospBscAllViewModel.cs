using SMK.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMK.Web.Models
{
    public class HospBscAllViewModel
    {
        [Display(Name = "醫事機構代碼")]
        public string HospId { get; set; }
        [Display(Name = "醫事機構名稱")]
        public string HospName { get; set; }   
        [Display(Name = "電話區域號碼")]
        public string HospTelArea { get; set; }
        [Display(Name = "電話號碼")]
        public string HospTel { get; set; }
        [Display(Name = "分區別")]
        public string BranchNo { get; set; }
        [Display(Name = "機構地址")]
        public string HospAddress { get; set; }
        [Display(Name = "特約類別")]
        public string ContType { get; set; }
        [Display(Name = "型態別")]
        public string HospType { get; set; }
        [Display(Name = "醫事機構種類")]
        public string HospKind { get; set; }
        [Display(Name = "終止合約或歇業日期")]
        public string HospEndDate { get; set; }        
        [Display(Name = "開業狀況")]
        public string OpenState { get; set; }
        [Display(Name = "開業狀況")]
        public string HospStartDate { get; set; }
    }

}