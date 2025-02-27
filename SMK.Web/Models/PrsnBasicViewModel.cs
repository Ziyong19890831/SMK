using SMK.Data.Attributes;
using SMK.Data.Dto;
using SMK.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using SMK.Data.Enums;

namespace SMK.Web.Models
{
    public class PrsnBasicQueryModel : PagedRequest
    {
        [Display(Name = "醫事機構代號")]
        public string HospId { get; set; }
        [Display(Name = "院區別")]
        public string HospSeqNo { get; set; }
        [Display(Name = "醫事機構名稱")]
        public string HospName { get; set; }
        [Display(Name = "機構狀態")]
        public string HospStatus { get; set; }
        [Display(Name = "人員合約起日")]
        public DateTime? PrsnStartDate { get; set; }
        [Display(Name = "人員合約迄日")]
        public DateTime? PrsnEndDate { get; set; }
        [Display(Name = "醫事人員類別")]
        public string PrsnType { get; set; }
        [Display(Name = "身份證號")]
        public string PrsnId { get; set; }
        [Display(Name = "姓名")]
        public string PrsnName { get; set; }
        [Display(Name ="可給藥")]
        public bool CouldTreat { get; set; }
        [Display(Name ="可衛教")]
        public bool CouldInstruct { get; set; }
    }

    public class PrsnBasicViewModel : PrsnBasic
    {
        public PrsnBasicViewModel()
        {

        }
        public static PrsnBasicViewModel GetPrsnBasicViewModel(PrsnBasic prsnBasic)
        {
            return new PrsnBasicViewModel()
            {
                PrsnId = prsnBasic.PrsnId,
                PrsnName = prsnBasic.PrsnName,
                PrsnBirthday = prsnBasic.PrsnBirthday,
                PrsnType = prsnBasic.PrsnType,
                MajorSpecialistNo = prsnBasic.MajorSpecialistNo,
                SubSpecialistNo = prsnBasic.SubSpecialistNo,
                Pemail = prsnBasic.Pemail,
                Remark = prsnBasic.Remark
            };
        }

        public string PrsnTypeName { get; set; }

        public string MajorSpecialistName { get; set; }

        public string SubSpecialistName { get; set; }

        /// <summary>
        /// 醫事人員合約醫院資料
        /// </summary>
        public List<PrsnContractViewModel> prsnContracts { get; set; }

        /// <summary>
        /// 證書清單
        /// </summary>
        public List<PrsnLicenceViewModel> prsnLicences { get; set; }
        
        /// <summary>
        /// Email
        /// </summary>
        public string PrsnEmails { get; set; }
    }

    /// <summary>
    /// 報表viewModel
    /// </summary>
    public class PrsnContactReportViewModel : PrsnContactReport
    {
        ///// <summary>
        ///// 合約起始日
        ///// </summary>
        //public string PrsnStartDate { get; set; }
        ///// <summary>
        ///// 合約終止日
        ///// </summary>
        //public string PrsnEndDate { get; set; }

        //public string PrsnTypeName { get; set; }

        //public string MajorSpecialistName { get; set; }

        //public string SubSpecialistName { get; set; }
    }
}
