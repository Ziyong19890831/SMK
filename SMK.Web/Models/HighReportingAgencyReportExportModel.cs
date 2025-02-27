namespace SMK.Web.Models.HighReportingAgencyReportExportModel
{
        public class HighReportingAgencyByAgencyResult
        {
            public string 層級 { get; set; }
            public string 分區別 { get; set; }
            public string 院所代碼 { get; set; }
            public string 院區別 { get; set; }
            public string 院所名稱 { get; set; }
            public string 給藥人次 { get; set; }
            public string 平均每人給藥次數 { get; set; }
            public string 平均每人給藥週數 { get; set; }
            public string 衛教人次 { get; set; }
            public string 平均每人衛教次數 { get; set; }
            public string 符合定義1 { get; set; }
            public string 符合定義2 { get; set; }
            public string 符合定義3 { get; set; }
            public string 符合定義4 { get; set; }
        }
        public class HighReportingAgencyByLevelResult
        {
            public string 層級 { get; set; }
            public string 給藥人次 { get; set; }
            public string 平均每人給藥次數 { get; set; }
            public string 平均每人給藥週數 { get; set; }
            public string 衛教人次 { get; set; }
            public string 平均每人衛教次數 { get; set; }
        }
        public class HighReportingAgencyByLevelSummaryResult
        {
            public string 層級 { get; set; }
            public string 給藥人次 { get; set; }
            public string 用藥人數 { get; set; } 
            public string 給藥週數 { get; set; }
            public string 平均每人給藥次數 { get; set; }
            public string 平均每人給藥週數 { get; set; }
            public string 衛教人次 { get; set; }
            public string 衛教人數 { get; set; }
            public string 平均每人衛教次數 { get; set; }
            public string 用藥執行機構數 { get; set; }
            public string 衛教執行機構數 { get; set; }
            public string 平均每機構用藥人次 { get; set; }
            public string 平均每機構衛教人次 { get; set; }
            public string N { get; set; }
        }
        public class HighReportingAgencyBySeasonResult
        {
            public string 用藥人次 { get; set; }
            public string 用藥人數 { get; set; }
            public string 給藥週數 { get; set; }
            public string 平均每人給藥次數 { get; set; }
            public string 平均每人給藥週數 { get; set; }
            public string 衛教人次 { get; set; }
            public string 衛教人數 { get; set; }
            public string 平均每人衛教次數 { get; set; }
            public string 用藥執行機構數 { get; set; }
            public string 衛教執行機構數 { get; set; }
            public string 平均每機構用藥人次 { get; set; }
            public string 平均每機構衛教人次 { get; set; }
        }
}
