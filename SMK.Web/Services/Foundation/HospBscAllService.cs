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

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class HospBscAllService : GenericService
    {
        private readonly IDbConnection _conn = null;
        private readonly IWebHostEnvironment _env;
        public HospBscAllService(SMKWEBContext context, SessionManager smgr, IWebHostEnvironment env)
        : base(context, smgr)
        {
            _conn = context.Database.GetDbConnection();
            _env = env;
        }

        public async Task<LogicRtnModel<PagedModel<HospBscAllViewModel>>> QueryHospBscAll(HospBscAllQueryModel model)
        {

            var list = context.HospBscAll
                          .WhereWhen(!string.IsNullOrEmpty(model.HospID),
                                     p => p.HospId == model.HospID)
                          .WhereWhen(!string.IsNullOrEmpty(model.HospName), x => x.HospName == model.HospName)
                          .Select(p => new HospBscAllViewModel()
                          {
                              BranchNo = p.BranchNo,
                              ContType = p.ContType,
                              HospAddress = p.HospAddress,
                              HospEndDate = p.HospEndDate,
                              HospId = p.HospId,
                              HospKind = p.HospKind,
                              HospName = p.HospName,
                              HospTel = p.HospTel,
                              HospTelArea = p.HospTelArea,
                              HospType = p.HospType,
                              OpenState = p.OpenState,
                              HospStartDate = p.HospStartDate,
                          });
            return await QueryPaging(model.get(), list);
        }


        public async Task<LogicRtnModel<HospBscAllQueryModel>> UploadHospBscAll(IFormFile formFile)
        {
            LogicRtnModel<HospBscAllQueryModel> logicRtnModel = new LogicRtnModel<HospBscAllQueryModel>();
            int Update_Count = 0;
            int Insert_Count = 0;
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                ExcelReader excelReader = new ExcelReader();
                List<List<string>> content = excelReader.ReadAsLIstOfList(ms, "");
                using (var txn = context.GetTransactionScope())
                {
                    //await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [HospBscAll]"); //把資料清空
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
                            HospBscAll obj = new HospBscAll();
                            for (int i = 0; i < 13; i++)
                            {
                                logicRtnModel = FillObjByIndex(obj, i, row, content.IndexOf(row));
                                if (!logicRtnModel.IsSuccess)
                                {
                                    return logicRtnModel;
                                }
                            }
                            if (context.HospBscAll.Any(x => x.HospId == obj.HospId))
                            {
                                var data = context.HospBscAll.Where(x => x.HospId == obj.HospId).AsNoTracking().FirstOrDefault();
                                if ((!string.IsNullOrEmpty(obj.HospEndDate) || !string.IsNullOrEmpty(obj.OpenState)) && obj.OpenState != "0")
                                {
                                    data.HospAddress = (string.IsNullOrEmpty(obj.HospAddress) || data.HospAddress != obj.HospAddress) ? data.HospAddress : obj.HospAddress;
                                    data.HospEndDate = obj.HospEndDate;
                                    data.OpenState = obj.OpenState;
                                    context.HospBscAll.Update(data);
                                }
                                else
                                {
                                    context.HospBscAll.UpdateRange(obj);
                                }
                                Update_Count++;
                            }
                            else
                            {
                                context.HospBscAll.Add(obj);
                                Insert_Count++;
                            }
                        }
                        catch (Exception e)
                        {
                            logicRtnModel.IsSuccess = false;
                            logicRtnModel.Data = new HospBscAllQueryModel()
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
                        DataInsertLog dataInsertLog = new Services.DataInsertLogService().dataInsertLog(formFile.FileName, Update_Count + Insert_Count);
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
            logicRtnModel.Msg = $"共寫入 {Update_Count + Insert_Count} 筆資料(新增 {Insert_Count} 筆，異動 {Update_Count} 筆)";
            logicRtnModel.IsSuccess = true;
            return logicRtnModel;
        }
        private LogicRtnModel<HospBscAllQueryModel> CheckExcelHeader(List<string> row)
        {
            HospBscAllQueryModel hospBscAllQueryModel = new HospBscAllQueryModel()
            {
                err = new List<string>()
            };
            string[] header = { "醫事機構代碼", "醫事機構名稱", "機構地址", "電話區域號碼", "電話號碼", "特約類別", "型態別", "醫事機構種類", "終止合約或歇業日期", "開業狀況", "原始合約起日", "分區別" };
            if (row.Count != header.Length)
            {
                hospBscAllQueryModel.err.Add("標題列應該有" + header.Length + "欄:" + string.Join(",", header));
            }
            else
            {
                for (int i = 0; i < row.Count; i++)
                {
                    if (row[i].Trim() != header[i])
                    {
                        hospBscAllQueryModel.err.Add($"標題列第{i + 1}({row[i].Trim()})欄名稱應該為{header[i]}");
                    }
                }
            }

            var rtnModel = new LogicRtnModel<HospBscAllQueryModel>();
            rtnModel.Data = hospBscAllQueryModel;
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
        private LogicRtnModel<HospBscAllQueryModel> FillObjByIndex(HospBscAll obj, int i, List<string> row, int rownumber)
        {
            HospBscAllQueryModel hospBscAllQueryModel = new HospBscAllQueryModel()
            {
                err = new List<string>()
            };
            switch (i)
            {
                case 0:
                    if (row[i].Trim().Length == 10)
                    {
                        obj.HospId = row[i].Trim();
                    }
                    else
                    {
                        hospBscAllQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為10");
                        int CaseNo = 0;
                        bool canConvert = int.TryParse(row[i].Trim(), out CaseNo);
                        if (canConvert == false)
                        {
                            hospBscAllQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，應該為數字格式");
                        }
                    }
                    break;
                case 1:
                    if (row[i].Trim().Length <= 80)
                    {
                        obj.HospName = row[i].Trim();
                    }
                    else
                    {
                        hospBscAllQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，長度應小於等於80");
                    }
                    break;
                case 2:
                    if (row[i].Trim().Length <= 80)
                    {
                        obj.HospAddress = row[i].Trim();
                    }
                    else
                    {
                        hospBscAllQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，長度應小於等於80");
                    }
                    break;
                case 3:
                    if (row[i].Trim().Length <= 5)
                    {
                        obj.HospTelArea = row[i].Trim();
                    }
                    else
                    {
                        hospBscAllQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，長度應小於等於5");
                    }
                    break;
                case 4:
                    if (row[i].Trim().Length <= 20)
                    {
                        obj.HospTel = row[i].Trim();
                    }
                    else
                    {
                        hospBscAllQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，長度應小於等於20");
                    }
                    break;
                case 5:
                    if (row[i].Trim().Length <= 5)
                    {
                        obj.ContType = row[i].Trim();
                    }
                    else
                    {
                        hospBscAllQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，長度應小於等於5");
                    }
                    break;
                case 6:
                    if (row[i].Trim().Length <= 5)
                    {
                        obj.HospType = row[i].Trim();
                    }
                    else
                    {
                        hospBscAllQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，長度應小於等於5");
                    }
                    break;
                case 7:
                    if (row[i].Trim().Length <= 5)
                    {
                        obj.HospKind = row[i].Trim();
                    }
                    else
                    {
                        hospBscAllQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，長度應小於等於5");
                    }
                    break;
                case 8:
                    if (row[i].Trim().Length <= 8)
                    {
                        obj.HospEndDate = row[i].Trim();
                    }
                    else
                    {
                        hospBscAllQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，長度應小於等於8");
                    }
                    break;
                case 9:
                    if (row[i].Trim().Length <= 1)
                    {
                        obj.OpenState = row[i].Trim();
                    }
                    else
                    {
                        hospBscAllQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，長度應小於等於1");
                    }
                    break;
                case 10:
                    if (row[i].Trim().Length <= 8)
                    {
                        obj.HospStartDate = row[i].Trim();
                    }
                    else
                    {
                        hospBscAllQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，長度應小於等於8");
                    }
                    break;
                case 11:
                    if (row[i].Trim().Length <= 1)
                    {
                        obj.BranchNo = row[i].Trim();
                    }
                    else
                    {
                        hospBscAllQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，長度應小於等於1");
                    }
                    break;

                default:
                    break;
            }
            var rtnModel = new LogicRtnModel<HospBscAllQueryModel>();
            rtnModel.Data = hospBscAllQueryModel;
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

    }
}
