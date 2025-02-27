

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SMK.Data.Entity
{
    [ModelMetadataType(typeof(PrsnBasicMetaData))]
    public partial class PrsnBasic
    {
        public class PrsnBasicMetaData
        {
            [Display(Name = "身份證號")]
            public string PrsnId { get; set; }

            [Display(Name = "姓名")]
            public string PrsnName { get; set; }

            [Display(Name = "出生日期")]
            public string PrsnBirthday { get; set; }
           
            [Display(Name = "人員類別")]
            public string PrsnType { get; set; }

            [Display(Name = "主要專科")]
            public string MajorSpecialistNo { get; set; }

            [Display(Name = "次要專科")]
            public string SubSpecialistNo { get; set; }

            [Display(Name = "備註")]
            public string Remark { get; set; }
            [Display(Name = "Email")]
            public string Pemail { get; set; }

        }
    }
}
