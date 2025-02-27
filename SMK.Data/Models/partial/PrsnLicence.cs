

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMK.Data.Attributes;

namespace SMK.Data.Entity
{
    [ModelMetadataType(typeof(PrsnLicenceMetaData))]
    public partial class PrsnLicence
    {
        public class PrsnLicenceMetaData
        {
            [Display(Name = "流水號")]
            public int Id { get; set; }
            [Display(Name = "身份證號")]
            public string PrsnId { get; set; }
            [Display(Name = "證書類型")]
            public string LicenceType { get; set; }
            [Display(Name = "證書證號")]
            public string LicenceNo { get; set; }

            /// <summary>
            /// 起始日期
            /// </summary>
            [Display(Name = "起始日期")]
            public string CertPublicDate { get; set; }

            /// <summary>
            /// 公告日
            /// </summary>
            [Display(Name = "公告日")]
            public string CertStartDate { get; set; }

            /// <summary>
            /// 到期日,與舊系統LicenceEndDate同值
            /// </summary>
            [Display(Name = "證書有效期限")]
            public string CertEndDate { get; set; }

            public string Remark { get; set; }

            [Display(Name = "建立時間")]
            public DateTime CreatedAt { get; set; } 

            [Display(Name = "更新時間")]
            public DateTime? UpdatedAt { get; set; }

            [Display(Name = "更新人員")]
            public string UpdatedBy { get; set; }
        }
    }
}
