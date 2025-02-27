using Newtonsoft.Json;
using SMK.Data.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMK.Data.Entity
{
    public partial class PrsnLicence
    {
        public int Id { get; set; }
        public string PrsnId { get; set; }
        public string LicenceType { get; set; }
        public string LicenceNo { get; set; }

        /// <summary>
        /// 起始日期
        /// </summary>
        [JsonConverter(typeof(WestTwDateConverter))]
        public string CertPublicDate { get; set; }

        /// <summary>
        /// 公告日
        /// </summary>
        [JsonConverter(typeof(WestTwDateConverter))]
        public string CertStartDate { get; set; }

        /// <summary>
        /// 到期日,與舊系統LicenceEndDate同值
        /// </summary>
        [JsonConverter(typeof(WestTwDateConverter))]
        public string CertEndDate { get; set; }

        public string Remark { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("Getdate()")]
        [Display(Name = "建立時間")]
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "更新時間")]
        [Column("UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [MaxLength(127)]
        [Display(Name = "更新人員")]
        [Column("UpdatedBy")]
        public string UpdatedBy { get; set; }
    }
}
