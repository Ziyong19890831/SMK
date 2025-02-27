using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMK.Data.Entity
{
    public partial class GenLoginLog
    {
        [Key]
        [Display(Name = "自動編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sysno { get; set; }

        [Display(Name = "使用者ID")]
        [MaxLength(36)]
        public string User_Id { get; set; }

        [Display(Name = "登入時間")]
        public DateTime LoginTime { get; set; } = DateTime.Now;

        [Display(Name = "登入訊息")]
        [MaxLength(20)]
        public string LoginMsg { get; set; }

        [Display(Name = "啟用")]
        public bool Enable { get; set; }

    }
}
