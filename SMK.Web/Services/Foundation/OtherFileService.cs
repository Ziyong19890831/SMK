using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.Style;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class OtherFileService : GenericService
    {
        private readonly IDbConnection _conn = null;
        private readonly IWebHostEnvironment _env;
        public OtherFileService(SMKWEBContext context, SessionManager smgr, IWebHostEnvironment env)
        : base(context, smgr)
        {
            _conn = context.Database.GetDbConnection();
            _env = env;
        }
        public async Task<LogicRtnModel<bool>> UploadICCardData(IFormFile file)
        {
            LogicRtnModel<bool> ret = new LogicRtnModel<bool>() { IsSuccess = true };
            List<ICCardData> iCCardDatas = new List<ICCardData>();

            var sr = new StreamReader(file.OpenReadStream());
            string line;

            sr.ReadLine(); //跳過第一行
            while ((line = sr.ReadLine()) != null)
            {
                ICCardData ICData = new ICCardData();
                //開始一行一行讀取            
                string[] Data = line.Split(',');
                try
                {
                    if (Data.Length == 21 || Data.Length == 22)
                    {
                        ICData.DataType = Data[0].ToString();
                        ICData.PersonID = Data[1].ToString();
                        ICData.Birthday = Data[2].ToString();
                        ICData.HospitalCode = Data[3].ToString();
                        ICData.PhysicianPersonID = Data[4].ToString();
                        ICData.ReadCardDatetime = Data[5].ToString();
                        ICData.MedicalSeries = Data[6].ToString();
                        ICData.ReissueNote = Int32.Parse(Data[7]);
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
                        ICData.MedicineCount = Int32.Parse(Data[20]);
                        ICData.CreateDT = DateTime.Now.ToShortDateString().ToString();

                        iCCardDatas.Add(ICData);
                    }
                    else
                    {
                        //string fileName = @"ExceptionFile\ICDataException" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                        //WriteException(fileName, line);
                        ret.ErrMsg = "第" + (iCCardDatas.Count() + 1).ToString() + "筆資料異常 ( 此筆資料欄位長度為 : " + Data.Length + " )";
                        return ret;
                    }
                }
                catch (Exception ex)
                {
                    ret.ErrMsg = "第" + (iCCardDatas.Count() + 1).ToString() + "筆資料異常，ID為 : " + Data[1] + "( 資料數字轉型失敗，或第一欄資料長度大於2 )";
                    return ret;
                }

            }

            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    DataInsertLog dataInsertLog = new Services.DataInsertLogService().dataInsertLog(file.FileName, iCCardDatas.Count());
                    await context.ICCardData.AddRangeAsync(iCCardDatas);
                    await context.DataInsertLog.AddRangeAsync(dataInsertLog);
                    await context.SaveChangesAsync();
                    await trans.CommitAsync();
                    ret.Msg = "共寫入" + iCCardDatas.Count() + "筆資料";
                }
                catch (Exception e)
                {
                    await trans.RollbackAsync();
                    ret.ErrMsg = e.Message;
                }
            }

            return ret;
        }



        public DateTime? BirthdayConvertDate(string Date)
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
        public DateTime? ReadCardDatetimeConvertDate(string Date)
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

    }
}
