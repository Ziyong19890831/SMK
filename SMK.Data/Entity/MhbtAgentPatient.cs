using System;
using System.Collections.Generic;

namespace SMK.Data.Entity
{
    public partial class MhbtAgentPatient
    {
        public string HospID { get; set; }
        public string HospAgentCode { get; set; }
        public string ID { get; set; }
        public string Birthday { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string InformADDR { get; set; }
        public string TelD { get; set; }
        public string TelN { get; set; }
        public string TelM { get; set; }
        public string SeqNo { get; set; }
        public string BranchCode { get; set; }
        public string TxtDate { get; set; }
        public string FuncMark { get; set; }
        public string TownCode { get; set; }
        public string TownName { get; set; }
    }
}
