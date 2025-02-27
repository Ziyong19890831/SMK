using Newtonsoft.Json;
using SMK.Data.Entity;
using System;

namespace SMK.Web.Models
{
    /// <summary>
    /// 整合QuickSmoking
    /// </summary>
    public class QuitSmokingViewModel
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [JsonProperty("PName")]
        public string PName { get; set; }

        /// <summary>
        /// 身分證號
        /// </summary>
        [JsonProperty("PersonID")]
        public string PersonId { get; set; }

        /// <summary>
        /// 醫事人員類型PrsnTyp
        /// </summary>
        [JsonProperty("RoleName")]
        public string RoleName { get; set; }

        /// <summary>
        /// 醫事人員類型代碼
        /// PrsnTyp
        /// </summary>
        [JsonProperty("RoleSNO")]
        public int RoleSno { get; set; }

        /// <summary>
        /// 證書類型 PrsnLicence
        /// </summary>
        [JsonProperty("CTypeClass")]
        public string CTypeClass { get; set; }

        /// <summary>
        /// 證書類型 PrsnLicence
        /// </summary>
        [JsonProperty("CTypeSNO")]
        public int CTypeSno { get; set; }

        /// <summary>
        /// 戒菸證書-醫師
        /// 證書名稱
        /// </summary>
        [JsonProperty("CTypeName")]
        public string CTypeName { get; set; }

        /// <summary>
        /// 證書號碼
        /// </summary>
        [JsonProperty("CertID")]
        public string CertId { get; set; }

        /// <summary>
        /// 起始日期
        /// </summary>
        [JsonProperty("CertPublicDate")]
        public DateTime CertPublicDate { get; set; }

        /// <summary>
        /// 公告日
        /// </summary>
        [JsonProperty("CertStartDate")]
        public DateTime CertStartDate { get; set; }

        /// <summary>
        /// 到期日
        /// </summary>
        [JsonProperty("CertEndDate")]
        public DateTime CertEndDate { get; set; }

        /// <summary>
        /// 證書號碼字串(基礎戒菸證字第002777號)
        /// </summary>
        [JsonProperty("CTypeString")]
        public string CTypeString { get; set; }
    }
}
