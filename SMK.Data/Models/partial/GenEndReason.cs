using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SMK.Data.Entity
{
    [ModelMetadataType(typeof(GenEndReasonMetaData))]
    public partial class GenEndReason
    {
        public class GenEndReasonMetaData
        {
            [Display(Name = "終止原因代碼")]
            public string EndReasonNo { get; set; }

            [Display(Name = "終止原因代碼名稱")]
            public string EndReasonName { get; set; }
        }
    }
}
