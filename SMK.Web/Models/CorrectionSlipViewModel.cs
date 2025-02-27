using SMK.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMK.Web.Models
{
    public class CorrectionSlipExport
    {
        public string HospId { get; set; }
        public string HospName { get; set; }
        public string HospSeqNo { get; set; }
        public int Jan { get; set; }
        public int Feb { get; set; }
        public int Mar { get; set; }
        public int Apr { get; set; }
        public int May { get; set; }
        public int Jun { get; set; }
        public int Jul { get; set; }
        public int Aug { get; set; }
        public int Sep { get; set; }
        public int Oct { get; set; }
        public int Nov { get; set; }
        public int Dec { get; set; }
        public int Jan2 { get; set; }
        public int Feb2 { get; set; }
        public int Mar2 { get; set; }
        public int Apr2 { get; set; }
        public int May2 { get; set; }
        public int Jun2 { get; set; }
        public int Jul2 { get; set; }
        public int Aug2 { get; set; }
        public int Sep2 { get; set; }
        public int Oct2 { get; set; }
        public int Nov2 { get; set; }
        public int Dec2 { get; set; }
    }
    public class CorrectionSlipViewModel
    {
        [Display(Name = "案件編號")]
        public string CaseNo { get; set; }

        [Display(Name = "收件日期")]
        public DateTime ReceiveDate { get; set; }
        [Display(Name = "機構代碼")]
        public string HospId { get; set; }
        [DisplayName("院區別")]
        public string HospSeqNo { get; set; }
        [Display(Name = "機構名稱")]
        public string HospName { get; set; }
        [Display(Name = "個案姓名")]
        public string Name { get; set; }
        [Display(Name = "身分證號")]
        public string ID { get; set; }
        [Display(Name = "出生日期")]
        public DateTime Birthday { get; set; }
        [Display(Name = "更-基本")]
        public string CorrectBasic { get; set; }
        [Display(Name = "更-就醫")]
        public string CorrectHosp { get; set; }
        [Display(Name = "更-衛教")]
        public string CorrectHealth { get; set; }
        [Display(Name = "更-其他")]
        public string CorrectOther { get; set; }
        [Display(Name = "更正項目")]
        public string CorrectItems { get; set; }
        [Display(Name = "更正項目_2")]
        public string CorrectItems2 { get; set; }
        [Display(Name = "備註(資料來源)")]
        public string source { get; set; }
        [Display(Name = "註記")]
        public string Memo { get; set; }

        [Display(Name = "建立時間")]
        public DateTime UpdateAt { get; set; }
        [Display(Name = "修改者")]
        public string UpdatedBy { get; set; }
        [Display(Name = "申請更正原因")]
        public string CorrectionReason { get; set; }
        [Display(Name = "原因說明")]
        public string ReasonStatement { get; set; }
    }
}