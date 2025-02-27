using Microsoft.AspNetCore.Mvc;
using SMK.Data.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMK.Data.Entity
{
    [ModelMetadataType(typeof(HospContractMetaData))]
    public partial class HospContract
    {
        public class HospContractMetaData
        {
            public string HospId { get; set; }
            public string SmkcontractType { get; set; }
            public string HospStartDate { get; set; }
            public string HospEndDate { get; set; }
            public string EndReasonNo { get; set; }
            public string CreateDate { get; set; }
            public string ModifyDate { get; set; }
            public int? ModifyPersonNo { get; set; }
            public string Remark { get; set; }
            public string HospSeqNo { get; set; }
        }
    }
}