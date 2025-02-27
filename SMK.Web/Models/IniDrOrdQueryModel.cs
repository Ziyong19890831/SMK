using SMK.Data.Dto;

namespace SMK.Web.Models
{
    public class IniDrOrdQueryModel : PagedRequest
    {
        public string DataId { get; set; }
        public string FeeYm { get; set; }
    }
}