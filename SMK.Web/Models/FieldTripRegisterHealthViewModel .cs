using SMK.Data.Entity;
using System;

namespace SMK.Web.Models
{
    /// <summary>
    /// 健保衛教資料
    /// </summary>
    public class FieldTripRegisterIniOpDtlViewModel : IniOpDtl
    {
        public string DTL { get; set; }
    }
    public class FieldTripRegisteIniDrOrdViewModel : IniDrOrd
    {
        public int Row_ID { get; set; }
    }

    public class FieldTripRegisteHealthQueryViewModel
    {
        public string DataType { get; set; }
        public string HospId { get; set; }
        public string HospSeqNo { get; set; }
        public string HospName { get; set; }
        public string Id { get; set; }
        public string Birthday { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string TelD { get; set; }
        public string TelM { get; set; }
        public string TownName { get; set; }
        public string InformADDR { get; set; }
        public string ExamYear { get; set; }
        public string ExamYearTW
        {
            get
            {
                if (this.ExamYear.Length == 4)
                {
                    return (int.Parse(this.ExamYear) - 1911).ToString();
                }
                return this.ExamYear;
            }
        }
        public string WeekCount { get; set; }
        public string FeeYm { get; set; }
        public string DataId { get; set; }
        public string FuncDate { get; set; }
        public string ApplDate { get; set; }
        public string Order_code1 { get; set; }
        public string OrderChiName1 { get; set; }
        public string Order_qty1 { get; set; }
        public string Order_code2 { get; set; }
        public string OrderChiName2 { get; set; }
        public string Order_qty2 { get; set; }
        public string Order_code3 { get; set; }
        public string OrderChiName3 { get; set; }
        public string Order_qty3 { get; set; }
        public string Order_code4 { get; set; }
        public string OrderChiName4 { get; set; }
        public string Order_qty4 { get; set; }
        public string InctructSerial { get; set; }
    }
}