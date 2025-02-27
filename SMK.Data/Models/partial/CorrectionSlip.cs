using Microsoft.AspNetCore.Mvc;
using SMK.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMK.Data.Entity
{
    [ModelMetadataType(typeof(CorrectionSlipData))]
    public partial class  CorrectionSlip
    {
        public class CorrectionSlipData
        {
            [Display(Name = "案件編號")]
            [Required(ErrorMessage = "請填入案件編號")]
            [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "必須為9位數字")]
            [StringLength(9)]
            [Key]
            public string CaseNo { get; set; }

            [Display(Name = "收件日期")]
            [DataType(DataType.Date, ErrorMessage = "必須是日期格式")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            [Required(ErrorMessage ="請選擇收件日期")]
            public DateTime? ReceiveDate { get; set; }
            [Display(Name = "機構代碼")]
            [Required(ErrorMessage = "請輸入機構代碼")]
            public string HospId { get; set; }
            [Display(Name = "機構名稱")]
            [Required(ErrorMessage = "請輸入機構名稱")]
            public string HospName { get; set; }
            [Display(Name = "個案姓名")]
            [Required(ErrorMessage = "請輸入個案姓名")]
            public string Name { get; set; }
            [Display(Name = "身分證號")]
            [RegularExpression(@"^[a-zA-Z]\d{9}$", ErrorMessage = "必須為身分證格式")]
            [Required(ErrorMessage = "請輸入身分證號")]
            [StringLength(10)]
            public string ID { get; set; }
            [Display(Name = "出生日期")]
            [DataType(DataType.Date, ErrorMessage = "必須是日期格式")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            [Required(ErrorMessage = "請選擇出生日期")]
            public DateTime? Birthday { get; set; }
            [Display(Name = "更-基本")]
            public string CorrectBasic { get; set; }
            [Display(Name = "更-就醫")]
            public string CorrectHosp { get; set; }
            [Display(Name = "更-衛教")]
            public string CorrectHealth { get; set; }
            [Display(Name = "更-其他")]
            public string CorrectOther { get; set; }
            [Display(Name = "更正項目")]
            public string CorrectItems { get; set; }
            [Display(Name = "更正項目_2")]
            public string CorrectItems2 { get; set; }
            [Display(Name = "備註(資料來源)")]
            public string source { get; set; }
            [Display(Name = "註記")]
            public string Memo { get; set; }

            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [DefaultValue("Getdate()")]
            [Display(Name = "建立時間")]
            [Column("UpdateAt")]
            public DateTime UpdateAt { get; set; } = DateTime.Now;

            [MaxLength(127)]
            [Display(Name = "修改者")]
            [Column("UpdatedBy")]
            public string UpdatedBy { get; set; }
        }

    }
}
