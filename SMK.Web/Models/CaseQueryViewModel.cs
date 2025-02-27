using System;

namespace SMK.Web.Models
{
    public class CaseQueryViewModel : IComparable<CaseQueryViewModel>
    {
        public string DataType { get; set; }
        public string DataId { get; set; }
        public string HospId { get; set; }
        /// <summary>
        /// 機構代碼
        /// </summary>

        public string FeeYm { get; set; }
        /// <summary>
        /// 療程年度
        /// </summary>

        public string ExamYear { get; set; }
        /// <summary>
        /// 療程次數
        /// </summary>

        public int? ExamTime { get; set; }
        /// <summary>
        /// 初診日
        /// </summary>

        public string FirstTreatDate { get; set; }
        /// <summary>
        /// 週數
        /// </summary>

        public int? WeekCount { get; set; }
        /// <summary>
        /// 衛教療程年度
        /// </summary>

        public string InstructExamYear { get; set; }
        /// <summary>
        /// 衛教療程次數
        /// </summary>

        public int? InstructExamTime { get; set; }
        /// <summary>
        /// 衛教初診日
        /// </summary>

        public string FirstInstructDate { get; set; }
        /// <summary>
        /// 衛教序次
        /// </summary>

        public int? InctructSerial { get; set; }
        /// <summary>
        /// 藥物申報
        /// </summary>

        public string MedApply { get; set; }
        /// <summary>
        /// 衛教申報
        /// </summary>

        public string InstructApply { get; set; }
        /// <summary>
        /// 追蹤申報
        /// </summary>

        public string TraceApply { get; set; }
        /// <summary>
        /// 釋出申報
        /// </summary>

        public string ReleaseApply { get; set; }
        /// <summary>
        /// 申報類別
        /// </summary>

        public string ApplType { get; set; }
        /// <summary>
        /// 申報日期
        /// </summary>

        public string ApplDate { get; set; }
        /// <summary>
        /// 案件分類
        /// </summary>

        public string CaseType { get; set; }
        /// <summary>
        /// 流水號
        /// </summary>

        public int? SeqNo { get; set; }
        /// <summary>
        /// 就醫科別
        /// </summary>

        public string FuncType { get; set; }
        /// <summary>
        /// 就醫日期
        /// </summary>

        public string FuncDate { get; set; }
        /// <summary>
        /// 調劑日期
        /// </summary>

        public string RelDate { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>

        public string Birthday { get; set; }
        /// <summary>
        /// 身分證號
        /// </summary>

        public string Id { get; set; }
        /// <summary>
        /// 就醫序號
        /// </summary>

        public string FuncSeqNo { get; set; }
        /// <summary>
        /// 給付類別
        /// </summary>

        public string PayType { get; set; }
        /// <summary>
        /// 部分負擔代號
        /// </summary>

        public string PartCode { get; set; }
        /// <summary>
        /// 主診斷代碼
        /// </summary>

        public string Icd9cmCode { get; set; }
        /// <summary>
        /// 次診斷代碼(一)
        /// </summary>

        public string Icd9cmCode1 { get; set; }
        /// <summary>
        /// 次診斷代碼(二)
        /// </summary>

        public string Icd9cmCode2 { get; set; }
        /// <summary>
        /// 給藥日份
        /// </summary>

        public int? DrugDays { get; set; }
        /// <summary>
        /// 醫事人員身分證號
        /// </summary>

        public string RelMode { get; set; }
        public string PrsnId { get; set; }
        /// <summary>
        /// 藥師身分證號
        /// </summary>

        public string DrugPrsnId { get; set; }
        /// <summary>
        /// 藥費點數
        /// </summary>

        public int? DrugDot { get; set; }
        /// <summary>
        /// 診療費點數
        /// </summary>

        public string DiagCode { get; set; }
        public int? DiagDot { get; set; }
        public int? CureDot { get; set; }
        /// <summary>
        /// 藥事服務費代碼
        /// </summary>

        public string DsvcCode { get; set; }
        /// <summary>
        /// 藥事服務費點數
        /// </summary>

        public int? DsvcDot { get; set; }
        /// <summary>
        /// 醫療費用點數
        /// </summary>

        public int? ExpDot { get; set; }
        /// <summary>
        /// 部份負擔金額
        /// </summary>

        public int? PartAmt { get; set; }
        /// <summary>
        /// 申請金額
        /// </summary>

        public int? ApplDot { get; set; }
        /// <summary>
        /// 原處方醫療機構代碼
        /// </summary>

        public string OrigHospId { get; set; }
        /// <summary>
        /// 性別
        /// </summary>

        public string IdSex { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 特定治療項目(一)
        /// </summary>

        public string CureItem1 { get; set; }
        /// <summary>
        /// 特定治療項目(二)
        /// </summary>

        public string CureItem2 { get; set; }
        /// <summary>
        /// 特定治療項目(三)
        /// </summary>

        public string CureItem3 { get; set; }
        /// <summary>
        /// 特定治療項目(四)
        /// </summary>

        public string CureItem4 { get; set; }
        /// <summary>
        /// 原案件分類
        /// </summary>

        public string OrigCaseType { get; set; }
        /// <summary>
        /// 行政協助項目部分負擔點數
        /// </summary>

        public int? OtherPartAmt { get; set; }
        /// <summary>
        /// 補報原因註記
        /// </summary>

        public string ApplCauseMark { get; set; }
        /// <summary>
        /// 國際疾病分類碼(三)
        /// </summary>

        public string Icd10cmCode2 { get; set; }
        /// <summary>
        /// 國際疾病分類碼(四)
        /// </summary>

        public string Icd10cmCode3 { get; set; }
        /// <summary>
        /// 國際疾病分類碼(五)
        /// </summary>

        public string Icd10cmCode4 { get; set; }
        /// <summary>
        /// 矯正機關代號
        /// </summary>

        public string CorrHospId { get; set; }
        public string AreaService { get; set; }
        public string SuppArea { get; set; }
        public string RealHospId { get; set; }
        public string HospDataType { get; set; }
        public decimal? AgencyPartAmt { get; set; }
        public string TranDate { get; set; }
        public string Name { get; set; }
        public int? MetDot { get; set; }
        public string data_id { get; set; }


        // iniOpDtl
        // sql += " select data_id,fee_ym,ExamYear,ExamTime,FirstTreatDate,WeekCount,InstructExamYear,InstructExamTime,FirstInstructDate,"
        // sql += " InctructSerial,MedApply,InstructApply,TraceApply,ReleaseApply,appl_type,HospID,appl_date,case_type,seq_no,func_type,func_date,"
        // sql += " cure_e_date,birthday,id,func_seq_no,pay_type,part_code,icd9cm_code,icd9cm_code1,icd9cm_code2,drug_days,rel_mode,prsn_id,"
        // sql += " drug_prsn_id,drug_dot,cure_dot,diag_code,diag_dot,dsvc_code,dsvc_dot,exp_dot,part_amt,appl_dot,Id_Sex,'' as Remark,"
        // sql += " cure_item1,cure_item2,cure_item3,cure_item4,'' as orig_hosp_id,area_service,supp_area,real_hosp_id,"
        // sql += " hosp_data_type,'' as orig_case_type,agency_part_amt,0 as other_part_amt,name,appl_cause_mark,'' as icd10cm_code2,icd10cm_code3,icd10cm_code4,met_dot,corr_hosp_id,tran_date"
        //
        //                             dtiniOpDtl.Rows.Add(New Object() {"", "1", dr.Item("data_id").ToString.Trim(), dr.Item("fee_ym").ToString.Trim(), dr.Item("ExamYear").ToString.Trim(), _
        //                                                       dr.Item("ExamTime").ToString.Trim(), DBtoDate(dr.Item("FirstTreatDate").ToString.Trim()), dr.Item("WeekCount").ToString.Trim(), _
        //                                                       dr.Item("InstructExamYear").ToString.Trim(), dr.Item("InstructExamTime").ToString.Trim(), DBtoDate(dr.Item("FirstInstructDate").ToString.Trim()), dr.Item("InctructSerial").ToString.Trim(), _
        //                                                       setFlg(dr.Item("MedApply").ToString.Trim()), setFlg(dr.Item("InstructApply").ToString.Trim()), dr.Item("TraceApply").ToString.Trim(), setFlg(dr.Item("ReleaseApply").ToString.Trim()), _
        //                                                       dr.Item("appl_type").ToString.Trim(), dr.Item("HospID").ToString.Trim(), DBtoDate(dr.Item("appl_date").ToString.Trim()), dr.Item("case_type").ToString.Trim(), dr.Item("seq_no").ToString.Trim(), _
        //                                                       dr.Item("func_type").ToString.Trim(), DBtoDate(dr.Item("func_date").ToString.Trim()), "", DBtoDate(dr.Item("birthday").ToString.Trim()), _
        //                                                       dr.Item("id").ToString.Trim(), dr.Item("func_seq_no").ToString.Trim(), dr.Item("pay_type").ToString.Trim(), dr.Item("part_code").ToString.Trim(), _
        //                                                       dr.Item("icd9cm_code").ToString.Trim(), dr.Item("icd9cm_code1").ToString.Trim(), dr.Item("icd9cm_code2").ToString.Trim(), dr.Item("drug_days").ToString.Trim(), _
        //                                                       dr.Item("rel_mode").ToString.Trim(), dr.Item("prsn_id").ToString.Trim(), dr.Item("drug_prsn_id").ToString.Trim(), dr.Item("drug_dot").ToString.Trim(), _
        //                                                       dr.Item("cure_dot").ToString.Trim(), dr.Item("diag_code").ToString.Trim(), dr.Item("diag_dot").ToString.Trim(), dr.Item("dsvc_code").ToString.Trim(), _
        //                                                       dr.Item("dsvc_dot").ToString.Trim(), dr.Item("exp_dot").ToString.Trim(), dr.Item("part_amt").ToString.Trim(), dr.Item("appl_dot").ToString.Trim(), dr.Item("Id_Sex").ToString.Trim(), _
        //                                                       dr.Item("Remark").ToString.Trim(), dr.Item("cure_item1").ToString.Trim(), dr.Item("cure_item2").ToString.Trim(), dr.Item("cure_item3").ToString.Trim(), dr.Item("cure_item4").ToString.Trim(), _
        //                                                       dr.Item("orig_hosp_id").ToString.Trim(), dr.Item("area_service").ToString.Trim(), dr.Item("supp_area").ToString.Trim(), dr.Item("real_hosp_id").ToString.Trim(), _
        //                                                       dr.Item("hosp_data_type").ToString.Trim(), dr.Item("orig_case_type").ToString.Trim(), dr.Item("agency_part_amt").ToString.Trim(), dr.Item("other_part_amt").ToString.Trim(), dr.Item("name").ToString.Trim(), _
        //                                                       dr.Item("appl_cause_mark").ToString.Trim(), dr.Item("icd10cm_code2").ToString.Trim(), dr.Item("icd10cm_code3").ToString.Trim(), dr.Item("icd10cm_code4").ToString.Trim(), dr.Item("met_dot").ToString.Trim(), _
        //                                                       dr.Item("corr_hosp_id").ToString.Trim(), DBtoDate(dr.Item("tran_date").ToString.Trim()), dr.Item("data_id").ToString.Trim() & dr.Item("fee_ym").ToString.Trim()})
                                    
                                    

        // iniDrDtl
        // sql = "select iniDrDtl.* from ("
        // sql += " select data_id,HospID,fee_ym,ExamYear,ExamTime,FirstTreatDate,WeekCount,InstructExamYear,InstructExamTime,FirstInstructDate,"
        // sql += " InctructSerial,MedApply,InstructApply,TraceApply,ReleaseApply,appl_type,appl_date,case_type,seq_no,func_type,func_date,"
        // sql += " rel_date,birthday,id,func_seq_no,pay_type,part_code,icd9cm_code,icd9cm_code1,icd9cm_code2,drug_days,prsn_id,drug_prsn_id,drug_dot,"
        // sql += " cure_dot,dsvc_code,dsvc_dot,exp_dot,part_amt,appl_dot,Id_Sex,'' as Remark,cure_item1,cure_item2,cure_item3,cure_item4,orig_hosp_id,area_service,'' as supp_area,'' as real_hosp_id,"
        // sql += " '' as hosp_data_type,orig_case_type,0 as agency_part_amt,other_part_amt,name,appl_cause_mark,icd10cm_code2,icd10cm_code3,icd10cm_code4,0 as met_dot,corr_hosp_id,tran_date"
        //     
        //
        //                             dtiniOpDtl.Rows.Add(New Object() {"", "2", dr2.Item("data_id").ToString.Trim(), dr2.Item("fee_ym").ToString.Trim(), dr2.Item("ExamYear").ToString.Trim(), _
        //                                                               dr2.Item("ExamTime").ToString.Trim(), DBtoDate(dr2.Item("FirstTreatDate").ToString.Trim()), dr2.Item("WeekCount").ToString.Trim(), _
        //                                                               dr2.Item("InstructExamYear").ToString.Trim(), dr2.Item("InstructExamTime").ToString.Trim(), DBtoDate(dr2.Item("FirstInstructDate").ToString.Trim()), dr2.Item("InctructSerial").ToString.Trim(), _
        //                                                               setFlg(dr2.Item("MedApply").ToString.Trim()), setFlg(dr2.Item("InstructApply").ToString.Trim()), dr2.Item("TraceApply").ToString.Trim(), setFlg(dr2.Item("ReleaseApply").ToString.Trim()), _
        //                                                               dr2.Item("appl_type").ToString.Trim(), dr2.Item("HospID").ToString.Trim(), DBtoDate(dr2.Item("appl_date").ToString.Trim()), dr2.Item("case_type").ToString.Trim(), dr2.Item("seq_no").ToString.Trim(), _
        //                                                               dr2.Item("func_type").ToString.Trim(), DBtoDate(dr2.Item("func_date").ToString.Trim()), DBtoDate(dr2.Item("rel_date").ToString.Trim()), DBtoDate(dr2.Item("birthday").ToString.Trim()), _
        //                                                               dr2.Item("id").ToString.Trim(), dr2.Item("func_seq_no").ToString.Trim(), dr2.Item("pay_type").ToString.Trim(), dr2.Item("part_code").ToString.Trim(), _
        //                                                               dr2.Item("icd9cm_code").ToString.Trim(), dr2.Item("icd9cm_code1").ToString.Trim(), dr2.Item("icd9cm_code2").ToString.Trim(), dr2.Item("drug_days").ToString.Trim(), _
        //                                                               "", dr2.Item("prsn_id").ToString.Trim(), dr2.Item("drug_prsn_id").ToString.Trim(), dr2.Item("drug_dot").ToString.Trim(), _
        //                                                               dr2.Item("cure_dot").ToString.Trim(), "", "", dr2.Item("dsvc_code").ToString.Trim(), _
        //                                                               dr2.Item("dsvc_dot").ToString.Trim(), dr2.Item("exp_dot").ToString.Trim(), dr2.Item("part_amt").ToString.Trim(), dr2.Item("appl_dot").ToString.Trim(), dr2.Item("Id_Sex").ToString.Trim(), dr2.Item("Remark").ToString.Trim(), _
        //                                                               dr2.Item("cure_item1").ToString.Trim(), dr2.Item("cure_item2").ToString.Trim(), dr2.Item("cure_item3").ToString.Trim(), dr2.Item("cure_item4").ToString.Trim(), dr2.Item("orig_hosp_id").ToString.Trim(), _
        //                                                               dr2.Item("area_service").ToString.Trim(), dr2.Item("supp_area").ToString.Trim(), dr2.Item("real_hosp_id").ToString.Trim(), _
        //                                                               dr2.Item("hosp_data_type").ToString.Trim(), dr2.Item("orig_case_type").ToString.Trim(), dr2.Item("agency_part_amt").ToString.Trim(), dr2.Item("other_part_amt").ToString.Trim(), dr2.Item("name").ToString.Trim(), _
        //                                                               dr2.Item("appl_cause_mark").ToString.Trim(), dr2.Item("icd10cm_code2").ToString.Trim(), dr2.Item("icd10cm_code3").ToString.Trim(), dr2.Item("icd10cm_code4").ToString.Trim(), dr2.Item("met_dot").ToString.Trim(), _
        //                                                               dr2.Item("corr_hosp_id").ToString.Trim(), DBtoDate(dr2.Item("tran_date")).ToString.Trim(), dr2.Item("data_id").ToString.Trim() & dr2.Item("fee_ym").ToString.Trim()})


        public int CompareTo(CaseQueryViewModel other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var dataTypeComparison = string.Compare(DataType, other.DataType, StringComparison.Ordinal);
            if (dataTypeComparison != 0) return dataTypeComparison;
            return string.Compare(FuncDate, other.FuncDate, StringComparison.Ordinal);
        }
    }
}