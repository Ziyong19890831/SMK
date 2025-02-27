using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SMK.Data.Entity
{
    [ModelMetadataType(typeof(GenBranchMetaData))]
    public partial class GenBranch
    {
        public class GenBranchMetaData
        {
            [Display(Name = "分區代碼")]
            public string BranchNo { get; set; }

            [Display(Name = "分區名稱")]
            public string BranchName { get; set; }
        }
    }
}
