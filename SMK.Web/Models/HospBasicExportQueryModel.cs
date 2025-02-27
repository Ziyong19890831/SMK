using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMK.Web.Models
{
    public class HospBasicExportQueryModel
    {
        [DisplayName("院所層級")]
        [Required(ErrorMessage ="請填寫 {0}")]
        public string HospCont { get; set; } 
        [DisplayName("合約狀況")]
        public string HospStatus { get; set; }
        [DisplayName("可給藥")]
        public bool CouldTreat { get; set; }
        [DisplayName("可衛教")]
        public bool CouldInstruct { get; set; }
        [DisplayName("治療品質改善")]
        public bool ContractType2 { get; set; }
        [DisplayName("衛教品質改善")]
        public bool ContractType3 { get; set; }
    }
}