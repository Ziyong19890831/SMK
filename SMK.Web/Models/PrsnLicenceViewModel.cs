using SMK.Data.Attributes;
using SMK.Data.Dto;
using SMK.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SMK.Web.Models
{
    public class PrsnLicenceViewModel : PrsnLicence
    {
        /// <summary>
        /// 證書類型
        /// </summary>
        public string LicenceName { get; set; }
    }
}
