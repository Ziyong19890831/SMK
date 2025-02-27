using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMK.Data.Entity
{
    [Table("CST_QS_QUOTA")]
    public class QsQuota
    {
        public string YEARS { get; set; }
        public string HOSP_ID { get; set; }
        public string HOSP_SEQ_NO { get; set; }
        public int? QUOTA { get; set; }
        public DateTime? TXT_DATE { get; set; }
        public string ADJUST_USER_ID { get; set; }
        public DateTime VALID_S_DATE { get; set; }
        public DateTime VALID_E_DATE { get; set; }
        public string REMARK { get; set; }
        public int CURE_TYPE { get; set; }
        public string CREAT_USER_ID { get; set; }
        public DateTime CREAT_DATE { get; set; }

    }
}
