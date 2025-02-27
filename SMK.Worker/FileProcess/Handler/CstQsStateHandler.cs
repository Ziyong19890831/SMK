using System;
using System.Collections.Generic;
using SMK.Data.Entity;
using SMK.Web.Extensions;

namespace SMK.Worker.FileProcess.Handler
{
    public class CstQsStateHandler : FileInHandler<MhbtQsState>
    {
        public override int Header { get; set; } = 0;
        public override string FilenamePattern => @"CST_QS_STATE.txt";
        
        public override string[] Parse(string line)
        {
            return line.Split("|");
        }

        public override MhbtQsState Transform(string[] values, Dictionary<string, object> args)
        {
            // sql = "insert into MhbtQsState(HospID,ID,Birthday,FuncDate,CureState,CureStateOther,Cure_Type,Seqno,TxtDate,AdjustUserID,HospSeqNo) values("
            // sql += "'" & strLineArray(0).ToString.Trim() & "',"
            // sql += "'" & StrConv(strLineArray(1).ToString.Trim(), VbStrConv.Narrow) & "',"
            // sql += "'" & strLineArray(2).ToString.Trim() & "',"
            // sql += "'" & strLineArray(3).ToString.Trim() & "',"
            // sql += "'" & strLineArray(4).ToString.Trim() & "',"
            // sql += "'" & strLineArray(5).ToString.Trim().Replace("'", "") & "',"
            // sql += "'" & strLineArray(8).ToString.Trim() & "',"
            // sql += "" & wkSeqno & ","
            // sql += "'" & Mid(strLineArray(6).ToString.Trim(), 1, 8) & "',"
            // sql += "'" & strLineArray(7).ToString.Trim() & "',"
            // sql += "'" & strLineArray(9).ToString.Trim() & "')"

            var now = (DateTime) args["Now"];
            return new MhbtQsState()
            {
                HospID = values[0].Trim(),
                ID = values[1].Trim(),
                Birthday = values[2].Trim(),
                FuncDate = values[3].Trim(),
                CureState = values[4].Trim(),
                CureStateOther = values[5].Trim(),
                CureType = values[8].Trim(),
                Seqno = 0,
                TxtDate = values[6].Trim().Length >= 8 ? values[6].Trim().Substring(0, 8) : "" ,
                AdjustUserID = values[7].Trim(),
                HospSeqNo = values[9].Trim(),
            };
        }
    }
}
