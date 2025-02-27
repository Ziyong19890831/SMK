using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SMK.Data.Enums
{
    public enum ApplicationType
    {
        [Description("新開辦")]
        new_order,
        [Description("解約")]
        End_order = 2,
        [Description("異動")]
        change = 3,
        [Description("新增")]
        Add = 4,
        [Description("先行同意")]
        Agree_In_Advance = 5,
        [Description("變更代碼")]
        Change_Code = 6
    }
}
