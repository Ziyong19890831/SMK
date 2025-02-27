using SMK.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMK.Data.Entity
{
    public partial class OrdOfB7
    {
        [Key]
        public string Data_ID { get; set; }
        public string Fee_YM { get; set; }
        public int Order_Seq_No { get; set; }
        public string Order_Type { get; set; }
        public string Order_Code { get; set; }
        public string Rel_Mode { get; set; }
        public string Drug_Num { get; set; }
        public string Drug_Fre { get; set; }
        public string Drug_Path { get; set; }
        public decimal? Order_Uprice { get; set; }
        public decimal? Order_Qty { get; set; }
        public int? Order_Dot { get; set; }
        public string Exe_Prsn_ID { get; set; }
        public string Remark { get; set; }
    }


}
