using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMK.Data.Entity
{
    public class ICCardData
    {
        public int ID { get; set; }
        [StringLength(2)]
        public string? DataType { get; set; }
        public string PersonID { get; set; }
        public string? Birthday { get; set; }
        public string HospitalCode { get; set; }
        public string PhysicianPersonID { get; set; }
        public string? ReadCardDatetime { get; set; }
        public string MedicalSeries { get; set; }
        public int? ReissueNote { get; set; }
        public string MedicalType { get; set; }
        public string MainMedicalCode { get; set; }
        public string MinorMedicalCodeFirst { get; set; }
        public string MinorMedicalCodeSecond { get; set; }
        public string MinorMedicalCodeThird { get; set; }
        public string MinorMedicalCodeFourth { get; set; }
        public string MinorMedicalCodeFifth { get; set; }
        public DateTime? MedicalDate { get; set; }
        public string PhysicianOrderType { get; set; }
        public string TreatCode { get; set; }
        public string MedicineMethod { get; set; }
        public string MedicineDay { get; set; }
        public int? MedicineCount { get; set; }
        public string CreateDT { get ; set; }
    }
}
