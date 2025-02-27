using System.ComponentModel;
using SMK.Data.Dto;
using SMK.Data.Enums;

namespace SMK.Web.Models
{
    public class IniFileInCtrlQueryModel : PagedRequest
    {
        [DisplayName("上傳日期")]
        public string CreatedAt { get; set; }

        [DisplayName("上傳檔名")]
        public string FileName { get; set; }

        [DisplayName("檔案狀態")]
        public FileInStatus fileInStatus { get; set; }
    }
}
