using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMK.Data.Entity
{
    public class ExceptionLog
    {
        [Key]
        [MaxLength(36)]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "SourceServer")]
        public string SourceServer { get; set; }

        [MaxLength(256)]
        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }

        [MaxLength(512)]
        [Display(Name = "Source")]
        public string Source { get; set; }

        [Display(Name = "StackTrace")]
        public string StackTrace { get; set; }

        [Display(Name = "ExtraData")]
        public string ExtraData { get; set; }

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