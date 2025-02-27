using Microsoft.AspNetCore.Authentication;

namespace SMK.Web.Models
{
    public class HospBasicExportModel
    {
        public string HospId { get; set; }
        public string HospSeqNo { get; set; }
        public string HospName { get; set; }
        public string HospTel { get; set; }
        public string HospFax { get; set; }
        public string HospOwnName { get; set; }
        public string HospOwnID { get; set; }
        public string ZIP { get; set; }
        public string HospAddress { get; set; }
        public string Contact1 { get; set; }
        public string ContactTel1 { get; set; }
        public int Bcnt { get; set; }
        public int Ccnt { get; set; }
        public int Icnt { get; set; }
        public int Jcnt { get; set; }
        public string HospContName { get; set; }
        public string BranchName { get; set; }
        public string MinHospStartDate { get; set; }
        public string MaxHospEndDate { get; set; }
    }
}