using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using SMK.Data.Utility;
using ICCardDataHelper;
using ICCardConsole.App_Code;
using System.Configuration;
using Microsoft.VisualBasic;
using System.Data.SqlClient;
using System.Configuration;
/// <summary>
/// Utility 的摘要描述
/// </summary>
public class Utility
{


    public Utility()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //

    }

    #region ICCard系統輔助功能

    public static int OpenICCardTxt(string filePath)
    {
        ICCard ICData = new ICCard();
        var i = 0;
        //透過路徑讀取檔案
        StreamReader sr = new StreamReader(filePath);
        string line;

        sr.ReadLine(); //跳過第一行
        while ((line = sr.ReadLine()) != null)
        {
            //開始一行一行讀取            
            string[] Data = line.Split(',');
            if (Data.Length == 21)
            {
                CountNum(i++);
                ICData.DataType = Convert.ToInt32(Data[0].ToString());
                ICData.PersonID = Data[1].ToString();
                ICData.Birthday = BirthdayConvertDate(Data[2].ToString());
                ICData.HospitalCode = Data[3].ToString();
                ICData.PhysicianPersonID = Data[4].ToString();
                ICData.ReadCardDatetime = ReadCardDatetimeConvertDate(Data[5].ToString());
                ICData.MedicalSeries = Data[6].ToString();
                ICData.ReissueNote = Data[7].ToString();
                ICData.MedicalType = Data[8].ToString();
                ICData.MainMedicalCode = Data[9].ToString();
                ICData.MinorMedicalCodeFirst = Data[10].ToString();
                ICData.MinorMedicalCodeSecond = Data[11].ToString();
                ICData.MinorMedicalCodeThird = Data[12].ToString();
                ICData.MinorMedicalCodeFourth = Data[13].ToString();
                ICData.MinorMedicalCodeFifth = Data[14].ToString();
                ICData.MedicalDate = BirthdayConvertDate(Data[15].ToString());
                ICData.PhysicianOrderType = Data[16].ToString();
                ICData.TreatCode = Data[17].ToString();
                ICData.MedicineMethod = Data[18].ToString();
                ICData.MedicineDay = Data[19].ToString();
                ICData.MedicineCount = Data[20].ToString();
                InsertData(ICData);

            }
            else
            {
                string fileName = @"ExceptionFile\ICDataException" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                WriteException(fileName, line);

            }

        }
        sr.Close();
        sr.Dispose();
        return i;
    }

    public static void DeleteICCard()
    {
        DataHelper ObjDH = new DataHelper();
        string sql = @"Delete ICCardData";
        ObjDH.executeNonQuery(sql, null);
    }

    public static void InsertData(ICCard iCCard)
    {

        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("DataType", iCCard.DataType);
        adict.Add("PersonID", iCCard.PersonID);
        adict.Add("Birthday", iCCard.Birthday);
        adict.Add("HospitalCode", iCCard.HospitalCode);
        adict.Add("PhysicianPersonID", iCCard.PhysicianPersonID);
        adict.Add("ReadCardDatetime", iCCard.ReadCardDatetime);
        adict.Add("medicalSeries", iCCard.MedicalSeries);
        adict.Add("ReissueNote", iCCard.ReissueNote);
        adict.Add("MedicalType", iCCard.MedicalType);
        adict.Add("MainMedicalCode", iCCard.MainMedicalCode);
        adict.Add("MinorMedicalCodeFirst", iCCard.MinorMedicalCodeFirst);
        adict.Add("MinorMedicalCodeSecond", iCCard.MinorMedicalCodeSecond);
        adict.Add("MinorMedicalCodeThird", iCCard.MinorMedicalCodeThird);
        adict.Add("MinorMedicalCodefourth", iCCard.MinorMedicalCodeFourth);
        adict.Add("MinorMedicalCodefifth", iCCard.MinorMedicalCodeFifth);
        if (iCCard.MedicalDate == null)
        {
            adict.Add("MedicalDate", DBNull.Value);
        }
        else
        {
            adict.Add("MedicalDate", iCCard.MedicalDate);
        }

        adict.Add("PhysicianOrderType", iCCard.PhysicianOrderType);
        adict.Add("TreatCode", iCCard.TreatCode);
        adict.Add("MedicineMethod", iCCard.MedicineMethod);
        adict.Add("MedicineDay", iCCard.MedicineDay);
        adict.Add("MedicineCount", iCCard.MedicineCount);
        string Sql = @"INSERT INTO [dbo].[ICCardData]
           ([DataType]
           ,[PersonID]
           ,[Birthday]
           ,[HospitalCode]
           ,[PhysicianPersonID]
           ,[ReadCardDatetime]
           ,[MedicalSeries]
           ,[ReissueNote]
           ,[MedicalType]
           ,[MainMedicalCode]
           ,[MinorMedicalCodeFirst]
           ,[MinorMedicalCodeSecond]
           ,[MinorMedicalCodeThird]
           ,[MinorMedicalCodeFourth]
           ,[MinorMedicalCodeFifth]
           ,[MedicalDate]
           ,[PhysicianOrderType]
           ,[TreatCode]
           ,[MedicineMethod]
           ,[MedicineDay]
           ,[MedicineCount])
     VALUES
           (@DataType
           ,@PersonID
           ,@Birthday
           ,@HospitalCode
           ,@PhysicianPersonID
           ,@ReadCardDatetime
           ,@MedicalSeries
           ,@ReissueNote
           ,@MedicalType
           ,@MainMedicalCode
           ,@MinorMedicalCodeFirst
           ,@MinorMedicalCodeSecond
           ,@MinorMedicalCodeThird
           ,@MinorMedicalCodeFourth
           ,@MinorMedicalCodeFifth
           ,@MedicalDate
           ,@PhysicianOrderType
           ,@TreatCode
           ,@MedicineMethod
           ,@MedicineDay
           ,@MedicineCount)";
        ObjDH.executeNonQuery(Sql, adict);

    }


    public static DateTime? BirthdayConvertDate(string Date)
    {
        if (Date != "")
        {
            string DateTimestring = (Convert.ToInt32(Date.Substring(0, 3)) + 1911).ToString() + Date.Substring(Date.Length - 4);
            DateTime Birthday = DateTime.ParseExact(DateTimestring, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            return Birthday;
        }
        else
        {
            return null;
        }

    }
    public static DateTime? ReadCardDatetimeConvertDate(string Date)
    {
        if (Date != "")
        {

            string DateTimestring = (Convert.ToInt32(Date.Substring(0, 3)) + 1911).ToString() + Date.Substring(3, Date.Length - 9);
            DateTime ReadCardDatetime = DateTime.ParseExact(DateTimestring, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            return ReadCardDatetime;
        }
        else
        {
            return null;
        }
    }

    public static int CountNum(int Num)
    {
        Num++;
        return Num;
    }
    #endregion

    #region VPNOpeAgentPatient系統輔助功能
    public static void DeleteAgentPatientTxt()
    {
        Dictionary<string, object> adict = new Dictionary<string, object>();
        DataHelper ObjDH = new DataHelper();
        string sql = @"TRUNCATE TABLE [MhbtAgentPatient]";
        ObjDH.executeNonQuery(sql, adict);
    }
    public static int OpeAgentPatientTxt(string filePath)
    {
        CST_AGENT_PATIENT AGENT_PATIENT = new CST_AGENT_PATIENT();
        var i = 0;
        //透過路徑讀取檔案
        StreamReader sr = new StreamReader(filePath);
        string line;
        while ((line = sr.ReadLine()) != null)
        {

            //開始一行一行讀取

            string[] Data = line.Split('|');
            if (Data.Length == 20)
            {


                CountNum(i++);
                AGENT_PATIENT.HospID = Data[0].ToString().Trim();
                AGENT_PATIENT.ID = Strings.StrConv(Data[2].ToString().Trim(), VbStrConv.Narrow);
                AGENT_PATIENT.Birthday = Data[3].ToString().Trim();
                AGENT_PATIENT.HospAgentCode = Data[1].ToString().Trim();
                AGENT_PATIENT.Name = Data[4].ToString().Trim();
                AGENT_PATIENT.Sex = GetMFString(Data[2].ToString().Trim());
                AGENT_PATIENT.Inform_ADDR = Data[6].ToString().Trim().Replace("'", "");
                AGENT_PATIENT.Tel_D = TelFormat(Data[7].ToString().Trim().Replace("-", "") + "-" + Data[8].ToString().Trim().Replace("-", "") + "#" + Data[9].ToString().Trim().Replace("-", ""));
                AGENT_PATIENT.Tel_N = TelFormat(Data[10].ToString().Trim().Replace("-", "") + "-" + Data[11].ToString().Trim().Replace("-", "") + "#" + Data[12].ToString().Trim().Replace("-", ""));
                AGENT_PATIENT.Tel_M = Data[13].ToString().Trim();
                AGENT_PATIENT.Seq_No = Data[14].ToString().Trim();
                AGENT_PATIENT.Branch_Code = Data[15].ToString().Trim();
                AGENT_PATIENT.Func_Mark = Data[17].ToString().Trim();
                AGENT_PATIENT.Txt_Date = Strings.Mid(Data[16].ToString().Trim(), 1, 8);
                AGENT_PATIENT.Town_Code = Data[18].ToString().Trim();
                AGENT_PATIENT.Town_Name = Data[19].ToString().Trim();

                VPNInsertData(AGENT_PATIENT);
            }
            else
            {
                string fileName = @"ExceptionFile\Agent_patientException" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                WriteException(fileName, line);

            }

        }
        sr.Close();
        sr.Dispose();
        return i;
    }

    public static void VPNInsertData(CST_AGENT_PATIENT cST_AGENT_PATIENT)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("HospID", cST_AGENT_PATIENT.HospID);
        adict.Add("HospAgentCode", cST_AGENT_PATIENT.HospAgentCode);
        adict.Add("ID", cST_AGENT_PATIENT.ID);
        adict.Add("Birthday", cST_AGENT_PATIENT.Birthday);
        adict.Add("Name", cST_AGENT_PATIENT.Name);
        adict.Add("Sex", cST_AGENT_PATIENT.Sex);
        adict.Add("Inform_ADDR", cST_AGENT_PATIENT.Inform_ADDR);
        adict.Add("Tel_D", cST_AGENT_PATIENT.Tel_D);
        adict.Add("Tel_N", cST_AGENT_PATIENT.Tel_N);
        adict.Add("Tel_M", cST_AGENT_PATIENT.Tel_M);
        adict.Add("Seq_No", cST_AGENT_PATIENT.Seq_No);
        adict.Add("Branch_Code", cST_AGENT_PATIENT.Branch_Code);
        adict.Add("Txt_Date", cST_AGENT_PATIENT.Txt_Date);
        adict.Add("Func_Mark", cST_AGENT_PATIENT.Func_Mark);
        adict.Add("Town_Code", cST_AGENT_PATIENT.Town_Code);
        adict.Add("Town_Name", cST_AGENT_PATIENT.Town_Name);
        string sql = @"insert into MhbtAgentPatient(HospID,ID,Birthday,HospAgentCode,Name,Sex,InformADDR,TelD,TelN,TelM,SeqNo,BranchCode,FuncMark,TxtDate,TownCode,TownName)
     VALUES
           (@HospID
           ,@ID
           ,@BIRTHDAY
           ,@HospAgentCode         
           ,@Name
           ,@Sex
           ,@Inform_ADDR
           ,@TEL_D
           ,@TEL_N
           ,@Tel_M
           ,@Seq_No
           ,@Branch_Code
           ,@Func_Mark
           ,@TXT_Date
           ,@Town_Code
           ,@Town_Name)";
        ObjDH.executeNonQuery(sql, adict);
    }
    #endregion

    #region VPNQSCureTxt系統輔助功能
    public static void DeleteQSCureTxt()
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql = @"TRUNCATE TABLE [MhbtQsCure]";
        ObjDH.executeNonQuery(sql, adict);
    }
    public static int OpeQSCureTxt(string filePath)
    {
        CST_QS_CURE QS_CURE = new CST_QS_CURE();
        var i = 0;
        //透過路徑讀取檔案
        StreamReader sr = new StreamReader(filePath);
        string line;
        while ((line = sr.ReadLine()) != null)
        {

            //開始一行一行讀取

            string[] Data = line.Split('|');
            if (Data.Length == 9)
            {
                CountNum(i++);
                QS_CURE.HospID = Data[0].ToString().Trim();
                QS_CURE.ID = Strings.StrConv(Data[1].ToString().Trim(), VbStrConv.Narrow);
                QS_CURE.Birthday = Data[2].ToString().Trim();
                QS_CURE.FuncDate = Data[3].ToString().Trim();
                QS_CURE.CureItem = Data[4].ToString();
                QS_CURE.CureNum = CheckZero(Data[5].ToString().Trim());
                QS_CURE.TxtDate = Strings.Mid(Data[6].ToString().Trim(), 1, 8);
                QS_CURE.AdjustUserID = Data[7].ToString().Trim();
                QS_CURE.HospSeqNo = Data[8].ToString().Trim();

                QSCureInsertData(QS_CURE);
            }
            else
            {
                string fileName = @"ExceptionFile\QSCureException" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                WriteException(fileName, line);

            }

        }
        sr.Close();
        sr.Dispose();
        return i;
    }
    public static void QSCureInsertData(CST_QS_CURE QS_CURE)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("HospID", QS_CURE.HospID);
        adict.Add("ID", QS_CURE.ID);
        adict.Add("Birthday", QS_CURE.Birthday);
        adict.Add("FuncDate", QS_CURE.FuncDate);
        adict.Add("CureItem", QS_CURE.CureItem);
        adict.Add("CureNum", QS_CURE.CureNum);
        adict.Add("TxtDate", QS_CURE.TxtDate);
        adict.Add("AdjustUserID", QS_CURE.AdjustUserID);
        adict.Add("HospSeqNo", QS_CURE.HospSeqNo);

        string sql = @"INSERT INTO [dbo].[MhbtQsCure]
           ([HospID]
           ,[ID]
           ,[Birthday]
           ,[FuncDate]
           ,[CureItem]
           ,[CureNum]
           ,[TxtDate]
           ,[AdjustUserID]
           ,[HospSeqNo])
     VALUES
           (@HospID
           ,@ID
           ,@Birthday
           ,@FuncDate
           ,@CureItem
           ,@CureNum
           ,@TxtDate
           ,@AdjustUserID
           ,@HospSeqNo)";
        ObjDH.executeNonQuery(sql, adict);
    }
    #endregion

    #region VPNQSDataTxt系統輔助功能
    public static void DeleteQsData()
    {
        Dictionary<string, object> adict = new Dictionary<string, object>();
        DataHelper ObjDH = new DataHelper();
        string sql = @"TRUNCATE TABLE [MhbtQsData]";
        ObjDH.executeNonQuery(sql, adict);
    }
    public static int OpeQSDataTxt(string filePath)
    {
        CST_QS_DATA QS_DATA = new CST_QS_DATA();
        var i = 0;
        //透過路徑讀取檔案
        StreamReader sr = new StreamReader(filePath);
        string line;
        while ((line = sr.ReadLine()) != null)
        {

            //開始一行一行讀取

            string[] Data = line.Split('|');
            if (Data.Length == 38)
            {
                CountNum(i++);
                QS_DATA.HospID = Data[0].ToString().Trim();
                QS_DATA.ID = Strings.StrConv(Data[1].ToString().Trim(), VbStrConv.Narrow);
                QS_DATA.Birthday = Data[2].ToString().Trim();
                QS_DATA.FuncDate = Data[3].ToString().Trim();
                QS_DATA.PrsnID = Data[4].ToString().Trim();
                QS_DATA.CureStage = Data[5].ToString().Trim();
                QS_DATA.ExamYear = Data[6].ToString().Trim();
                QS_DATA.SmokeYear = CheckZero(Data[7].ToString().Trim());
                QS_DATA.SmokeMon = CheckZero(Data[8].ToString().Trim());
                QS_DATA.SmokeDayNum = CheckZero(Data[9].ToString().Trim());
                QS_DATA.BaseWeight = CheckZero(Data[10].ToString().Trim());
                QS_DATA.CureWeek = CheckZero(Data[11].ToString().Trim());
                QS_DATA.WeekTot = CheckZero(Data[12].ToString().Trim());
                QS_DATA.SmokeFirst = Data[13].ToString().Trim();
                QS_DATA.SmokeStop = Data[14].ToString().Trim();
                QS_DATA.SmokeNoGp = Data[15].ToString().Trim();
                QS_DATA.SmokeMuch = Data[16].ToString().Trim();
                QS_DATA.SmokeBed = Data[17].ToString().Trim();
                QS_DATA.SmokeSick = Data[18].ToString().Trim();
                QS_DATA.SmokeNico = "";
                QS_DATA.SmokeLung = "";
                QS_DATA.SmokeScore = Data[19].ToString().Trim();
                QS_DATA.CureAgree = Data[20].ToString().Trim();
                QS_DATA.BranchCode = Data[21].ToString().Trim();
                QS_DATA.TxtDate = Strings.Mid(Data[22].ToString().Trim(), 1, 8);
                QS_DATA.AdjustUserID = Data[23].ToString().Trim() == "" ? "Null" : CheckNull(Data[23].ToString().Trim());
                QS_DATA.FeeMark = Data[24].ToString().Trim();
                QS_DATA.CoCheck = Data[25].ToString().Trim() == "" ? null : CheckZero(Data[25].ToString().Trim());
                QS_DATA.TraceDate = Data[26].ToString().Trim();
                QS_DATA.TraceState = Data[27].ToString().Trim();
                QS_DATA.CurtState = Data[28].ToString().Trim();
                QS_DATA.TraceCoCheck = Data[29].ToString() == "" ? null : CheckZero(Data[29].ToString().Trim());
                QS_DATA.SideEffect = "";
                QS_DATA.CureStateOther = "";
                QS_DATA.Trace_Date2 = Data[30].ToString().Trim();
                QS_DATA.Trace_State2 = Data[31].ToString().Trim();
                QS_DATA.Cure_State2 = Data[32].ToString().Trim();
                QS_DATA.Trace_Co_Check2 = Data[33].ToString().Trim();
                QS_DATA.Case_Source = Data[34].ToString().Trim();
                QS_DATA.Cure_Type = Data[35].ToString().Trim();
                QS_DATA.Case_Kind = Data[36].ToString().Trim();
                QS_DATA.HospSeqNo = Data[37].ToString().Trim();


                QSDataInsertData(QS_DATA);
            }
            else
            {
                string fileName = @"ExceptionFile\QSDataException" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                WriteException(fileName, line);

            }

        }
        sr.Close();
        sr.Dispose();
        ExecuteUpdateMhbtQsData();
        return i;
    }
    public static void QSDataInsertData(CST_QS_DATA QS_CURE)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("HospID", QS_CURE.HospID);
        adict.Add("ID", QS_CURE.ID);
        adict.Add("Birthday", QS_CURE.Birthday);
        adict.Add("FuncDate", QS_CURE.FuncDate);
        adict.Add("PrsnID", QS_CURE.PrsnID);
        adict.Add("CureStage", QS_CURE.CureStage);
        adict.Add("ExamYear", QS_CURE.ExamYear);
        adict.Add("SmokeYear", QS_CURE.SmokeYear);
        adict.Add("SmokeMon", QS_CURE.SmokeMon);
        adict.Add("SmokeDayNum", QS_CURE.SmokeDayNum);
        adict.Add("BaseWeight", QS_CURE.BaseWeight);
        adict.Add("CureWeek", QS_CURE.CureWeek);
        adict.Add("WeekTot", QS_CURE.WeekTot);
        adict.Add("SmokeFirst", QS_CURE.SmokeFirst);
        adict.Add("SmokeStop", QS_CURE.SmokeStop);
        adict.Add("SmokeNoGp", QS_CURE.SmokeNoGp);
        adict.Add("SmokeMuch", QS_CURE.SmokeMuch);
        adict.Add("SmokeBed", QS_CURE.SmokeBed);
        adict.Add("SmokeSick", QS_CURE.SmokeSick);
        adict.Add("SmokeNico", QS_CURE.SmokeNico);
        adict.Add("SmokeLung", QS_CURE.SmokeLung);
        adict.Add("SmokeScore", QS_CURE.SmokeScore);
        adict.Add("CureAgree", QS_CURE.CureAgree);
        adict.Add("BranchCode", QS_CURE.BranchCode);
        adict.Add("TxtDate", QS_CURE.TxtDate);
        adict.Add("AdjustUserID", QS_CURE.AdjustUserID);
        adict.Add("FeeMark", QS_CURE.FeeMark);
        if (QS_CURE.CoCheck == null)
        {
            adict.Add("CoCheck", DBNull.Value);
        }
        else
        {
            adict.Add("CoCheck", QS_CURE.CoCheck);
        }
        adict.Add("TraceDate", QS_CURE.TraceDate);
        adict.Add("TraceState", QS_CURE.TraceState);
        adict.Add("CurtState", QS_CURE.CurtState);
        if (QS_CURE.TraceCoCheck == null)
        {
            adict.Add("TraceCoCheck", DBNull.Value);
        }
        else
        {
            adict.Add("TraceCoCheck", QS_CURE.TraceCoCheck);
        }
        adict.Add("SideEffect", QS_CURE.SideEffect);
        adict.Add("CureStateOther", QS_CURE.CureStateOther);
        adict.Add("Trace_Date2", QS_CURE.Trace_Date2);
        adict.Add("Trace_State2", QS_CURE.Trace_State2);
        adict.Add("Cure_State2", QS_CURE.Cure_State2);
        adict.Add("Trace_Co_Check2", QS_CURE.Trace_Co_Check2);
        adict.Add("Case_Source", QS_CURE.Case_Source);
        adict.Add("Cure_Type", QS_CURE.Cure_Type);
        adict.Add("Case_Kind", QS_CURE.Case_Kind);
        adict.Add("HospSeqNo", QS_CURE.HospSeqNo);
        string sql = @"INSERT INTO [dbo].[MhbtQsData]
           ([HospID]
           ,[ID]
           ,[Birthday]
           ,[FuncDate]
           ,[PrsnID]
           ,[CureStage]
           ,[ExamYear]
           ,[SmokeYear]
           ,[SmokeMon]
           ,[SmokeDayNum]
           ,[BaseWeight]
           ,[CureWeek]
           ,[WeekTot]
           ,[SmokeFirst]
           ,[SmokeStop]
           ,[SmokeNoGp]
           ,[SmokeMuch]
           ,[SmokeBed]
           ,[SmokeSick]
           ,[SmokeNico]
           ,[SmokeLung]
           ,[SmokeScore]
           ,[CureAgree]
           ,[BranchCode]
           ,[TxtDate]
           ,[AdjustUserID]
           ,[FeeMark]
           ,[CoCheck]
           ,[TraceDate]
           ,[TraceState]
           ,[CurtState]
           ,[TraceCoCheck]
           ,[SideEffect]
           ,[CureStateOther]
   
           ,[Trace_Date2]
           ,[Trace_State2]
           ,[Cure_State2]
           ,[Trace_Co_Check2]
           ,[Case_Source]
           ,[Cure_Type]
           ,[Case_Kind]
           ,[HospSeqNo])
     VALUES
           (@HospID
           ,@ID
           ,@Birthday
           ,@FuncDate
           ,@PrsnID
           ,@CureStage
           ,@ExamYear
           ,@SmokeYear
           ,@SmokeMon
           ,@SmokeDayNum
           ,@BaseWeight
           ,@CureWeek
           ,@WeekTot
           ,@SmokeFirst
           ,@SmokeStop
           ,@SmokeNoGp
           ,@SmokeMuch
           ,@SmokeBed
           ,@SmokeSick
           ,@SmokeNico
           ,@SmokeLung
           ,@SmokeScore
           ,@CureAgree
           ,@BranchCode
           ,@TxtDate
           ,@AdjustUserID
           ,@FeeMark
           ,@CoCheck
           ,@TraceDate
           ,@TraceState
           ,@CurtState
           ,@TraceCoCheck
           ,@SideEffect
           ,@CureStateOther
           ,@Trace_Date2
           ,@Trace_State2
           ,@Cure_State2
           ,@Trace_Co_Check2
           ,@Case_Source
           ,@Cure_Type
           ,@Case_Kind
           ,@HospSeqNo)";
        ObjDH.executeNonQuery(sql, adict);
    }
    public static void ExecuteUpdateMhbtQsData()
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        objDH.queryData("UpdateMhbtQsData", null);
    }
    public static DateTime? ConvertD(string Datetimes)
    {
        if (Datetimes != "")
        {
            try
            {
                DateTime ConvertDate = DateTime.ParseExact(Datetimes, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                return ConvertDate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        else
        {
            return null;
        }

    }

    #endregion

    #region VPNQsStateTxt系統輔助功能
    public static void DeleteQsState()
    {
        Dictionary<string, object> adict = new Dictionary<string, object>();
        DataHelper ObjDH = new DataHelper();
        string sql = @"TRUNCATE TABLE [MhbtQsState]";
        ObjDH.executeNonQuery(sql, adict);
    }
    public static int OpeQSStateTxt(string filePath)
    {
        CST_QS_STATE QS_STATE = new CST_QS_STATE();
        var i = 0;
        //透過路徑讀取檔案
        StreamReader sr = new StreamReader(filePath);
        string line;
        while ((line = sr.ReadLine()) != null)
        {

            //開始一行一行讀取

            string[] Data = line.Split('|');
            if (Data.Length == 10)
            {
                CountNum(i++);
                QS_STATE.HospID = Data[0].ToString().Trim();
                QS_STATE.ID = Strings.StrConv(Data[1].ToString().Trim(), VbStrConv.Narrow);
                QS_STATE.Birthday = Data[2].ToString().Trim();
                QS_STATE.FuncDate = Data[3].ToString().Trim();
                QS_STATE.CureState = Data[4].ToString().Trim();
                QS_STATE.CureStateOther = Data[5].ToString().Trim().Replace("'", "");
                QS_STATE.CureType = Data[8].ToString();
                QS_STATE.SeqNo = i;
                QS_STATE.TxtDate = Strings.Mid(Data[6].ToString().Trim(), 1, 8);
                QS_STATE.AdjustUserID = Data[7].ToString().Trim(); ;
                QS_STATE.HospSeqNo = Data[9].ToString().Trim(); ;

                QSStateInsertData(QS_STATE);
            }
            else
            {
                string fileName = @"ExceptionFile\QSStateException" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                WriteException(fileName, line);

            }

        }
        sr.Close();
        sr.Dispose();
        return i;
    }
    public static void QSStateInsertData(CST_QS_STATE QS_STATE)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("HospID", QS_STATE.HospID);
        adict.Add("ID", QS_STATE.ID);
        adict.Add("Birthday", QS_STATE.Birthday);
        adict.Add("FuncDate", QS_STATE.FuncDate);
        adict.Add("SeqNo", QS_STATE.SeqNo);
        adict.Add("CureState", QS_STATE.CureState);
        adict.Add("CureStateOther", QS_STATE.CureStateOther);
        adict.Add("TxtDate", QS_STATE.TxtDate);
        adict.Add("AdjustUserID", QS_STATE.AdjustUserID);
        adict.Add("Cure_Type", QS_STATE.CureType);
        adict.Add("HospSeqNo", QS_STATE.HospSeqNo);

        string sql = @"INSERT INTO [dbo].[MhbtQsState]
           ([HospID]
           ,[ID]
           ,[Birthday]
           ,[FuncDate]
           ,[SeqNo]
           ,[CureState]
           ,[CureStateOther]
           ,[TxtDate]
           ,[AdjustUserID]
           ,[Cure_Type]
           ,[HospSeqNo])
     VALUES
           (@HospID
           ,@ID
           ,@Birthday
           ,@FuncDate
           ,@SeqNo
           ,@CureState
           ,@CureStateOther
           ,@TxtDate
           ,@AdjustUserID
           ,@Cure_Type
           ,@HospSeqNo)";
        ObjDH.executeNonQuery(sql, adict);
    }
    #endregion

    #region iniDrDtl系統輔助功能

    public static int OpeniniDrDtlTxt(string filePath)
    {

        iniDrDtl iniDrDtl = new iniDrDtl();
        var i = 0;
        //透過路徑讀取檔案
        StreamReader sr = new StreamReader(filePath);

        string line;
        sr.ReadLine(); //跳過第一行
        while ((line = sr.ReadLine()) != null)
        {
            //開始一行一行讀取            
            string[] Data = line.Split(',');
            if (Data.Length == 46)
            {
                CountNum(i++);
                string wkData_id = Data[3].ToString().Trim() + Data[0].ToString().Trim() +
                                    (Int32.Parse(Data[4].ToString().Trim().Substring(0, 4)) - 1911).ToString().PadLeft(3, '0') + Data[4].ToString().Trim().Substring(4, 2) + Data[4].ToString().Trim().Substring(6, 2) +
                                    Data[5].ToString().Trim().PadLeft(2, '0') + Data[6].ToString().Trim().PadLeft(6, '0') + "30";
                iniDrDtl.data_id = wkData_id;
                iniDrDtl.HospID = Data[0].ToString().Trim();
                iniDrDtl.fee_ym = Data[2].ToString().Trim();
                iniDrDtl.ExamYear = Data[2].ToString().Trim().Substring(0, 4);
                iniDrDtl.InstructExamYear = string.Empty;
                iniDrDtl.appl_type = Data[3].ToString().Trim();
                iniDrDtl.appl_date = Data[4].ToString().Trim();
                iniDrDtl.case_type = Data[5].ToString().Trim();
                iniDrDtl.seq_no = returnNum(Data[6]);
                iniDrDtl.cure_item1 = Data[10].ToString().Trim();
                iniDrDtl.cure_item2 = Data[11].ToString().Trim();
                iniDrDtl.cure_item3 = Data[12].ToString().Trim();
                iniDrDtl.cure_item4 = Data[13].ToString().Trim();
                iniDrDtl.func_type = Data[14].ToString().Trim();
                iniDrDtl.func_date = Data[15].ToString().Trim();

                iniDrDtl.rel_date = Data[16].ToString().Trim();
                iniDrDtl.birthday = Data[17].ToString().Trim();
                iniDrDtl.id = Data[18].ToString().Trim();
                iniDrDtl.func_seq_no = Data[29].ToString().Trim();
                iniDrDtl.pay_type = Data[19].ToString().Trim();
                iniDrDtl.part_code = Data[23].ToString().Trim();
                iniDrDtl.icd9cm_code = Data[20].ToString().Trim().Length >= 5 ? Data[20].ToString().Trim().Substring(0, 5) : string.Empty;
                iniDrDtl.icd9cm_code1 = Data[21].ToString().Trim().Length >= 5 ? Data[21].ToString().Trim().Substring(0, 5) : string.Empty;
                iniDrDtl.icd9cm_code2 = Data[22].ToString().Trim().Length >= 5 ? Data[22].ToString().Trim().Substring(0, 5) : string.Empty;
                iniDrDtl.drug_days = Int32.Parse(Data[25].ToString().Trim());

                iniDrDtl.prsn_id = Data[26].ToString().Trim();
                iniDrDtl.drug_prsn_id = Data[27].ToString().Trim();
                iniDrDtl.drug_dot = Int32.Parse(Data[28].ToString().Trim());
                iniDrDtl.cure_dot = Int32.Parse(Data[30].ToString().Trim());
                iniDrDtl.dsvc_code = Data[31].ToString().Trim();
                iniDrDtl.dsvc_dot = Int32.Parse(Data[32].ToString().Trim());
                iniDrDtl.exp_dot = Int32.Parse(Data[35].ToString().Trim());
                iniDrDtl.part_amt = Int32.Parse(Data[34].ToString().Trim());
                iniDrDtl.appl_dot = Int32.Parse(Data[33].ToString().Trim());
                iniDrDtl.orig_hosp_id = Data[7].ToString().Trim();
                iniDrDtl.Id_Sex = Utility.GetMFString(Data[18].ToString().Trim());

                iniDrDtl.orig_case_type = Data[9].ToString().Trim();
                iniDrDtl.other_part_amt = Int32.Parse(Data[38].ToString().Trim());
                iniDrDtl.appl_cause_mark = Data[39].ToString().Trim();
                iniDrDtl.icd10cm_code2 = Data[40].ToString().Trim();
                iniDrDtl.icd10cm_code3 = Data[41].ToString().Trim();
                iniDrDtl.icd10cm_code4 = Data[42].ToString().Trim();
                iniDrDtl.corr_hosp_id = Data[43].ToString().Trim();
                iniDrDtl.area_service = Data[44].ToString().Trim();

                iniDrDtl.name = Data[36].ToString().Trim();
                iniDrDtl.tran_date = Data[45].ToString().Trim();

                //iniDrDtl.ExamTime = returnNum(Data[4]);
                iniDrDtl.FirstTreatDate = string.Empty;
                //iniDrDtl.WeekCount = returnNum(Data[6]);

                //iniDrDtl.InstructExamTime = returnNum(Data[8]);
                iniDrDtl.FirstInstructDate = Data[9].ToString();
                //iniDrDtl.InctructSerial = returnNum(Data[10]);
                iniDrDtl.MedApply = string.Empty;
                iniDrDtl.InstructApply = string.Empty;
                iniDrDtl.TraceApply = string.Empty;
                iniDrDtl.ReleaseApply = string.Empty;


                iniDrDtlInsertData(iniDrDtl);
            }
            else
            {
                string fileName = @"ExceptionFile\iniDrDtlException" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                WriteException(fileName, line);

            }


        }
        sr.Close();
        sr.Dispose();
        return i;
    }
    //public static string GetMFString(string GetString)
    //{
    //    if (GetString.Trim() == "") {
    //        return "U";
    //    }
    //    switch (GetString.Trim().Substring(1,1))
    //    {
    //        case "1":
    //        case "A":
    //        case "C":
    //        case "X":
    //            return "M";
    //        case "2":
    //        case "B":
    //        case "D":
    //        case "Y":
    //            return "F";
    //        default:
    //            return "U";
    //    }
    //}

    public static void iniDrDtlInsertData(iniDrDtl iniDrDtl)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("data_id", iniDrDtl.data_id);
        adict.Add("HospID", iniDrDtl.HospID);
        adict.Add("fee_ym", iniDrDtl.fee_ym);
        adict.Add("ExamYear", iniDrDtl.ExamYear);
        if (iniDrDtl.ExamTime == null)
        {
            adict.Add("ExamTime", DBNull.Value);
        }
        else
        {
            adict.Add("ExamTime", iniDrDtl.ExamTime);
        }
        adict.Add("FirstTreatDate", iniDrDtl.FirstTreatDate);
        if (iniDrDtl.WeekCount == null)
        {
            adict.Add("WeekCount", DBNull.Value);
        }
        else
        {
            adict.Add("WeekCount", iniDrDtl.WeekCount);
        }

        adict.Add("InstructExamYear", iniDrDtl.InstructExamYear);
        if (iniDrDtl.InstructExamTime == null)
        {
            adict.Add("InstructExamTime", DBNull.Value);
        }
        else
        {
            adict.Add("InstructExamTime", iniDrDtl.InstructExamTime);
        }
        adict.Add("FirstInstructDate", iniDrDtl.FirstInstructDate);
        if (iniDrDtl.InctructSerial == null)
        {
            adict.Add("InctructSerial", DBNull.Value);
        }
        else
        {
            adict.Add("InctructSerial", iniDrDtl.InctructSerial);
        }
        adict.Add("MedApply", iniDrDtl.MedApply);
        adict.Add("InstructApply", iniDrDtl.InstructApply);
        adict.Add("TraceApply", iniDrDtl.TraceApply);
        adict.Add("ReleaseApply", iniDrDtl.ReleaseApply);
        adict.Add("appl_type", iniDrDtl.appl_type);
        adict.Add("appl_date", iniDrDtl.appl_date);
        adict.Add("case_type", iniDrDtl.case_type);
        if (iniDrDtl.seq_no == null)
        {
            adict.Add("seq_no", DBNull.Value);
        }
        else
        {
            adict.Add("seq_no", iniDrDtl.seq_no);
        }
        adict.Add("func_type", iniDrDtl.func_type);
        adict.Add("func_date", iniDrDtl.func_date);
        adict.Add("rel_date", iniDrDtl.rel_date);
        adict.Add("birthday", iniDrDtl.birthday);
        adict.Add("id", iniDrDtl.id);
        adict.Add("func_seq_no", iniDrDtl.func_seq_no);
        adict.Add("pay_type", iniDrDtl.pay_type);
        adict.Add("part_code", iniDrDtl.part_code);
        adict.Add("icd9cm_code", iniDrDtl.icd9cm_code);
        adict.Add("icd9cm_code1", iniDrDtl.icd9cm_code1);
        adict.Add("icd9cm_code2", iniDrDtl.icd9cm_code2);
        if (iniDrDtl.drug_days == null)
        {
            adict.Add("drug_days", DBNull.Value);
        }
        else
        {
            adict.Add("drug_days", iniDrDtl.drug_days);
        }

        adict.Add("prsn_id", iniDrDtl.prsn_id);
        adict.Add("drug_prsn_id", iniDrDtl.drug_prsn_id);
        if (iniDrDtl.drug_dot == null)
        {
            adict.Add("drug_dot", DBNull.Value);
        }
        else
        {
            adict.Add("drug_dot", iniDrDtl.drug_dot);
        }

        if (iniDrDtl.cure_dot == null)
        {
            adict.Add("cure_dot", DBNull.Value);
        }
        else
        {
            adict.Add("cure_dot", iniDrDtl.cure_dot);
        }

        adict.Add("dsvc_code", iniDrDtl.dsvc_code);
        if (iniDrDtl.dsvc_dot == null)
        {
            adict.Add("dsvc_dot", DBNull.Value);
        }
        else
        {
            adict.Add("dsvc_dot", iniDrDtl.dsvc_dot);
        }
        if (iniDrDtl.exp_dot == null)
        {
            adict.Add("exp_dot", DBNull.Value);
        }
        else
        {
            adict.Add("exp_dot", iniDrDtl.exp_dot);
        }
        if (iniDrDtl.part_amt == null)
        {
            adict.Add("part_amt", DBNull.Value);
        }
        else
        {
            adict.Add("part_amt", iniDrDtl.part_amt);
        }
        if (iniDrDtl.appl_dot == null)
        {
            adict.Add("appl_dot", DBNull.Value);
        }
        else
        {
            adict.Add("appl_dot", iniDrDtl.appl_dot);
        }
        adict.Add("orig_hosp_id", iniDrDtl.orig_hosp_id);
        adict.Add("Id_Sex", iniDrDtl.Id_Sex);
        adict.Add("cure_item1", iniDrDtl.cure_item1);
        adict.Add("cure_item2", iniDrDtl.cure_item2);
        adict.Add("cure_item3", iniDrDtl.cure_item3);
        adict.Add("cure_item4", iniDrDtl.cure_item4);
        adict.Add("orig_case_type", iniDrDtl.orig_case_type);
        if (iniDrDtl.other_part_amt == null)
        {
            adict.Add("other_part_amt", DBNull.Value);
        }
        else
        {
            adict.Add("other_part_amt", iniDrDtl.other_part_amt);
        }

        adict.Add("appl_cause_mark", iniDrDtl.appl_cause_mark);
        adict.Add("icd10cm_code2", iniDrDtl.icd10cm_code2);
        adict.Add("icd10cm_code3", iniDrDtl.icd10cm_code3);
        adict.Add("icd10cm_code4", iniDrDtl.icd10cm_code4);
        adict.Add("corr_hosp_id", iniDrDtl.corr_hosp_id);
        adict.Add("area_service", iniDrDtl.area_service);
        adict.Add("tran_date", iniDrDtl.tran_date);
        adict.Add("name", iniDrDtl.name);
        string sql = @"
            IF NOT EXISTS (Select 1 from iniDrDtl where data_id=@data_id and fee_ym=@fee_ym)
            BEGIN 
            INSERT INTO [dbo].[iniDrDtl] ([data_id],[HospID],[fee_ym],[ExamYear],[ExamTime],[FirstTreatDate],[WeekCount]
            ,[InstructExamYear],[InstructExamTime],[FirstInstructDate],[InctructSerial],[MedApply],[InstructApply],[TraceApply]
            ,[ReleaseApply],[appl_type],[appl_date],[case_type],[seq_no],[func_type],[func_date],[rel_date],[birthday],[id],[func_seq_no]
            ,[pay_type],[part_code],[icd9cm_code],[icd9cm_code1],[icd9cm_code2],[drug_days],[prsn_id],[drug_prsn_id],[drug_dot],[cure_dot]
            ,[dsvc_code],[dsvc_dot],[exp_dot],[part_amt],[appl_dot],[orig_hosp_id],[Id_Sex],[cure_item1],[cure_item2],[cure_item3],[cure_item4]
            ,[orig_case_type],[other_part_amt],[appl_cause_mark],[icd10cm_code2],[icd10cm_code3],[icd10cm_code4],[corr_hosp_id]
            ,[area_service],[tran_date],[name])     
            VALUES
            (@data_id,@HospID,@fee_ym,@ExamYear,@ExamTime,@FirstTreatDate,@WeekCount,@InstructExamYear,@InstructExamTime
            ,@FirstInstructDate,@InctructSerial,@MedApply,@InstructApply,@TraceApply,@ReleaseApply,@appl_type,@appl_date,@case_type
            ,@seq_no,@func_type,@func_date,@rel_date,@birthday,@id,@func_seq_no,@pay_type,@part_code,@icd9cm_code,@icd9cm_code1
            ,@icd9cm_code2,@drug_days,@prsn_id,@drug_prsn_id,@drug_dot,@cure_dot,@dsvc_code,@dsvc_dot,@exp_dot,@part_amt,@appl_dot
            ,@orig_hosp_id,@Id_Sex,@cure_item1,@cure_item2,@cure_item3,@cure_item4,@orig_case_type,@other_part_amt,@appl_cause_mark
            ,@icd10cm_code2,@icd10cm_code3,@icd10cm_code4,@corr_hosp_id,@area_service,@tran_date,@name) 
            END  ";
        ObjDH.executeNonQuery(sql, adict);
    }

    #endregion

    #region iniDrOrd系統輔助功能
    public static int OpeniniDrOrdTxt(string filePath)
    {

        iniDrOrd iniDrOrd = new iniDrOrd();
        var i = 0;
        //透過路徑讀取檔案
        StreamReader sr = new StreamReader(filePath);
        string line;
        sr.ReadLine(); //跳過第一行
        while ((line = sr.ReadLine()) != null)
        {

            //開始一行一行讀取

            string[] Data = line.Split(',');
            if (Data.Length == 19)
            {
                CountNum(i++);
                string wkData_id = Data[3].ToString().Trim() + Data[0].ToString().Trim() +
                                    (Int32.Parse(Data[4].ToString().Trim().Substring(0, 4)) - 1911).ToString().PadLeft(3, '0') + Data[4].ToString().Trim().Substring(4, 2) + Data[4].ToString().Trim().Substring(6, 2) +
                                    Data[5].ToString().Trim().PadLeft(2, '0') + Data[6].ToString().Trim().PadLeft(6, '0') + "30";
                iniDrOrd.data_id = wkData_id;
                iniDrOrd.order_seq_no = returnNum(Data[7]);
                iniDrOrd.fee_ym = Data[2].ToString();
                iniDrOrd.order_type = Data[8].ToString().Substring(0, 1);
                iniDrOrd.order_code = Data[9].ToString().Substring(0, 11);
                //iniDrOrd.drug_num = Strings.StrConv(Data[10].ToString().Trim(), VbStrConv.Narrow).Substring( 0, 6);
                //iniDrOrd.drug_fre = Strings.StrConv(Data[11].ToString().Trim(), VbStrConv.Narrow).Substring(0, 18).Replace("'", "");
                //iniDrOrd.drug_path = Data[12].ToString().Substring(0,15);
                iniDrOrd.drug_num = Data[10].ToString().Trim();
                iniDrOrd.drug_fre = Data[11].ToString().Trim();
                iniDrOrd.drug_path = Data[12].ToString().Trim();
                iniDrOrd.order_uprice = returnDec(Data[14]);
                iniDrOrd.order_qty = returnDec(Data[13]);
                iniDrOrd.order_dot = returnNum(Data[15]);
                iniDrOrd.order_drug_day = returnNum(Data[16]);
                iniDrOrd.exe_prsn_id = Data[17].ToString();
                iniDrOrd.tran_date = Data[18].ToString();

                iniDrOrdInsertData(iniDrOrd);
            }
            else
            {
                string fileName = @"ExceptionFile\iniDrOrdException" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                WriteException(fileName, line);

            }

        }
        sr.Close();
        sr.Dispose();
        return i;
    }
    public static void iniDrOrdInsertData(iniDrOrd iniDrOrd)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("data_id", iniDrOrd.data_id);
        if (iniDrOrd.order_seq_no == null)
        {
            adict.Add("order_seq_no", DBNull.Value);
        }
        else
        {
            adict.Add("order_seq_no", iniDrOrd.order_seq_no);
        }
        adict.Add("fee_ym", iniDrOrd.fee_ym);
        adict.Add("order_type", iniDrOrd.order_type);
        adict.Add("order_code", iniDrOrd.order_code);
        adict.Add("drug_num", iniDrOrd.drug_num);
        adict.Add("drug_fre", iniDrOrd.drug_fre);
        adict.Add("drug_path", iniDrOrd.drug_path);
        if (iniDrOrd.order_uprice == null)
        {
            adict.Add("order_uprice", DBNull.Value);
        }
        else
        {
            adict.Add("order_uprice", iniDrOrd.order_uprice);
        }
        if (iniDrOrd.order_qty == null)
        {
            adict.Add("order_qty", DBNull.Value);
        }
        else
        {
            adict.Add("order_qty", iniDrOrd.order_qty);
        }
        if (iniDrOrd.order_dot == null)
        {
            adict.Add("order_dot", DBNull.Value);
        }
        else
        {
            adict.Add("order_dot", iniDrOrd.order_dot);
        }
        if (iniDrOrd.order_drug_day == null)
        {
            adict.Add("order_drug_day", DBNull.Value);
        }
        else
        {
            adict.Add("order_drug_day", iniDrOrd.order_drug_day);
        }
        adict.Add("exe_prsn_id", iniDrOrd.exe_prsn_id);
        adict.Add("tran_date", iniDrOrd.tran_date);

        string sql = @"
            IF NOT EXISTS (Select 1 from [iniDrOrd] where data_id=@data_id and order_seq_no=@order_seq_no and fee_ym=@fee_ym)
            BEGIN 
            INSERT INTO [dbo].[iniDrOrd]
           ([data_id],[order_seq_no],[fee_ym],[order_type],[order_code],[drug_num],[drug_fre],[drug_path],[order_uprice]
            ,[order_qty],[order_dot],[order_drug_day],[exe_prsn_id],[tran_date])     
            VALUES(@data_id,@order_seq_no,@fee_ym,@order_type,@order_code,@drug_num,@drug_fre,@drug_path,@order_uprice,@order_qty
            ,@order_dot,@order_drug_day,@exe_prsn_id,@tran_date)
            END  ";
        ObjDH.executeNonQuery(sql, adict);
    }
    #endregion

    #region iniOpDtl系統輔助功能
    public static int OpeniniOpDtlTxt(string filePath)
    {
        iniOpDtl iniOpDtl = new iniOpDtl();
        var i = 0;
        //透過路徑讀取檔案
        StreamReader sr = new StreamReader(filePath);
        string line;
        sr.ReadLine(); //跳過第一行
        while ((line = sr.ReadLine()) != null)
        {

            //開始一行一行讀取

            string[] Data = line.Split(',');
            if (Data.Length == 48)
            {
                CountNum(i++);
                string wkData_id = Data[35].ToString();
                string fee_ym = Data[0].ToString().Trim();
                iniOpDtl.data_id = wkData_id;
                iniOpDtl.fee_ym = fee_ym;
                iniOpDtl.ExamYear = string.Empty;
                iniOpDtl.InstructExamYear = string.Empty;
                iniOpDtl.appl_type = Data[1].ToString().Trim();
                iniOpDtl.HospID = Data[2].ToString().Trim();
                iniOpDtl.appl_date = Data[3].ToString().Trim();
                iniOpDtl.case_type = Data[4].ToString().Trim();
                iniOpDtl.seq_no = returnNum(Data[5].Trim());

                iniOpDtl.cure_item1 = Data[6].ToString().Trim();
                iniOpDtl.cure_item2 = Data[7].ToString().Trim();
                iniOpDtl.cure_item3 = Data[8].ToString().Trim();
                iniOpDtl.cure_item4 = Data[9].ToString().Trim();
                iniOpDtl.func_type = Data[10].ToString().Trim();
                iniOpDtl.func_date = Data[11].ToString().Trim();
                iniOpDtl.cure_e_date = string.Empty;

                iniOpDtl.birthday = Data[13].ToString().Trim();
                iniOpDtl.id = Data[14].ToString().Trim();
                iniOpDtl.func_seq_no = Data[15].ToString().Trim();
                iniOpDtl.pay_type = Data[16].ToString().Trim();
                iniOpDtl.part_code = Data[17].ToString().Trim();
                iniOpDtl.icd9cm_code = Data[18].ToString().Trim();
                iniOpDtl.icd9cm_code1 = Data[19].ToString().Trim();
                iniOpDtl.icd9cm_code2 = Data[20].ToString().Trim();
                iniOpDtl.drug_days = returnNum(Data[21]);
                iniOpDtl.rel_mode = Data[22].ToString().Trim();
                iniOpDtl.prsn_id = Data[23].ToString().Trim();
                iniOpDtl.drug_prsn_id = Data[24].ToString().Trim();

                iniOpDtl.drug_dot = returnNum(Data[25]);
                iniOpDtl.cure_dot = returnNum(Data[26]);
                iniOpDtl.diag_code = Data[27].ToString().Trim();
                iniOpDtl.diag_dot = returnNum(Data[28]);
                iniOpDtl.dsvc_code = Data[29].ToString().Trim();
                iniOpDtl.dsvc_dot = returnNum(Data[30]);
                iniOpDtl.exp_dot = returnNum(Data[31]);
                iniOpDtl.part_amt = returnNum(Data[32]);
                iniOpDtl.appl_dot = returnNum(Data[33]);
                iniOpDtl.Id_Sex = GetMFString(Data[14].ToString().Trim());
                iniOpDtl.area_service = Data[37].ToString().Trim();
                iniOpDtl.supp_area = Data[38].ToString().Trim();
                iniOpDtl.real_hosp_id = Data[39].ToString().Trim();

                iniOpDtl.hosp_data_type = Data[36].ToString().Trim();
                iniOpDtl.agency_part_amt = returnDec(Data[40]);
                iniOpDtl.name = Data[41].ToString().Trim();
                iniOpDtl.appl_cause_mark = Data[42].ToString().Trim();
                iniOpDtl.icd10cm_code3 = Data[43].ToString().Trim();
                iniOpDtl.icd10cm_code4 = Data[44].ToString().Trim();
                iniOpDtl.met_dot = returnNum(Data[45]);
                iniOpDtl.corr_hosp_id = Data[46].ToString().Trim();
                iniOpDtl.tran_date = Data[47].ToString().Trim();

                iniOpDtl.ExamTime = null;
                iniOpDtl.FirstTreatDate = string.Empty;
                iniOpDtl.WeekCount = null;
                iniOpDtl.InstructExamTime = null;
                iniOpDtl.FirstInstructDate = string.Empty;
                iniOpDtl.InctructSerial = null;
                iniOpDtl.MedApply = string.Empty;
                iniOpDtl.InstructApply = string.Empty;
                iniOpDtl.TraceApply = string.Empty;
                iniOpDtl.ReleaseApply = string.Empty;

                iniOpDtlDeleteData(wkData_id,fee_ym);

                iniOpDtlInsertData(iniOpDtl);
            }
            else
            {
                string fileName = @"ExceptionFile\iniOpDtlException" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                WriteException(fileName, line);

            }

        }
        sr.Close();
        sr.Dispose();
        return i;
    }
    public static void iniOpDtlDeleteData(string data_id, string fee_ym)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql = "delete from iniOpDtl where data_id=@data_id and fee_ym=@fee_ym";
        adict.Clear();
        adict.Add("data_id", data_id);
        adict.Add("fee_ym", fee_ym);
        ObjDH.executeNonQuery(sql, adict);

    }
    public static void iniOpDtlInsertData(iniOpDtl iniOpDtl)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("data_id", iniOpDtl.data_id);
        adict.Add("fee_ym", iniOpDtl.fee_ym);
        adict.Add("ExamYear", iniOpDtl.ExamYear);
        if (iniOpDtl.ExamTime == null)
        {
            adict.Add("ExamTime", DBNull.Value);
        }
        else
        {
            adict.Add("ExamTime", iniOpDtl.ExamTime);
        }
        adict.Add("FirstTreatDate", iniOpDtl.FirstTreatDate);
        if (iniOpDtl.WeekCount == null)
        {
            adict.Add("WeekCount", DBNull.Value);
        }
        else
        {
            adict.Add("WeekCount", iniOpDtl.WeekCount);
        }

        adict.Add("InstructExamYear", iniOpDtl.InstructExamYear);
        if (iniOpDtl.InstructExamTime == null)
        {
            adict.Add("InstructExamTime", DBNull.Value);
        }
        else
        {
            adict.Add("InstructExamTime", iniOpDtl.InstructExamTime);
        }
        adict.Add("FirstInstructDate", iniOpDtl.FirstInstructDate);
        if (iniOpDtl.InctructSerial == null)
        {
            adict.Add("InctructSerial", DBNull.Value);
        }
        else
        {
            adict.Add("InctructSerial", iniOpDtl.InctructSerial);
        }
        adict.Add("MedApply", iniOpDtl.MedApply);
        adict.Add("InstructApply", iniOpDtl.InstructApply);
        adict.Add("TraceApply", iniOpDtl.TraceApply);
        adict.Add("ReleaseApply", iniOpDtl.ReleaseApply);
        adict.Add("appl_type", iniOpDtl.appl_type);
        adict.Add("HospID", iniOpDtl.HospID);
        adict.Add("appl_date", iniOpDtl.appl_date);
        adict.Add("case_type", iniOpDtl.case_type);
        if (iniOpDtl.seq_no == null)
        {
            adict.Add("seq_no", DBNull.Value);
        }
        else
        {
            adict.Add("seq_no", iniOpDtl.seq_no);
        }
        adict.Add("func_type", iniOpDtl.func_type);
        adict.Add("func_date", iniOpDtl.func_date);
        adict.Add("cure_e_date", iniOpDtl.cure_e_date);
        adict.Add("birthday", iniOpDtl.birthday);
        adict.Add("id", iniOpDtl.id);
        adict.Add("func_seq_no", iniOpDtl.func_seq_no);
        adict.Add("pay_type", iniOpDtl.pay_type);
        adict.Add("part_code", iniOpDtl.part_code);
        adict.Add("icd9cm_code", iniOpDtl.icd9cm_code);
        adict.Add("icd9cm_code1", iniOpDtl.icd9cm_code1);
        adict.Add("icd9cm_code2", iniOpDtl.icd9cm_code2);
        if (iniOpDtl.drug_days == null)
        {
            adict.Add("drug_days", DBNull.Value);
        }
        else
        {
            adict.Add("drug_days", iniOpDtl.drug_days);
        }
        adict.Add("rel_mode", iniOpDtl.rel_mode);
        adict.Add("prsn_id", iniOpDtl.prsn_id);
        adict.Add("drug_prsn_id", iniOpDtl.drug_prsn_id);
        if (iniOpDtl.drug_dot == null)
        {
            adict.Add("drug_dot", DBNull.Value);
        }
        else
        {
            adict.Add("drug_dot", iniOpDtl.drug_dot);
        }

        if (iniOpDtl.cure_dot == null)
        {
            adict.Add("cure_dot", DBNull.Value);
        }
        else
        {
            adict.Add("cure_dot", iniOpDtl.cure_dot);
        }
        adict.Add("diag_code", iniOpDtl.diag_code);
        if (iniOpDtl.diag_dot == null)
        {
            adict.Add("diag_dot", DBNull.Value);
        }
        else
        {
            adict.Add("diag_dot", iniOpDtl.diag_dot);
        }
        adict.Add("dsvc_code", iniOpDtl.dsvc_code);
        if (iniOpDtl.dsvc_dot == null)
        {
            adict.Add("dsvc_dot", DBNull.Value);
        }
        else
        {
            adict.Add("dsvc_dot", iniOpDtl.dsvc_dot);
        }
        if (iniOpDtl.exp_dot == null)
        {
            adict.Add("exp_dot", DBNull.Value);
        }
        else
        {
            adict.Add("exp_dot", iniOpDtl.exp_dot);
        }
        if (iniOpDtl.part_amt == null)
        {
            adict.Add("part_amt", DBNull.Value);
        }
        else
        {
            adict.Add("part_amt", iniOpDtl.part_amt);
        }
        if (iniOpDtl.appl_dot == null)
        {
            adict.Add("appl_dot", DBNull.Value);
        }
        else
        {
            adict.Add("appl_dot", iniOpDtl.appl_dot);
        }
      
        adict.Add("Id_Sex", iniOpDtl.Id_Sex);
        adict.Add("cure_item1", iniOpDtl.cure_item1);
        adict.Add("cure_item2", iniOpDtl.cure_item2);
        adict.Add("cure_item3", iniOpDtl.cure_item3);
        adict.Add("cure_item4", iniOpDtl.cure_item4);
        adict.Add("area_service", iniOpDtl.area_service);
        adict.Add("supp_area", iniOpDtl.supp_area);
        adict.Add("real_hosp_id", iniOpDtl.real_hosp_id);
        adict.Add("hosp_data_type", iniOpDtl.hosp_data_type);
        if (iniOpDtl.agency_part_amt == null)
        {
            adict.Add("agency_part_amt", DBNull.Value);
        }
        else
        {
            adict.Add("agency_part_amt", iniOpDtl.agency_part_amt);
        }
      
        adict.Add("name", iniOpDtl.name);
        adict.Add("appl_cause_mark", iniOpDtl.appl_cause_mark);
        adict.Add("icd10cm_code3", iniOpDtl.icd10cm_code3);
        adict.Add("icd10cm_code4", iniOpDtl.icd10cm_code4);
        if (iniOpDtl.met_dot == null)
        {
            adict.Add("met_dot", DBNull.Value);
        }
        else
        {
            adict.Add("met_dot", iniOpDtl.met_dot);
        }
        adict.Add("corr_hosp_id", iniOpDtl.corr_hosp_id);
        adict.Add("tran_date", iniOpDtl.tran_date);
        string sql = @"
            IF NOT EXISTS (Select 1 from iniOpDtl where data_id=@data_id and fee_ym=@fee_ym)
            BEGIN 
            INSERT INTO [dbo].[iniOpDtl]([data_id],[fee_ym],[ExamYear],[ExamTime],[FirstTreatDate],[WeekCount],[InstructExamYear],[InstructExamTime]
            ,[FirstInstructDate],[InctructSerial],[MedApply],[InstructApply],[TraceApply],[ReleaseApply],[appl_type],[HospID],[appl_date]
            ,[case_type],[seq_no],[func_type],[func_date],[cure_e_date],[birthday],[id],[func_seq_no],[pay_type],[part_code],[icd9cm_code]
            ,[icd9cm_code1],[icd9cm_code2],[drug_days],[rel_mode],[prsn_id],[drug_prsn_id],[drug_dot],[cure_dot]
            ,[diag_code],[diag_dot],[dsvc_code],[dsvc_dot],[exp_dot],[part_amt],[appl_dot],[Id_Sex],[cure_item1],[cure_item2]
            ,[cure_item3],[cure_item4],[area_service],[supp_area],[real_hosp_id],[hosp_data_type],[agency_part_amt],[name]
            ,[appl_cause_mark],[icd10cm_code3],[icd10cm_code4],[met_dot],[corr_hosp_id],[tran_date])     
            VALUES
            (@data_id,@fee_ym,@ExamYear,@ExamTime,@FirstTreatDate,@WeekCount,@InstructExamYear,@InstructExamTime,@FirstInstructDate
            ,@InctructSerial,@MedApply,@InstructApply,@TraceApply,@ReleaseApply,@appl_type,@HospID,@appl_date,@case_type,@seq_no
            ,@func_type,@func_date,@cure_e_date,@birthday,@id,@func_seq_no,@pay_type,@part_code,@icd9cm_code,@icd9cm_code1,@icd9cm_code2
            ,@drug_days,@rel_mode,@prsn_id,@drug_prsn_id,@drug_dot,@cure_dot,@diag_code,@diag_dot,@dsvc_code,@dsvc_dot,@exp_dot
            ,@part_amt,@appl_dot,@Id_Sex,@cure_item1,@cure_item2,@cure_item3,@cure_item4,@area_service,@supp_area,@real_hosp_id
            ,@hosp_data_type,@agency_part_amt,@name,@appl_cause_mark,@icd10cm_code3,@icd10cm_code4,@met_dot,@corr_hosp_id,@tran_date)
            END  ";
        ObjDH.executeNonQuery(sql, adict);
    }
    #endregion

    #region iniOpOrd系統輔助功能
    public static int OpeniniOpOrdTxt(string filePath)
    {
        iniOpOrd iniOpOrd = new iniOpOrd();
        var i = 0;
        //透過路徑讀取檔案
        StreamReader sr = new StreamReader(filePath);
        string line;
        sr.ReadLine(); //跳過第一行
        while ((line = sr.ReadLine()) != null)
        {

            //開始一行一行讀取
            
            string[] Data = line.Split(',');
            if (Data.Length == 16)
            {
                CountNum(i++);
                string fee_ym = Data[12].ToString().Trim();
                string wkData_id = Data[0].ToString();
                int order_seq_no = (int)returnNum(Data[1]);
                iniOpOrd.data_id = wkData_id;
                iniOpOrd.order_seq_no = order_seq_no;
                iniOpOrd.fee_ym = fee_ym;
                iniOpOrd.order_type = Data[2].ToString().Trim();
                iniOpOrd.order_code = Data[3].ToString().Trim();
                iniOpOrd.rel_mode = Data[4].ToString().Trim();
                iniOpOrd.drug_num = Data[5].ToString().Trim();
                iniOpOrd.drug_fre = Data[6].ToString().Trim();
                iniOpOrd.drug_path = Data[7].ToString().Trim();
                iniOpOrd.order_uprice = returnDec(Data[8]);
                iniOpOrd.order_qty = returnDec(Data[9]);
                iniOpOrd.order_dot = returnNum(Data[10]);
                iniOpOrd.exe_prsn_id = Data[11].ToString().Trim();
                iniOpOrd.cure_path = Data[13].ToString().Trim();
                iniOpOrd.order_drug_day = returnNum(Data[14]);
                iniOpOrd.tran_date = Data[15].ToString().Trim();
                iniOpOrd.chr_mark = string.Empty;
                iniOpOrdDeleteData(wkData_id,fee_ym,order_seq_no.ToString());
                iniOpOrdInsertData(iniOpOrd);
            }
            else
            {
                string fileName = @"ExceptionFile\iniOpOrdException" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                WriteException(fileName, line);

            }

        }
        sr.Close();
        sr.Dispose();
        return i;
    }
    public static void iniOpOrdDeleteData(string data_id,string fee_ym,string order_seq_no)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Clear();
        adict.Add("data_id", data_id);
        adict.Add("fee_ym", fee_ym);
        adict.Add("order_seq_no", order_seq_no);
        string sql = "delete from iniOpOrd where data_id=@data_id and fee_ym=@fee_ym and order_seq_no=@order_seq_no";
        ObjDH.executeNonQuery(sql, adict);
    }
    public static void iniOpOrdInsertData(iniOpOrd iniOpOrd)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("data_id", iniOpOrd.data_id);
        if (iniOpOrd.order_seq_no == null)
        {
            adict.Add("order_seq_no", DBNull.Value);
        }
        else
        {
            adict.Add("order_seq_no", iniOpOrd.order_seq_no);
        }
        adict.Add("fee_ym", iniOpOrd.fee_ym);
        adict.Add("order_type", iniOpOrd.order_type);
        adict.Add("order_code", iniOpOrd.order_code);
        adict.Add("rel_mode", iniOpOrd.rel_mode);
        adict.Add("chr_mark", iniOpOrd.chr_mark);
        adict.Add("drug_num", iniOpOrd.drug_num);
        adict.Add("drug_fre", iniOpOrd.drug_fre);
        adict.Add("drug_path", iniOpOrd.drug_path);
        if (iniOpOrd.order_uprice == null)
        {
            adict.Add("order_uprice", DBNull.Value);
        }
        else
        {
            adict.Add("order_uprice", iniOpOrd.order_uprice);
        }
        if (iniOpOrd.order_qty == null)
        {
            adict.Add("order_qty", DBNull.Value);
        }
        else
        {
            adict.Add("order_qty", iniOpOrd.order_qty);
        }
        if (iniOpOrd.order_dot == null)
        {
            adict.Add("order_dot", DBNull.Value);
        }
        else
        {
            adict.Add("order_dot", iniOpOrd.order_dot);
        }
        adict.Add("exe_prsn_id", iniOpOrd.exe_prsn_id);
        adict.Add("cure_path", iniOpOrd.cure_path);
        if (iniOpOrd.order_drug_day == null)
        {
            adict.Add("order_drug_day", DBNull.Value);
        }
        else
        {
            adict.Add("order_drug_day", iniOpOrd.order_drug_day);
        }
        adict.Add("tran_date", iniOpOrd.tran_date);

        string sql = @"
            IF NOT EXISTS (Select 1 from iniOpOrd where data_id=@data_id and order_seq_no=@order_seq_no and fee_ym=@fee_ym)
            BEGIN 
           INSERT INTO [dbo].[iniOpOrd]
            ([data_id],[order_seq_no],[fee_ym],[order_type],[order_code],[rel_mode],[chr_mark],[drug_num],[drug_fre],[drug_path]
            ,[order_uprice],[order_qty],[order_dot],[exe_prsn_id],[cure_path],[order_drug_day],[tran_date])
            VALUES
            (@data_id,@order_seq_no,@fee_ym,@order_type,@order_code,@rel_mode,@chr_mark,@drug_num,@drug_fre,@drug_path,@order_uprice
            ,@order_qty,@order_dot,@exe_prsn_id,@cure_path,@order_drug_day,@tran_date)
            END  ";
        ObjDH.executeNonQuery(sql, adict);
    }
    #endregion

    #region
    public static void UpdateFirstDateAndExamYear(string wkTranDateS , string wkTranDateE ){ //, ByVal objMessage As Object, ByVal objFCount As Object, ByVal objTCount As Object, ByVal objCount As Object) {
        string sql = string.Empty;
        int wkValue, wkMaxValue;
        int wkCount;
        DateTime wkCurrDate;

        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();

        try
        {
            try
            {

                //更新申報類別***
                sql = "exec UpdateApplyWeekCount '" + wkTranDateS + "','" + wkTranDateE + "'";
                adict.Clear();
                ObjDH.executeNonQuery(sql, adict);
                

                //建立暫存資料表
                sql = "exec UpdateTempTable";
                adict.Clear();
                ObjDH.executeNonQuery(sql, adict);

                //院所療程初診日
                sql = "select count(1) as cnt from (select distinct id from iniOpDtl where tran_date>='" + wkTranDateS + "' and tran_date<='" + wkTranDateE + "') f";
                DataTable DT = ObjDH.queryData(sql, adict);
                wkMaxValue = Int32.Parse(DT.Rows[0]["cnt"].ToString());
                
                wkValue = 0;
                sql = "select distinct id from iniOpDtl where tran_date>='" + wkTranDateS + "' and tran_date<='" + wkTranDateE + "'";
                DT = ObjDH.queryData(sql, adict);
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    sql = "exec UpdateOpProcess '" + DT.Rows[i]["id"].ToString().Trim() + "','" + wkTranDateS + "','" + wkTranDateE + "'";
                    ObjDH.executeNonQuery(sql,adict);
                }

                //更新OP
                sql = "exec UpdateOpData '" + wkTranDateS + "','" + wkTranDateE + "'";
                adict.Clear();
                ObjDH.executeNonQuery(sql, adict);

                //院所療程次數
                
                
                sql = "select distinct id from iniOpDtl where tran_date>='" + wkTranDateS + "' and tran_date<='" + wkTranDateE + "'";
                DT = ObjDH.queryData(sql, adict);
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    sql = "exec UpdateExamTimeOp '" + DT.Rows[i]["id"].ToString().Trim() + "','" + wkTranDateS + "','" + wkTranDateE + "'";
                    ObjDH.executeNonQuery(sql, adict);
                }

                //更新OP
                sql = "exec UpdateOpData '" + wkTranDateS + "','" + wkTranDateE + "'";
                adict.Clear();
                ObjDH.executeNonQuery(sql, adict);

                //建立暫存資料表
                sql = "exec UpdateTempTable";
                adict.Clear();
                ObjDH.executeNonQuery(sql, adict);

                //藥局療程初診日
                


                sql = "select distinct id from iniDrDtl where tran_date>='" + wkTranDateS + "' and tran_date<='" + wkTranDateE + "'";
                DT = ObjDH.queryData(sql, adict);
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    sql = "exec UpdateDrProcess '" + DT.Rows[i]["id"].ToString().Trim() + "','" + wkTranDateS + "','" + wkTranDateE + "'";
                    ObjDH.executeNonQuery(sql, adict);
                }
                

                //更新Dr
                sql = "exec UpdateDrData '" + wkTranDateS + "','" + wkTranDateE + "'";
                adict.Clear();
                ObjDH.executeNonQuery(sql, adict);

                //藥局療程次數



                wkValue = 0;
                sql = "select distinct id from iniDrDtl where tran_date>='" + wkTranDateS + "' and tran_date<='" + wkTranDateE + "'";
                DT = ObjDH.queryData(sql, adict);
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    sql = "exec UpdateExamTimeDr '" + DT.Rows[i]["id"].ToString().Trim() + "','" + wkTranDateS + "','" + wkTranDateE + "'";
                    ObjDH.executeNonQuery(sql, adict);
                }

                //更新Dr
                sql = "exec UpdateDrData '" + wkTranDateS + "','" + wkTranDateE + "'";
                adict.Clear();
                ObjDH.executeNonQuery(sql, adict);
            }
            catch (Exception)
            {
                throw;
            }
        }
        catch (Exception)
        {

            throw;
        }

    }
    #endregion

    #region SamplingList系統輔助功能
    public static int OpenSamplingListTxt(string filePath)
    {
        SamplingListData SamplingListData = new SamplingListData();
        var i = 0;
        //透過路徑讀取檔案
        StreamReader sr = new StreamReader(filePath);
        string line;
        sr.ReadLine(); //跳過第一行
        while ((line = sr.ReadLine()) != null)
        {

            //開始一行一行讀取
            CountNum(i++);
            string[] Data = line.Split(',');
            if (Data.Length == 2)
            {
                SamplingListData.data_id = Data[0].ToString();
                SamplingListData.fee_ym = Data[1].ToString();
                SamplingListInsertData(SamplingListData);
            }


        }
        sr.Close();
        sr.Dispose();
        return i;
    }
    private static void SamplingListInsertData(SamplingListData SamplingListData)
    {

        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("fee_ym", SamplingListData.fee_ym);
        adict.Add("data_id", SamplingListData.data_id);

        string sql = @"
            IF NOT EXISTS (Select 1 from SamplingList where data_id=@data_id and fee_ym=@fee_ym)
            BEGIN 
            INSERT INTO [dbo].[SamplingList]
            ([fee_ym],[data_id])     
            VALUES
            (@fee_ym,@data_id)
            END  ";
        ObjDH.executeNonQuery(sql, adict);
    }
    #endregion

    #region HospBscAll系統輔助功能
    public static void DeleteHospBscAllTxt()
    {
        Dictionary<string, object> adict = new Dictionary<string, object>();
        DataHelper ObjDH = new DataHelper();
        string sql = @"TRUNCATE TABLE [HospBscAll]";
        ObjDH.executeNonQuery(sql, adict);
    }
    private static void HospBscAllInsertData(HospBscAllData HospBscAll)
    {

        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("HospID", HospBscAll.HospID);
        adict.Add("HospName", HospBscAll.HospName);
        adict.Add("HospTelArea", HospBscAll.HospTelArea);
        adict.Add("HospTel", HospBscAll.HospTel);
        adict.Add("BranchNo", HospBscAll.BranchNo);
        adict.Add("HospAddress", HospBscAll.HospAddress);
        adict.Add("ContType", HospBscAll.ContType);
        adict.Add("HospType", HospBscAll.HospType);
        adict.Add("HospKind", HospBscAll.HospKind);
        adict.Add("HospEndDate", HospBscAll.HospEndDate);
        string sql = @"
         IF NOT EXISTS (Select 1 from HospBscAll where HospID=@HospID)
            BEGIN 
            INSERT INTO [dbo].[HospBscAll]
           ([HospID]
           ,[HospName]
           ,[HospTelArea]
           ,[HospTel]
           ,[BranchNo]
           ,[HospAddress]
           ,[ContType]
           ,[HospType]
           ,[HospKind]
           ,[HospEndDate])
     VALUES
           (@HospID
           ,@HospName
           ,@HospTelArea
           ,@HospTel
           ,@BranchNo
           ,@HospAddress
           ,@ContType
           ,@HospType
           ,@HospKind
           ,@HospEndDate) 
            END ";
        ObjDH.executeNonQuery(sql, adict);
    }

    public static int OpenHospBscAllTxt(string filePath)
    {
        HospBscAllData HospBscAll = new HospBscAllData();
        var i = 0;
        //透過路徑讀取檔案
        StreamReader sr = new StreamReader(filePath);
        string line;
        sr.ReadLine(); //跳過第一行
        while ((line = sr.ReadLine()) != null)
        {

            //開始一行一行讀取
            CountNum(i++);
            string[] Data = line.Split(',');
            if (Data.Length == 11)
            {
                HospBscAll.HospID = Data[1].ToString().Trim().Replace(@"""", "");
                HospBscAll.HospName = Data[2].ToString().Trim().Replace(@"""", ""); ;
                HospBscAll.HospTelArea = Data[4].ToString().Trim().Replace(@"""", ""); ;
                HospBscAll.HospTel = Data[5].ToString().Trim().Replace(@"""", ""); ;
                HospBscAll.BranchNo = Data[0].ToString().Trim().Replace(@"""", ""); ;
                HospBscAll.HospAddress = Data[3].ToString().Trim().Replace(@"""", ""); ;
                HospBscAll.ContType = Data[6].ToString().Trim().Replace(@"""", ""); ;
                HospBscAll.HospType = Data[7].ToString().Trim().Replace(@"""", ""); ;
                HospBscAll.HospKind = Data[8].ToString().Trim().Replace(@"""", ""); ;
                HospBscAll.HospEndDate = Data[9].ToString().Trim().Replace(@"""", ""); ;
                HospBscAllInsertData(HospBscAll);
            }


        }
        sr.Close();
        sr.Dispose();
        return i;
    }
    #endregion

    #region 輔助功能
    public static int? returnNum(string Num)
    {
        if (Num == "")
        {
            return null;
        }
        else
        {
            return Convert.ToInt32(Num);
        }

    }
    public static decimal? returnDec(string Num)
    {
        if (Num == "")
        {
            return null;
        }
        else
        {
            return Convert.ToDecimal(Num);
        }

    }
    public static void InsertLog(string FileName,int Count)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql = @"INSERT INTO [dbo].[DataInsertLog]
           ([FileName]
           ,[FinishDate],ReCordCount)
     VALUES
           (@FileName
           ,getdate()
            ,@ReCordCount)";
        adict.Add("FileName", FileName);
        adict.Add("ReCordCount", Count);
        ObjDH.executeNonQuery(sql, adict);
    }

    public static void WriteException(string fileName,string line)
    {
        if (!File.Exists(fileName))
        {
            FileStream fs = File.Create(fileName);
            fs.Close();
            fs.Dispose();
        }
        using (StreamWriter sw = File.AppendText(fileName))
        {
            sw.WriteLine(line);

        }
    }
    public static string GetMFString(string GetString)
    {
        if (Strings.Trim(GetString) == "") return "U";
        switch (Strings.UCase(Strings.Mid(Strings.Trim(GetString), 2, 1)))
        {
            case "1":
                return "M";
            case "A":
                return "M";
            case "C":
                return "M";
            case "X":
                return "M";
            case "2":
                return "F";
            case "B":
                return "F";
            case "D":
                return "F";
            case "Y":
                return "F";
            default:
                return "U";
        }

    }
    public static string TelFormat(string wkString)
    {
        string[] wkStrArray, wkStrArray1;
        string wkTel_d_ac, wkTel_d, wkTel_d_extn;

        wkStrArray = Strings.Split(wkString, "-");
        wkTel_d_ac = wkStrArray[0].ToString().Trim();
        wkStrArray1 = Strings.Split(wkStrArray[1].ToString().Trim(), "#");
        wkTel_d = wkStrArray1[0].ToString().Trim();
        wkTel_d_extn = wkStrArray1[1].ToString().Trim();

        if (wkTel_d_ac != "" & wkTel_d != "" & wkTel_d_extn != "")
            return wkTel_d_ac + "-" + wkTel_d + "#" + wkTel_d_extn;        
        else if (wkTel_d_ac == "" & wkTel_d != "" & wkTel_d_extn != "")
            return wkTel_d + "#" + wkTel_d_extn;
        else if (wkTel_d_ac != "" & wkTel_d != "" & wkTel_d_extn == "")
            return wkTel_d_ac + "-" + wkTel_d;
        else if (wkTel_d_ac == "" & wkTel_d != "" & wkTel_d_extn == "")
            return wkTel_d;
        else if (wkTel_d_ac != "" & wkTel_d == "" & wkTel_d_extn != "")
            return wkTel_d_ac + "-#" + wkTel_d_extn;
        else if (wkTel_d_ac != "" & wkTel_d == "" & wkTel_d_extn == "")
            return wkTel_d_ac + "-";
        else if (wkTel_d_ac == "" & wkTel_d == "" & wkTel_d_extn != "")
            return "#" + wkTel_d_extn;
        else
            return "";
    }
    public static decimal? CheckZero(string wkString)
    {
        if (string.IsNullOrEmpty(wkString) || Strings.Trim(wkString) == "")
        {
            return 0;
        }
        else
        {
            return Convert.ToDecimal(Strings.Trim(wkString));
        }
            
    }
    public static string CheckNull(string wkString)
    {
        if (string.IsNullOrEmpty(wkString) || Strings.Trim(wkString) == "")
        {
            return "";
        }
        else
        {
            return Strings.Trim(wkString);
        }

    }
  
    public static void CreateFolder()
    {
        string[] repositoryUrls = ConfigurationManager.AppSettings.AllKeys
                             .Select(key => ConfigurationManager.AppSettings[key])
                             .ToArray();

        foreach(var x in repositoryUrls)
        {
            if(x.Contains("Txt")) System.IO.Directory.CreateDirectory(x);
        }
       
    }
    #endregion
}
