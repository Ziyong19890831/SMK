using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace SMK.Data.Entity
{
    public partial class MhbtQsCure
    {
        public string HospID { get; set; }
        public string ID { get; set; }
        public string Birthday { get; set; }
        public string FuncDate { get; set; }
        public string CureItem { get; set; }
        public decimal CureNum { get; set; }
        public string TxtDate { get; set; }
        public string AdjustUserID { get; set; }
        public string HospSeqNo { get; set; }
    }
}
