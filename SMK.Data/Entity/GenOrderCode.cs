using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SMK.Data.Entity
{
    public partial class GenOrderCode
    {
        [Key]
        public string OrderCode { get; set; }
        public string OrderChiName { get; set; }
        public string OrderEngName { get; set; }
        public string OrderCate { get; set; }
        public string Remark { get; set; }
    }
}
