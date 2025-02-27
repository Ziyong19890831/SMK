using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMK.Web.Models
{
    public class HighReportingAgencyReportQueryModel
    {
        [DisplayName("查詢起日")]
        [Required(ErrorMessage = "請填寫 {0}")]
        [MaxLength(6, ErrorMessage = "只能填寫yyyyMM")]
        public string STARTDATE { get; set; }
        [DisplayName("查詢迄日")]
        [Required(ErrorMessage = "請填寫 {0}")]
        [MaxLength(6, ErrorMessage = "只能填寫yyyyMM")]
        public string ENDDATE { get; set; }
        [DisplayName("查詢類別")]
        [Required(ErrorMessage = "請選擇 {0}")]
        public string Type { get; set; }
    }
}
