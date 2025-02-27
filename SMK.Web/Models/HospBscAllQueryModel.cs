using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using SMK.Data.Dto;

namespace SMK.Web.Models
{
    public class HospBscAllQueryModel : PagedRequest
    {
        [DisplayName("醫事機構代碼")]
        [Required(ErrorMessage ="請填寫 {0}")]
        public string HospID { get; set; }
        
        [DisplayName("醫事機構名稱")]
        [Required(ErrorMessage ="請填寫 {0}")]
        public string HospName { get; set; }
        public IFormFile file { get; set; }
        public List<string> err { get; set; }
        public HospBscAllQueryModel()
        {
            
        }

    }
}