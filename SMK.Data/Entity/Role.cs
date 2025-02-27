﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMK.Data.Entity
{
    public class Role
    {
        [Key]
        [MaxLength(36)]
        public string Id { get; set; }

        [Display(Name = "角色")]
        public string Name { get; set; }

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