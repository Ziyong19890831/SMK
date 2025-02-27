using SMK.Data.Dto;
using SMK.Data.Entity;
using System;
using System.ComponentModel;

namespace SMK.Web.Models
{
    /// <summary>
    /// 服務人次查詢model
    /// </summary>
    public class ServiceReportQueryModel : PagedRequest
    {
        [DisplayName("醫事機構")]
        public string HospId { get; set; }
        public string HospSeqNo { get; set; }
        [DisplayName("醫事機構名稱")]
        public string HospName { get; set; }

        [DisplayName("年度")]
        public string Year { get; set; }    
        
        [DisplayName("達成率(%)")]
        public double? Rate { get; set; }
    }

    /// <summary>
    /// 服務人次查詢結果
    /// </summary>
    public class ServiceReportViewModel : GenHospCont
    {
        /// <summary>
        /// 機構名稱
        /// </summary>
        public string HospName { get; set; }

        /// <summary>
        /// 用藥人次天花板
        /// </summary>
        public int TopTreatCount
        {
            //get
            //{
            //    return IsDefaultCount ? (IsTopTreatCount ? QualityImproveCount : QualityDefaultCount) : 0;
            //}
            get; set;
        }

        /// <summary>
        /// 衛教人次天花板
        /// </summary>
        public int TopInstructCount
        {
            //get
            //{
            //    return IsDefaultCount ? (IsTopInstructCount ? QualityImproveCount : QualityDefaultCount) : 0;
            //}
            get; set;
        }

        /// <summary>
        /// 用藥人次
        /// </summary>
        public int TreatCount { get; set; }

        /// <summary>
        /// 衛教人次
        /// </summary>
        public int InstructCount { get; set; }

        public string HospID{get;set;}

        public string HospSeqNo {get;set;}

        /// <summary>
        /// 是否有主約
        /// </summary>
        public bool IsDefaultCount { get; set; } = false;

        /// <summary>
        /// 是否有治療品質改善合約
        /// </summary>
        public bool IsTopTreatCount { get; set; } = false;

        /// <summary>
        /// 是否有衛教品質改善合約
        /// </summary>
        public bool IsTopInstructCount { get; set; } = false;

        /// <summary>
        /// 查詢年
        /// </summary>
        public string Year { get; set; }
        public string HospStatus { get; set; }
        /// <summary>
        /// 實際治療人次
        /// </summary>
        public int TreatReal { get; set; }
        /// <summary>
        /// 實際衛教人次
        /// </summary>
        public int InstructReal { get; set; }
        /// <summary>
        /// 治療服務達成率(%)
        /// </summary>
        public double TreatSussueRate { get; set; }
        /// <summary>
        /// 衛教服務達成率(%)
        /// </summary>
        public double InstructSussueRate { get; set; }
    }
}
