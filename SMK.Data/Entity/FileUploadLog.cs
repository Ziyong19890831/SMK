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
    public partial class FileUploadLog
    {
        [MaxLength(36)]
        [Key]
        public string Id { get; set; }

        [NotMapped]
        [Display(Name = "檔案類型")]
        public FileType FileType { get;set;}

        [Column("FileType")]
        public string FileTypeStr
        {
            get => this.FileType.ToString();
            set => this.FileType = value.ToEnum<FileType>();
        }

        [Display(Name ="檔名")]
        [MaxLength(128)]
        public string FileName { get; set; }

        [NotMapped]
        [Display(Name = "檔案狀態")]
        public FileStatus FileStatus { get; set; }

        [Column("FileStatus")]
        public string FileStatusStr
        {
            get => this.FileStatus.ToString();
            set => this.FileStatus = value.ToEnum<FileStatus>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("Getdate()")]
        [Display(Name = "建立時間")]
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [MaxLength(127)]
        [Display(Name = "建立人員")]
        [Column("CreatedBy")]
        public string CreatedBy { get; set; }

        [Display(Name = "更新時間")]
        [Column("UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [MaxLength(127)]
        [Display(Name = "更新人員")]
        [Column("UpdatedBy")]
        public string UpdatedBy { get; set; }
    }
}
