using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICCardConsole.App_Code
{
    class ICCardData
    {
       
    }
    public class ICCard
    {
        public int DataType { get; set; }
        public string PersonID { get; set; }
        public DateTime? Birthday { get; set; }
        public string HospitalCode { get; set; }
        public string PhysicianPersonID { get; set; }
        public DateTime? ReadCardDatetime { get; set; }
        public string MedicalSeries { get; set; }
        public string ReissueNote { get; set; }
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
        public string MedicineCount { get; set; }
    }
}
