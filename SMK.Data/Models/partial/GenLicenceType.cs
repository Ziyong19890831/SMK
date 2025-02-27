
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SMK.Data.Entity
{
    [ModelMetadataType(typeof(GenLicenceTypeMetaData))]
    public partial class GenLicenceType
    {
        public class GenLicenceTypeMetaData
        {
            [Display(Name = "證書類別")]
            public string LicenceType { get; set; }

            [Display(Name = "證書類別名稱")]
            public string LicenceName { get; set; }
        }
    }
}
