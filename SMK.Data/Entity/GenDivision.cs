using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SMK.Data.Entity
{
    public partial class GenDivision
    {
        [MaxLength(2)]
        [Key]
        public string DivisionNo { get; set; }
        
        [MaxLength(20)]
        public string DivisionName { get; set; }
        
        [MaxLength(5)]
        public string StatDivisionCode { get; set; }
    }
}
