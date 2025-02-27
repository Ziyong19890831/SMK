using System.ComponentModel;
using SMK.Data.Dto;

namespace SMK.Web.Models
{
    public class IniExportInCtrlQueryModel : PagedRequest
    {
        [DisplayName("上傳日期")]
        public string CreatedAt { get; set; }

        [DisplayName("檔案日期")]
        public string FileDate { get; set; }

        [DisplayName("上傳檔名")]
        public string FileName { get; set; }
        [DisplayName("查詢年月")]
        public string fee_ym { get; set; }
    }
}
