using Newtonsoft.Json;
using SMK.Data.Attributes;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Data.Utility;
using SMK.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SMK.Web.Models
{
    /// <summary>
    /// 合約查詢model
    /// </summary>
    public class HospContractQueryModel : PagedRequest
    {
        ///<summary>
        ///
        ///</summary>
        [DisplayName("醫事機構代碼")]
        public string HospID { get; set; }

        public string HospSeqNo { get; set; }

        ///<summary>
        ///醫院名稱
        ///</summary>
        [DisplayName("醫事機構名稱")]
        public string HospName { get; set; }

        ///<summary>
        ///機構狀態
        ///</summary>
        [DisplayName("機構狀態")]
        public string HospStatus { get; set; }

        /// <summary>
        /// 合約類別
        /// </summary>
        [DisplayName("合約類別")]
        public string SMKContractType { get; set; }

        ///<summary>
        ///合約狀態
        ///HospContract.SMKContractType = '01'//主約
        ///HospContract.SMKContractType in ('02','03')//品質改善合約
        ///</summary>
        [DisplayName("合約狀態")]
        public string ContractStatus { get; set; }

        ///<summary>
        ///機構合約起日
        ///</summary>
        [DisplayName("機構合約起日")]
        public string HospStartDate { get; set; }
        /// <summary>
        /// 機構合約迄日
        /// </summary>
        [DisplayName("機構合約迄日")]
        public string HospEndDate { get; set; }

        ///<summary>
        ///最新契約迄日
        ///</summary>
        [DisplayName("最新契約迄日")]
        public string HospNewEndDate { get; set; }

        ///<summary>
        ///合約生效日期
        ///</summary>


        ///<summary>
        ///建立日期-起
        ///</summary>
        public string StartCreateDate { get; set; }

        ///<summary>
        ///建立日期-迄
        ///</summary>
        public string EndCreateDate { get; set; }

        /// <summary>
        /// 查詢違約機構，EndReasonNo=22
        /// </summary>
        public bool IsViolation { get; set; } = false;

        /// <summary>
        /// 簽約核准
        /// </summary>
        public bool IsApproval { get; set; }

        public IFormFile file { get; set; }

        public List<string> err { get; set; }

        ///<summary>
        ///收件年月起迄-起
        ///</summary>
        public string Application_StartCreateDate { get; set; }
        public string QuotaHosp_StartCreateDate { get; set; }

        ///<summary>
        ///收件年月起迄-迄
        ///</summary>
        public string Application_EndCreateDate { get; set; }
        public string QuotaHosp_EndCreateDate { get; set; }

        ///<summary>
        ///申請類型
        ///</summary>
        public string ApplicationType { get; set; }
        /// <summary>
        /// 上傳檔案的類型
        /// </summary>
        public string UploadType { get; set; }
    }

    /// <summary>
    /// 機構合約model
    /// </summary>
    public class HospBasicContractViewModel
    {
        public int Id { get; set; }

        ///<summary>
        ///
        ///</summary>
        public string HospID { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public string HospSeqNo { get; set; }

        ///<summary>
        ///醫院名稱
        ///</summary>
        public string HospName { get; set; }

        ///<summary>
        ///機構狀態
        ///</summary>
        public HospStatus? HospStatus { get; set; }

        ///<summary>
        ///機構狀態
        ///</summary>
        [DisplayName("機構狀態")]
        public string HospStatusName { get; set; }

        /// <summary>
        /// 機構狀態
        /// </summary>
        public string HospStatusStr
        {
            get
            {
                return this.HospStatus.HasValue ? this.HospStatus.GetEnumDescription() : string.Empty;
            }
        }

        /// <summary>
        /// 合約狀態
        /// </summary>
        public ContractStatus? ContractStatus { get; set; }

        /// <summary>
        /// 合約狀態
        /// </summary>
        public string ContractStatusStr
        {
            get
            {
                return this.ContractStatus.HasValue ? this.ContractStatus.GetEnumDescription() : string.Empty;
            }
        }


        /// <summary>
        /// 合約類別代碼
        /// </summary>
        public string SMKContractType { get; set; }

        /// <summary>
        /// 合約類別名稱
        /// </summary>
        public string SMKContractTypeNam { get; set; }

        ///<summary>
        ///合約生效日期
        ///</summary>
        [JsonConverter(typeof(WestTwDateConverter))]
        public string HospStartDate { get; set; }

        ///<summary>
        ///合約終止日期
        ///</summary>
        [JsonConverter(typeof(WestTwDateConverter))]
        public string HospEndDate { get; set; }


        ///<summary>
        ///合約終止日期
        ///</summary>
        [JsonConverter(typeof(WestTwDateConverter))]
        public string HospNewEndDate { get; set; }

        ///<summary>
        ///合約終止日期
        ///</summary>
        [JsonConverter(typeof(WestTwDateConverter))]
        public string CreateDate { get; set; }
    }


    public class JudgeContractViewModel
    {

        public HospBasicContractViewModel contract { get; set; }
    }

    public class ApplicationHospBasicContractViewModel : ApplyHospChange
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
                    if (this.ChangeHospID == true)
                    {
                        data_string += "院所代碼,";
                    }
                    if (this.ChangeHospName == true)
                    {
                        data_string += "院所名稱,";
                    }
                    if (this.ChangeHospAddress == true)
                    {
                        data_string += "院所地址,";
                    }
                    if (this.ChangeHospUserName == true)
                    {
                        data_string += "負責人,";
                    }
                }
                data_string = data_string.TrimEnd(',');

                return data_string;
            }
        }

        public string Get_Note { get; set; }


        public string new_Note
        {
            get
            {
                string data_string = string.Empty;
                data_string += (this.Get_Note == "" || this.Get_Note == null) ? "" : this.Get_Note + ",";
                if (this.ChangeHospID == true)
                {
                    data_string += $"原【院所代碼】{this.HospID} 異動為 {this.NewHospID} ,";
                    data_string += $"原【院區別】{this.HospSeqNo} 異動為 {this.NewHospSeqNo} ,";
                }
                if (this.ChangeHospName == true)
                {
                    data_string += $"原【院所名稱】{this.HospName} 異動為 {this.NewHospName} ,";
                }
                if (this.ChangeHospAddress == true)
                {
                    data_string += $"原【院所地址】{this.HospAddress} 異動為 {this.NewHospAddress} ,";
                }
                if (this.ChangeHospUserName == true)
                {
                    data_string += $"原【負責人】{this.HospUserName} 異動為 {this.NewHospUserName} ,";
                }
                data_string = data_string.TrimEnd(',');
                return data_string;
            }
        }
    }

    public class ApplicationHospBasicViewModel
    {
        public string FeeYM { get; set; }
        public string FeeYMD { get; set; }
        public string Application_Type { get; set; }
        public string Change_Type { get; set; }
        public string HospID { get; set; }
        public string HospSeqNo { get; set; }
        public string HospName { get; set; }
        public string Get_Note { get; set; }
    }

    public class QuotaHospHospBasicContractViewModel : QuotaHosp
    {
        public string ApplyTreatChange { get; set; }
        public string ApplyHealthEduChange { get; set; }
    }
}
