using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SMK.Data.Enums
{
    public enum LoginLog
    {
        [Description("登入成功")]
        success = 1,
        [Description("登入失敗")]
        error = 2,
        [Description("登入失敗達三次")]
        error_Lock = 3
    }
}
