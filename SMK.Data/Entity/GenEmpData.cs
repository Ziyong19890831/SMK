using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMK.Data.Entity
{
    public partial class GenEmpData
    {
        [MaxLength(36)]
        [Key]
        public string Id { get; set; }

        [Display(Name = "帳號")]
        [MaxLength(128)]
        public string Account { get; set; }

        [Display(Name = "密碼")]
        [MaxLength(256)]
        public string Pwd { get; set; }

        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "啟用")]
        public bool Enable { get; set; }

        [Display(Name = "最後登入時間")]
        public DateTime LastLoginDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("Getdate()")]
        [Display(Name = "建立時間")]
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "更新時間")]
        [Column("UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [MaxLength(127)]
        [Display(Name = "更新人員")]
        [Column("UpdatedBy")]
        public string UpdatedBy { get; set; }

        [Display(Name = "變更密碼時間")]
        [Column("PasswordModifyAt")]
        public DateTime? PasswordModifyAt { get; set; }

        [Display(Name = "登入錯誤次數")]
        public int LoginError { get; set; }

        [Display(Name = "上次登入錯誤時間")]
        [Column("LoginErrorAt")]
        public DateTime? LoginErrorAt { get; set; }
    }
}
