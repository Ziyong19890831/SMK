using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace SMK.Data.Entity
{
    public partial class ScheduleTxtLog
    {
        [Key]
        [Display(Name = "自動編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sysno { get; set; }

        [Display(Name = "輸出檔案名稱")]
        [MaxLength(50)]
        public string? FilesName { get; set; }

        [Display(Name = "輸出筆數")]
        public int? OutPutCount { get; set; }

        [Display(Name = "排程開始時間")]
        public DateTime? StartTime { get; set; } = DateTime.Now;   
        
        [Display(Name = "排程結束時間")]
        public DateTime? EndTime { get; set; } = DateTime.Now;      
        
        [Display(Name = "排程狀態")]
        [MaxLength(20)]
        public string Schedulestate { get; set; } 
    }
}
