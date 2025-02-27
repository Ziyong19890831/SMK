using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SMK.Data.Entity
{
    [ModelMetadataType(typeof(GenHospContMetaData))]
    public partial class GenHospCont
    {
        public class GenHospContMetaData
        {
            [Display(Name = "健保特約類別")]
            public string HospContType { get; set; }

            [Display(Name = "健保特約類別名稱")]
            public string HospContName { get; set; }
        }
    }
}



