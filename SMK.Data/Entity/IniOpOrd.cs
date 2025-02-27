using System;
using System.Collections.Generic;

namespace SMK.Data.Entity
{
    /// <summary>
    /// 健保門診處方及治療明細資料-更新檔
    /// </summary>
    public partial class IniOpOrd
    {
        /// <summary>
        /// 電腦序號
        /// </summary>
        public string DataId { get; set; }

        /// <summary>
        /// 流水號
        /// </summary>
        public int OrderSeqNo { get; set; }

        /// <summary>
        /// 費用年月
        /// </summary>
        public string FeeYm { get; set; }

        /// <summary>
        /// 醫令類別
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        /// 醫令代碼
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 調劑方式
        /// </summary>
        public string RelMode { get; set; }

        /// <summary>
        /// 連續處方註記
        /// </summary>
        public string ChrMark { get; set; }

        /// <summary>
        /// 藥品用量
        /// </summary>
        public string DrugNum { get; set; }

        /// <summary>
        /// 藥品使用頻率
        /// </summary>
        public string DrugFre { get; set; }

        /// <summary>
        /// 用藥途徑/作用部位
        /// </summary>
        public string DrugPath { get; set; }

        /// <summary>
        /// 醫令單價
        /// </summary>
        public decimal? OrderUprice { get; set; }

        /// <summary>
        /// 醫令數量
        /// </summary>
        public decimal? OrderQty { get; set; }

        /// <summary>
        /// 醫令點數
        /// </summary>
        public int? OrderDot { get; set; }

        /// <summary>
        /// 執行醫事人員代號
        /// </summary>
        public string ExePrsnId { get; set; }

        /// <summary>
        /// 診療部位
        /// </summary>
        public string CurePath { get; set; }

        /// <summary>
        /// 醫令給藥日份
        /// </summary>
        public int? OrderDrugDay { get; set; }

        /// <summary>
        /// 轉檔日期
        /// </summary>
        public string TranDate { get; set; }

    }
}
