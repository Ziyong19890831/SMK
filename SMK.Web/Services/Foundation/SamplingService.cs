using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SMK.Data;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Web.Extensions;
using SMK.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;
using Yozian.WebCore.Library.Utility.Excel;

namespace SMK.Web.Services.Foundation
{
    /// <summary>
    /// 抽樣作業
    /// </summary>
    [ScopedService]
    public class SamplingService : GenericService
    {
        private readonly IDbConnection _conn = null;
        private readonly IWebHostEnvironment _env;

        public SamplingService(SMKWEBContext context, SessionManager smgr, IWebHostEnvironment env)
            : base(context, smgr)
        {
            _conn = context.Database.GetDbConnection();
            _env = env;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<LogicRtnModel<SamplingExportViewModel>> Upload(IFormFile file,string year)
        {
            string sql = @"delete SamplingData from SamplingData D join SamplingList L on D.fee_ym = L.fee_ym and D.data_id = L.data_id where substring(L.SamplingNo,1,3)=@year";
            var ret = await _conn.ExecuteAsync(sql, new { year = year });
            sql = @"delete from SamplingList where substring(SamplingNo,1,3)=@year";
            ret = await _conn.ExecuteAsync(sql,new { year = year});
            LogicRtnModel<SamplingExportViewModel> logicRtnModel = new LogicRtnModel<SamplingExportViewModel>();
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                ExcelReader excelReader = new ExcelReader();
                List<List<string>> content = excelReader.ReadAsLIstOfList(ms, "");
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
                            SamplingList samplingList = new SamplingList();
                            SamplingData samplingData = new SamplingData()
                            {
                                OrderSeqNo = 1
                            };
                            for (int i = 0; i < 7; i++)
                            {
                                logicRtnModel = FillObjByIndex(samplingList,samplingData, i, row, content.IndexOf(row));
                                if (!logicRtnModel.IsSuccess)
                                {
                                    return logicRtnModel;
                                }
                            }
                            context.SamplingList.Add(samplingList);
                            context.SamplingData.Add(samplingData);

                        }
                        catch (Exception e)
                        {
                            logicRtnModel.IsSuccess = false;
                            logicRtnModel.Data = new SamplingExportViewModel()
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
        private LogicRtnModel<SamplingExportViewModel> FillObjByIndex(SamplingList samplingList,SamplingData samplingData, int i, List<string> row, int rownumber)
        {
            SamplingExportViewModel samplingExportViewModel = new SamplingExportViewModel()
            {
                err = new List<string>()
            };
            switch (i)
            {
                case 0:
                    if (row[i].Trim().Length == 28)
                    {
                        samplingList.DataId = row[i].Trim();
                        samplingData.DataId = row[i].Trim();
                    }
                    else
                    {
                        samplingExportViewModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為29");
                    }
                    break;
                case 1:
                    if (row[i].Trim().Length == 6)
                    {
                        samplingList.FeeYm = row[i].Trim();
                        samplingData.FeeYm = row[i].Trim();
                    }
                    else
                    {
                        samplingExportViewModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，長度不為6");
                    }
                    break;
                case 2:

                    samplingData.Review = row[i].Trim();
                    break;
                case 3:
                    int Reviewamt;
                    if (Int32.TryParse(row[i].Trim(),out Reviewamt))
                    {
                        samplingData.Reviewamt = Reviewamt;
                    }
                    else
                    {
                        samplingExportViewModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，應該為數字");
                    }
                    break;
                case 4:
                    samplingData.Appeals = row[i].Trim();
                    break;
                case 5:
                    int Appealsamt;
                    if (Int32.TryParse(row[i].Trim(), out Appealsamt))
                    {
                        samplingData.Appealsamt = Appealsamt;
                    }
                    else
                    {
                        samplingExportViewModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，應該為數字");
                    }
                    break;
                case 6:                    
                    if (row[i].Trim().Length == 7)
                    {
                        samplingList.SamplingNo = row[i].Trim();
                    }
                    else
                    {
                        samplingExportViewModel.err.Add($"第{rownumber}列第{i + 1}欄資料({row[i].Trim()})錯誤，長度應為7碼");
                    }
                    break;
                       
                default:
                    break;
            }
            var rtnModel = new LogicRtnModel<SamplingExportViewModel>();
            rtnModel.Data = samplingExportViewModel;
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

        private LogicRtnModel<SamplingExportViewModel> CheckExcelHeader(List<string> row)
        {
            SamplingExportViewModel correctionSlipQueryModel = new SamplingExportViewModel()
            {
                err = new List<string>()
            };
            string[] header = { "data_id", "費用年月", "審查委員", "申復補付金額", "申復審查委員", "最終核扣金額","抽樣編號" };
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

            var rtnModel = new LogicRtnModel<SamplingExportViewModel>();
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
        /// <summary>
        /// 抽樣查詢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SamplingItemDto>> GetSamplingItemsAsync(SamplingQueryModel request)
        {
            var wkYYYYMM_S = request.FeeStart?.ToYYYYMMFromTaiwan();
            var wkYYYYMM_E = request.FeeEnd?.ToYYYYMMFromTaiwan();

            if (string.IsNullOrEmpty(wkYYYYMM_S) || string.IsNullOrEmpty(wkYYYYMM_E))
            {

                throw new ApplicationException("費用年月不可為空白!");
            }

            var items = await GetSamplingItemsAsyncImpl(wkYYYYMM_S, wkYYYYMM_E);


            foreach (var item in items)
            {
                item.Birthday = item.Birthday?.ToSimpleTaiwanDateFromAD();
                item.FirstTreatDate = item.FirstTreatDate?.ToSimpleTaiwanDateFromAD();
                item.FuncDate = item.FuncDate?.ToSimpleTaiwanDateFromAD();
                item.FeeYearMonth = item.FeeYearMonth?.ToSlashTaiwanDateFromYYYYMM();
            }

            return items;
        }

        private async Task<IList<SamplingItemDto>> GetSamplingItemsAsyncImpl(string wkYYYYMM_S, string wkYYYYMM_E)
        {
            var sql =
@"SELECT b.data_id as DataId, 
       b.fee_ym as FeeYearMonth, 
       a.samplingno, 
       b.hospid, 
       b.id, 
       b.birthday,
       CASE
           WHEN RTRIM(ISNULL(c.name, '')) = ''
           THEN d.name
           ELSE c.name
       END AS name, 
       b.examyear, 
       b.examtime, 
       b.firsttreatdate, 
       b.weekcount, 
       b.func_date as FuncDate, 
       b.drug_days as DrugDays, 
       b.appl_dot as ApplDot
FROM SamplingList a
     JOIN DtlWithSet b ON a.data_id = b.data_id
                          AND a.fee_ym = b.fee_ym
     LEFT JOIN MhbtAgentPatient c ON b.hospid = c.hospid
                                     AND b.id = c.id
                                     AND b.birthday = c.birthday
     LEFT JOIN
(
    SELECT DISTINCT 
           a.hospid, 
           a.id, 
           a.birthday, 
           MAX(a.name) AS name
    FROM
    (
        SELECT hospid, 
               id, 
               birthday, 
               name
        FROM iniOpDtl I
        WHERE name IS NOT NULL
              AND EXISTS
        (
            SELECT 'x'
            FROM SamplingList a
                 JOIN DtlWithSet b ON a.data_id = b.data_id
                                      AND a.fee_ym = b.fee_ym
            WHERE a.fee_ym >= @wkYYYYMM_S
                  AND a.fee_ym <= @wkYYYYMM_E
                  AND b.ID = I.id
        )
        UNION
        SELECT hospid, 
               id, 
               birthday, 
               name
        FROM iniDrDtl I
        WHERE name IS NOT NULL
              AND EXISTS
        (
            SELECT 'x'
            FROM SamplingList a
                 JOIN DtlWithSet b ON a.data_id = b.data_id
                                      AND a.fee_ym = b.fee_ym
            WHERE a.fee_ym >= @wkYYYYMM_S
                  AND a.fee_ym <= @wkYYYYMM_E
                  AND b.ID = I.id
        )
    ) a
    GROUP BY a.hospid, 
             a.id, 
             a.birthday
) d ON b.hospid = d.hospid
       AND b.id = d.id
       AND b.birthday = d.birthday
WHERE a.fee_ym >= @wkYYYYMM_S
      AND a.fee_ym <= @wkYYYYMM_E
ORDER BY b.fee_ym, 
         b.hospid, 
         b.func_date, 
         b.id";

            var items = await _conn.QueryAsync<SamplingItemDto>(sql, new { wkYYYYMM_S, wkYYYYMM_E }, commandTimeout: 2000000);
            return items.ToList();
        }

        /// <summary>
        /// 戒菸服務專業審查調閱清單
        /// </summary>
        /// <param name="feeYmS"></param>
        /// <param name="feeYmE"></param>
        /// <returns></returns>
        public async Task<Rew1500ViewModel> ExportRew1500Async(string feeYmS, string feeYmE,string fileType)
        {
            var yyyyMM_S = feeYmS.ToYYYYMMFromTaiwan();
            var yyyyMM_E = feeYmE.ToYYYYMMFromTaiwan();

            if (string.IsNullOrEmpty(yyyyMM_S) || string.IsNullOrEmpty(yyyyMM_E))
            {
                throw new ApplicationException("費用年月不可為空白!");
            }

            var list = await GetRew1500ItemsAsyncImpl(yyyyMM_S, yyyyMM_E);

            // 遮蔽敏感資訊
            foreach (var item in list)
            {
                item.Id = item.Id.MaskText('*', startIndex: 4, count: 3);
                item.Name = item.Name.MaskText('O', startIndex: 1, count: 1);
            }

            var printDate = DateTime.Today.ToString("yyyy/MM/dd");
            var feeYm_S = feeYmS.ToSlashTaiwanDateFromTaiwanYYYMM();
            var feeYm_E = feeYmE.ToSlashTaiwanDateFromTaiwanYYYMM();

            var vm = new Rew1500ViewModel
            {
                FileName = $"戒菸服務專業審查調閱清單({feeYmS.ToYYYYMMFromTaiwan()}-{feeYmE.ToYYYYMMFromTaiwan()}).{fileType.ToString()}",
                Stream = new MemoryStream()
            };

            var fi = new FileInfo(Path.Combine(_env.ContentRootPath, "ExcelTemplates/REW1500_Template.xlsx"));

            using (var excel = new ExcelPackage())
            using (var package = new ExcelPackage(fi))
            {
                excel.Encryption.Password = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString();
                excel.Encryption.IsEncrypted = true;
                var workbookTemplate = package.Workbook;
                var worksheetTemplate = workbookTemplate.Worksheets.First();

                var g = list.GroupBy(x => new { x.BranchName, x.HospName, x.HospId, x.OHospName, x.CaseType });
                int sheetNumber = 0;
                foreach (var keyValues in g)
                {
                    var branchName = keyValues.Key.BranchName;
                    var hospName = keyValues.Key.HospName;
                    var hospId = keyValues.Key.HospId;
                    var caseType = keyValues.Key.CaseType;

                    var excelParameters = new (string Key, string Value)[]
                    {
                       ("##PrintDate##",  printDate),
                       ("##BranchName##", branchName),
                       ("##HospName##", hospName),
                       ("##HospId##", hospId),
                       ("##CaseType##", caseType),
                       ("##FEE_YM_S##", feeYm_S),
                       ("##FEE_YM_E##", feeYm_E)
                    };

                    sheetNumber++;
                    var sheetName = $"{branchName}_{sheetNumber}";
                    var gList = keyValues.OrderBy(x => x.Id).ToList();

                    CreateWorksheetByData(gList, sheetName, excelParameters, excel, worksheetTemplate);
                }

                if (!g.Any())
                {
                    excel.Workbook.Worksheets.Add("查無資料");
                    excel.Workbook.Worksheets.First().Cells[1, 1, 1, 1].Value = "查無資料";
                }

                excel.SaveAs(vm.Stream, DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString());
                vm.Stream.Seek(0, SeekOrigin.Begin);
            }

            return vm;
        }


        private static void CreateWorksheetByData(List<Rew1500ItemDto> list, string sheetName, (string Key, string Value)[] excelParameters, ExcelPackage excel, ExcelWorksheet worksheetTemplate)
        {
            excel.Encryption.Password = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString();
            excel.Encryption.IsEncrypted = true;
            const int headerRowLength = 8;
            const int headerColLength = 7;

            var sheet1 = excel.Workbook.Worksheets.Add(sheetName);

            #region Header

            worksheetTemplate.Cells[1, 1, headerRowLength, headerColLength].Copy(sheet1.Cells);

            foreach (var cell in sheet1.Cells.AsEnumerable())
            {
                var text = Convert.ToString(cell.Value);

                if (text != null)
                {
                    while (text.Contains("##"))
                    {
                        var parameter = excelParameters.FirstOrDefault(x => text.Contains(x.Key));
                        if (parameter == default)
                        {
                            break;
                        }
                        cell.Value = text = text.Replace(parameter.Key, parameter.Value);
                    }
                }

            }

            for (var i = 1; i <= headerColLength; i++)
            {
                sheet1.Column(i).Width = worksheetTemplate.Column(i).Width;
            }

            for (var i = 1; i <= headerRowLength; i++)
            {
                sheet1.Row(i).Height = worksheetTemplate.Row(i).Height;
            }

            #endregion Header

            #region Details

            int row = headerRowLength + 1;
            foreach (var item in list)
            {
                // detail text
                sheet1.Cells[row, 1].Value = item.ApplDate;
                sheet1.Cells[row, 2].Value = item.SeqNo;
                sheet1.Cells[row, 3].Value = item.Id;
                sheet1.Cells[row, 4].Value = item.Name;
                sheet1.Cells[row, 5].Value = item.Birthday;
                sheet1.Cells[row, 6].Value = item.FuncDate;
                sheet1.Cells[row, 7].Value = item.ApplDot;

                // detail style
                for (var i = 1; i <= headerColLength; i++)
                {
                    var cell = sheet1.Cells[row, i];
                    cell.Style.Font.Size = 11;
                    cell.Style.Font.Name = "新細明體";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                sheet1.Cells[row, 7].Style.Numberformat.Format = "#,##0";

                sheet1.Row(row).Height = 21;

                row++;
            }

            #endregion Details

            #region 小計

            sheet1.Cells[row, 1].Value = "送審案件：";
            sheet1.Cells[row, 2].Value = list.Count;
            sheet1.Cells[row, 3].Value = "件";
            sheet1.Cells[row, 5].Value = "申請金額(點數)：";
            sheet1.Cells[row, 6].Value = list.Sum(x => x.ApplDot.GetValueOrDefault());
            sheet1.Cells[row, 7].Value = "元";
            for (var i = 1; i <= headerColLength; i++)
            {
                var cell = sheet1.Cells[row, i];
                cell.Style.Font.Size = 11;
                cell.Style.Font.Name = "新細明體";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            sheet1.Cells[row, 2].Style.Numberformat.Format = "#,##0";
            sheet1.Cells[row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            sheet1.Cells[row, 6].Style.Numberformat.Format = "#,##0";
            sheet1.Cells[row, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            sheet1.Row(row).Height = 40;

            #endregion 小計

            #region 列印

            var srcPrinterSettings = worksheetTemplate.PrinterSettings;
            var tarPrinterSettings = sheet1.PrinterSettings;

            tarPrinterSettings.HeaderMargin = srcPrinterSettings.HeaderMargin;
            tarPrinterSettings.FooterMargin = srcPrinterSettings.FooterMargin;
            tarPrinterSettings.TopMargin = srcPrinterSettings.TopMargin;
            tarPrinterSettings.BottomMargin = srcPrinterSettings.BottomMargin;
            tarPrinterSettings.LeftMargin = srcPrinterSettings.LeftMargin;
            tarPrinterSettings.RightMargin = srcPrinterSettings.RightMargin;
            tarPrinterSettings.RepeatColumns = srcPrinterSettings.RepeatColumns;
            tarPrinterSettings.RepeatRows = srcPrinterSettings.RepeatRows;
            tarPrinterSettings.PrintArea = sheet1.Cells;

            #endregion 列印
        }


        private async Task<IList<Rew1500ItemDto>> GetRew1500ItemsAsyncImpl(string wkFeeYM_S, string wkFeeYM_E)
        {
            var sql =
@"SELECT f.branchname AS BranchName, 
       f.hospid AS HospId, 
       f.hospname AS HospName, 
       f.o_hospname AS OHospName, 
       f.appl_type AS ApplType, 
       f.case_type AS CaseType, 
       f.appl_date AS ApplDate, 
       f.fee_ym AS FeeYM, 
       f.seq_no AS SeqNo, 
       f.id AS Id, 
       f.name AS Name, 
       f.birthday AS Birthday, 
       f.func_date AS FuncDate, 
       f.appl_dot AS ApplDot
FROM
(
    SELECT d.branchname, 
           c.hospid, 
           c.hospname, 
           NULL AS o_hospname,
           CASE
               WHEN b.appl_type = '1'
               THEN '送核'
               WHEN b.appl_type = '2'
               THEN '補報'
           END AS appl_type, 
           b.case_type, 
           CONVERT(VARCHAR, SUBSTRING(b.appl_date, 1, 4) - 1911) + '/' + SUBSTRING(b.appl_date, 5, 2) + '/' + SUBSTRING(b.appl_date, 7, 2) AS appl_date, 
           CONVERT(VARCHAR, SUBSTRING(b.fee_ym, 1, 4) - 1911) + '/' + SUBSTRING(b.fee_ym, 5, 2) AS fee_ym, 
           b.seq_no, 
           SUBSTRING(b.id, 1, 4) + '***' + SUBSTRING(b.id, 8, 3) AS id, 
           SUBSTRING((CASE
                          WHEN RTRIM(ISNULL(f.name, '')) = ''
                          THEN b.name
                          ELSE f.name
                      END), 1, 1) + 'O' + SUBSTRING((CASE
                                                         WHEN RTRIM(ISNULL(f.name, '')) = ''
                                                         THEN b.name
                                                         ELSE f.name
                                                     END), 3, 1) AS name, 
           CONVERT(VARCHAR, SUBSTRING(b.birthday, 1, 4) - 1911) + '/' + SUBSTRING(b.birthday, 5, 2) + '/' + SUBSTRING(b.birthday, 7, 2) AS birthday, 
           CONVERT(VARCHAR, SUBSTRING(b.func_date, 1, 4) - 1911) + '/' + SUBSTRING(b.func_date, 5, 2) + '/' + SUBSTRING(b.func_date, 7, 2) AS func_date, 
           b.appl_dot
    FROM SamplingList a
         JOIN
    (
        SELECT data_id, 
               fee_ym, 
               seq_no, 
               hospid, 
               id, 
               appl_type, 
               case_type, 
               appl_date, 
               birthday, 
               func_date, 
               appl_dot, 
               name
        FROM iniOpDtl
        WHERE 1 = 1
              AND fee_ym >= @wkFeeYM_S
              AND fee_ym <= @wkFeeYM_E
    ) b ON a.fee_ym = b.fee_ym
           AND a.data_id = b.data_id
         JOIN
    (
        SELECT *
        FROM HospBasic
    ) c ON b.hospid = c.hospid
         JOIN GenBranch d ON c.branchno = d.branchno
         LEFT JOIN MhbtAgentPatient f ON b.hospid = f.hospid
                                         AND b.id = f.id
                                         AND b.birthday = f.birthday
    WHERE a.fee_ym >= @wkFeeYM_S
          AND a.fee_ym <= @wkFeeYM_E
    UNION ALL
    SELECT d.branchname, 
           c.hospid, 
           c.hospname, 
           e.hospname AS o_hospname,
           CASE
               WHEN b.appl_type = '1'
               THEN '送核'
               WHEN b.appl_type = '2'
               THEN '補報'
           END AS appl_type, 
           b.case_type, 
           CONVERT(VARCHAR, SUBSTRING(b.appl_date, 1, 4) - 1911) + '/' + SUBSTRING(b.appl_date, 5, 2) + '/' + SUBSTRING(b.appl_date, 7, 2) AS appl_date, 
           CONVERT(VARCHAR, SUBSTRING(b.fee_ym, 1, 4) - 1911) + '/' + SUBSTRING(b.fee_ym, 5, 2) AS fee_ym, 
           b.seq_no, 
           SUBSTRING(b.id, 1, 4) + '***' + SUBSTRING(b.id, 8, 3) AS id, 
           SUBSTRING((CASE
                          WHEN RTRIM(ISNULL(f.name, '')) = ''
                          THEN b.name
                          ELSE f.name
                      END), 1, 1) + 'O' + SUBSTRING((CASE
                                                         WHEN RTRIM(ISNULL(f.name, '')) = ''
                                                         THEN b.name
                                                         ELSE f.name
                                                     END), 3, 1) AS name, 
           CONVERT(VARCHAR, SUBSTRING(b.birthday, 1, 4) - 1911) + '/' + SUBSTRING(b.birthday, 5, 2) + '/' + SUBSTRING(b.birthday, 7, 2) AS birthday, 
           CONVERT(VARCHAR, SUBSTRING(b.func_date, 1, 4) - 1911) + '/' + SUBSTRING(b.func_date, 5, 2) + '/' + SUBSTRING(b.func_date, 7, 2) AS func_date, 
           b.appl_dot
    FROM SamplingList a
         JOIN
    (
        SELECT data_id, 
               fee_ym, 
               seq_no, 
               hospid, 
               id, 
               appl_type, 
               case_type, 
               appl_date, 
               birthday, 
               func_date, 
               orig_hosp_id, 
               appl_dot, 
               name
        FROM iniDrDtl
        WHERE 1 = 1
              AND fee_ym >= @wkFeeYM_S
              AND fee_ym <= @wkFeeYM_E
    ) b ON a.fee_ym = b.fee_ym
           AND a.data_id = b.data_id
         JOIN
    (
        SELECT *
        FROM HospBasic
    ) c ON b.hospid = c.hospid
         JOIN GenBranch d ON c.branchno = d.branchno
         LEFT JOIN
    (
        SELECT *
        FROM HospBasic
        WHERE hospseqno = '00'
    ) e ON b.orig_hosp_id = e.hospid
         LEFT JOIN MhbtAgentPatient f ON b.hospid = f.hospid
                                         AND b.id = f.id
                                         AND b.birthday = f.birthday
    WHERE a.fee_ym >= @wkFeeYM_S
          AND a.fee_ym <= @wkFeeYM_E
) f
ORDER BY f.hospid, 
         f.id, 
         f.func_date;";

            var items = await _conn.QueryAsync<Rew1500ItemDto>(sql, new { wkFeeYM_S, wkFeeYM_E }, commandTimeout: 2000000);
            return items.ToList();
        }
    }
}
