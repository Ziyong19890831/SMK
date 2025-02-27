using Newtonsoft.Json;
using SMK.Data.Attributes;
using System;
using System.Collections.Generic;

namespace SMK.Data.Entity
{
    public partial class PrsnBasic
    {
        public string PrsnId { get; set; }
        public string PrsnName { get; set; }
        [JsonConverter(typeof(WestTwDateConverter))]
        public string PrsnBirthday { get; set; }
        public string PrsnType { get; set; }
        public string MajorSpecialistNo { get; set; }
        public string SubSpecialistNo { get; set; }
        public string Remark { get; set; }
        public string Pemail { get; set; }
    }
}
