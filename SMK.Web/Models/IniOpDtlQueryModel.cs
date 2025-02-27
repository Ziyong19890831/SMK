using SMK.Data.Dto;
using SMK.Data.Entity;

namespace SMK.Web.Models
{
    public class IniOpDtlQueryModel : PagedRequest
    {
        public int DataType { get; set; }
        public string DataId { get; set; }
        public string FeeYm { get; set; }
    }
}