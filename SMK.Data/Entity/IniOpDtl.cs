using System;
using System.Collections.Generic;

namespace SMK.Data.Entity
{
    /// <summary>
    /// 健保門診處方及治療明細資料-原始檔
    /// </summary>
    public partial class IniOpDtl
    {
        /// <summary>
        /// 電腦序號
        /// </summary>
        public string DataId { get; set; }

        /// <summary>
        /// 費用年月
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
        /// 機構代碼
        /// </summary>
        public string HospId { get; set; }

        /// <summary>
        /// 申報日期
        /// </summary>
        public string ApplDate { get; set; }

        /// <summary>
        /// 案件類別
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
        /// 治療結束日期
        /// </summary>
        public string CureEDate { get; set; }

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
        /// 處方調劑方式
        /// </summary>
        public string RelMode { get; set; }

        /// <summary>
        /// 醫事人員身分證號
        /// </summary>
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
        public int? CureDot { get; set; }

        /// <summary>
        /// 診察費代碼
        /// </summary>
        public string DiagCode { get; set; }

        /// <summary>
        /// 診察費點數
        /// </summary>
        public int? DiagDot { get; set; }

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
        /// 性別
        /// </summary>
        public string IdSex { get; set; }

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
        /// 特定地區醫療服務
        /// </summary>
        public string AreaService { get; set; }

        /// <summary>
        /// 支援區域
        /// </summary>
        public string SuppArea { get; set; }

        /// <summary>
        /// 實際提供醫療服務之醫事服務機構代號
        /// </summary>
        public string RealHospId { get; set; }

        /// <summary>
        /// 醫事類別
        /// </summary>
        public string HospDataType { get; set; }

        /// <summary>
        /// 代辦部分負擔金額
        /// </summary>
        public decimal? AgencyPartAmt { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 補報原因註記
        /// </summary>
        public string ApplCauseMark { get; set; }

        /// <summary>
        /// 國際疾病分類碼(四)
        /// </summary>
        public string Icd10cmCode3 { get; set; }

        /// <summary>
        /// 國際疾病分類碼(五)
        /// </summary>
        public string Icd10cmCode4 { get; set; }

        /// <summary>
        /// 特殊材料明細點數小計
        /// </summary>
        public int? MetDot { get; set; }

        /// <summary>
        /// 矯正機關代號
        /// </summary>
        public string CorrHospId { get; set; }

        /// <summary>
        /// 轉檔日期
        /// </summary>
        public string TranDate { get; set; }

        /// <summary>
        /// 修正後分院號
        /// </summary>
        public string HospSeqNo { get; set; }

    }
}
