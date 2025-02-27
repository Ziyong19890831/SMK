using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.Models
{
    public class DataInsertLogModel
    {
        [DisplayName("序號")]
        public int ISNO { get; set; }
        [DisplayName("資料名稱")]
        public string FileName { get; set; }
        [DisplayName("匯入結束時間")]
        public DateTime? FinishDate { get; set; }
        [DisplayName("筆數")]
        public int? RecordCount { get; set; }
    }
}
