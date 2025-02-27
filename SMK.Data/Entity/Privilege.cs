using SMK.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Yozian.Extension;

namespace SMK.Data.Entity
{
    /// <summary>
    /// 功能頁-> menu -> 可多層
    /// </summary>
    public partial class Privilege
    {
        [Key]
        [MaxLength(36)]
        public string Id { get; set; }

        [MaxLength(36)]
        public string ParentId { get; set; }

        public string Name { get; set; }
        public int Sort { get; set; }

        [Column("Type")]
        public string LinkTypeStr
        {
            get => this.LinkType.ToString();
            set => this.LinkType = value.ToEnum<PrivilegeNodeType>();
        }

        [NotMapped]
        public PrivilegeNodeType LinkType { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        [MaxLength(512)]
        public string QueryParams { get; set; }

        [MaxLength(512)]
        public string Remark { get; set; }

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
    }
}
