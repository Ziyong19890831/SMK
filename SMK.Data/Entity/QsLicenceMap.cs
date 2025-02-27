using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMK.Data.Entity
{
    /// <summary>
    /// quitsmoking證書類別與合約管理證書類別map
    /// </summary>
    public class QsLicenceMap
    {
        /// <summary>
        /// 流水碼
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// SMK GenLicenceType.LicenceType
        /// </summary>
        public string LicenceType { get; set; }

        /// <summary>
        /// quitsmoking licence type
        /// ex:1
        /// </summary>
        public int CTypeSNO { get; set; }

        /// <summary>
        /// quitsmoking licence Name
        /// ex:戒菸證書-醫師
        /// </summary>
        public string CTypeName { get; set; }

    }
}
