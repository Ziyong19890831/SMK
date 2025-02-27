using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SMK.Data.Entity
{
    public partial class GenDivision
    {
        public class GenDivisionMetaData
        {
            [Display(Name = "終止原因代碼")]
            public string EndReasonNo { get; set; }

            [Display(Name = "終止原因代碼名稱")]
            public string EndReasonName { get; set; }
        }
    }
}
