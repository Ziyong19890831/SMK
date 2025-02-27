using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SMK.Data.Enums
{
    public enum HospStatus
    {
        [Description("申請合約")]
        ApplyHosp = 1,
        [Description("合約有效")]
        EnabledHosp = 2,
        [Description("合約終止")]
        TerminationHosp = 3
    }
}
