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

namespace SMK.Web.Models
{
    /// <summary>
    /// 專審查詢
    /// </summary>
    public class ProReviewQueryModel : PagedRequest
    {
        ///<summary>
        ///
        ///</summary>
        public string HospID { get; set; }

        ///<summary>
        ///醫院名稱
        ///</summary>
        public string HospName { get; set; }
        /// <summary>
        /// 費用年月起
        /// </summary>
        public string SFeeYM { get; set; }
        /// <summary>
        /// 費用年月迄
        /// </summary>
        public string EFeeYM { get; set; }
    }


    public class ProReviewDto
    {

        ///<summary>
        /// 醫事機構代碼
        ///</summary>
        public string HospId { get; set; }

        ///<summary>
        /// 院所層級
        ///</summary>
        public string HospContType { get; set; }

        ///<summary>
        /// 分院別
        ///</summary>
        public string HospSeqNo { get; set; }

        ///<summary>
        /// 機構名稱
        ///</summary>
        public string HospName { get; set; }

        ///<summary>
        /// 抽樣編號
        ///</summary>
        public string SamplingNo { get; set; }

        ///<summary>
        /// 費用年月
        ///</summary>
        public string FeeYN { get; set; }

        ///<summary>
        /// 姓名
        ///</summary>
        public string UserName { get; set; }

        ///<summary>
        /// 身分證號
        ///</summary>
        public string UserId { get; set; }

        ///<summary>
        /// 就醫日期
        ///</summary>
        public string FuncDate { get; set; }

        ///<summary>
        /// 核扣金額
        ///</summary>
        public int ReviewAmt { get; set; }

        ///<summary>
        /// 審查委員
        ///</summary>
        public string Review { get; set; }

        ///<summary>
        /// 申復補付金額
        ///</summary>
        public int AppealsAmt { get; set; }

        ///<summary>
        /// 申復審查委員
        ///</summary>
        public string Appeals { get; set; }
    }
}
