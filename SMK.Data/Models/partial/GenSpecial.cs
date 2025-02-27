using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SMK.Data.Entity
{
    [ModelMetadataType(typeof(GenSpecialMetaData))]
    public partial class GenSpecial
    {
        public class GenSpecialMetaData
        {
            [Display(Name = "專科代碼")]
            public string SpecialistNo { get; set; }

            [Display(Name = "專科名稱")]
            public string SpecialistName { get; set; }
        }
    }
}
