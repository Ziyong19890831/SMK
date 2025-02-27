using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using SMK.Data.Entity;
using System;

namespace SMK.Web.Models
{
    public class MhbtQsDataViewModel : MhbtQsData
    {
        public string StringCureWeek { get; set; }
    }
    public class MhbtQsCureViewModel : MhbtQsCure
    {
        public int Row_ID { get; set; }
        public string CureNumString { get; set; }
    }
    /// <summary>
    /// VPN治療輸出模型
    /// </summary>
    public class TreatmentViewModel
    {
        public string DataType { get; set; }
        public string HospId { get; set; }
        public string HospSeqNo { get; set; }
        public string HospName { get; set; }
        public string ID { get; set; }
        public string Birthday { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string TelD { get; set; }
        public string TelM { get; set; }
        public string TownName { get; set; }
        public string InformADDR { get; set; }
        public string ExamYear { get; set; }
        public string CureStage { get; set; }
        public string CureWeek { get; set; }
        public string FuncDate { get; set; }
        public string CureItem1 { get; set; }
        public string OrderChiName1 { get; set; }
        public string CureNum1 { get; set; }
        public string CureItem2 { get; set; }
        public string OrderChiName2 { get; set; }
        public string CureNum2 { get; set; }
        public string CureItem3 { get; set; }
        public string OrderChiName3 { get; set; }
        public string CureNum3 { get; set; }
        public string CureItem4 { get; set; }
        public string OrderChiName4 { get; set; }
        public string CureNum4 { get; set; }

    }

    /// <summary>
    /// VPN衛教輸出模型
    /// </summary>
    public class HealthMentViewModel
    {
        public string DataType { get; set; }
        public string HospId { get; set; }
        public string HospSeqNo { get; set; }
        public string HospName { get; set; }
        public string ID { get; set; }
        public string Birthday { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string TelD { get; set; }
        public string TelM { get; set; }
        public string TownName { get; set; }
        public string InformADDR { get; set; }
        public string ExamYear { get; set; }
        public string FuncDate { get; set; }
        public string CureStage { get; set; }
        public string CureWeek { get; set; }
    }

}
