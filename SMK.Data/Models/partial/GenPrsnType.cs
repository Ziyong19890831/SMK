using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SMK.Data.Entity
{
    [ModelMetadataType(typeof(GenPrsnTypeMetaData))]
    public partial class GenPrsnType
    {
        public class GenPrsnTypeMetaData
        {
            [Display(Name = "醫事人員類別代碼")]
            public string PrsnType { get; set; }

            [Display(Name = "醫事人員類別名稱")]
            public string PrsnTypeName { get; set; }
        }
    }
}
