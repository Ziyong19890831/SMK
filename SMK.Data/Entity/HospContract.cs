using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMK.Data.Entity
{
    public partial class HospContract
    {
        public int Id { get; set; }
        public string HospId { get; set; }
        public string SmkcontractType { get; set; }
        public string HospStartDate { get; set; }
        public string HospEndDate { get; set; }
        public string EndReasonNo { get; set; }
        public string CreateDate { get; set; }
        public string ModifyDate { get; set; }
        public int? ModifyPersonNo { get; set; }
        public string Remark { get; set; }
        public string HospSeqNo { get; set; }

        /// <summary>
        /// 上傳檔案的紀錄時間
        /// 後面需要使用yyyyMMddhhmmss當作檔案名稱紀錄
        /// </summary>
        public DateTime? UpdateFileTime { get; set; }
    }
}
