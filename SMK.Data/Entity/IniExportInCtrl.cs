using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SMK.Data.Enums;

namespace SMK.Data.Entity
{
    public partial class IniExportInCtrl
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(6)")]
        public string fee_ym { get; set; }
        
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

        public DateTime? StatusUpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}