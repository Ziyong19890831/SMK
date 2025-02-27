using System;
using System.Collections.Generic;

namespace SMK.Data.Entity
{
    public partial class GenHospCont
    {
        public string HospContType { get; set; }
        public string HospContName { get; set; }

        public int QualityDefaultCount { get; set; }
        public int QualityImproveCount { get; set; }
    }
}
