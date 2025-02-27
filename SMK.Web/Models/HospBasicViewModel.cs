using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Data.Utility;
using SMK.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.Models
{
    public partial class HospBasicViewModel
    {
        /// <summary>
        /// 醫事機構Id
        /// </summary>
        [Required(ErrorMessage = "請輸入醫事機構代碼")]
        [Display(Name = "醫事機構代碼")]
        public string HospId { get; set; }

        /// <summary>
        /// 醫事機構 子代碼
        /// </summary>
        [Required(ErrorMessage = "請輸入醫事機構 子代碼")]
        [Display(Name = "醫事機構 子代碼")]
        public string HospSeqNo { get; set; }

        /// <summary>
        /// 醫事機構名稱
        /// </summary>
        [Display(Name = "醫事機構名稱")]
        public string HospName { get; set; }

        /// <summary>
        /// 院所電話
        /// </summary>
        [Display(Name = "院所電話")]
        public string HospTel { get; set; }

        /// <summary>
        /// 院所傳真電話
        /// </summary>
        [Display(Name = "院所傳真電話")]
        public string HospFax { get; set; }

        /// <summary>
        /// 院所email
        /// </summary>
        [Display(Name = "院所Email")]
        public string HospEmail { get; set; }


        /// <summary>
        /// 聯絡人1
        /// </summary>
        [Display(Name = "聯絡人")]
        public string Contact1 { get; set; }

        /// <summary>
        /// 聯絡電話1
        /// </summary>
        [Display(Name = "聯絡電話")]
        public string ContactTel1 { get; set; }

        /// <summary>
        /// 聯絡傳真1
        /// </summary>
        [Display(Name = "聯絡傳真")]
        public string ContactFax1 { get; set; }

        /// <summary>
        /// 聯絡email1
        /// </summary>
        [Display(Name = "聯絡人 Email")]
        public string ContactEmail1 { get; set; }

        /// <summary>
        /// 聯絡人2
        /// </summary>
        [Display(Name = "聯絡人2")]
        public string Contact2 { get; set; }

        /// <summary>
        /// 聯絡電話2
        /// </summary>
        [Display(Name = "聯絡電話2")]
        public string ContactTel2 { get; set; }

        /// <summary>
        /// 聯絡傳真2
        /// </summary>
        [Display(Name = "聯絡傳真2")]
        public string ContactFax2 { get; set; }

        /// <summary>
        /// 聯絡email2
        /// </summary>
        [Display(Name = "聯絡email2")]
        public string ContactEmail2 { get; set; }

        /// <summary>
        /// 負責人名字
        /// </summary>
        [Display(Name = "負責人名字")]
        public string HospOwnName { get; set; }

        /// <summary>
        /// 負責人Id
        /// </summary>
        [Display(Name = "負責人ID")]
        public string HospOwnId { get; set; }


        /// <summary>
        /// 分局
        /// </summary>
        [Display(Name = "分局")]
        public string BranchNo { get; set; }

        /// <summary>
        /// 郵遞區號
        /// </summary>
        [Display(Name = "郵遞區號")]
        public string Zip { get; set; }

        /// <summary>
        /// 縣市
        /// </summary>
        [Display(Name = "縣市")]
        public string DivisionNo { get; set; }

        /// <summary>
        /// 鄉鎮
        /// </summary>
        [Display(Name = "鄉鎮")]
        public string SubDivisionNo { get; set; }

        /// <summary>
        /// 醫事機構地址
        /// </summary>
        [Display(Name = "醫事機構地址")]
        public string HospAddress { get; set; }

        /// <summary>
        /// 機構狀態
        /// </summary>
        [Display(Name = "機構狀態")]
        public HospStatus? HospStatus { get; set; }

        /// <summary>
        /// 機構狀態
        /// </summary>
        public string HospStatusString =>
            this.HospStatus.HasValue ? this.HospStatus.GetEnumDescription() : string.Empty;

        public string FirstHospId { get; set; }
        /// <summary>
        /// 前次代碼
        /// </summary>
        [Display(Name = "前次代碼")]
        public string PrevHospID { get; set; }
        [Display(Name = "最後代碼")]
        public string LastHospId { get; set; }
        public string LastContType { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        [Display(Name = "備註")]
        public string Remark { get; set; }

        public string ChFlg1 { get; set; }
        public string ChFlg2 { get; set; }
        public string ChFlg3 { get; set; }
        public string PrevHospSeqNo { get; set; }
        public string LastHospSeqNo { get; set; }
        public string FirstHospSeqNo { get; set; }
        /// <summary>
        /// 機構簡稱
        /// </summary>
        [Display(Name = "機構簡稱")]
        public string HospAbbr { get; set; }


        /// <summary>
        /// HospContract.CreateDate最小值
        /// 建立日期
        /// </summary>
        [Display(Name = "建立日期")]
        public string CreateDate { get; set; }


        /// <summary>
        /// 新機構代碼
        /// </summary>
        [Display(Name = "新機構代碼")]
        public string NewHospId { get; set; }

        /// <summary>
        /// 新機構子代碼
        /// </summary>
        [Display(Name = "新機構子代碼")]
        public string NewHospSeqNo { get; set; }

        public List<HospContractViewModel> HospContracts { get; set; } = new List<HospContractViewModel>();
        public List<HospContractTypeViewModel> HospContractTypes { get; set; } = new List<HospContractTypeViewModel>();

        [Display(Name = "機構層級")]
        public GenHospCont HospCont { get; set; }
        [Display(Name = "配額調整")]
        public int? Quota { get; set; }
        [Display(Name = "用藥配額調整")]
        public int? MedicationQuota { get; set; }
        [Display(Name = "衛教配額調整")]
        public int? InstructionsQuota { get; set; }

        [Display(Name = "最新健保特約類別")]
        public string HospContType { get; set; }

        [Display(Name = "主約時間")]
        public string HospContMainType_StartDay { get; set; }       
        [Display(Name = "主約契約")]
        public string HospContMainType_HospContType { get; set; }
    }

    public class HospContractTypeViewModel : HospContractType
    {
        public HospContractTypeViewModel()
        {

        }

        public HospContractTypeViewModel(HospContractType hospContractType)
        {
            HospId = hospContractType.HospId;
            HospSeqNo = hospContractType.HospSeqNo;
            CntEDate = hospContractType.CntEDate;
            CntSDate = hospContractType.CntSDate;
            HospContType = hospContractType.HospContType;
        }
    }

    public class HospContractViewModel : HospContract
    {
        public HospContractViewModel() { }
        public HospContractViewModel(HospContract hospContract)
        {
            Id = hospContract.Id;
            HospId = hospContract.HospId;
            SmkcontractType = hospContract.SmkcontractType;
            HospStartDate = hospContract.HospStartDate;
            HospEndDate = hospContract.HospEndDate;
            EndReasonNo = hospContract.EndReasonNo;
            CreateDate = hospContract.CreateDate;
            ModifyDate = hospContract.ModifyDate;
            ModifyPersonNo = hospContract.ModifyPersonNo;
            Remark = hospContract.Remark;
            HospSeqNo = hospContract.HospSeqNo;
            UpdateFileTime = hospContract.UpdateFileTime;
        }
        public string TWHospStartDate
        {
            get => HospStartDate.WestToTwDate();
            set
            {
                HospStartDate = value.TwDateToDateTime();
            }
        }
        public string TWHospEndDate
        {
            get => HospEndDate.WestToTwDate();
            set
            {
                HospEndDate = value.TwDateToDateTime();
            }
        }

        protected HospContract GetHospContract()
        {
            return new HospContract()
            {
                HospId = this.HospId,
                SmkcontractType = this.SmkcontractType,
                HospStartDate = this.HospStartDate,
                HospEndDate = this.HospEndDate,
                EndReasonNo = this.EndReasonNo,
                CreateDate = this.CreateDate,
                ModifyDate = this.ModifyDate,
                ModifyPersonNo = this.ModifyPersonNo,
                Remark = this.Remark,
                HospSeqNo = this.HospSeqNo,
            };
        }
    }

    /// <summary>
    /// 醫事機構與醫事人員合約
    /// </summary>
    public class HospPrsnViewModel
    {

        /// <summary>
        /// 醫事機構Id
        /// </summary>
        [Display(Name = "醫事機構代碼")]
        public string HospId { get; set; }

        /// <summary>
        /// 醫事機構 子代碼
        /// </summary>
        [Display(Name = "醫事機構子代碼")]
        public string HospSeqNo { get; set; }

        /// <summary>
        /// 醫事機構名稱
        /// </summary>
        [Display(Name = "醫事機構名稱")]
        public string HospName { get; set; }


        /// <summary>
        /// 機構狀態
        /// </summary>
        [Display(Name = "機構狀態")]
        public HospStatus? HospStatus { get; set; }

        /// <summary>
        /// 機構狀態
        /// </summary>
        public string HospStatusString =>
            this.HospStatus.HasValue ? this.HospStatus.GetEnumDescription() : string.Empty;

        public string VueState { get; set; }
    }
}
