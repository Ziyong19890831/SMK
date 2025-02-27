using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using SMK.Data.Dto;

namespace SMK.Web.Models
{
    public class QsQuotaQueryModel : PagedRequest
    {
        [DisplayName("醫事機構代碼")]
        [Required(ErrorMessage ="請填寫 {0}")]
        public string HospID { get; set; }
        [DisplayName("分院識別號")]
        [Required(ErrorMessage ="請填寫 {0}")]
        [MaxLength(2)]
        public string HOSP_SEQ_NO { get; set; }
        [DisplayName("療程年度別")]
        [Required(ErrorMessage ="請填寫 {0}")]
        public string YEARS { get; set; }
        
        public IFormFile file { get; set; }
        public List<string> err { get; set; }
        public QsQuotaQueryModel()
        {
        }

    }
}