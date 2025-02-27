using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SMK.Data.Entity
{
    public partial class GenSubDivision
    {
        [MaxLength(4)]
        [Key]
        public string SubdivisionNo { get; set; }
        [MaxLength(20)]
        public string SubdivisionName { get; set; }
        [MaxLength(2)]
        public string DivisionNo { get; set; }
        [MaxLength(2)]
        public string DivisionNoOld { get; set; }
        [MaxLength(5)]
        public string ZIP { get; set; }
        [MaxLength(20)]
        public string Type { get; set; }
        [MaxLength(7)]
        public string StatSubDivisionCode { get; set; }
    }
}
