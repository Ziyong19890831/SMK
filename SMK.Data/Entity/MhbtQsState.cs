using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SMK.Data.Utility;

namespace SMK.Data.Entity
{
    public partial class MhbtQsState : IHasSeqNo
    {
        public string HospID { get; set; }
        public string ID { get; set; }
        public string Birthday { get; set; }
        public string FuncDate { get; set; }
        public int Seqno { get; set; }
        public string CureState { get; set; }
        public string CureStateOther { get; set; }
        public string TxtDate { get; set; }
        public string AdjustUserID { get; set; }
        public string CureType { get; set; }
        public string HospSeqNo { get; set; }

        public int SeqNo
        {
            set
            {
                Seqno = value;
            }
        }
    }
}
