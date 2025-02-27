using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMK.Data.Entity
{
    public partial class DataInsertLog
    {
        [Key]
        public int ISNO { get; set; }
        public string FileName { get; set; }
        public DateTime? FinishDate { get; set; }
        public int? RecordCount { get; set; }
    }
}
