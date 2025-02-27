using SMK.Data.Dto;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static SMK.Data.Enums.SmokingServicesType;

namespace SMK.Web.Models
{
    public class FieldTripQueryViewModel : PagedRequest
    {
        [DisplayName("醫事機構代碼")]
        [Required(ErrorMessage = "必填")]
        public string HospID { get; set; }      
        [DisplayName("醫事機構名稱")]
        public string HospName { get; set; }
        [Required(ErrorMessage = "必填")]
        public string HospSeqNo { get; set; }
        [DisplayName("就醫日期(起/迄)")]
        [Required(ErrorMessage = "必填")]
        public string FuncStartDate { get; set; }
        [DisplayName("訖年月日")]
        [Required(ErrorMessage = "必填")]
        public string FuncEndDate { get; set; }
        [DisplayName("戒菸服務類型")]
        [Required(ErrorMessage ="必填")]
        public SmokingServicesTypeEnums smokingServicesTypeEnums { get; set; }

        [DisplayName("自訂義下載")]
        public List<string> SelectCheckBox { get; set; }
    }

    public class GetRegisterMedicalOrdersViewModel
    {
        public string HospID { get; set; }
        public string HospSeqNo { get; set; }
        public string HospName { get; set; }
        public string ID { get; set; }
        public string Birthday { get; set; }
        public string Name { get; set; }
        public string FuncDate { get; set; }
        public string OrderChiName { get; set; }
        public string OrderCode { get; set; }
        public string OrderUprice { get; set; }
        public string OrderQty { get; set; }
        public string OrderDot { get; set; }
        public string DataID { get; set; }
        public string FeeYM { get; set; }
        public string OrderSeqNo { get; set; }
        public string MedApply { get; set; }
        public string InstructApply { get; set; }
        public string TraceApply { get; set; }
        public string ReleaseApply { get; set; }
    }

}
