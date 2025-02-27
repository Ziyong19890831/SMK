using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMK.Data.Entity
{
    public partial class HospContractType
    {
        public string HospId { get; set; }
        public string HospContType { get; set; }
        public string CntSDate { get; set; }
        public string CntEDate { get; set; }
        public string HospSeqNo { get; set; }
    }
}
