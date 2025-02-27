using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMK.Data.Entity
{
    public partial class MhbtQsData2 : IEquatable<MhbtQsData2>
    {
        [DisplayName("機構代碼")]
        [StringLength(10)]
        [Required]
        public string HospId { get; set; }
        [DisplayName("身分證號")]
        [StringLength(10)]
        [Required]
        public string ID { get; set; }
        [DisplayName("出生日期")]
        [StringLength(8)]
        [Required]
        public string Birthday { get; set; }
        [DisplayName("就診日期")]
        [StringLength(8)]
        [Required]
        public string FuncDate { get; set; }
        [DisplayName("療程序號")]
        [StringLength(1)]
        [Required]
        public string CureStage { get; set; }
        [DisplayName("療程年度")]
        [StringLength(4)]
        [Required]
        public string ExamYear { get; set; }
        [DisplayName("療程年度")]
        [StringLength(1)]
        [Required]
        public string Cure_Type { get; set; }
        [DisplayName("院區別")]
        [StringLength(2)]
        [Required]
        public string HospSeqNo { get; set; }


        [DisplayName("體重")]
        public decimal BaseWeight { get; set; }
        [DisplayName("身高")]
        public decimal Height { get; set; }
        [DisplayName("身分證號(醫)")]
        [StringLength(10)]
        public string PrsnID { get; set; }
        [DisplayName("追蹤CO檢測值一年")]
        [StringLength(2)]
        public string Trace_Co_Check3 { get; set; }
        [DisplayName("已追蹤日期(一年)")]
        [StringLength(8)]
        public string Trace_Date3 { get; set; }
        [DisplayName("戒菸結果(一年)")]
        [StringLength(2)]
        public string Cure_State3 { get; set; }
        [DisplayName("追蹤情形(一年)")]
        [StringLength(3)]
        public string Trace_State3 { get; set; }


        public bool Equals(MhbtQsData2 other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return HospId == other.HospId && ID == other.ID && Birthday == other.Birthday &&
                   FuncDate == other.FuncDate && CureStage == other.CureStage && ExamYear == other.ExamYear &&
                   Cure_Type == other.Cure_Type && HospSeqNo == other.HospSeqNo;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MhbtQsData2) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(HospId);
            hashCode.Add(ID);
            hashCode.Add(Birthday);
            hashCode.Add(FuncDate);
            hashCode.Add(CureStage);
            hashCode.Add(ExamYear);
            hashCode.Add(Cure_Type);
            hashCode.Add(HospSeqNo);
            return hashCode.ToHashCode();
        }
    }
}
