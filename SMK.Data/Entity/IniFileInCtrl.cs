using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SMK.Data.Enums;

namespace SMK.Data.Entity
{
    public partial class IniFileInCtrl
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string Filename { get; set; }
        
        public DateTime StartedAt { get; set; }

        private FileInStatus status;
        [Column(TypeName = "varchar(20)")]
        public FileInStatus Status
        {
            get => status;
            set
            {
                status = value;
                StatusUpdatedAt = DateTime.Now;
            }
        }

        // public int TotalCount { get; set; }
        // public int FailedCount { get; set; }
        // public int SucceedCount { get; set; }
        // public int SkippedCount { get; set; }
        public DateTime? StatusUpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}