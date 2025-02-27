using System;
using System.Collections.Generic;

namespace SMK.Data.Entity
{
    public partial class SamplingData
    {
        public string FeeYm { get; set; }
        public string DataId { get; set; }
        public int OrderSeqNo { get; set; }
        public string Review { get; set; }
        public string Reviewdate { get; set; }
        public string Reviewremark { get; set; }
        public string Appeals { get; set; }
        public string Appealsdate { get; set; }
        public string Appealsremark { get; set; }
        public int? Reviewamt { get; set; }
        public int? Appealsamt { get; set; }
    }
}
