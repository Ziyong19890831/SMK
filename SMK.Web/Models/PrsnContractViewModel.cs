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
using SMK.Web.Helpers;
using Microsoft.AspNetCore.Http;
using SMK.Web.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SMK.Web.Models

{    /// <summary>
     /// 合約查詢model
     /// </summary>
    public class PrsnContractQueryModel : PagedRequest
    {
        [Display(Name = "身份證號")]
        public string PrsnId { get; set; }

        [Display(Name = "姓名")]
        public string PrsnName { get; set; }

        [Display(Name = "醫事人員類別")]
        public string PrsnType { get; set; }

        [Display(Name = "合約生效日")]
        public string PrsnStartDate { get; set; }

        [Display(Name = "證書類別")]
        public string LicenceType { get; set; }

        [Display(Name = "合約類別")]
        public string SmkcontractType { get; set; }

        [Display(Name = "建立日期")]
        public string StartCreateDate { get; set; }

        [Display(Name = "建立日期")]
        public string EndCreateDate { get; set; }

        [Display(Name = "醫事機構代碼")]
        public string HospId { get; set; }
        [Display(Name = "院區別")]
        public string HospSeqNo { get; set; }
        [Display(Name = "醫事機構名稱")]
        public string HospName { get; set; }

        [Display(Name = "簽約核准")]
        public bool IsApproval { get; set; }

        public IFormFile file { get; set; }

        public List<string> err { get; set; }

        ///<summary>
        ///收件年月起迄-起
        ///</summary>
        public string Application_StartCreateDate { get; set; }

        ///<summary>
        ///收件年月起迄-迄
        ///</summary>
        public string Application_EndCreateDate { get; set; }

        ///<summary>
        ///申請類型
        ///</summary>
        public string ApplicationType { get; set; }
    }

    public class JudgePrsnContractViewModel
    {
        public PrsnContractViewModel contract { get; set; }
    }
    /// <summary>
    /// 醫事人員合約審核清單
    /// </summary>
    public class PrsnContractListViewModel : PrsnContract
    {

        /// <summary>
        /// 姓名
        /// </summary>
        public string PrsnName { get; set; }
        /// <summary>
        /// 機構名稱
        /// </summary>
        public string HospName { get; set; }
        /// <summary>
        /// 合約名稱
        /// </summary>
        public string SmkcontractTypeNam { get; set; }

        /// <summary>
        /// 機構狀態
        /// </summary>
        public HospStatus? HospStatus { get; set; }

        /// <summary>
        /// 機構狀態
        /// </summary>
        public string HospStatusString =>
            this.HospStatus.HasValue ? this.HospStatus.GetEnumDescription() : string.Empty;

        /// <summary>
        /// 醫事人員類別名稱
        /// </summary>
        public string PrsnTypeNam { get; set; }

        /// <summary>
        /// 終止原因名稱
        /// </summary>
        public string EndReasonName { get; set; }

    }
    /// <summary>
    /// 醫事人員編輯合約清單
    /// </summary>
    public class PrsnContractViewModel : PrsnContract
    {
        /// <summary>
        /// 機構名稱
        /// </summary>
        public string HospName { get; set; }

        /// <summary>
        /// 終止原因名稱
        /// </summary>
        public string EndReasonName { get; set; }

        /// <summary>
        /// 合約名稱
        /// </summary>
        public string SmkcontractTypeNam { get; set; }

        /// <summary>
        /// 是否違規 HospContract.EndReasonNo=="22"
        /// </summary>
        public bool IsOffend { get; set; }
        //{
        //    get
        //    {
        //        //22	違約終止
        //        return this.HospEndReasonNo == "22";
        //    }
        //}
    }
    public class SMKToQSMSContract
    {
        public string HospId { get; set; }
        public string PrsnId { get; set; }
        public string SmkcontractType { get; set; }
        public string PrsnStartDate { get; set; }
        public string PrsnEndDate { get; set; }
        public string CreateDate { get; set; }
        public string CouldTreat { get; set; }
        public string CouldInstruct { get; set; }
    }

    public class ApplyPrsnNewVM : ApplyPrsnNew
    {
        public string Change_Type { get; set; }
        public string Note
        {
            get
            {
                string New_Note = string.Empty;
                if (string.IsNullOrWhiteSpace(this.Application_Type))
                {
                    New_Note += $"{this.SMKLogNote}";
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.SMKLogNote))
                    {
                        New_Note += $"{this.SMKLogNote}，";
                    }
                    if (!string.IsNullOrEmpty(this.Contract_Effective_Date))
                    {
                        New_Note += $"{this.Contract_Effective_Date.ToSlashTaiwanDateFromYYYYMMDD()}合約生效";
                    }
                }
                return New_Note;
            }
        }
    }

    public class ApplyPrsnChangeVM : ApplyPrsnChange
    {
        public string Change_Type { get; set; }

        public string ChangeType
        {
            get
            {
                string data_string = string.Empty;
                if (this.Change_Type == "不適用")
                {
                    data_string = "不適用";
                }
                else
                {
                    if (this.ChangeID == true)
                    {
                        data_string += "ID,";
                    }
                    if (this.ChangeName == true)
                    {
                        data_string += "姓名,";
                    }
                }
                data_string = data_string.TrimEnd(',');
                return data_string;
            }
        }

        public string Note { get; set; }


        public string new_Note
        {
            get
            {
                string data_string = string.Empty;
                if (this.ChangeID == true)
                {
                    data_string += $"原【ID】{this.ID} 異動為 {this.NewID} ,";
                }
                if (this.ChangeName == true)
                {
                    data_string += $"原【姓名】{this.Name}  異動為  {this.NewName} ,";
                }
                data_string = data_string.TrimEnd(',');
                return data_string;
            }
        }
    }

    public class ApplicationPrsnContractViewModel
    {
        public string FeeYM { get; set; }
        public string FeeYMD { get; set; }
        public string Application_Type { get; set; }
        public string Change_Type { get; set; }
        public string HospID { get; set; }
        public string HospSeqNo { get; set; }
        public string HospName { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserTitle { get; set; }
        public string UserServise { get; set; }
        public string Note { get; set; }
    }


}
