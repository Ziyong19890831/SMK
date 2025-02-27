using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMK.Data.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace SMK.Web.Models
{
    public class SamplingViewModel
    {
        public SamplingQueryModel Query { get; set; }
        public List<SamplingItemDto> SamplingItems { get; set; } = new List<SamplingItemDto>();
    }
    public class SamplingExportViewModel : PagedRequest
    {
        [DisplayName("抽樣年月")]
        [Required(ErrorMessage = "請填寫 {0}")]
        public string year { get; set; }
        
        public IFormFile file { get; set; }
        
        public List<string> err { get; set; }
        public SamplingExportViewModel()
        {
            year = (DateTime.Now.Year - 1911).ToString();
        }
    }
    /// <summary>
    /// 抽樣作業查詢
    /// </summary>
    public class SamplingQueryModel : PagedRequest
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
        /// 機構比率
        /// </summary>
        [DisplayName("機構比率")]
        public int HospRatio { get; set; }

        /// <summary>
        /// 依比率當月申報金額最高之醫事機構
        /// </summary>
        [DisplayName("依比率當月申報金額最高之醫事機構")]
        public bool ChkCondition1 { get; set; }

        /// <summary>
        /// 依比率當月申報件數最高之醫事機構
        /// </summary>
        [DisplayName("依比率當月申報件數最高之醫事機構")]
        public bool ChkCondition2 { get; set; }

        /// <summary>
        /// 依匯入的醫事機構
        /// </summary>
        [DisplayName("依匯入的醫事機構")]
        public bool ChkCondition3 { get; set; }

        /// <summary>
        /// 單次藥費金額大於5000，小於100
        /// </summary>
        [DisplayName("單次藥費金額大於5000，小於100")]
        public bool ChkCondition4 { get; set; }

        /// <summary>
        /// 抽樣比率
        /// </summary>
        [DisplayName("抽樣比率")]
        public int SamplingRatio { get; set; }

    }

    public class SamplingItemDto
    {
        /// <summary>
        /// 電腦序號//電腦序號dr.Item("data_id")
        /// </summary>
        [DisplayName("電腦序號")]
        public string DataId { get; set; }

        /// <summary>
        /// 機構代碼//dr.Item("HospID").ToString.Trim()
        /// </summary>
        [DisplayName("機構代碼")]
        public string HospID { get; set; }

        /// <summary>
        /// 身分證號//dr.Item("id").ToString.Trim()
        /// </summary>
        [DisplayName("身分證號")]
        public string Id { get; set; }

        /// <summary>
        /// 出生日期//DBtoDate(dr.Item("birthday").ToString.Trim())
        /// </summary>
        [DisplayName("出生日期")]
        public string Birthday { get; set; }

        /// <summary>
        /// 姓名    //dr.Item("name").ToString.Trim()
        /// </summary>
        [DisplayName("姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 療程年度//dr.Item("ExamYear").ToString.Trim()
        /// </summary>
        [DisplayName("療程年度")]
        public string ExamYear { get; set; }

        /// <summary>
        /// 療程次數//dr.Item("ExamTime").ToString.Trim()
        /// </summary>
        [DisplayName("療程次數")]
        public int? ExamTime { get; set; }

        /// <summary>
        /// 初診日  //DBtoDate(dr.Item("FirstTreatDate").ToString.Trim())
        /// </summary>
        [DisplayName("初診日")]
        public string FirstTreatDate { get; set; }

        /// <summary>
        /// 週數    //dr.Item("WeekCount").ToString.Trim()
        /// </summary>
        [DisplayName("週數")]
        public int? WeekCount { get; set; }

        /// <summary>
        /// 就醫日期 //DBtoDate(dr.Item("func_date").ToString.Trim())
        /// </summary>
        [DisplayName("就醫日期")]
        public string FuncDate { get; set; }

        /// <summary>
        /// 給藥日份 // dr.Item("drug_days").ToString.Trim()
        /// </summary>
        [DisplayName("給藥日份")]
        public int? DrugDays { get; set; }

        /// <summary>
        /// 申請金額 // dr.Item("appl_dot").ToString.Trim()
        /// </summary>
        [DisplayName("申請金額")]
        public int? ApplDot { get; set; }

        /// <summary>
        /// 抽樣編號 // wkSamplingNo
        /// </summary>
        [DisplayName("抽樣編號")]
        public string SamplingNo { get; set; }

        /// <summary>
        /// 費用年月//FeeYearMonth
        /// </summary>
        [DisplayName("費用年月")]
        public string FeeYearMonth { get; set; }

    }

    public class Rew1500ViewModel
    {
        public string FileName { get; set; }
        public Stream Stream { get; set; }
    }

    public class Rew1500ItemDto
    {
        public string BranchName { get; set; }
        public string HospId { get; set; }
        public string HospName { get; set; }
        public string OHospName { get; set; }
        public string ApplType { get; set; }
        public string CaseType { get; set; }
        public string ApplDate { get; set; }
        public string FeeYM { get; set; }
        public int? SeqNo { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Birthday { get; set; }
        public string FuncDate { get; set; }
        public int? ApplDot { get; set; }
    }


}
