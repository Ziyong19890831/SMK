using Newtonsoft.Json;
using SMK.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMK.Data.Entity
{
    public partial class PrsnContract
    {
        public int Id { get; set; }
        public string HospId { get; set; }
        public string PrsnId { get; set; }
        public string SmkcontractType { get; set; }
        [JsonConverter(typeof(WestTwDateConverter))]
        public string PrsnStartDate { get; set; }
        [JsonConverter(typeof(WestTwDateConverter))]
        public string PrsnEndDate { get; set; }
        [JsonConverter(typeof(WestTwDateConverter))]
        public string CreateDate { get; set; }
        [JsonConverter(typeof(WestTwDateConverter))]
        public string ModifyDate { get; set; }
        public int? ModifyPersonNo { get; set; }
        public string Remark { get; set; }
        public string HospSeqNo { get; set; }
        public string CouldTreat { get; set; }
        public string CouldInstruct { get; set; }
        public string EndReasonNo { get; set; }
    }
}
