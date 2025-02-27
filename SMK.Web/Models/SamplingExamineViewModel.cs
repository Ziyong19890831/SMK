using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace SMK.Web.Models
{
    public class SamplingExamineViewModel
    {


    }

    public class SamplingExamineQueryModel
    {
        /// <summary>
        /// 費用年月起
        /// </summary>
        [DisplayName("費用年月")]
        public string FeeStart { get; set; }

        /// <summary>
        /// 費用年月迄
        /// </summary>
        [DisplayName("費用年月")]
        public string FeeEnd { get; set; }

        /// <summary>
        /// 醫事機構代碼
        /// </summary>
        [DisplayName("醫事機構代碼")]
        public string HospID { get; set; }

        /// <summary>
        /// 院區別
        /// </summary>
        [DisplayName("院區別")]
        public string HospSeqNo { get; set; }


        public string AccessNo { get; set; }

    }

    public class SamplingExamineCreateData
    {
        public bool isChecked { get; set; }
        public string samplingno { get; set; }
        public string fee_ym { get; set; }
        public string data_id { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string func_date { get; set; }
        public int? appl_dot { get; set; }
        public string review { get; set; }
        public string reviewdate { get; set; }
        public string appeals { get; set; }
        public string appealsdate { get; set; }
        public string chkflg { get; set; }

        public string fee_ym_taiwan { get; set; }
        public string func_date_taiwan { get; set; }
        public string reviewdate_taiwan { get; set; }
        public string appealsdate_taiwan { get; set; }
    }

    public class SamplingCreateRequest
    {
        public bool isReview { get; set; }
        public string review { get; set; }
        public string reviewdate { get; set; }
        public bool isAppeals { get; set; }
        public string appeals { get; set; }
        public string appealsdate { get; set; }

        public List<SamplingExamineCreateData> Items { get; set; }
    }

    public class SamplingResultQueryRequest
    {
        public string SamplingNo { get; set; }
    }

    public class SamplingResultQueryData
    {
        public bool isChecked { get; set; }
        public string fee_ym { get; set; }
        public string data_id { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public string func_date { get; set; }
        public string hospid { get; set; }
        public string hospseqno { get; set; }
        public string hospname { get; set; }
        public string review { get; set; }
        public string reviewdate { get; set; }
        public string reviewremark { get; set; }
        public int? reviewamt { get; set; }
        public string appeals { get; set; }
        public string appealsdate { get; set; }
        public string appealsremark { get; set; }
        public int? appealsamt { get; set; }
        public int order_seq_no { get; set; }
        public string order_type { get; set; }
        public int? order_drug_day { get; set; }
        public string order_code { get; set; }
        public string orderchiname { get; set; }
        public string drug_num { get; set; }
        public string drug_fre { get; set; }
        public string drug_path { get; set; }
        public decimal? order_qty { get; set; }
        public decimal? order_uprice { get; set; }
        public int? order_dot { get; set; }
        public string chkflg { get; set; }
    }

    public class SaveSamplingResultDataRequest
    {
        public string review { get; set; }
        public string appeals { get; set; }
        public List<SamplingResultQueryData> items { get; set; }
    }
    public class ExportMedicineReportRequest
    {
        public string data_id { get; set; }
        public string fee_ym { get; set; }
        public string id { get; set; }
        public string feeYmS { get; set; }
        public string feeYmE { get; set; }
        public string wkHospID { get; set; }
        public string[] wkDataID { get; set; }
    }
    public class ExportRewRequest
    {
        public string feeYmS { get; set; }
        public string feeYmE { get; set; }
        public string wkHospID { get; set; }
        public string[] wkDataID { get; set; }
        public string fileType { get; set; }
    }

    public class RewFileViewModel
    {
        public string FileName { get; set; }
        public Stream Stream { get; set; }
    }

    public class Rew1300ItemDto
    {
        public int? seq_no { get; set; }
        public string samplingno { get; set; }
        public string hospid { get; set; }
        public string hospname { get; set; }
        public string fee_ym { get; set; }
        public string firsttreatdate { get; set; }
        public string appl_type { get; set; }
        public string case_type { get; set; }
        public string name { get; set; }
        public string func_date { get; set; }
        public string func_type { get; set; }
        public int? drug_days { get; set; }
        public string birthday { get; set; }
        public string id { get; set; }
        public string func_seq_no { get; set; }
        public string pay_type { get; set; }
        public string part_code { get; set; }
        public string icd10cm_code2 { get; set; }
        public string icd10cm_code3 { get; set; }
        public string icd10cm_code4 { get; set; }
        public string area_service { get; set; }
        public string rel_type { get; set; }
        public int? drug_dot { get; set; }
        public int? cure_dot { get; set; }
        public int? dsvc_dot { get; set; }
        public string prsn_id { get; set; }
        public string drug_prsn_id { get; set; }
        public int? part_amt { get; set; }
        public int? appl_dot { get; set; }
        public int? other_part_amt { get; set; }
        public int? order_seq_no { get; set; }
        public string order_type { get; set; }
        public int? order_drug_day { get; set; }
        public string order_code { get; set; }
        public string orderchiname { get; set; }
        public string drug_num { get; set; }
        public string drug_fre { get; set; }
        public string drug_path { get; set; }
        public decimal? order_qty { get; set; }
        public decimal? order_uprice { get; set; }
        public int? order_dot { get; set; }
        public string icd9cm_code1 { get; set; }
        public string icd9cm_code2 { get; set; }
        public string rel_mode { get; set; }

    }

    public class SamplingExamineQueryRequest
    {
        public string FeeStart { get; set; }
        public string FeeEnd { get; set; }
        public string HospID { get; set; }
    }

    public class SamplingExamineQueryData
    {
        public bool IsChecked { get; set; }
        public string fee_ym { get; set; }
        public string data_id { get; set; }
        public string samplingno { get; set; }
        public string hospid { get; set; }
        public string hospname { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public string func_date { get; set; }
        public string firsttreatdate { get; set; }
        public int? seq_no { get; set; }
        public int? drug_days { get; set; }
        public int? cure_dot { get; set; }
        public int? dsvc_dot { get; set; }
        public int? other_part_amt { get; set; }
        public int? appl_dot { get; set; }
        public int? part_amt { get; set; }
        public int? finish_amt { get; set; }
        public string reviewremark { get; set; }
        public int? reviewamt { get; set; }
        public string review { get; set; }
        public string reviewdate { get; set; }
        public string appealsremark { get; set; }
        public int? appealsamt { get; set; }
        public string appeals { get; set; }
        public string appealsdate { get; set; }
        public string chkflg { get; set; }

        public bool IsExpanded { get; set; }
        public List<SamplingExamineQueryDetailData> Details { get; set; }
    }

    public class SamplingExamineQueryDetailData
    {
        public int order_seq_no { get; set; }
        public string order_type { get; set; }
        public int? order_drug_day { get; set; }
        public string order_code { get; set; }
        public string orderchiname { get; set; }
        public string drug_num { get; set; }
        public string drug_fre { get; set; }
        public string drug_path { get; set; }
        public decimal? order_qty { get; set; }
        public decimal? order_uprice { get; set; }
        public int? order_dot { get; set; }
        public string reviewremark { get; set; }
        public int? reviewamt { get; set; }
        public string review { get; set; }
        public string reviewdate { get; set; }
        public string appealsremark { get; set; }
        public int? appealsamt { get; set; }
        public string appeals { get; set; }
        public string appealsdate { get; set; }
    }

    public class SamplingExamineReceiveData
    {
        public bool IsChecked { get; set; }
        public string samplingno { get; set; }
        public string hospid { get; set; }
        public string hospname { get; set; }
        public string fee_ym { get; set; }
        public string id { get; set; }
        public string birthday { get; set; }
        public string name { get; set; }
        public string func_date { get; set; }
        public string accessdate { get; set; }
        public string accessno { get; set; }
        public string replydate { get; set; }
        public string replyno { get; set; }
        public string data_id { get; set; }
        public string chkflg { get; set; }
    }

    public class SamplingExamineReceiveRequest
    {
        public string FeeStart { get; set; }
        public string FeeEnd { get; set; }
        public string HospID { get; set; }
        public string AccessNo { get; set; }
    }

    public class SaveSamplingExamineReceiveRequest
    {
        public bool IsAccess { get; set; }
        public bool IsReply { get; set; }

        /// <summary>
        /// 審查編號
        /// </summary>
        public string AccessNo { get; set; }
        /// <summary>
        /// 調閱收件日期
        /// </summary>
        public string AccessDate { get; set; }
        /// <summary>
        /// 申復編號
        /// </summary>
        public string ReplyNo { get; set; }
        /// <summary>
        /// 申復收件日期
        /// </summary>
        public string ReplyDate { get; set; }

        public SamplingExamineReceiveData[] Items { get; set; }
    }

    public class Rew1600ItemDto
    {
        public string fee_ym { get; set; }
        public string hospid { get; set; }
        public string hospname { get; set; }
        public string appl_type { get; set; }
        public string appl_date { get; set; }
        public int? seq_no { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string birthday { get; set; }
        public int? reviewamt { get; set; }
        public string func_date { get; set; }
        public string remark { get; set; }
        public string reviewremark { get; set; }
    }

    public class Rew1800ItemDto
    {
        public string fee_ym { get; set; }
        public string hospid { get; set; }
        public string hospname { get; set; }
        public string appl_type { get; set; }
        public string appl_date { get; set; }
        public int? seq_no { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string birthday { get; set; }
        public int? reviewamt { get; set; }
        public int? appealsamt { get; set; }
        public string func_date { get; set; }
        public string remark { get; set; }
        public string reviewremark { get; set; }
    }
}
