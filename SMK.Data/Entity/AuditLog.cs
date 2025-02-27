using SMK.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text;
using Yozian.Extension;

namespace SMK.Data.Entity
{
    public partial class AuditLog
    {
        [MaxLength(36)]
        [Key]
        public string Id { get; set; }

        [Display(Name = "帳號")]
        [MaxLength(128)]
        public string Account { get; set; }

        [MaxLength(64)]
        public string SourceTable { get; set; }

        [NotMapped]
        [Display(Name = "行為")]
        public AuditActionType ActionType { get; set; }

        [Column("ActionType")]
        public string ActionTypeStr
        {
            get => this.ActionType.ToString();
            set => this.ActionType = value.ToEnum<AuditActionType>();
        }

        [MaxLength(36)]
        public string RecordId { get; set; }

        [Column(TypeName = "Text")]
        [Display(Name = "異動前資料")]
        public string OriginalRecord { get; set; }

        [Column(TypeName = "Text")]
        [Display(Name = "異動後資料")]
        public string CurrentRecord { get; set; }

        [MaxLength(1024)]
        [Display(Name = "異動欄位")]
        public string InvolvedColumns { get; set; }

        [Column("ActionRemark")]
        [MaxLength(1024)]
        [Display(Name = "變更備註")]
        public string ActionRemark { get; set; }

        [Column("Description")]
        [MaxLength(511)]
        [Display(Name = "描述")]
        public string Description { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("Getdate()")]
        [Display(Name = "建立時間")]
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
