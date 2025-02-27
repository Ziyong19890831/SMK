using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SMK.Data.Entity
{
    public partial class HospBscAll
    {
        /// <summary>
        /// 醫事機構代碼
        /// </summary>
        public string HospId { get; set; }
        /// <summary>
        /// 醫事機構名稱
        /// </summary>
        public string HospName { get; set; }
        /// <summary>
        /// 電話區域號碼 
        /// </summary>
        public string HospTelArea { get; set; }
        /// <summary>
        /// 電話號碼
        /// </summary>
        public string HospTel { get; set; }
        /// <summary>
        /// 分區別
        /// </summary>
        public string BranchNo { get; set; }
        public string HospAddress { get; set; }
        /// <summary>
        /// 特約類別
        /// </summary>
        public string ContType { get; set; }
        /// <summary>
        /// 型態別
        /// </summary>
        public string HospType { get; set; }
        /// <summary>
        /// 醫事機構種類
        /// </summary>
        public string HospKind { get; set; }
        /// <summary>
        /// 原始合約起日
        /// </summary>
        [MaxLength(8)]
        public string HospStartDate { get; set; }
        /// <summary>
        /// 終止合約或歇業日期
        /// </summary>
        public string HospEndDate { get; set; }
        /// <summary>
        /// 開業狀況
        /// </summary>
        [MaxLength(1)]
        public string OpenState { get; set; }
    }
}
