using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SMK.Data.Entity
{
    [ModelMetadataType(typeof(GenSmkcontractMetaData))]
    public partial class GenSmkcontract
    {
        public class GenSmkcontractMetaData
        {
            [Display(Name = "戒菸合約類別代碼")]
            public string SmkcontractType { get; set; }

            [Display(Name = "戒菸合約類別名稱")]
            public string SmkcontractName { get; set; }
        }
    }
}
