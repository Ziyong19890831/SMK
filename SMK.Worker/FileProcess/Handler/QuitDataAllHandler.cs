using System.Collections.Generic;
using SMK.Data.Entity;
using SMK.Web.Extensions;

namespace SMK.Worker.FileProcess.Handler
{
    /// <summary>
    /// 戒菸率調查檔
    /// </summary>
    public class QuitDataAllHandler : FileInHandler<QuitDataAll>
    {
        public override int Header { get; set; } = 1;
        public override string FilenamePattern => @"QuitDataAll.txt";

        public override QuitDataAll Transform(string[] values, Dictionary<string, object> args)
        {
            // sql = "insert into QuitDataAll(CaseNo,FirstMonth,TimeSpan,HospID,HospSeqNo,ID,Birthday,Edition,VisitDate,Result,QuitPnt,QuitCtn,Edu,Job) values("
            // sql += "'" & strLineArray(0).ToString.Trim() & "',"
            // sql += "'" & strLineArray(1).ToString.Trim() & "',"
            // sql += "'" & strLineArray(2).ToString.Trim() & "',"
            // sql += "'" & strLineArray(3).ToString.Trim() & "',"
            // sql += "'" & strLineArray(4).ToString.Trim() & "',"
            // sql += "'" & strLineArray(5).ToString.Trim() & "',"
            // sql += "'" & strLineArray(6).ToString.Trim() & "',"
            // sql += "'" & strLineArray(7).ToString.Trim() & "',"
            // sql += "'" & strLineArray(8).ToString.Trim() & "',"
            // sql += "'" & strLineArray(9).ToString.Trim() & "',"
            // sql += "'" & strLineArray(10).ToString.Trim() & "',"
            // sql += "'" & strLineArray(11).ToString.Trim() & "',"
            // sql += "'" & strLineArray(12).ToString.Trim() & "',"
            // sql += "'" & strLineArray(13).ToString.Trim() & "')"
            return new QuitDataAll()
            {
                CaseNo = values[0].Trim(),
                FirstMonth = values[1].Trim(),
                TimeSpan = values[2].Trim().ToInt32(),
                HospID = values[3].Trim(),
                HospSeqNo = values[4].Trim(),
                ID = values[5].Trim(),
                Birthday = values[6].Trim(),
                Edition = values[7].Trim(),
                VisitDate = values[8].Trim(),
                Result = values[9].Trim(),
                QuitPnt = values[10].Trim(),
                QuitCtn = values[11].Trim(),
                Edu = values[12].Trim(),
                Job = values[13].Trim(),
            };
        }
    }
}
