using SMK.Data.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.Models
{
    public class RegularMonthlyReportQueryModel : PagedRequest
    {
        [DisplayName("合約最後日期")]
        public string ED1 { get; set; }
        [DisplayName("健保檔最後日期")]
        public string EX { get; set; }
        [DisplayName("合約資料第一天")]
        [StringLength(8, ErrorMessage = "只能填寫yyyyMMdd")]
        [Required(ErrorMessage = "必填")]
        public string YSTART { get; set; }
        [DisplayName("合約資料最後一天")]
        [StringLength(8, ErrorMessage = "只能填寫yyyyMMdd")]
        [Required(ErrorMessage = "必填")]
        public string YEND { get; set; }
        [DisplayName("年度")]
        [StringLength(4, ErrorMessage = "只能填寫yyyyMMdd")]
        [Required(ErrorMessage = "必填")]
        public string YY { get; set; }
        [DisplayName("健保資料最後一天")]
        [StringLength(8, ErrorMessage = "只能填寫yyyyMMdd")]
        [Required(ErrorMessage = "必填")]
        public string YYE { get; set; }

        [DisplayName("健保費用年月起")]
        [StringLength(6, ErrorMessage = "只能填寫yyyyMM")]
        [Required(ErrorMessage = "必填")]
        public string YSTART1 { get; set; }
        [DisplayName("健保費用年月迄")]
        [StringLength(6, ErrorMessage = "只能填寫yyyyMM")]
        [Required(ErrorMessage = "必填")]
        public string YEND1 { get; set; }
    }
    public class ExportTotalTableResult
    {
        public string 年度 { get; set; }
        public string 總計 { get; set; }
        public Int32 合約機構數_年底 { get; set; }
        public Int32 執行機構數 { get; set; }
        public Int32 合約人員數_年底 { get; set; }
        public Int32 合約人員數_年度 { get; set; }
        public Int32 執行人員數_年度 { get; set; }
        public Int32 人次 { get; set; }
        public Int32 用藥人次 { get; set; }
        public Int32 衛教人次 { get; set; }
        public Int32 人數 { get; set; }
        public Int32 用藥人數 { get; set; }
        public Int32 衛教人數 { get; set; }
        public Int32 用藥_衛教人數 { get; set; }
        public Int32 給藥週數 { get; set; }

    }
    public class ExportCategoryList
    {
        public string N { get; set; }
        public string 類別 { get; set; }
        public string 合約機構數_年底 { get; set; }
        public string 合約機構數_年度 { get; set; }
        public string 執行機構數 { get; set; }
        public string 合約人員數_年底 { get; set; }
        public string 合約人員數_年度 { get; set; }
        public string 執行人員數 { get; set; }
        public Int32 用藥人次 { get; set; }
        public Int32 衛教人次 { get; set; }
        public Int32 申報人次 { get; set; }
        public Int32 用藥人數 { get; set; }
        public Int32 衛教人數 { get; set; }
        public Int32 用藥_衛教人數 { get; set; }
        public Int32 申報人數 { get; set; }
        public double 平均每人給藥次數 { get; set; }
        public double 平均每人衛教次數 { get; set; }
        public Int32 用藥週數 { get; set; }
        public double 平均每人用藥週數 { get; set; }
        public Int32 申報金額 { get; set; }
    }
    public class ExportCategoryListContractFileResult
    {
        public string N { get; set; }
        public string 類別 { get; set; }
        public string 合約機構數_年底 { get; set; }
        public string 合約機構數_年度 { get; set; }
        public string 執行機構數 { get; set; }
        public string 合約人員數_年底 { get; set; }
        public string 合約人員數_年度 { get; set; }
        public string 執行人員數 { get; set; }

    }
    public class ExportCategoryListHealthInsuranceFileResult
    {
        public string N { get; set; }
        public string 類別 { get; set; }
        public Int32 用藥人次 { get; set; }
        public Int32 衛教人次 { get; set; }
        public Int32 申報人次 { get; set; }
        public Int32 用藥人數 { get; set; }
        public Int32 衛教人數 { get; set; }
        public Int32 用藥_衛教人數 { get; set; }
        public Int32 申報人數 { get; set; }
        public Int32 用藥週數 { get; set; }
        public Int32 申報金額 { get; set; }
    }
}
