using System;
using System.Collections.Generic;
using SMK.Data.Entity;
using SMK.Web.Extensions;

namespace SMK.Worker.FileProcess.Handler
{
    public class CstAgentPatientHandler : FileInHandler<MhbtAgentPatient>
    {
        public override int Header { get; set; } = 0;
        public override string FilenamePattern => @"CST_AGENT_PATIENT.txt";
        public override string[] Parse(string line)
        {
            return line.Split('|');
        }

        public override MhbtAgentPatient Transform(string[] values, Dictionary<string, object> args)
        {
            // sql = "insert into MhbtAgentPatient(HospID,ID,Birthday,HospAgentCode,Name,Sex,InformADDR,TelD,TelN,TelM,SeqNo,BranchCode,FuncMark,TxtDate,TownCode,TownName) values("
            // sql += "'" & strLineArray(0).ToString.Trim() & "',"
            // sql += "'" & StrConv(strLineArray(2).ToString.Trim(), VbStrConv.Narrow) & "',"
            // sql += "'" & strLineArray(3).ToString.Trim() & "',"
            // sql += "'" & strLineArray(1).ToString.Trim() & "',"
            // sql += "N'" & strLineArray(4).ToString.Trim().Replace("'", "") & "',"
            // sql += "'" & GetMFString(strLineArray(2).ToString.Trim()) & "',"
            // sql += "'" & strLineArray(6).ToString.Trim().Replace("'", "") & "',"
            // sql += "'" & TelFormat(strLineArray(7).ToString.Trim() & "-" & strLineArray(8).ToString.Trim() & "#" & strLineArray(9).ToString.Trim()) & "',"
            // sql += "'" & TelFormat(strLineArray(10).ToString.Trim() & "-" & strLineArray(11).ToString.Trim() & "#" & strLineArray(12).ToString.Trim()) & "',"
            // sql += "'" & strLineArray(13).ToString.Trim() & "',"
            // sql += "'" & strLineArray(14).ToString.Trim() & "',"
            // sql += "'" & strLineArray(15).ToString.Trim() & "',"
            // sql += "'" & strLineArray(17).ToString.Trim() & "',"
            // sql += "'" & Mid(strLineArray(16).ToString.Trim(), 1, 8) & "',"
            // sql += "'" & strLineArray(18).ToString.Trim() & "',"
            // sql += "'" & strLineArray(19).ToString.Trim() & "')"
            return new MhbtAgentPatient()
            {
                HospID = values[0].Trim(),
                ID = values[2].Trim(),
                Birthday = values[3].Trim(),
                HospAgentCode = values[1].Trim(),
                Name = values[4].Trim('\\', ' '),
                Sex = values[2].Trim().ToGender(),
                InformADDR = values[6].Trim('\\', ' '),
                TelD = StringExtensions.ToTelPhone(values[7].Trim(), values[8].Trim(), values[9].Trim()),
                TelN = StringExtensions.ToTelPhone(values[10].Trim(), values[11].Trim(), values[12].Trim()),
                TelM = values[13].Trim(),
                SeqNo = values[14].Trim(),
                BranchCode = values[15].Trim(),
                FuncMark = values[17].Trim(),
                TxtDate = values[16].Trim().Length >= 8 ? values[16].Trim().Substring(0, 8) : values[16].Trim(),
                TownCode = values[18].Trim(),
                TownName = values[19].Trim(),
            };
        }
    }
}
