using System;
using System.Collections.Generic;

namespace SMK.Data.Entity
{
    public partial class HospBasic
    {
        public string HospId { get; set; }
        public string HospName { get; set; }
        public string HospTel { get; set; }
        public string HospFax { get; set; }
        public string HospEmail { get; set; }
        public string Contact1 { get; set; }
        public string ContactTel1 { get; set; }
        public string ContactFax1 { get; set; }
        public string ContactEmail1 { get; set; }
        public string Contact2 { get; set; }
        public string ContactTel2 { get; set; }
        public string ContactFax2 { get; set; }
        public string ContactEmail2 { get; set; }
        public string HospOwnName { get; set; }
        public string HospOwnId { get; set; }
        public string BranchNo { get; set; }
        public string Zip { get; set; }
        public string DivisionNo { get; set; }
        public string SubDivisionNo { get; set; }
        public string HospAddress { get; set; }
        public string HospStatus { get; set; }
        public string FirstHospId { get; set; }
        public string PrevHospID { get; set; }
        public string LastHospId { get; set; }
        public string LastContType { get; set; }
        public string Remark { get; set; }
        public string ChFlg1 { get; set; }
        public string ChFlg2 { get; set; }
        public string ChFlg3 { get; set; }
        public string PrevHospSeqNo { get; set; }
        public string LastHospSeqNo { get; set; }
        public string HospSeqNo { get; set; }
        public string FirstHospSeqNo { get; set; }
        public string HospAbbr { get; set; }
        public string CreateDate { get; set; }
        public string ModifyDate { get; set; }
    }
}
