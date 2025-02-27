using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using SMK.Data.Dto;

namespace SMK.Web.Models
{
    public class CorrectionSlipQueryModel : PagedRequest
    {
        [DisplayName("醫事機構代碼")]
        [Required(ErrorMessage ="請填寫 {0}")]
        public string HospID { get; set; }
        [DisplayName("醫事機構代碼")]
        [Required(ErrorMessage ="請填寫 {0}")]
        [MaxLength(2)]
        public string HospSeqNo { get; set; }
        [DisplayName("醫事機構名稱")]
        [Required(ErrorMessage ="請填寫 {0}")]
        public string HospName { get; set; }
        [DisplayName("收件日期(起)")]
        [Required(ErrorMessage = "請填寫 {0}")]
        public string FuncSDate { get; set; }
        [DisplayName("收件日期(訖)")]
        [Required(ErrorMessage = "請填寫 {0}")]
        public string FuncEDate { get; set; }
        [DisplayName("案件編號")]
        [Required(ErrorMessage ="請填寫 {0}")]
        public string CaseNo { get; set; }

        public IFormFile file { get; set; }
        [DisplayName("匯出年份")]
        public int year { get; set; }
        public List<string> err { get; set; }
        public CorrectionSlipQueryModel()
        {
            year = DateTime.Now.Year;
        }

    }
}