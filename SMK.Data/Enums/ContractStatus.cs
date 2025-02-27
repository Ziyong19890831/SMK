using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SMK.Data.Enums
{
    /// <summary>
    /// 合約類別
    /// HospContract.SMKContractType = '01'//主約
    /// HospContract.SMKContractType in ('02','03')//品質改善合約
    /// </summary>
    public enum ContractStatus
    {
        [Description("主約")]
        MainContract = 1,
        [Description("品質改善合約")]
        ImproveContractType = 2
    }
}
