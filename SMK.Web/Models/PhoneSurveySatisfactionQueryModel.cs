using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMK.Web.Models
{
    public class PhoneSurveySatisfactionQueryModel
    {
        [DisplayName("合約年月")]
        [Required(ErrorMessage = "請填寫 {0}")]
        public string ym { get; set; }
        
    }


}
