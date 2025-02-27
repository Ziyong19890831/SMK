using SMK.Data.Dto;
using SMK.Data.Entity;
using System.ComponentModel;

namespace SMK.Web.Models
{
    public class IniReportQueryModel : PagedRequest
    {
        [DisplayName("合約年月")]
        public string ContractYmS { get; set; }
        [DisplayName("合約年月")]
        public string ContractYmE { get; set; }
        [DisplayName("健保年月")]
        public string NhiYmS { get; set; }
        [DisplayName("健保年月")]
        public string NhiYmE { get; set; }

    }
}