using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMK.Data.Entity
{
    public partial class CallBoard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "系統流水號")]
        public int SysNo { get; set; }       

        [Display(Name = "公告日期")]
        [Required]
        public DateTime AnnouncementDate { get; set; }        

        [Display(Name = "公告內容")]
        [MaxLength(500)]
        [Required]
        public string Note { get; set; }        

        [Display(Name = "狀態")]
        //0:停用 1:啟用
        [DefaultValue(false)]
        public bool Condition { get; set; }

        [Display(Name = "是否被刪除")]
        //1:已刪除 0:未刪除
        [DefaultValue(false)]
        public bool Action { get; set; }

        [Display(Name = "創建日期")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [MaxLength(127)]
        [Display(Name = "修改者")]
        public string UpdatedBy { get; set; }

        [Display(Name = "修改日期")]
        public DateTime? TxtDate { get; set; }
    }
}