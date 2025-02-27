using System;
using System.Collections.Generic;
using SMK.Data.Entity;
using SMK.Web.Extensions;

namespace SMK.Worker.FileProcess.Handler
{
    public class CstQsDataHandler : FileInHandler<MhbtQsData>
    {
        public override int Header { get; set; } = 0;
        public override string FilenamePattern => @"CST_QS_DATA.txt";
        
        public override string[] Parse(string line)
        {
            return line.Split("|");
        }

        public override MhbtQsData Transform(string[] values, Dictionary<string, object> args)
        {
            DateTime now = (DateTime) args["Now"];
            // sql = "insert into MhbtQsData(HospID,ID,Birthday,FuncDate,PrsnID,CureStage,ExamYear,SmokeYear,SmokeMon,SmokeDayNum,BaseWeight,CureWeek,WeekTot,"
            // sql += "SmokeFirst,SmokeStop,SmokeNoGp,SmokeMuch,SmokeBed,SmokeSick,SmokeNico,SmokeLung,SmokeScore,CureAgree,BranchCode,TxtDate,AdjustUserID,"
            // sql += "FeeMark,CoCheck,TraceDate,TraceState,CurtState,TraceCoCheck,SideEffect,CureStateOther,"
            // sql += "Trace_Date2,Trace_State2,Cure_State2,Trace_Co_Check2,Case_Source,Cure_Type,Case_Kind,HospSeqNo) values("
            // sql += "'" & strLineArray(0).ToString.Trim() & "',"
            // sql += "'" & StrConv(strLineArray(1).ToString.Trim(), VbStrConv.Narrow) & "',"
            // sql += "'" & strLineArray(2).ToString.Trim() & "',"
            // sql += "'" & strLineArray(3).ToString.Trim() & "',"
            // sql += "'" & strLineArray(4).ToString.Trim() & "',"
            // sql += "'" & strLineArray(5).ToString.Trim() & "',"
            // sql += "'" & strLineArray(6).ToString.Trim() & "',"
            // sql += "" & CheckZero(strLineArray(7).ToString.Trim()) & ","
            // sql += "" & CheckZero(strLineArray(8).ToString.Trim()) & ","
            // sql += "" & CheckZero(strLineArray(9).ToString.Trim()) & ","
            // sql += "" & CheckZero(strLineArray(10).ToString.Trim()) & ","
            // sql += "" & CheckZero(strLineArray(11).ToString.Trim()) & ","
            // sql += "" & CheckZero(strLineArray(12).ToString.Trim()) & ","
            // sql += "'" & strLineArray(13).ToString.Trim() & "',"
            // sql += "'" & strLineArray(14).ToString.Trim() & "',"
            // sql += "'" & strLineArray(15).ToString.Trim() & "',"
            // sql += "'" & strLineArray(16).ToString.Trim() & "',"
            // sql += "'" & strLineArray(17).ToString.Trim() & "',"
            // sql += "'" & strLineArray(18).ToString.Trim() & "',"
            // sql += "'','',"
            // sql += "'" & CheckNull(strLineArray(19).ToString.Trim()) & "',"
            // sql += "'" & strLineArray(20).ToString.Trim() & "',"
            // sql += "'" & strLineArray(21).ToString.Trim() & "',"
            //
            // sql += "'" & Mid(strLineArray(22).ToString.Trim(), 1, 8) & "',"
            // sql += "'" & strLineArray(23).ToString.Trim() & "',"
            //
            // sql += "'" & strLineArray(24).ToString.Trim() & "',"
            // If strLineArray(25).ToString.Trim() = "" Then
            //     sql += "NULL,"
            // Else
            //     sql += "" & CheckZero(strLineArray(25).ToString.Trim()) & ","
            // End If
            // sql += "'" & strLineArray(26).ToString.Trim() & "',"
            // sql += "'" & strLineArray(27).ToString.Trim() & "',"
            // sql += "'" & strLineArray(28).ToString.Trim() & "',"
            // If strLineArray(29).ToString.Trim() = "" Then
            //     sql += "NULL,"
            // Else
            //     sql += "" & CheckZero(strLineArray(29).ToString.Trim()) & ","
            // End If
            // sql += "'','',"
            // sql += "'" & strLineArray(30).ToString.Trim() & "',"
            // sql += "'" & strLineArray(31).ToString.Trim() & "',"
            // sql += "'" & strLineArray(32).ToString.Trim() & "',"
            // sql += "'" & strLineArray(33).ToString.Trim() & "',"
            // sql += "'" & strLineArray(34).ToString.Trim() & "',"
            // sql += "'" & strLineArray(35).ToString.Trim() & "',"
            // sql += "'" & strLineArray(36).ToString.Trim() & "',"
            // sql += "'" & strLineArray(37).ToString.Trim() & "')"
            return new MhbtQsData()
            {
                HospId = values[0].Trim(),
                ID = values[1].Trim(),
                Birthday = values[2].Trim(),
                FuncDate = values[3].Trim(),
                PrsnID = values[4].Trim(),
                CureStage = values[5].Trim(),
                ExamYear = values[6].Trim(),
                SmokeYear = values[7].Trim().ToDecimal(),
                SmokeMon = values[8].Trim().ToDecimal(),
                SmokeDayNum = values[9].Trim().ToDecimal(),
                BaseWeight = values[10].Trim().ToDecimal(),
                CureWeek = values[11].Trim().ToDecimal(),
                WeekTot = values[12].Trim().ToDecimal(),
                SmokeFirst = values[13].Trim(),
                SmokeStop = values[14].Trim(),
                SmokeNoGp = values[15].Trim(),
                SmokeMuch = values[16].Trim(),
                SmokeBed = values[17].Trim(),
                SmokeSick = values[18].Trim(),
                SmokeNico = "",
                SmokeLung = "",
                SmokeScore = values[19].Trim().ToDecimal(),
                CureAgree = values[20].Trim(),
                BranchCode = values[21].Trim(),
                TxtDate = values[22].Trim().Substring(0, 8),
                AdjustUserId = values[23].Trim(),
                FeeMark = values[24].Trim(),
                CoCheck = values[25].Trim().ToDecimal(),
                TraceDate = values[26].Trim(),
                TraceState = values[27].Trim(),
                CurtState = values[28].Trim(),
                TraceCoCheck = values[29].Trim().ToDecimal(),
                SideEffect = "",
                CureStateOther = "",
                TraceDate2 = values[30].Trim(),
                TraceState2 = values[31].Trim(),
                CureState2 = values[32].Trim(),
                TraceCoCheck2 = values[33].Trim().ToDecimal(),
                CaseSource = values[34].Trim(),
                CureType = values[35].Trim(),
                CaseKind = values[36].Trim(),
                HospSeqNo = values[37].Trim(),
            };
        }
    }
}
