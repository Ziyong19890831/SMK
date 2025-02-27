using System;
using System.Collections.Generic;
using SMK.Data.Entity;
using SMK.Web.Extensions;

namespace SMK.Worker.FileProcess.Handler
{
    public class CstQsCureHandler : FileInHandler<MhbtQsCure>
    {
        public override int Header { get; set; } = 0;
        public override string FilenamePattern => @"CST_QS_CURE.txt";
        
        public override string[] Parse(string line)
        {
            return line.Split("|");
        }

        public override MhbtQsCure Transform(string[] values, Dictionary<string, object> args)
        {
            // sql = "insert into MhbtQsCure(HospID,ID,Birthday,FuncDate,CureItem,CureNum,TxtDate,AdjustUserID,HospSeqNo) values("
            // sql += "'" & strLineArray(0).ToString.Trim() & "',"
            // sql += "'" & StrConv(strLineArray(1).ToString.Trim(), VbStrConv.Narrow) & "',"
            // sql += "'" & strLineArray(2).ToString.Trim() & "',"
            // sql += "'" & strLineArray(3).ToString.Trim() & "',"
            // sql += "'" & strLineArray(4).ToString.Trim() & "',"
            // sql += "" & CheckZero(strLineArray(5).ToString.Trim()) & ","
            // sql += "'" & Mid(strLineArray(6).ToString.Trim(), 1, 8) & "',"
            // sql += "'" & strLineArray(7).ToString.Trim() & "',"
            // sql += "'" & strLineArray(8).ToString.Trim() & "')"

            var now = (DateTime) args["Now"];
            return new MhbtQsCure()
            {
                HospID = values[0].Trim(),
                ID = values[1].Trim(),
                Birthday = values[2].Trim(),
                FuncDate = values[3].Trim(),
                CureItem = values[4].Trim(),
                CureNum = values[5].Trim().ToDecimal(),
                TxtDate = values[6].Trim().SafeSubstring(0, 8),
                AdjustUserID = values[7].Trim(),
                HospSeqNo = values[8].Trim(),
            };
        }
    }
}
