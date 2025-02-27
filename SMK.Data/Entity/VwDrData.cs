using System;
using System.Collections.Generic;

namespace SMK.Data.Entity
{
    public partial class VwDrData
    {
        public string DataId { get; set; }
        public string FeeYm { get; set; }
        public string Hospid { get; set; }
        public string Id { get; set; }
        public string Birthday { get; set; }
        public string Name { get; set; }
        public string Examyear { get; set; }
        public int? Examtime { get; set; }
        public string Firsttreatdate { get; set; }
        public int? Weekcount { get; set; }
        public string ApplDate { get; set; }
        public string ApplType { get; set; }
        public string CaseType { get; set; }
        public string FuncDate { get; set; }
        public string RelDate { get; set; }
        public int? DrugDays { get; set; }
        public int? DrugDot { get; set; }
        public int? CureDot { get; set; }
        public int? ApplDot { get; set; }
        public string Medapply { get; set; }
    }
}
