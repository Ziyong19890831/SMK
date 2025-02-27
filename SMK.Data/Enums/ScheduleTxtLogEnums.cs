using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SMK.Data.Enums
{
    public class ScheduleTxtLogEnums
    {
        public enum ScheduleTxtLog
        {
            [Description("執行中")]
            Running = 1,
            [Description("完成")]
            Success = 2,
            [Description("失敗")]
            Error = 3
        }
    }
}
