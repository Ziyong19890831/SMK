namespace SMK.Web.Models
{
    public class PrsnContractExportModel
    {
        public string HospID { get; set; }
        public string HospSeqNo { get; set; }
        public string HospName { get; set; }
        public string ZIP { get; set; }
        public string HospAddress { get; set; }
        public string PrsnName { get; set; }
        public string PrsnID { get; set; }
        public string PrsnBirthday { get; set; }
        public string PrsnTypeName { get; set; }
        public string CouldInstruct { get; set; }
        public string CouldTreat { get; set; }
        public string MinPrsnStartDate { get; set; }
        public string MaxPrsnEndDate { get; set; }
        public string LastContType { get; set; }
        public string HospStatus { get; set; }
        public string PEmail { get; set; }
        public string HospEmail { get; set; }
        public string ContactEmail1 { get; set; }
        public string ContactEmail2 { get; set; }
    }
}