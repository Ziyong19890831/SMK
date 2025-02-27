using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SMK.Data;
using SMK.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;
using Yozian.WebCore.Library.Utility.Excel;
using SMK.Data.Entity;
using SMK.Data.Dto;
using Yozian.Extension;
using System.Text.RegularExpressions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class CorrectionSlipService : GenericService
    {
        private readonly IDbConnection _conn = null;
        private readonly IWebHostEnvironment _env;
        public CorrectionSlipService(SMKWEBContext context, SessionManager smgr, IWebHostEnvironment env)
        : base(context, smgr)
        {
            _conn = context.Database.GetDbConnection();
            _env = env;
        }

        public async Task<LogicRtnModel<PagedModel<CorrectionSlipExport>>> Export(CorrectionSlipQueryModel model)
        {
            var list = context.CorrectionSlip
                          .Where(x => x.ReceiveDate.Year == model.year)
                          .GroupBy(x => new { x.HospId, x.HospSeqNo, x.HospName })
                          .Select(p => new CorrectionSlipExport()
                          {
                              HospId = p.Key.HospId,
                              HospSeqNo = p.Key.HospSeqNo,
                              HospName = p.Key.HospName,
                              Jan = p.Where(x => x.ReceiveDate.Month == 1).Count(),
                              Feb = p.Where(x => x.ReceiveDate.Month == 2).Count(),
                              Mar = p.Where(x => x.ReceiveDate.Month == 3).Count(),
                              Apr = p.Where(x => x.ReceiveDate.Month == 4).Count(),
                              May = p.Where(x => x.ReceiveDate.Month == 5).Count(),
                              Jun = p.Where(x => x.ReceiveDate.Month == 6).Count(),
                              Jul = p.Where(x => x.ReceiveDate.Month == 7).Count(),
                              Aug = p.Where(x => x.ReceiveDate.Month == 8).Count(),
                              Sep = p.Where(x => x.ReceiveDate.Month == 9).Count(),
                              Oct = p.Where(x => x.ReceiveDate.Month == 10).Count(),
                              Nov = p.Where(x => x.ReceiveDate.Month == 11).Count(),
                              Dec = p.Where(x => x.ReceiveDate.Month == 12).Count(),
                              Jan2 = p.Where(x => x.ReceiveDate.Month == 1).Select(x => x.ReceiveDate.Day).Distinct().Count(),
                              Feb2 = p.Where(x => x.ReceiveDate.Month == 2).Select(x => x.ReceiveDate.Day).Distinct().Count(),
                              Mar2 = p.Where(x => x.ReceiveDate.Month == 3).Select(x => x.ReceiveDate.Day).Distinct().Count(),
                              Apr2 = p.Where(x => x.ReceiveDate.Month == 4).Select(x => x.ReceiveDate.Day).Distinct().Count(),
                              May2 = p.Where(x => x.ReceiveDate.Month == 5).Select(x => x.ReceiveDate.Day).Distinct().Count(),
                              Jun2 = p.Where(x => x.ReceiveDate.Month == 6).Select(x => x.ReceiveDate.Day).Distinct().Count(),
                              Jul2 = p.Where(x => x.ReceiveDate.Month == 7).Select(x => x.ReceiveDate.Day).Distinct().Count(),
                              Aug2 = p.Where(x => x.ReceiveDate.Month == 8).Select(x => x.ReceiveDate.Day).Distinct().Count(),
                              Sep2 = p.Where(x => x.ReceiveDate.Month == 9).Select(x => x.ReceiveDate.Day).Distinct().Count(),
                              Oct2 = p.Where(x => x.ReceiveDate.Month == 10).Select(x => x.ReceiveDate.Day).Distinct().Count(),
                              Nov2 = p.Where(x => x.ReceiveDate.Month == 11).Select(x => x.ReceiveDate.Day).Distinct().Count(),
                              Dec2 = p.Where(x => x.ReceiveDate.Month == 12).Select(x => x.ReceiveDate.Day).Distinct().Count(),
                          });
            return await QueryPaging(model.get(), list);
        }
        public async Task<LogicRtnModel<PagedModel<CorrectionSlipViewModel>>> QueryCorrectionSlip(CorrectionSlipQueryModel model)
        {
            string pattern = @"([A-Z]\d{3})(\d{3})(\d{3})";
            string substitutionID = @"$1OOO$3";
            string substitutionName = @"$1OOO$1";
            RegexOptions options = RegexOptions.Multiline;

            Regex regex = new Regex(pattern, options);

            DateTime? ReceiveSDate = null;
            DateTime? ReceiveEDate = null;
            if (!string.IsNullOrEmpty(model.FuncSDate))
            {
                int Cyear = Int32.Parse(model.FuncSDate.Split('/')[0]);
                int month = Int32.Parse(model.FuncSDate.Split('/')[1]);
                ReceiveSDate = new DateTime(Cyear + 1911, month, 1);
            }
            if (!string.IsNullOrEmpty(model.FuncEDate))
            {
                int Cyear = Int32.Parse(model.FuncEDate.Split('/')[0]);
                int month = Int32.Parse(model.FuncEDate.Split('/')[1]);
                ReceiveEDate = new DateTime(Cyear + 1911, month, DateTime.DaysInMonth(Cyear + 1911, month)).AddDays(1);
            }
            var list = context.CorrectionSlip
                          .WhereWhen(ReceiveSDate.HasValue,
                                    p => p.ReceiveDate > ReceiveSDate.Value)
                          .WhereWhen(ReceiveEDate.HasValue,
                                    p => p.ReceiveDate < ReceiveEDate.Value)
                          .WhereWhen(!string.IsNullOrEmpty(model.CaseNo),
                                     p => p.CaseNo == model.CaseNo)
                          .WhereWhen(!string.IsNullOrEmpty(model.HospID),
                                     p => p.HospId == model.HospID)
                          .WhereWhen(!string.IsNullOrEmpty(model.HospSeqNo),
                                     p => p.HospSeqNo == model.HospSeqNo)
                          .WhereWhen(!string.IsNullOrEmpty(model.HospName), x => x.HospName == model.HospName)
                          .Select(p => new CorrectionSlipViewModel()
                          {
                              CaseNo = p.CaseNo,
                              Birthday = p.Birthday.Value,
                              CorrectBasic = p.CorrectBasic,
                              CorrectHealth = p.CorrectHealth,
                              CorrectHosp = p.CorrectHosp,
                              CorrectItems = p.CorrectItems,
                              CorrectItems2 = p.CorrectItems2,
                              CorrectOther = p.CorrectOther,
                              HospId = p.HospId,
                              HospSeqNo = p.HospSeqNo,
                              HospName = p.HospName,
                              ID = regex.Replace(p.ID, substitutionID),
                              Memo = p.Memo,
                              Name = p.Name.Remove(2).Insert(1, "O"),
                              ReceiveDate = p.ReceiveDate,
                              source = p.source,
                              UpdateAt = p.UpdateAt,
                              UpdatedBy = p.UpdatedBy,
                              CorrectionReason = p.CorrectionReason,
                              ReasonStatement = p.ReasonStatement
                          });
            list = list.OrderByDescending(x => x.ReceiveDate);
            return await QueryPaging(model.get(), list);
        }


        public async Task<LogicRtnModel<CorrectionSlipQueryModel>> UploadCorrectionSlip(IFormFile formFile)
        {
            LogicRtnModel<CorrectionSlipQueryModel> logicRtnModel = new LogicRtnModel<CorrectionSlipQueryModel>();
            int all_Count = 0;
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                ExcelReader excelReader = new ExcelReader();
                List<List<string>> content = excelReader.ReadAsLIstOfList(ms, "");
                all_Count = content.Count() - 1;
                using (var txn = context.GetTransactionScope())
                {
                    foreach (List<string> row in content)
                    {
                        try
                        {
                            if (content.First() == row)
                            {
                                logicRtnModel = CheckExcelHeader(row);
                                if (logicRtnModel.IsSuccess)
                                {
                                    continue;
                                }
                                else
                                {
                                    return logicRtnModel;
                                }
                            }
                            string rowdata = string.Empty;
                            CorrectionSlip obj = new CorrectionSlip();
                            for (int i = 0; i < 18; i++)
                            {
                                logicRtnModel = FillObjByIndex(obj, i, row, content.IndexOf(row));
                                if (!logicRtnModel.IsSuccess)
                                {
                                    return logicRtnModel;
                                }
                            }
                            obj.UpdatedBy = identity.Account;
                            if (!context.CorrectionSlip.Any(x => x.CaseNo == obj.CaseNo && x.ReceiveDate == obj.ReceiveDate))
                            {
                                context.CorrectionSlip.Add(obj);
                            }
                            else
                            {
                                context.CorrectionSlip.Update(obj);
                            }
                        }
                        catch (Exception e)
                        {
                            logicRtnModel.IsSuccess = false;
                            logicRtnModel.Data = new CorrectionSlipQueryModel()
                            {
                                err = new List<string>()
                            };
                            logicRtnModel.ErrMsg = e.InnerException.Message;
                            logicRtnModel.Data.err.Add(e.InnerException.Message);
                            return logicRtnModel;
                        }
                    }
                    try
                    {
                        DataInsertLog dataInsertLog = new Services.DataInsertLogService().dataInsertLog(formFile.FileName, all_Count);
                        await context.DataInsertLog.AddRangeAsync(dataInsertLog);
                        await context.SaveChangesAsync();
                        await txn.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        await txn.RollbackAsync();
                        logicRtnModel.IsSuccess = false;
                        logicRtnModel.ErrMsg = e.InnerException.Message;
                        logicRtnModel.Data.err.Add(e.InnerException.Message);
                        return logicRtnModel;
                    }
                }
            }
            logicRtnModel.Msg = "共寫入" + all_Count + "筆資料";
            logicRtnModel.IsSuccess = true;
            return logicRtnModel;
        }
        private LogicRtnModel<CorrectionSlipQueryModel> CheckExcelHeader(List<string> row)
        {
            CorrectionSlipQueryModel correctionSlipQueryModel = new CorrectionSlipQueryModel()
            {
                err = new List<string>()
            };
            string[] header = { "案件編號", "收件日期", "機構代碼", "院區別", "機構名稱", "個案姓名", "身分證號", "出生日期", "更-基本", "更-就醫", "更-衛教", "更-其他", "更正項目", "更正項目_2", "備註(資料來源)", "註記", "申請更正原因", "原因說明" };
            if (row.Count != header.Length)
            {
                correctionSlipQueryModel.err.Add("標題列應該有" + header.Length + "欄:" + string.Join(",", header));
            }
            else
            {
                for (int i = 0; i < row.Count; i++)
                {
                    if (row[i].Trim() != header[i])
                    {
                        correctionSlipQueryModel.err.Add($"標題列第{i + 1}({row[i].Trim()})欄名稱應該為{header[i]}");
                    }
                }
            }

            var rtnModel = new LogicRtnModel<CorrectionSlipQueryModel>();
            rtnModel.Data = correctionSlipQueryModel;
            if (rtnModel.Data.err.Count > 0)
            {
                rtnModel.IsSuccess = false;
                rtnModel.ErrMsg = string.Join("\r\n", rtnModel.Data.err.ToArray());
            }
            else
            {
                rtnModel.IsSuccess = true;
            }
            return rtnModel;




        }
        private LogicRtnModel<CorrectionSlipQueryModel> FillObjByIndex(CorrectionSlip obj, int i, List<string> row, int rownumber)
        {
            CorrectionSlipQueryModel correctionSlipQueryModel = new CorrectionSlipQueryModel()
            {
                err = new List<string>()
            };
            switch (i)
            {
                case 0:
                    if (row[i].Trim().Length == 9)
                    {
                        obj.CaseNo = row[i].Trim();
                    }
                    else
                    {
                        correctionSlipQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為9");
                        int CaseNo = 0;
                        bool canConvert = int.TryParse(row[i].Trim(), out CaseNo);
                        if (canConvert == false)
                        {
                            correctionSlipQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，應該為數字格式");
                        }
                    }
                    break;
                case 1:
                    string get_Day = ConvertToDateString(row[i]);
                    if (get_Day.Split('/').Length != 3 || get_Day == "error")
                    {
                        correctionSlipQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMMdd，或月分與日期異常");
                    }
                    else
                    {
                        try
                        {
                            int Reyear = Int32.Parse(get_Day.Split('/')[0]) + 1911;
                            int Remonth = Int32.Parse(get_Day.Split('/')[1]);
                            int Reday = Int32.Parse(get_Day.Split('/')[2]);
                            obj.ReceiveDate = new DateTime(Reyear, Remonth, Reday);
                        }
                        catch (Exception)
                        {
                            correctionSlipQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，日期應該為數字");
                        }

                    }

                    break;
                case 2:
                    obj.HospId = row[i];
                    break;
                case 3:
                    if (row[i].Trim().Length == 2)
                    {
                        obj.HospSeqNo = row[i];
                    }
                    else
                    {
                        correctionSlipQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為2");
                    }
                    break;
                case 4:
                    obj.HospName = row[i];
                    break;
                case 5:
                    obj.Name = row[i];
                    break;
                case 6:
                    if (row[i].Trim().Length == 10)
                    {
                        obj.ID = row[i].Trim();
                    }
                    else
                    {
                        correctionSlipQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為10");
                    }

                    break;
                case 7:
                    string Birday = ConvertToDateString(row[i]);
                    if (Birday.Split('/').Length != 3 || Birday == "error")
                    {
                        correctionSlipQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，格式應該為yyyMMdd，或月分與日期異常");
                    }
                    else
                    {
                        try
                        {
                            int Byear = Int32.Parse(Birday.Split('/')[0]) + 1911;
                            int Bmonth = Int32.Parse(Birday.Split('/')[1]);
                            int Bday = Int32.Parse(Birday.Split('/')[2]);
                            obj.Birthday = new DateTime(Byear, Bmonth, Bday);
                        }
                        catch (Exception)
                        {
                            correctionSlipQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，日期應該為數字");
                        }

                    }
                    break;
                case 8:
                    obj.CorrectBasic = row[i];
                    break;
                case 9:
                    obj.CorrectHosp = row[i];
                    break;
                case 10:
                    obj.CorrectHealth = row[i];
                    break;
                case 11:
                    obj.CorrectOther = row[i];
                    break;
                case 12:
                    obj.CorrectItems = row[i];
                    break;
                case 13:
                    obj.CorrectItems2 = row[i];
                    break;
                case 14:
                    obj.source = row[i];
                    break;
                case 15:
                    obj.Memo = row[i];
                    break;
                case 16:
                    obj.CorrectionReason = row[i];
                    break;
                case 17:
                    obj.ReasonStatement = row[i];
                    break;
                default:
                    break;
            }
            var rtnModel = new LogicRtnModel<CorrectionSlipQueryModel>();
            rtnModel.Data = correctionSlipQueryModel;
            if (rtnModel.Data.err.Count > 0)
            {
                rtnModel.IsSuccess = false;
                rtnModel.ErrMsg = string.Join("\r\n", rtnModel.Data.err.ToArray());
            }
            else
            {
                rtnModel.IsSuccess = true;
            }
            return rtnModel;
        }

        public static string ConvertToDateString(string input)
        {
            string year, month, day;
            string output;
            bool error_Check;

            if (input.Length == 6)
            {
                year = input.Substring(0, 2);
                month = input.Substring(2, 2);
                day = input.Substring(4, 2);
                error_Check = check_Day_Month(month, day);
                output = $"{year}/{month}/{day}";
            }
            else if (input.Length == 7)
            {
                year = input.Substring(0, 3);
                month = input.Substring(3, 2);
                day = input.Substring(5, 2);
                error_Check = check_Day_Month(month, day);
                output = $"{year}/{month}/{day}";
            }
            else
            {
                error_Check = false;
                output = "error";
            }

            if(error_Check != true)
            {
                output = "error";
            }
            
            return output;
        }

        public static bool check_Day_Month(string Month, string Day)
        {
            bool output;

            if (Month.Length != 2 || Int32.Parse(Month) > 12)
            {
                output = false;
            }
            else if (Day.Length != 2 || Int32.Parse(Day) > 31)
            {
                output = false;
            }
            else
            {
                output = true;
            }

            return output;
        }
    }
}
