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
    /// <summary>
    /// 月報表-年概況統計表
    /// </summary>
    public partial class IniMonthDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Display(Name = "合約年月")]
        [MaxLength(6)]
        public string ContractYM { get; set; }

        [Display(Name = "合約機構數_排除解約機構")]
        public int ContractTotal { get; set; }

        [Display(Name = "合約機構數_累計(含解約機構)")]
        public int ContractAllTotal { get; set; }

        [Display(Name = "執行機構數_累計(未排除解約人員)")]
        public int RunTimeContractAllTotal { get; set; }

        [Display(Name = "合約人員數_排除解約人員")]
        public int ContractPersonTotal { get; set; }

        [Display(Name = "合約人員數_累計(含解約人員)")]
        public int ContractPersonAllTotal { get; set; }

        [Display(Name = "執行人員數_累計(未排除解約人員)")]
        public int RunTimeContractPersonAllTotal { get; set; }

        [Display(Name = "健保年月")]
        [MaxLength(6)]
        public string NhiYM { get; set; }

        [Display(Name = "總計人次(用藥+衛教)")]
        public int TreatInstructCnt { get; set; }

        [Display(Name = "用藥人次")]
        public int TreatCnt { get; set; }

        [Display(Name = "衛教人次")]
        public int InstructCnt { get; set; }

        [Display(Name = "總計人數(用藥+衛教)")]
        public int TreatInstructSum { get; set; }

        [Display(Name = "用藥人數")]
        public int TreatSum { get; set; }

        [Display(Name = "衛教人數")]
        public int InstructSum { get; set; }

        [Display(Name = "用藥 + 衛教人數")]
        public int TreatAddInstruct { get; set; }

        [Display(Name = "給藥週數")]
        public int TreatWeek { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("Getdate()")]
        [Display(Name = "建立時間")]
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
