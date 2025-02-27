using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SMK.Data.Entity
{
    public partial class GenDrugBasic
    {
        [Key]
        [Display(Name = "健保署編碼")]
        public string DrugNo {get;set;}
        public string DrugType {get;set;}
        [Display(Name = "中文品名")]
        public string OrderChiName {get;set;}
        [Display(Name = "英文品名")]
        public string OrderEngName {get;set;}
        [Display(Name = "廠商")]
        public string DrugCompany {get;set;}
        public string DrugIngredient {get;set;}
        public decimal DrugContent {get;set;}
        [Display(Name = "單價")]
        public decimal UnitPrice {get;set;}
        [Display(Name = "開始日期")]
        public string OrderStartDate {get;set;}
        [Display(Name = "結束日期")]
        public string OrderEndDate { get; set; }
        [Display(Name = "處方藥或指示用藥")]
        public string? prescription { get; set; }
        [Display(Name = "有無健保價")]
        public bool? HealthCarePrice {get;set;}
    }
}
