using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SMK.Data.Enums;

namespace SMK.Data.Entity
{
    public partial class PrsnContactReport
    {
        [Key]
        public string 身分證號 { get; set; }
        public string 姓名 { get; set; }
        public string 出生日期 { get; set; }
        public string 人員類別 { get; set; }
        public string 醫事機構代碼 { get; set; }
        public string 院區別 { get; set; }
        public string 機構名稱 { get; set; }
        public string 機構狀態 { get; set; }
        public string 服務類型 { get; set; }
        public string 人員合約起日 { get; set; }
        public string 人員合約迄日 { get; set; }
        public string 人員類別代號 { get; set; }
    }
}
