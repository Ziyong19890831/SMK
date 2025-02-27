using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICCardConsole.App_Code
{
    class VPNData
    {

    }
    public class CST_AGENT_PATIENT
    {
        public string HospID { get; set; }
        public string HospAgentCode { get; set; }
        public string ID { get; set; }
        public string Birthday { get; set; }        
        public string Name { get; set; }
        public string Sex { get; set; }
        public string Inform_ADDR { get; set; }
        public string Tel_D { get; set; }
        public string Tel_N { get; set; }
        public string Tel_M { get; set; }
        public string Seq_No { get; set; }
        public string Branch_Code { get; set; }
        public string Txt_Date { get; set; }
        public string Func_Mark { get; set; }        
        public string Town_Code { get; set; }
        public string Town_Name { get; set; }
  
    }

    public class CST_QS_CURE
    {
        public string HospID { get; set; }
        public string ID { get; set; }
        public string Birthday { get; set; }
        public string FuncDate { get; set; }
        public string CureItem { get; set; }
        public decimal? CureNum { get; set; }
        public string TxtDate { get; set; }
        public string AdjustUserID { get; set; }
        public string HospSeqNo { get; set; }
    }
    public class CST_QS_DATA
    {
        public string HospID { get; set; }
        public string ID { get; set; }
        public string Birthday { get; set; }
        public string FuncDate { get; set; }
        public string FirstTreatDate { get; set; }
        public string PrsnID { get; set; }
        public string CureStage { get; set; }
        public string ExamYear { get; set; }
        public decimal? SmokeYear { get; set; }
        public decimal? SmokeMon { get; set; }
        public decimal? SmokeDayNum { get; set; }
        public decimal? BaseWeight { get; set; }
        public decimal? CureWeek { get; set; }
        public decimal? WeekTot { get; set; }
        public string SmokeFirst { get; set; }
        public string SmokeStop { get; set; }
        public string SmokeNoGp { get; set; }
        public string SmokeMuch { get; set; }
        public string SmokeBed { get; set; }
        public string SmokeSick { get; set; }
        public string SmokeNico { get; set; }
        public string SmokeLung { get; set; }
        public string SmokeScore { get; set; }
        public string CureAgree { get; set; }
        public string BranchCode { get; set; }
        public string TxtDate { get; set; }
        public string AdjustUserID { get; set; }
        public string FeeMark { get; set; }
        public decimal? CoCheck { get; set; }
        public string TraceDate { get; set; }
        public string TraceState { get; set; }
        public string CurtState { get; set; }
        public decimal? TraceCoCheck { get; set; }
        public string SideEffect { get; set; }
        public string CureStateOther { get; set; }
        public string ChType { get; set; }
        public string Trace_Date2 { get; set; }
        public string Trace_State2 { get; set; }
        public string Cure_State2 { get; set; }
        public string Trace_Co_Check2 { get; set; }
        public string Case_Source { get; set; }
        public string Cure_Type { get; set; }
        public string Case_Kind { get; set; }
        public string HospSeqNo { get; set; }
    }
    public class CST_QS_STATE
    {
        public string HospID { get; set; }
        public string ID { get; set; }
        public string Birthday { get; set; }
        public string FuncDate { get; set; }
        public int SeqNo { get; set; }
        public string CureState { get; set; }
        public string CureStateOther { get; set; }
        public string CureType { get; set; }
        public string TxtDate { get; set; }
        public string AdjustUserID { get; set; }        
        public string HospSeqNo { get; set; }
    }
}
