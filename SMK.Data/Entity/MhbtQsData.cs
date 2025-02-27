using System;
using System.Collections.Generic;

namespace SMK.Data.Entity
{
    public partial class MhbtQsData : IEquatable<MhbtQsData>
    {
        public string HospId { get; set; }
        public string ID { get; set; }
        public string Birthday { get; set; }
        public string FuncDate { get; set; }
        public string FirstTreatDate { get; set; }
        public string PrsnID { get; set; }
        public string CureStage { get; set; }
        public string ExamYear { get; set; }
        public decimal SmokeYear { get; set; }
        public decimal SmokeMon { get; set; }
        public decimal SmokeDayNum { get; set; }
        public decimal BaseWeight { get; set; }
        public decimal CureWeek { get; set; }
        public decimal WeekTot { get; set; }
        public string SmokeFirst { get; set; }
        public string SmokeStop { get; set; }
        public string SmokeNoGp { get; set; }
        public string SmokeLung { get; set; }
        public string SmokeMuch { get; set; }
        public string SmokeBed { get; set; }
        public string SmokeSick { get; set; }
        public string SmokeNico { get; set; }
        public decimal SmokeScore { get; set; }
        public string CureAgree { get; set; }
        public string BranchCode { get; set; }
        public string TxtDate { get; set; }
        public string AdjustUserId { get; set; }
        public string FeeMark { get; set; }
        public decimal CoCheck { get; set; }
        public string TraceDate { get; set; }
        public string TraceState { get; set; }
        public string CurtState { get; set; }
        public decimal TraceCoCheck { get; set; }
        public string SideEffect { get; set; }
        public string CureStateOther { get; set; }
        public string ChType { get; set; }
        public string TraceDate2 { get; set; }
        public string TraceState2 { get; set; }
        public string CureState2 { get; set; }
        public decimal TraceCoCheck2 { get; set; }
        public string CaseSource { get; set; }
        public string CureType { get; set; }
        public string CaseKind { get; set; }
        public string HospSeqNo { get; set; }

        public bool Equals(MhbtQsData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return HospId == other.HospId && ID == other.ID && Birthday == other.Birthday &&
                   FuncDate == other.FuncDate && CureStage == other.CureStage && ExamYear == other.ExamYear &&
                   CurtState == other.CurtState && CureType == other.CureType && HospSeqNo == other.HospSeqNo;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MhbtQsData) obj);
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
            hashCode.Add(CurtState);
            hashCode.Add(CureType);
            hashCode.Add(HospSeqNo);
            return hashCode.ToHashCode();
        }
    }
}
