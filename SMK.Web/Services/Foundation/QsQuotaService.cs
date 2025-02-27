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
using Dapper;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class QsQuotaService : GenericService
    {
        private readonly IDbConnection _conn = null;
        private readonly IWebHostEnvironment _env;
        public QsQuotaService(SMKWEBContext context, SessionManager smgr, IWebHostEnvironment env)
        : base(context, smgr)
        {
            _conn = context.Database.GetDbConnection();
            _env = env;
        }
        public async Task<LogicRtnModel<PagedModel<QsQuotaViewModel>>> QueryQsQuota(QsQuotaQueryModel model)
        {
            
            var list = context.QsQuota
                          .WhereWhen(!string.IsNullOrEmpty( model.YEARS), 
                                    p => p.YEARS ==model.YEARS)
                          .WhereWhen(!string.IsNullOrEmpty(model.HospID),
                                     p => p.HOSP_ID == model.HospID)
                          .WhereWhen(!string.IsNullOrEmpty(model.HOSP_SEQ_NO),
                                     p => p.HOSP_SEQ_NO == model.HOSP_SEQ_NO)
                          .Select(p => new QsQuotaViewModel()
                          {
                              YEARS = p.YEARS,
                              HOSP_ID = p.HOSP_ID,
                              HOSP_SEQ_NO = p.HOSP_SEQ_NO,
                              CURE_TYPE = p.CURE_TYPE,
                              QUOTA = p.QUOTA,
                              VALID_S_DATE = p.VALID_S_DATE,
                              VALID_E_DATE = p.VALID_E_DATE,
                              REMARK = p.REMARK,
                              CREAT_USER_ID = p.CREAT_USER_ID
                          }) ;
            return await QueryPaging(model.get(), list);
        }


        public async Task<LogicRtnModel<QsQuotaQueryModel>> UploadQsQuota(IFormFile formFile)
        {
            LogicRtnModel<QsQuotaQueryModel> logicRtnModel = new LogicRtnModel<QsQuotaQueryModel>();
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                ExcelReader excelReader = new ExcelReader();
                List<List<string>> content = excelReader.ReadAsLIstOfList(ms,"");
                string YEARS = string.Empty;
                if (formFile.FileName.Length <= 12 || !formFile.FileName.ToUpper().Contains("_QS_QUOTA"))
                {
                    logicRtnModel.IsSuccess = false;
                    logicRtnModel.ErrMsg = "檔名不符合規定 年度_QS_QUOTA";
                    return logicRtnModel;
                }
                else
                {
                    Int32 yr = 0;
                    if(!Int32.TryParse( formFile.FileName.Substring(0, 3),out yr))
                    {
                        logicRtnModel.IsSuccess = false;
                        logicRtnModel.ErrMsg = "年度必須為三碼數字";
                        return logicRtnModel;
                    }
                    YEARS = yr.ToString();
                    
                }
                
                using (var txn = context.GetTransactionScope())
                {
                    context.Database.ExecuteSqlRaw("Delete from CST_QS_QUOTA where YEARS = @YEARS", new SqlParameter("@YEARS", YEARS));
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
                            QsQuota obj = new QsQuota();
                            for (int i = 0; i < 9; i++)
                            {
                                logicRtnModel = FillObjByIndex(obj, i, row, content.IndexOf(row));
                                if (!logicRtnModel.IsSuccess)
                                {
                                    return logicRtnModel;
                                }
                            }
                            obj.CREAT_USER_ID = identity.Account;
                            context.QsQuota.Add(obj);
                            
                        }
                        catch (Exception e)
                        {
                            logicRtnModel.IsSuccess = false;
                            logicRtnModel.Data = new QsQuotaQueryModel()
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
            logicRtnModel.IsSuccess = true;
            return logicRtnModel;
        }
        private LogicRtnModel<QsQuotaQueryModel> CheckExcelHeader(List<string> row)
        {
            QsQuotaQueryModel qsQuotaQueryModel = new QsQuotaQueryModel() { 
                err = new List<string>()
            };
                            

            string[] header = { "療程年度別", "醫事機構代碼", "分院識別號", "療程類別", "當年度用藥/衛教配額", "生效起日", "生效迄日", "異動說明", "異動人員" };
            if (row.Count != header.Length )
            {
                qsQuotaQueryModel.err.Add("標題列應該有"+ header.Length+ "欄:"+string.Join(",",header));
            }
            else
            {
                for (int i = 0; i < row.Count; i++)
                {
                    if (row[i].Trim() != header[i])
                    {
                        qsQuotaQueryModel.err.Add($"標題列第{i + 1}({row[i].Trim()})欄名稱應該為{header[i]}");
                    }
                }
            }
            
            var rtnModel = new LogicRtnModel<QsQuotaQueryModel>();
            rtnModel.Data = qsQuotaQueryModel;
            if(rtnModel.Data.err.Count> 0)
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
        private LogicRtnModel<QsQuotaQueryModel> FillObjByIndex(QsQuota obj,int i,List<string> row,int rownumber )
        {
            QsQuotaQueryModel qsQuotaQueryModel = new QsQuotaQueryModel()
            {
                err = new List<string>()
            };
            switch (i)
            {
                case 0:
                    if (row[i].Trim().Length == 3)
                    {
                        obj.YEARS = row[i].Trim();
                    }
                    else
                    {
                        qsQuotaQueryModel.err.Add($"第{rownumber}列第{i+1}欄資料({row[i].Trim()})錯誤，長度不為3");
                        int CaseNo = 0;
                        bool canConvert = int.TryParse(row[i].Trim(), out CaseNo);
                        if (canConvert == false)
                        {
                            qsQuotaQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，應該為數字格式");
                        }
                    }
                    break;
                case 1:
                    obj.HOSP_ID = row[i].Trim();

                    break;
                case 2:
                    obj.HOSP_SEQ_NO = row[i].Trim();
                    break;
                case 3:
                    int CURE_TYPE = 0;
                    if (!int.TryParse(row[i].Trim(), out CURE_TYPE))
                    {
                        qsQuotaQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，應該為數字格式");
                    }
                    else
                    {
                        obj.CURE_TYPE = CURE_TYPE;
                    }
                    
                    break;
                case 4:
                    int QUOTA = 0;
                    if (!int.TryParse(row[i].Trim(), out QUOTA))
                    {
                        qsQuotaQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，應該為數字格式");
                    }
                    else
                    {
                        obj.QUOTA = QUOTA;
                    }
                    break;
                case 5:
                    if (row[i].Trim().Length == 8)
                    {
                        try
                        {
                            obj.VALID_S_DATE = DateTime.ParseExact(row[i].Trim(), "yyyyMMdd",
                CultureInfo.InvariantCulture);
                        }
                        catch (Exception e)
                        {
                            qsQuotaQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤:"+e.InnerException.Message);
                        }
                        
                    }
                    else
                    {
                        qsQuotaQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為8");
                    }
                    
                    break;
                case 6:
                    if (row[i].Trim().Length == 8)
                    {
                        try
                        {
                            obj.VALID_E_DATE = DateTime.ParseExact(row[i].Trim(),"yyyyMMdd",
                CultureInfo.InvariantCulture);
                        }
                        catch (Exception e)
                        {
                            qsQuotaQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤:" + e.InnerException.Message);
                        }

                    }
                    else
                    {
                        qsQuotaQueryModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為8");
                    }
                    break;
                case 7:
                    obj.REMARK = row[i].Trim();
                    break;
                case 8:
                    obj.ADJUST_USER_ID = row[i].Trim();
                    break;
                
                default:
                    break;
            }
            var rtnModel = new LogicRtnModel<QsQuotaQueryModel>();
            rtnModel.Data = qsQuotaQueryModel;
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
