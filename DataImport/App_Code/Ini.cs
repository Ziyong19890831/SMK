using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICCardConsole.App_Code
{
    class Ini
    {
    }
    
    public class iniDrDtl
    {
        public string data_id { get; set; }
        public string HospID { get; set; }
        public string fee_ym { get; set; }
        public string ExamYear { get; set; }
        public int? ExamTime { get; set; }
        public string FirstTreatDate { get; set; }
        public int? WeekCount { get; set; }
        public string InstructExamYear { get; set; }
        public int? InstructExamTime { get; set; }
        public string FirstInstructDate { get; set; }
        public int? InctructSerial { get; set; }
        public string MedApply { get; set; }
        public string InstructApply { get; set; }
        public string TraceApply { get; set; }
        public string ReleaseApply { get; set; }
        public string appl_type { get; set; }
        public string appl_date { get; set; }
        public string case_type { get; set; }
        public int? seq_no { get; set; }
        public string func_type { get; set; }
        public string func_date { get; set; }
        public string rel_date { get; set; }
        public string birthday { get; set; }
        public string id { get; set; }
        public string func_seq_no { get; set; }
        public string pay_type { get; set; }
        public string part_code { get; set; }
        public string icd9cm_code { get; set; }
        public string icd9cm_code1 { get; set; }
        public string icd9cm_code2 { get; set; }
        public int? drug_days { get; set; }
        public string prsn_id { get; set; }
        public string drug_prsn_id { get; set; }
        public int? drug_dot { get; set; }
        public int? cure_dot { get; set; }
        public string dsvc_code { get; set; }
        public int? dsvc_dot { get; set; }
        public int? exp_dot { get; set; }
        public int? part_amt { get; set; }
        public int? appl_dot { get; set; }
        public string orig_hosp_id { get; set; }
        public string Id_Sex { get; set; }
        public string cure_item1 { get; set; }
        public string cure_item2 { get; set; }
        public string cure_item3 { get; set; }
        public string cure_item4 { get; set; }
        public string orig_case_type { get; set; }
        public int? other_part_amt { get; set; }
        public string appl_cause_mark { get; set; }
        public string icd10cm_code2 { get; set; }
        public string icd10cm_code3 { get; set; }
        public string icd10cm_code4 { get; set; }
        public string corr_hosp_id { get; set; }
        public string area_service { get; set; }
        public string tran_date { get; set; }
        public string name { get; set; }

    }
    public class iniDrOrd
    {
        public string data_id { get; set; }
        public int? order_seq_no { get; set; }
        public string fee_ym { get; set; }
        public string order_type { get; set; }        
        public string order_code { get; set; }
        public string drug_num { get; set; }
        public string drug_fre { get; set; }
        public string drug_path { get; set; }
        public decimal? order_uprice { get; set; }
        public decimal? order_qty { get; set; }
        public int? order_dot { get; set; }
        public int? order_drug_day { get; set; }
        public string exe_prsn_id { get; set; }
        public string tran_date { get; set; }
    }
    public class iniOpDtl
    {
        public string data_id { get; set; }
        public string fee_ym { get; set; }
        public string ExamYear { get; set; }        
        public int? ExamTime { get; set; }
        public string FirstTreatDate { get; set; }
        public int? WeekCount { get; set; }
        public string InstructExamYear { get; set; }
        public int? InstructExamTime { get; set; }
        public string FirstInstructDate { get; set; }
        public int? InctructSerial { get; set; }
        public string MedApply { get; set; }
        public string InstructApply { get; set; }
        public string TraceApply { get; set; }
        public string ReleaseApply { get; set; }
        public string appl_type { get; set; }
        public string HospID { get; set; }
        public string appl_date { get; set; }
        public string case_type { get; set; }
        public int? seq_no { get; set; }
        public string func_type { get; set; }
        public string func_date { get; set; }
        public string cure_e_date { get; set; }
        public string birthday { get; set; }
        public string id { get; set; }
        public string func_seq_no { get; set; }
        public string pay_type { get; set; }
        public string part_code { get; set; }
        public string icd9cm_code { get; set; }
        public string icd9cm_code1 { get; set; }
        public string icd9cm_code2 { get; set; }
        public int? drug_days { get; set; }
        public string rel_mode { get; set; }
        public string prsn_id { get; set; }
        public string drug_prsn_id { get; set; }
        public int? drug_dot { get; set; }
        public int? cure_dot { get; set; }
        public string diag_code { get; set; }
        public int? diag_dot { get; set; }
        public string dsvc_code { get; set; }
        public int? dsvc_dot { get; set; }
        public int? exp_dot { get; set; }
        public int? part_amt { get; set; }
        public int? appl_dot { get; set; }
        public string Id_Sex { get; set; }
        public string cure_item1 { get; set; }
        public string cure_item2 { get; set; }
        public string cure_item3 { get; set; }
        public string cure_item4 { get; set; }
        public string area_service { get; set; }
        public string supp_area { get; set; }
        public string real_hosp_id { get; set; }
        public string hosp_data_type { get; set; }
        public decimal? agency_part_amt { get; set; }
        public string name { get; set; }
        public string appl_cause_mark { get; set; }
        public string icd10cm_code3 { get; set; }
        public string icd10cm_code4 { get; set; }
        public int? met_dot { get; set; }
        public string corr_hosp_id { get; set; }
        public string tran_date { get; set; }
    }
    public class iniOpOrd
    {
        public string data_id { get; set; }
        public int? order_seq_no { get; set; }
        public string fee_ym { get; set; }
        public string order_type { get; set; }
        public string order_code { get; set; }
        public string rel_mode { get; set; }
        public string chr_mark { get; set; }
        public string drug_num { get; set; }
        public string drug_fre { get; set; }
        public string drug_path { get; set; }
        public decimal? order_uprice { get; set; }
        public decimal? order_qty { get; set; }
        public int? order_dot { get; set; }
        public string exe_prsn_id { get; set; }
        public string cure_path { get; set; }
        public int? order_drug_day { get; set; }        
        public string tran_date { get; set; }
    }
}
