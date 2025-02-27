using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Web.Exceptions;
using SMK.Web.Extensions;
using SMK.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Web.Services.Foundation
{
    /// <summary>
    /// 專業審查
    /// </summary>
    [ScopedService]
    public class SamplingExamineService : GenericService
    {
        private readonly IDbConnection _conn = null;
        private readonly IWebHostEnvironment _env;

        public SamplingExamineService(SMKWEBContext context, SessionManager smgr, IWebHostEnvironment env)
            : base(context, smgr)
        {
            _conn = context.Database.GetDbConnection();
            _env = env;
        }

        public async Task<IList<SamplingExamineCreateData>> GetSamplingExamineCreateDatasAsync(SamplingExamineQueryModel request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var wkYYYYMM_S = request.FeeStart?.ToYYYYMMFromTaiwan();
            var wkYYYYMM_E = request.FeeEnd?.ToYYYYMMFromTaiwan();

            if (string.IsNullOrEmpty(wkYYYYMM_S) || string.IsNullOrEmpty(wkYYYYMM_E))
            {
                throw new ApplicationException("費用年月不可為空白!");
            }

            if (string.IsNullOrEmpty(request.HospID))
            {
                throw new ApplicationException("醫療院所不可為空白!");
            }

            var list = await GetSamplingExamineCreateDatasImpl(wkYYYYMM_S, wkYYYYMM_E, request.HospID, request.AccessNo?.Trim());

            foreach (var item in list)
            {
                item.fee_ym_taiwan = item.fee_ym?.ToSlashTaiwanDateFromYYYYMM();
                item.func_date_taiwan = item.func_date?.ToSlashTaiwanDateFromYYYYMMDD();
                item.reviewdate_taiwan = item.reviewdate?.ToSlashTaiwanDateFromYYYYMMDD();
                item.appealsdate_taiwan = item.appealsdate?.ToSlashTaiwanDateFromYYYYMMDD();
            }

            return list;

        }

        private async Task<IList<SamplingExamineCreateData>> GetSamplingExamineCreateDatasImpl(string wkYYYYMM_S, string wkYYYYMM_E, string wkHospID, string accessNo)
        {
            var sql =
@"SELECT a.samplingno, 
       b.fee_ym, 
       b.data_id, 
       b.id,
       CASE
           WHEN RTRIM(ISNULL(b.name, '')) = ''
           THEN d.name
           ELSE b.name
       END AS name, 
       b.func_date, 
       b.appl_dot, 
       c.review, 
       c.reviewdate, 
       c.appeals, 
       c.appealsdate, 
       a.chkflg
FROM
(
    SELECT *
    FROM SamplingList
    WHERE fee_ym >= @wkYYYYMM_S
          AND fee_ym <= @wkYYYYMM_E
) a
JOIN
(
    SELECT a.fee_ym, 
           a.data_id, 
           a.id, 
           b.name, 
           a.func_date, 
           a.appl_dot, 
           a.hospid, 
           a.birthday
    FROM iniOpDtl a
         LEFT JOIN MhbtAgentPatient b ON a.hospid = b.hospid
                                         AND a.id = b.id
                                         AND a.birthday = b.birthday
    WHERE a.fee_ym >= @wkYYYYMM_S
          AND a.fee_ym <= @wkYYYYMM_E
    UNION
    SELECT a.fee_ym, 
           a.data_id, 
           a.id, 
           b.name, 
           a.func_date, 
           a.appl_dot, 
           a.hospid, 
           a.birthday
    FROM iniDrDtl a
         LEFT JOIN MhbtAgentPatient b ON a.hospid = b.hospid
                                         AND a.id = b.id
                                         AND a.birthday = b.birthday
    WHERE a.fee_ym >= @wkYYYYMM_S
          AND a.fee_ym <= @wkYYYYMM_E
) b ON a.fee_ym = b.fee_ym
       AND a.data_id = b.data_id
LEFT JOIN
(
    SELECT DISTINCT 
           fee_ym, 
           data_id, 
           review, 
           reviewdate, 
           appeals, 
           appealsdate
    FROM SamplingData
) c ON a.fee_ym = c.fee_ym
       AND a.data_id = c.data_id
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
        FROM iniOpDtl
        WHERE name IS NOT NULL
              AND fee_ym >= @wkYYYYMM_S
              AND fee_ym <= @wkYYYYMM_E
        UNION
        SELECT hospid, 
               id, 
               birthday, 
               name
        FROM iniDrDtl
        WHERE name IS NOT NULL
              AND fee_ym >= @wkYYYYMM_S
              AND fee_ym <= @wkYYYYMM_E
    ) a
    GROUP BY a.hospid, 
             a.id, 
             a.birthday
) d ON b.hospid = d.hospid
       AND b.id = d.id
       AND b.birthday = d.birthday
WHERE b.hospid = @wkHospID";

            if (!string.IsNullOrEmpty(accessNo))
            {
                sql += " and a.accessno = @accessNo";
            }

            var items = await _conn.QueryAsync<SamplingExamineCreateData>(sql, new
            {
                wkYYYYMM_S,
                wkYYYYMM_E,
                wkHospID,
                accessNo
            }, commandTimeout: 2000000);

            return items.ToList();
        }

        public async Task SaveSamplingCreateDataAsync(SamplingCreateRequest request)
        {
            if (request == null || !request.isReview && !request.isAppeals)
            {
                throw new ApplicationException("審查選項請至少勾選一項");
            }

            var reviewDate = request.reviewdate?.ToYYYYMMDDFromTaiwan();
            var appealsDate = request.appealsdate?.ToYYYYMMDDFromTaiwan();

            if ((!string.IsNullOrEmpty(request.review) && string.IsNullOrEmpty(reviewDate))
                || (string.IsNullOrEmpty(request.review) && !string.IsNullOrEmpty(reviewDate)))
            {
                throw new ApplicationException("審查委員及日期皆須輸入");
            }

            if ((!string.IsNullOrEmpty(request.appeals) && string.IsNullOrEmpty(appealsDate))
                || (string.IsNullOrEmpty(request.appeals) && !string.IsNullOrEmpty(appealsDate)))
            {
                throw new ApplicationException("申覆審查委員及日期皆須輸入");
            }

            if (request.Items == null || request.Items.All(x => !x.isChecked))
            {
                throw new ApplicationException("請至少選擇一個項目");
            }

            var checkedItems = request.Items.Where(x => x.isChecked).ToList();

            foreach (var item in checkedItems)
            {
                var samplingList = await context.SamplingList.FirstOrDefaultAsync(x => x.DataId == item.data_id && x.FeeYm == x.FeeYm);
                if (samplingList != null)
                {
                    samplingList.ChkFlg = (item.chkflg == "1") ? item.chkflg : null;
                }

                var samplingDatas = await context.SamplingData.Where(x => x.DataId == item.data_id && x.FeeYm == x.FeeYm).ToListAsync();
                if (!samplingDatas.Any())
                {
                    var iniOpOrdWithIniDrOrds =
                        await context
                            .IniOpOrd
                            .Where(x => x.DataId == item.data_id && x.FeeYm == x.FeeYm)
                            .Select(x => new
                            {
                                x.FeeYm,
                                x.DataId,
                                x.OrderSeqNo
                            })
                            .Concat(
                                context
                                .IniDrOrd
                                .Where(x => x.DataId == item.data_id && x.FeeYm == x.FeeYm)
                                .Select(x => new
                                {
                                    x.FeeYm,
                                    x.DataId,
                                    x.OrderSeqNo
                                }))
                            .Distinct()
                            .ToListAsync();

                    samplingDatas
                        = iniOpOrdWithIniDrOrds
                            .Select(x => new SamplingData
                            {
                                FeeYm = x.FeeYm,
                                DataId = x.DataId,
                                OrderSeqNo = x.OrderSeqNo
                            })
                            .ToList();

                    context.SamplingData.AddRange(samplingDatas);
                }

                foreach (var samplingData in samplingDatas)
                {
                    if (request.isReview)
                    {
                        samplingData.Review = request.review?.Trim()?.TruncateString(20);
                        samplingData.Reviewdate = reviewDate;
                    }
                    else
                    {
                        samplingData.Review = null;
                        samplingData.Reviewdate = null;
                    }

                    if (request.isAppeals)
                    {
                        samplingData.Appeals = request.appeals?.Trim().TruncateString(20);
                        samplingData.Appealsdate = appealsDate;
                    }
                    else
                    {
                        samplingData.Appeals = null;
                        samplingData.Appealsdate = null;
                    }
                }

            }

            await context.SaveChangesAsync();
        }

        public async Task<IList<string>> GetAccessnosAsync(string feeStart, string feeEnd)
        {
            var wkYYYYMM_S = feeStart?.ToYYYYMMFromTaiwan();
            var wkYYYYMM_E = feeEnd?.ToYYYYMMFromTaiwan();

            if (string.IsNullOrEmpty(wkYYYYMM_S) || string.IsNullOrEmpty(wkYYYYMM_E))
            {
                throw new ApplicationException("費用年月不可為空白!");
            }

            var accessnos
                 = await context.SamplingList
                    .Where(x => x.Accessno != null)
                    .Where(x => x.FeeYm.CompareTo(wkYYYYMM_S) >= 0 && x.FeeYm.CompareTo(wkYYYYMM_E) <= 0)
                    .Select(x => x.Accessno)
                    .Distinct()
                    .OrderByDescending(x => x)
                    .ToListAsync();

            return accessnos;
        }


        public async Task<IList<SamplingResultQueryData>> GetWithCreateSamplingResultQueryDatasAsync(SamplingResultQueryRequest request)
        {
            if (string.IsNullOrEmpty(request?.SamplingNo))
            {
                throw new ApplicationException("抽樣編號不可為空白!");
            }

            await CreateSamplingDataIfNotExistsAsyncImpl(request.SamplingNo);

            var list = await GetSamplingResultQueryDatasAsyncImpl(request.SamplingNo);

            foreach (var item in list)
            {
                item.func_date = item.func_date?.ToSlashTaiwanDateFromYYYYMMDD();
                item.reviewdate = item.reviewdate?.ToSlashTaiwanDateFromYYYYMMDD();
                item.appealsdate = item.appealsdate?.ToSlashTaiwanDateFromYYYYMMDD();
            }

            return list;
        }

        private async Task CreateSamplingDataIfNotExistsAsyncImpl(string wkSamplingNo)
        {
            var sql =
@"IF NOT EXISTS
(
    SELECT *
    FROM SamplingData
    WHERE EXISTS
    (
        SELECT data_id
        FROM SamplingList
        WHERE SamplingNo = @wkSamplingNo
              AND SamplingData.data_id = SamplingList.data_id
    )
)
    INSERT INTO SamplingData
    (fee_ym, 
     data_id, 
     order_seq_no
    )
           SELECT fee_ym, 
                  data_id, 
                  order_seq_no
           FROM iniOpOrd
           WHERE EXISTS
           (
               SELECT 'x'
               FROM SamplingList
               WHERE SamplingNo = @wkSamplingNo
                     AND iniOpOrd.data_id = SamplingList.data_id
           )
           UNION
           SELECT fee_ym, 
                  data_id, 
                  order_seq_no
           FROM iniDrOrd
           WHERE EXISTS
           (
               SELECT 'x'
               FROM SamplingList
               WHERE SamplingNo = @wkSamplingNo
                     AND iniDrOrd.data_id = SamplingList.data_id
           );";

            var result = await _conn.ExecuteAsync(sql, new { wkSamplingNo });
        }

        private async Task<IList<SamplingResultQueryData>> GetSamplingResultQueryDatasAsyncImpl(string wkSamplingNo)
        {
            var sql =
@"SELECT a.fee_ym, 
       a.data_id,
       CASE
           WHEN RTRIM(ISNULL(f.name, '')) = ''
           THEN h.name
           ELSE f.name
       END AS name, 
       c.id, 
       c.func_date, 
       c.hospid, 
       g.hospseqno, 
       g.hospname, 
       b.review, 
       b.reviewdate, 
       b.reviewremark, 
       b.reviewamt, 
       b.appeals, 
       b.appealsdate, 
       b.appealsremark, 
       b.appealsamt, 
       d.order_seq_no, 
       d.order_type, 
       d.order_drug_day, 
       d.order_code, 
       e.orderchiname, 
       d.drug_num, 
       d.drug_fre, 
       d.drug_path, 
       d.order_qty, 
       d.order_uprice, 
       d.order_dot,
       CASE
           WHEN a.chkflg = '1'
           THEN 'V'
           ELSE ' '
       END AS chkflg
FROM SamplingList a
     LEFT JOIN SamplingData b ON a.fee_ym = b.fee_ym
                                 AND a.data_id = b.data_id
     JOIN
(
    SELECT a.fee_ym, 
           a.data_id, 
           a.id, 
           a.func_date, 
           a.hospid, 
           a.birthday
    FROM iniOpDtl a
    UNION
    SELECT a.fee_ym, 
           a.data_id, 
           a.id, 
           a.func_date, 
           a.hospid, 
           a.birthday
    FROM iniDrDtl a
) c ON a.fee_ym = c.fee_ym
       AND a.data_id = c.data_id
     JOIN
(
    SELECT a.fee_ym, 
           a.data_id, 
           a.order_seq_no, 
           a.order_type, 
           a.order_drug_day, 
           a.order_code, 
           a.drug_num, 
           a.drug_fre, 
           a.drug_path, 
           a.order_qty, 
           a.order_uprice, 
           a.order_dot
    FROM iniOpOrd a
    UNION
    SELECT a.fee_ym, 
           a.data_id, 
           a.order_seq_no, 
           a.order_type, 
           a.order_drug_day, 
           a.order_code, 
           a.drug_num, 
           a.drug_fre, 
           a.drug_path, 
           a.order_qty, 
           a.order_uprice, 
           a.order_dot
    FROM iniDrOrd a
) d ON b.fee_ym = d.fee_ym
       AND b.data_id = d.data_id
       AND b.order_seq_no = d.order_seq_no
     LEFT JOIN GenOrderCode e ON d.order_code = e.ordercode
     LEFT JOIN MhbtAgentPatient f ON c.hospid = f.hospid
                                     AND c.id = f.id
                                     AND c.birthday = f.birthday
     LEFT JOIN HospBasic g ON c.hospid = g.hospid
                              AND g.hospseqno = '00'
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
        FROM iniOpDtl
        WHERE name IS NOT NULL
        UNION
        SELECT hospid, 
               id, 
               birthday, 
               name
        FROM iniDrDtl
        WHERE name IS NOT NULL
    ) a
    GROUP BY a.hospid, 
             a.id, 
             a.birthday
) h ON c.hospid = h.hospid
       AND c.id = h.id
       AND c.birthday = h.birthday
WHERE a.samplingno = @wkSamplingNo
ORDER BY d.order_seq_no;";

            var items = await _conn.QueryAsync<SamplingResultQueryData>(sql, new { wkSamplingNo }, commandTimeout: 2000000);
            var list = items.ToList();
            return list;
        }

        public async Task SaveSamplingResultDataAsync(SaveSamplingResultDataRequest request)
        {
            if (request == null)
            {
                throw new ApplicationException("參數錯誤");
            }

            string review = request.review;
            string appeals = request.appeals;
            List<SamplingResultQueryData> items = request.items;

            if (items == null || items.All(x => !x.isChecked))
            {
                throw new ApplicationException("請至少選擇一個項目");
            }

            var checkedItems = items.Where(x => x.isChecked).ToList();

            if (checkedItems.Any(x => (!string.IsNullOrEmpty(review) && !x.reviewamt.HasValue)
                                || (!string.IsNullOrEmpty(appeals) && !x.appealsamt.HasValue)))
            {
                throw new ApplicationException("金額欄位必須為數字，可為零");
            }

            if (checkedItems.Any(x => string.IsNullOrEmpty(review) && (x.reviewamt.HasValue || !string.IsNullOrEmpty(x.reviewremark))))
            {
                throw new ApplicationException("需有審查委員相關資料");
            }

            if (checkedItems.Any(x => string.IsNullOrEmpty(appeals) && (x.appealsamt.HasValue || !string.IsNullOrEmpty(x.appealsremark))))
            {
                throw new ApplicationException("需有申復審查委員相關資料");
            }

            if (checkedItems.Any(x => (x.appealsamt.HasValue && string.IsNullOrEmpty(x.appealsremark))
                                    || (!x.appealsamt.HasValue && !string.IsNullOrEmpty(x.appealsremark))))
            {
                throw new ApplicationException("申復審查欄與申復補付金額皆須輸入");
            }

            if (checkedItems.Any(x => x.appealsamt.HasValue && x.reviewamt.HasValue && x.appealsamt > x.reviewamt))
            {
                throw new ApplicationException("補付金額不可大於追扣金額");
            }

            if (checkedItems.Any(x => x.appealsremark == "不予補付" && x.appealsamt != 0))
            {
                throw new ApplicationException("補付金額必須為0");
            }

            var updatedList = new List<SamplingData>();
            foreach (var item in checkedItems)
            {
                var samplingData
                    = await context
                        .SamplingData
                        .SingleOrDefaultAsync(x =>
                                x.FeeYm == item.fee_ym
                                && x.DataId == item.data_id
                                && x.OrderSeqNo == item.order_seq_no);

                samplingData.Reviewremark = item.reviewremark;
                samplingData.Reviewamt = item.reviewamt;

                if (string.IsNullOrEmpty(item.appealsremark))
                {
                    samplingData.Appealsremark = null;
                    samplingData.Appealsamt = null;
                }
                else
                {
                    samplingData.Appealsremark = item.appealsremark;
                    samplingData.Appealsamt = item.appealsamt;
                }

                updatedList.Add(samplingData);
            }

            await context.SaveChangesAsync();
        }


        /// <summary>
        /// 門診療服務點數及醫令清單
        /// </summary>
        /// <param name="feeYmS"></param>
        /// <param name="feeYmE"></param>
        /// <returns></returns>
        public async Task<RewFileViewModel> ExportRew1300Async(ExportRewRequest request)
        {
            if (request == null)
            {
                throw new ApplicationException("request參數不可為空!");
            }

            var yyyyMM_S = request.feeYmS.ToYYYYMMFromTaiwan();
            var yyyyMM_E = request.feeYmE.ToYYYYMMFromTaiwan();

            if (string.IsNullOrEmpty(yyyyMM_S) || string.IsNullOrEmpty(yyyyMM_E))
            {
                throw new ApplicationException("費用年月不可為空白!");
            }

            var list = await GetRew1300ItemsAsyncImpl(yyyyMM_S, yyyyMM_E, request.wkHospID, request.wkDataID);


            var printDate = DateTime.Today.ToString("yyyy/MM/dd");

            var vm = new RewFileViewModel
            {
                FileName = $"門診療服務點數及醫令清單({request.feeYmS.ToYYYYMMFromTaiwan()}-{request.feeYmE.ToYYYYMMFromTaiwan()}).{request.fileType.ToString()}",
                Stream = new MemoryStream()
            };

            var fi = new FileInfo(Path.Combine(_env.ContentRootPath, "ExcelTemplates/REW1300_Template.xlsx"));

            using (var excel = new ExcelPackage())
            using (var package = new ExcelPackage(fi))
            {
                excel.Encryption.Password = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString();
                excel.Encryption.IsEncrypted = true;
                var workbookTemplate = package.Workbook;
                var worksheetTemplate = workbookTemplate.Worksheets.First();

                var g
                    = list
                        .GroupBy(x => new { x.hospid, x.id, x.birthday, x.func_date })
                        .OrderBy(x => x.Key.hospid).ThenBy(x => x.Key.id).ThenBy(x => x.Key.birthday).ThenBy(x => x.Key.func_date);

                int sheetNumber = 0;
                foreach (var keyValues in g)
                {
                    var hospid = keyValues.Key.hospid;
                    var id = keyValues.Key.id;
                    var birthday = keyValues.Key.birthday;
                    var func_date = keyValues.Key.func_date;

                    var excelParameters = new (string Key, string Value)[]
                    {
                        ("##seq_no##",  keyValues.Select(x=>x.seq_no).FirstOrDefault()?.ToString()),
                        ("##samplingno##",  keyValues.Select(x=>x.samplingno).FirstOrDefault()?.ToString()),
                        ("##Today##",  printDate),
                        ("##hospid##",  keyValues.Select(x=>x.hospid).FirstOrDefault()?.ToString()),
                        ("##hospname##",  keyValues.Select(x=>x.hospname).FirstOrDefault()?.ToString()),
                        ("##fee_ym##",  keyValues.Select(x=>x.fee_ym).FirstOrDefault()?.ToString()),
                        ("##firsttreatdate##",  keyValues.Select(x=>x.firsttreatdate).FirstOrDefault()?.ToString()),
                        ("##appl_type##",  keyValues.Select(x=>x.appl_type).FirstOrDefault()?.ToString()),
                        ("##case_type##",  keyValues.Select(x=>x.case_type).FirstOrDefault()?.ToString()),
                        ("##name##",  keyValues.Select(x=>x.name).FirstOrDefault()?.ToString()),
                        ("##func_date##",  keyValues.Select(x=>x.func_date).FirstOrDefault()?.ToString()),
                        ("##func_type##",  keyValues.Select(x=>x.func_type).FirstOrDefault()?.ToString()),
                        ("##drug_days##",  keyValues.Select(x=>x.drug_days).FirstOrDefault()?.ToString()),
                        ("##birthday##",  keyValues.Select(x=>x.birthday).FirstOrDefault()?.ToString()),
                        ("##id##",  keyValues.Select(x=>x.id).FirstOrDefault()?.ToString()),
                        ("##func_seq_no##",  keyValues.Select(x=>x.func_seq_no).FirstOrDefault()?.ToString()),
                        ("##pay_type##",  keyValues.Select(x=>x.pay_type).FirstOrDefault()?.ToString()),
                        ("##part_code##",  keyValues.Select(x=>x.part_code).FirstOrDefault()?.ToString()),
                        ("##icd9cm_code1##",  keyValues.Select(x=>x.icd9cm_code1).FirstOrDefault()?.ToString()),
                        ("##icd9cm_code2##",  keyValues.Select(x=>x.icd9cm_code2).FirstOrDefault()?.ToString()),
                        ("##area_service##",  keyValues.Select(x=>x.area_service).FirstOrDefault()?.ToString()),
                        ("##rel_mode##",  keyValues.Select(x=>x.rel_mode).FirstOrDefault()?.ToString()),

                        ("##drug_dot##",  keyValues.Select(x=>x.drug_dot).FirstOrDefault()?.ToString()),
                        ("##cure_dot##",  keyValues.Select(x=>x.cure_dot).FirstOrDefault()?.ToString()),
                        ("##prsn_id##",  keyValues.Select(x=>x.prsn_id).FirstOrDefault()?.ToString()),
                        ("##drug_prsn_id##", keyValues.Select(x => x.drug_prsn_id).FirstOrDefault()?.ToString()),
                        ("##cure_dot##",  keyValues.Select(x=>x.cure_dot).FirstOrDefault()?.ToString()),
                        ("##dsvc_dot##",  keyValues.Select(x=>x.dsvc_dot).FirstOrDefault()?.ToString()),
                        ("##other_part_amt##",  keyValues.Select(x=>x.other_part_amt).FirstOrDefault()?.ToString()),
                        ("##order_dot##",  keyValues.Sum(x=>x.order_dot)?.ToString()),
                        ("##part_amt##",  keyValues.Select(x=>x.part_amt).FirstOrDefault()?.ToString()),
                        ("##appl_dot##",  keyValues.Select(x=>x.appl_dot).FirstOrDefault()?.ToString()),
                    };

                    sheetNumber++;
                    var sheetName = $"{id}_{sheetNumber}";
                    var gList = keyValues.OrderBy(x => x.seq_no).ToList();

                    CreateRew1300WorksheetByData(gList, sheetName, excelParameters, excel, worksheetTemplate);
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

        private static void CreateRew1300WorksheetByData(List<Rew1300ItemDto> list, string sheetName, (string Key, string Value)[] excelParameters, ExcelPackage excel, ExcelWorksheet worksheetTemplate)
        {


            var sheet1 = excel.Workbook.Worksheets.Add(sheetName);

            #region Header
            const int headerRowLength = 11;
            const int headerColLength = 12;

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
                        cell.Style.WrapText = true;
                    }
                }
            }

            for (var i = 1; i <= headerColLength; i++)
            {
                sheet1.Column(i).Width = worksheetTemplate.Column(i).Width;
            }

            for (var i = 1; i <= headerRowLength; i++)
            {
                var tarExcelRow = sheet1.Row(i);
                var srcExcelRow = worksheetTemplate.Row(i);
                tarExcelRow.Height = srcExcelRow.Height;
                tarExcelRow.CustomHeight = srcExcelRow.CustomHeight;
            }

            #endregion Header

            #region Details

            int row = headerRowLength + 1;
            foreach (var item in list)
            {
                // detail text
                sheet1.Cells[row, 1].Value = item.order_seq_no;
                sheet1.Cells[row, 2].Value = item.order_type;
                sheet1.Cells[row, 3].Value = item.order_drug_day;
                sheet1.Cells[row, 4].Value = item.order_code;
                sheet1.Cells[row, 5].Value = item.orderchiname;
                sheet1.Cells[row, 6].Value = item.drug_num;
                sheet1.Cells[row, 7].Value = item.drug_fre;
                sheet1.Cells[row, 8].Value = item.drug_path;
                sheet1.Cells[row, 9].Value = item.order_qty;
                sheet1.Cells[row, 10].Value = item.order_uprice;
                sheet1.Cells[row, 11].Value = item.order_dot;

                // detail style
                for (var i = 1; i <= headerColLength; i++)
                {
                    var cell = sheet1.Cells[row, i];
                    var style = cell.Style;
                    style.Font.Size = 10;
                    style.Font.Name = "新細明體";
                    style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    style.Border.BorderAround(ExcelBorderStyle.Thin);
                    style.WrapText = true;
                }

                var excelRow = sheet1.Row(row);
                excelRow.Height = 21;
                excelRow.CustomHeight = false;

                row++;
            }

            #endregion Details

            #region 小計

            const int footerRowStart = 13;
            const int footerRowLength = 8;
            var footerCells = sheet1.Cells[row, 1, row + footerRowLength, headerColLength];
            worksheetTemplate.Cells[footerRowStart, 1, footerRowStart + footerRowLength, headerColLength].Copy(footerCells);

            foreach (var cell in footerCells.AsEnumerable())
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
                        cell.Style.WrapText = true;
                    }
                }
            }

            for (var i = 1; i <= headerColLength; i++)
            {
                sheet1.Column(i).Width = worksheetTemplate.Column(i).Width;
            }

            for (var i = 0; i < footerRowLength; i++)
            {
                var tarExcelRow = sheet1.Row(row + i);
                var srcExcelRow = worksheetTemplate.Row(footerRowStart + i);
                tarExcelRow.Height = srcExcelRow.Height;
                tarExcelRow.CustomHeight = srcExcelRow.CustomHeight;
            }

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
            // 橫向
            tarPrinterSettings.Orientation = eOrientation.Landscape;
            tarPrinterSettings.PaperSize = ePaperSize.A4;
            #endregion 列印
        }


        private async Task<IList<Rew1300ItemDto>> GetRew1300ItemsAsyncImpl(string wkFeeYM_S, string wkFeeYM_E, string wkHospID, string[] wkDataID)
        {
            var sql =
@"SELECT b.seq_no, 
       a.samplingno, 
       b.hospid, 
       c.hospname, 
       CONVERT(VARCHAR, SUBSTRING(b.fee_ym, 1, 4) - 1911) + '年' + SUBSTRING(b.fee_ym, 5, 2) + '月' AS fee_ym, 
       CONVERT(VARCHAR, SUBSTRING(b.firsttreatdate, 1, 4) - 1911) + '年' + SUBSTRING(b.firsttreatdate, 5, 2) + '月' + SUBSTRING(b.firsttreatdate, 7, 2) + '日' AS firsttreatdate, 
       '送核' AS appl_type, 
       b.case_type,
       CASE
           WHEN RTRIM(ISNULL(d.name, '')) = ''
           THEN b.name
           ELSE d.name
       END AS name, 
       CONVERT(VARCHAR, SUBSTRING(b.func_date, 1, 4) - 1911) + '年' + SUBSTRING(b.func_date, 5, 2) + '月' + SUBSTRING(b.func_date, 7, 2) + '日' AS func_date, 
       b.func_type, 
       b.drug_days, 
       CONVERT(VARCHAR, SUBSTRING(b.birthday, 1, 4) - 1911) + '年' + SUBSTRING(b.birthday, 5, 2) + '月' + SUBSTRING(b.birthday, 7, 2) + '日' AS birthday, 
       b.id, 
       b.func_seq_no, 
       b.pay_type, 
       b.part_code, 
       b.icd10cm_code2, 
       b.icd10cm_code3, 
       b.icd10cm_code4, 
       b.area_service, 
       '交付調劑' AS rel_type, 
       b.drug_dot, 
       b.cure_dot, 
       b.dsvc_dot, 
       b.prsn_id, 
       b.drug_prsn_id, 
       b.part_amt, 
       b.appl_dot, 
       b.other_part_amt, 
       e.order_seq_no, 
       e.order_type, 
       e.order_drug_day, 
       e.order_code, 
       f.orderchiname, 
       e.drug_num, 
       e.drug_fre, 
       e.drug_path, 
       e.order_qty, 
       e.order_uprice, 
       e.order_dot, 
       b.icd9cm_code1, 
       b.icd9cm_code2,
       CASE
           WHEN b.rel_mode = '0'
           THEN '■自行調劑 □交付調劑'
           WHEN b.rel_mode = '1'
           THEN '□自行調劑 ■交付調劑'
           ELSE '□自行調劑 □交付調劑'
       END AS rel_mode
FROM samplinglist a
     JOIN
(
    SELECT data_id, 
           fee_ym, 
           hospid, 
           seq_no, 
           firsttreatdate, 
           case_type, 
           func_date, 
           func_type, 
           rel_mode, 
           drug_days, 
           birthday, 
           id, 
           func_seq_no, 
           pay_type, 
           part_code, 
           NULL AS icd10cm_code2, 
           icd10cm_code3, 
           icd10cm_code4, 
           area_service, 
           drug_dot, 
           cure_dot, 
           dsvc_dot, 
           prsn_id, 
           drug_prsn_id, 
           part_amt, 
           appl_dot, 
           NULL AS other_part_amt, 
           icd9cm_code1, 
           icd9cm_code2, 
           name
    FROM iniOpDtl
    WHERE 1 = 1
          AND fee_ym >= @wkFeeYM_S
          AND fee_ym <= @wkFeeYM_E
) b ON a.data_id = b.data_id
       AND a.fee_ym = b.fee_ym
     LEFT JOIN
(
    SELECT *
    FROM HospBasic
    WHERE hospseqno = '00'
) c ON b.hospid = c.hospid
     LEFT JOIN
(
    SELECT *
    FROM MhbtAgentPatient
) d ON b.hospid = d.hospid
       AND b.id = d.id
       AND b.birthday = d.birthday
     LEFT JOIN
(
    SELECT data_id, 
           fee_ym, 
           order_seq_no, 
           order_type, 
           order_drug_day, 
           order_code, 
           drug_num, 
           drug_fre, 
           drug_path, 
           order_qty, 
           order_uprice, 
           order_dot
    FROM iniOpOrd
    WHERE 1 = 1
          AND fee_ym >= @wkFeeYM_S
          AND fee_ym <= @wkFeeYM_E
) e ON b.data_id = e.data_id
       AND b.fee_ym = e.fee_ym
     LEFT JOIN GenOrderCode f ON e.order_code = f.ordercode
WHERE a.fee_ym >= @wkFeeYM_S
      AND a.fee_ym <= @wkFeeYM_E
      AND b.hospid = @wkHospID
      AND a.data_id IN @wkDataID
UNION ALL
SELECT b.seq_no, 
       a.samplingno, 
       b.hospid, 
       c.hospname, 
       CONVERT(VARCHAR, SUBSTRING(b.fee_ym, 1, 4) - 1911) + '年' + SUBSTRING(b.fee_ym, 5, 2) + '月' AS fee_ym, 
       CONVERT(VARCHAR, SUBSTRING(b.firsttreatdate, 1, 4) - 1911) + '年' + SUBSTRING(b.firsttreatdate, 5, 2) + '月' + SUBSTRING(b.firsttreatdate, 7, 2) + '日' AS firsttreatdate, 
       '送核' AS appl_type, 
       b.case_type,
       CASE
           WHEN RTRIM(ISNULL(d.name, '')) = ''
           THEN b.name
           ELSE d.name
       END AS name, 
       CONVERT(VARCHAR, SUBSTRING(b.func_date, 1, 4) - 1911) + '年' + SUBSTRING(b.func_date, 5, 2) + '月' + SUBSTRING(b.func_date, 7, 2) + '日' AS func_date, 
       b.func_type, 
       b.drug_days, 
       CONVERT(VARCHAR, SUBSTRING(b.birthday, 1, 4) - 1911) + '年' + SUBSTRING(b.birthday, 5, 2) + '月' + SUBSTRING(b.birthday, 7, 2) + '日' AS birthday, 
       b.id, 
       b.func_seq_no, 
       b.pay_type, 
       b.part_code, 
       b.icd10cm_code2, 
       b.icd10cm_code3, 
       b.icd10cm_code4, 
       b.area_service, 
       '交付調劑' AS rel_type, 
       b.drug_dot, 
       0 AS cure_dot, 
       b.dsvc_dot, 
       b.prsn_id, 
       b.drug_prsn_id, 
       b.part_amt, 
       b.appl_dot, 
       b.other_part_amt, 
       e.order_seq_no, 
       e.order_type, 
       e.order_drug_day, 
       e.order_code, 
       f.orderchiname, 
       e.drug_num, 
       e.drug_fre, 
       e.drug_path, 
       e.order_qty, 
       e.order_uprice, 
       e.order_dot, 
       b.icd9cm_code1, 
       b.icd9cm_code2,
       CASE
           WHEN b.rel_mode = '0'
           THEN '■自行調劑 □交付調劑'
           WHEN b.rel_mode = '1'
           THEN '□自行調劑 ■交付調劑'
           ELSE '□自行調劑 □交付調劑'
       END AS rel_mode
FROM samplinglist a
     JOIN
(
    SELECT data_id, 
           fee_ym, 
           hospid, 
           seq_no, 
           firsttreatdate, 
           case_type, 
           func_date, 
           func_type, 
           NULL AS rel_mode, 
           drug_days, 
           birthday, 
           id, 
           func_seq_no, 
           pay_type, 
           part_code, 
           icd10cm_code2, 
           icd10cm_code3, 
           icd10cm_code4, 
           area_service, 
           drug_dot, 
           cure_dot, 
           dsvc_dot, 
           prsn_id, 
           drug_prsn_id, 
           part_amt, 
           appl_dot, 
           other_part_amt, 
           icd9cm_code1, 
           icd9cm_code2, 
           name
    FROM iniDrDtl
    WHERE 1 = 1
          AND fee_ym >= @wkFeeYM_S
          AND fee_ym <= @wkFeeYM_E
) b ON a.data_id = b.data_id
       AND a.fee_ym = b.fee_ym
     LEFT JOIN
(
    SELECT *
    FROM HospBasic
    WHERE hospseqno = '00'
) c ON b.hospid = c.hospid
     LEFT JOIN
(
    SELECT *
    FROM MhbtAgentPatient
) d ON b.hospid = d.hospid
       AND b.id = d.id
       AND b.birthday = d.birthday
     LEFT JOIN
(
    SELECT data_id, 
           fee_ym, 
           order_seq_no, 
           order_type, 
           order_drug_day, 
           order_code, 
           drug_num, 
           drug_fre, 
           drug_path, 
           order_qty, 
           order_uprice, 
           order_dot
    FROM iniDrOrd
    WHERE 1 = 1
          AND fee_ym >= @wkFeeYM_S
          AND fee_ym <= @wkFeeYM_E
) e ON b.data_id = e.data_id
       AND b.fee_ym = e.fee_ym
     LEFT JOIN GenOrderCode f ON e.order_code = f.ordercode
WHERE a.fee_ym >= @wkFeeYM_S
      AND a.fee_ym <= @wkFeeYM_E
      AND b.hospid = @wkHospID
      AND a.data_id IN @wkDataID";

            var items = await _conn.QueryAsync<Rew1300ItemDto>(sql, new { wkFeeYM_S, wkFeeYM_E, wkHospID, wkDataID }, commandTimeout: 2000000);
            return items.ToList();
        }


        public async Task<IList<SamplingExamineQueryData>> GetSamplingExamineQueryDatasAsync(SamplingExamineQueryRequest request)
        {
            if (request == null)
            {
                throw new ApplicationException("request參數不可為空!");
            }

            var yyyyMM_S = request.FeeStart.ToYYYYMMFromTaiwan();
            var yyyyMM_E = request.FeeEnd.ToYYYYMMFromTaiwan();

            if (string.IsNullOrEmpty(yyyyMM_S) || string.IsNullOrEmpty(yyyyMM_E))
            {
                throw new ApplicationException("費用年月不可為空白!");
            }

            var list = await GetSamplingExamineQueryDatasAsyncImpl(yyyyMM_S, yyyyMM_E, request.HospID);

            foreach (var item in list)
            {
                item.func_date = item.func_date?.ToSlashTaiwanDateFromYYYYMMDD();
                item.firsttreatdate = item.firsttreatdate?.ToSlashTaiwanDateFromYYYYMMDD();
                item.reviewdate = item.reviewdate?.ToSlashTaiwanDateFromYYYYMMDD();
                item.appealsdate = item.appealsdate?.ToSlashTaiwanDateFromYYYYMMDD();
            }
            return list;
        }

        private async Task<IList<SamplingExamineQueryData>> GetSamplingExamineQueryDatasAsyncImpl(string wkYYYYMM_S, string wkYYYYMM_E, string wkHospID)
        {
            var hospidSql = string.Empty;
            if (!string.IsNullOrEmpty(wkHospID))
            {
                hospidSql = "    WHERE c.hospid = @wkHospID";
            }


            var sql =
$@"SELECT f.fee_ym, 
       f.data_id, 
       f.samplingno, 
       f.hospid, 
       f.hospname, 
       f.name, 
       f.id, 
       f.func_date, 
       f.firsttreatdate, 
       f.seq_no, 
       f.drug_days, 
       f.cure_dot, 
       f.dsvc_dot, 
       f.other_part_amt, 
       f.appl_dot, 
       f.part_amt, 
       f.finish_amt,
       CASE
           WHEN LEN(f.reviewremark) > 0
           THEN SUBSTRING(f.reviewremark, 1, LEN(f.reviewremark) - 1)
       END AS reviewremark, 
       f.reviewamt, 
       f.review, 
       f.reviewdate,
       CASE
           WHEN LEN(f.appealsremark) > 0
           THEN SUBSTRING(f.appealsremark, 1, LEN(f.appealsremark) - 1)
       END AS appealsremark, 
       f.appealsamt, 
       f.appeals, 
       f.appealsdate, 
       f.chkflg
FROM
(
    SELECT a.fee_ym, 
           a.data_id, 
           a.samplingno, 
           c.hospid, 
           d.hospname,
           CASE
               WHEN RTRIM(ISNULL(e.name, '')) = ''
               THEN f.name
               ELSE e.name
           END AS name, 
           c.id, 
           c.func_date, 
           c.firsttreatdate, 
           c.seq_no, 
           c.drug_days, 
           c.cure_dot, 
           c.dsvc_dot, 
           c.other_part_amt, 
           c.appl_dot, 
           c.part_amt, 
           c.finish_amt, 
    (
        SELECT CAST(reviewremark AS VARCHAR) + '、'
        FROM SamplingData
        WHERE fee_ym = a.fee_ym
              AND data_id = a.data_id FOR XML PATH('')
    ) AS reviewremark, 
           SUM(b.reviewamt) AS reviewamt, 
           MAX(b.review) AS review, 
           MAX(b.reviewdate) AS reviewdate, 
    (
        SELECT CAST(appealsremark AS VARCHAR) + '、'
        FROM SamplingData
        WHERE fee_ym = a.fee_ym
              AND data_id = a.data_id FOR XML PATH('')
    ) AS appealsremark, 
           SUM(b.appealsamt) AS appealsamt, 
           MAX(b.appeals) AS appeals, 
           MAX(b.appealsdate) AS appealsdate,
           CASE
               WHEN a.chkflg = '1'
               THEN 'V'
               ELSE ' '
           END AS chkflg
    FROM
    (
        SELECT *
        FROM SamplingList
        WHERE fee_ym >= @wkYYYYMM_S
              AND fee_ym <= @wkYYYYMM_E
    ) a
    JOIN
    (
        SELECT *
        FROM SamplingData
        WHERE fee_ym >= @wkYYYYMM_S
              AND fee_ym <= @wkYYYYMM_E
    ) b ON a.fee_ym = b.fee_ym
           AND a.data_id = b.data_id
    JOIN
    (
        SELECT a.fee_ym, 
               a.data_id, 
               a.hospid, 
               a.id, 
               a.birthday, 
               a.func_date, 
               a.firsttreatdate, 
               a.seq_no, 
               a.drug_days, 
               a.cure_dot, 
               a.dsvc_dot, 
               NULL AS other_part_amt, 
               a.appl_dot, 
               a.part_amt, 
               a.appl_dot - a.part_amt AS finish_amt
        FROM iniOpDtl a
        WHERE a.fee_ym >= @wkYYYYMM_S
              AND a.fee_ym <= @wkYYYYMM_E
        UNION ALL
        SELECT a.fee_ym, 
               a.data_id, 
               a.hospid, 
               a.id, 
               a.birthday, 
               a.func_date, 
               a.firsttreatdate, 
               a.seq_no, 
               a.drug_days, 
               a.cure_dot, 
               a.dsvc_dot, 
               other_part_amt, 
               a.appl_dot, 
               a.part_amt, 
               a.appl_dot - a.part_amt AS finish_amt
        FROM iniDrDtl a
        WHERE a.fee_ym >= @wkYYYYMM_S
              AND a.fee_ym <= @wkYYYYMM_E
    ) c ON a.fee_ym = c.fee_ym
           AND a.data_id = c.data_id
    LEFT JOIN HospBasic d ON c.hospid = d.hospid
                             AND d.hospseqno = '00'
    LEFT JOIN MhbtAgentPatient e ON c.hospid = e.hospid
                                    AND c.id = e.id
                                    AND c.birthday = e.birthday
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
            FROM iniOpDtl
            WHERE name IS NOT NULL
            UNION
            SELECT hospid, 
                   id, 
                   birthday, 
                   name
            FROM iniDrDtl
            WHERE name IS NOT NULL
        ) a
        GROUP BY a.hospid, 
                 a.id, 
                 a.birthday
    ) f ON c.hospid = f.hospid
           AND c.id = f.id
           AND c.birthday = f.birthday
    {hospidSql}
    GROUP BY a.fee_ym, 
             a.data_id, 
             a.samplingno, 
             c.hospid, 
             d.hospname, 
             e.name, 
             f.name, 
             c.id, 
             c.func_date, 
             c.firsttreatdate, 
             c.seq_no, 
             c.drug_days, 
             c.cure_dot, 
             c.dsvc_dot, 
             c.other_part_amt, 
             c.appl_dot, 
             c.part_amt, 
             c.finish_amt, 
             a.chkflg
) f;";

            var items = await _conn.QueryAsync<SamplingExamineQueryData>(sql, new { wkYYYYMM_S, wkYYYYMM_E, wkHospID }, commandTimeout: 2000000);
            return items.ToList();
        }

        public async Task<IList<SamplingExamineQueryDetailData>> GetSamplingExamineQueryDetailDatasAsync(string fee_ym, string data_id)
        {
            var sql =
@"SELECT b.order_seq_no, 
       b.order_type, 
       b.order_drug_day, 
       b.order_code, 
       c.orderchiname, 
       b.drug_num, 
       b.drug_fre, 
       b.drug_path, 
       b.order_qty, 
       b.order_uprice, 
       b.order_dot, 
       a.reviewremark, 
       a.reviewamt, 
       a.review, 
       a.reviewdate, 
       a.appealsremark, 
       a.appealsamt, 
       a.appeals, 
       a.appealsdate
FROM SamplingData a
     JOIN
(
    SELECT a.fee_ym, 
           a.data_id, 
           a.order_seq_no, 
           a.order_type, 
           a.order_drug_day, 
           a.order_code, 
           a.drug_num, 
           a.drug_fre, 
           a.drug_path, 
           a.order_qty, 
           a.order_uprice, 
           a.order_dot
    FROM iniOpOrd a
    UNION
    SELECT a.fee_ym, 
           a.data_id, 
           a.order_seq_no, 
           a.order_type, 
           a.order_drug_day, 
           a.order_code, 
           a.drug_num, 
           a.drug_fre, 
           a.drug_path, 
           a.order_qty, 
           a.order_uprice, 
           a.order_dot
    FROM iniDrOrd a
) b ON a.fee_ym = b.fee_ym
       AND a.data_id = b.data_id
       AND a.order_seq_no = b.order_seq_no
     LEFT JOIN GenOrderCode c ON b.order_code = c.ordercode
WHERE a.fee_ym = @fee_ym
      AND a.data_id = @data_id
ORDER BY b.order_seq_no;";

            var items = await _conn.QueryAsync<SamplingExamineQueryDetailData>(sql, new { fee_ym, data_id }, commandTimeout: 2000000);
            foreach (var item in items)
            {
                item.reviewdate = item.reviewdate?.ToSlashTaiwanDateFromYYYYMMDD();
                item.appealsdate = item.appealsdate?.ToSlashTaiwanDateFromYYYYMMDD();
            }
            return items.ToList();
        }

        /// <summary>
        /// 取得專審/申復收件狀況資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IList<SamplingExamineReceiveData>> GetSamplingExamineReceiveDatasAsync(SamplingExamineReceiveRequest request)
        {
            if (request == null)
            {
                throw new ApplicationException("request參數不可為空!");
            }

            var yyyyMM_S = request.FeeStart.ToYYYYMMFromTaiwan();
            var yyyyMM_E = request.FeeEnd.ToYYYYMMFromTaiwan();

            if (string.IsNullOrEmpty(yyyyMM_S) || string.IsNullOrEmpty(yyyyMM_E))
            {
                throw new ApplicationException("費用年月不可為空白!");
            }

            var list = await GetSamplingExamineReceiveDatasAsyncImpl(yyyyMM_S, yyyyMM_E, request.HospID, request.AccessNo);
            foreach (var item in list)
            {
                item.fee_ym = item.fee_ym?.ToSlashTaiwanDateFromYYYYMM();
                item.birthday = item.birthday?.ToSlashTaiwanDateFromYYYYMMDD();
                item.func_date = item.func_date?.ToSlashTaiwanDateFromYYYYMMDD();
                item.replydate = item.replydate?.ToSlashTaiwanDateFromYYYYMMDD();
            }
            return list;
        }

        private async Task<IList<SamplingExamineReceiveData>> GetSamplingExamineReceiveDatasAsyncImpl(
            string wkYYYYMM_S, string wkYYYYMM_E, string wkHospID, string wkAccessNo)
        {
            var hospidSql = string.Empty;
            if (!string.IsNullOrEmpty(wkHospID))
            {
                hospidSql = "            AND b.hospid = @wkHospID";
            }

            var accessnoSql = string.Empty;
            if (!string.IsNullOrEmpty(wkAccessNo))
            {
                accessnoSql = "            AND a.accessno = @wkAccessNo";
            }

            var sql =
$@"SELECT a.samplingno, 
       b.hospid, 
       c.hospname, 
       b.fee_ym, 
       b.id, 
       b.birthday,
       CASE
           WHEN RTRIM(ISNULL(b.name, '')) = ''
           THEN d.name
           ELSE b.name
       END AS name, 
       b.func_date, 
       a.accessdate, 
       a.accessno, 
       a.replydate, 
       a.replyno, 
       b.data_id,
       CASE
           WHEN a.chkflg = '1'
           THEN 'V'
           ELSE ' '
       END AS chkflg
FROM
(
    SELECT *
    FROM SamplingList
    WHERE fee_ym >= @wkYYYYMM_S
          AND fee_ym <= @wkYYYYMM_E
) a
JOIN
(
    SELECT a.fee_ym, 
           a.data_id, 
           a.id, 
           b.name, 
           a.func_date, 
           a.appl_dot, 
           a.hospid, 
           a.birthday
    FROM iniOpDtl a
         LEFT JOIN MhbtAgentPatient b ON a.hospid = b.hospid
                                         AND a.id = b.id
                                         AND a.birthday = b.birthday
    WHERE a.fee_ym >= @wkYYYYMM_S
          AND a.fee_ym <= @wkYYYYMM_E
    UNION
    SELECT a.fee_ym, 
           a.data_id, 
           a.id, 
           b.name, 
           a.func_date, 
           a.appl_dot, 
           a.hospid, 
           a.birthday
    FROM iniDrDtl a
         LEFT JOIN MhbtAgentPatient b ON a.hospid = b.hospid
                                         AND a.id = b.id
                                         AND a.birthday = b.birthday
    WHERE a.fee_ym >= @wkYYYYMM_S
          AND a.fee_ym <= @wkYYYYMM_E
) b ON a.fee_ym = b.fee_ym
       AND a.data_id = b.data_id
LEFT JOIN
(
    SELECT hospid, 
           hospname
    FROM HospBasic
    WHERE hospseqno = '00'
) c ON b.hospid = c.hospid
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
        FROM iniOpDtl
        WHERE name IS NOT NULL
              AND fee_ym >= @wkYYYYMM_S
              AND fee_ym <= @wkYYYYMM_E
        UNION
        SELECT hospid, 
               id, 
               birthday, 
               name
        FROM iniDrDtl
        WHERE name IS NOT NULL
              AND fee_ym >= @wkYYYYMM_S
              AND fee_ym <= @wkYYYYMM_E
    ) a
    GROUP BY a.hospid, 
             a.id, 
             a.birthday
) d ON b.hospid = d.hospid
       AND b.id = d.id
       AND b.birthday = d.birthday
WHERE 1 = 1
{hospidSql}
{accessnoSql}";

            var items = await _conn.QueryAsync<SamplingExamineReceiveData>(sql, new { wkYYYYMM_S, wkYYYYMM_E, wkHospID, wkAccessNo }, commandTimeout: 2000000);
            return items.ToList();
        }

        public async Task SaveSamplingExamineReceiveDataAsync(SaveSamplingExamineReceiveRequest request)
        {
            if (request == null)
            {
                throw new ApplicationException("參數錯誤");
            }

            if (request.Items == null || request.Items.All(x => !x.IsChecked))
            {
                throw new ApplicationException("請至少選擇一個項目");
            }

            if (!request.IsAccess && !request.IsReply)
            {
                throw new ApplicationException("收件選項請至少勾選一項");
            }

            if ((!string.IsNullOrEmpty(request.AccessNo) && string.IsNullOrEmpty(request.AccessDate))
            || (string.IsNullOrEmpty(request.AccessNo) && !string.IsNullOrEmpty(request.AccessDate)))
            {
                throw new ApplicationException("調閱收件及日期皆須輸入");
            }

            var replyDate = request.ReplyDate?.ToYYYYMMDDFromTaiwan();

            if ((!string.IsNullOrEmpty(request.ReplyNo) && string.IsNullOrEmpty(replyDate))
            || (string.IsNullOrEmpty(request.ReplyNo) && !string.IsNullOrEmpty(replyDate)))
            {
                throw new ApplicationException("申覆審查委員及日期皆須輸入");
            }

            var checkedItems = request.Items.Where(x => x.IsChecked).ToList();

            foreach (var item in checkedItems)
            {
                var feeYYYYMM = item.fee_ym.ToYYYYMMFromTaiwan();
                var samplingList = await context.SamplingList.FirstOrDefaultAsync(x => x.DataId == item.data_id && x.FeeYm == feeYYYYMM);
                if (samplingList == null)
                    continue;

                if (request.IsAccess && !string.IsNullOrEmpty(request.AccessNo))
                {
                    samplingList.Accessno = request.AccessNo?.TruncateString(6);
                    samplingList.Accessdate = request.AccessDate?.TruncateString(7);
                }
                else
                {
                    samplingList.Accessno = null;
                    samplingList.Accessdate = null;
                }

                if (request.IsReply && !string.IsNullOrEmpty(request.ReplyNo))
                {
                    samplingList.Replyno = request.ReplyNo?.TruncateString(6);
                    samplingList.Replydate = replyDate;
                }
                else
                {
                    samplingList.Replyno = null;
                    samplingList.Replydate = null;
                }
            }

            await context.SaveChangesAsync();
        }

        #region 專業審查追扣核定總表

        /// <summary>
        /// 專業審查追扣核定總表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<RewFileViewModel> ExportRew1600Async(ExportRewRequest request)
        {
            if (request == null)
            {
                throw new ApplicationException("request參數不可為空!");
            }

            var yyyyMM_S = request.feeYmS.ToYYYYMMFromTaiwan();
            var yyyyMM_E = request.feeYmE.ToYYYYMMFromTaiwan();

            if (string.IsNullOrEmpty(yyyyMM_S) || string.IsNullOrEmpty(yyyyMM_E))
            {
                throw new ApplicationException("費用年月不可為空白!");
            }

            // follow原SQL只判斷空字串或有正確的醫院ID邏輯做處理.
            if (request.wkHospID == null)
            {
                request.wkHospID = string.Empty;
            }

            var list = await GetRew1600ItemsAsyncImpl(yyyyMM_S, yyyyMM_E, request.wkHospID, request.wkDataID);


            var printDate = DateTime.Today.ToString("yyyy/MM/dd");
            var feeYm_S = request.feeYmS.ToDateFromTaiwan().Value.ToString("yyyy/MM");
            var feeYm_E = request.feeYmE.ToDateFromTaiwan().Value.ToString("yyyy/MM");

            var vm = new RewFileViewModel
            {
                FileName = $"專業審查追扣核定總表({request.feeYmS.ToYYYYMMFromTaiwan()}).{request.fileType.ToString()}",
                Stream = new MemoryStream()
            };

            var fi = new FileInfo(Path.Combine(_env.ContentRootPath, "ExcelTemplates/REW1600_Template.xlsx"));

            using (var excel = new ExcelPackage())
            using (var package = new ExcelPackage(fi))
            {
                excel.Encryption.Password = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString();
                excel.Encryption.IsEncrypted = true;
                var workbookTemplate = package.Workbook;
                var worksheetTemplate = workbookTemplate.Worksheets.First();

                var g = list.GroupBy(x => new { x.hospid, x.hospname });

                int sheetNumber = 0;
                foreach (var keyValues in g)
                {
                    var hospname = keyValues.Key.hospname;
                    var hospid = keyValues.Key.hospid;


                    var excelParameters = new (string Key, string Value)[]
                    {
                        ("##hospname##",  hospname),
                        ("##PrintDate##",  printDate),
                        ("##hospid##",  hospid),
                        ("##FEE_YM_S##",  feeYm_S),
                        ("##FEE_YM_E##",  feeYm_E),
                        ("##SeqNoCount##",  keyValues.Count().ToString()),
                        ("##ReviewAmtSum##",  keyValues.Sum(x=>x.reviewamt)?.ToString())
                    };

                    sheetNumber++;
                    var sheetName = $"{hospid}_{sheetNumber}";
                    var gList = keyValues.OrderBy(x => x.seq_no).ToList();

                    CreateRew1600WorksheetByData(gList, sheetName, excelParameters, excel, worksheetTemplate);
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

        private static void CreateRew1600WorksheetByData(List<Rew1600ItemDto> list, string sheetName, (string Key, string Value)[] excelParameters, ExcelPackage excel, ExcelWorksheet worksheetTemplate)
        {
            excel.Encryption.Password = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString();
            excel.Encryption.IsEncrypted = true;
            var sheet1 = excel.Workbook.Worksheets.Add(sheetName);

            #region Header
            const int headerRowLength = 7;
            const int headerColLength = 10;

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
                        cell.Style.WrapText = true;
                    }
                }
            }

            for (var i = 1; i <= headerColLength; i++)
            {
                sheet1.Column(i).Width = worksheetTemplate.Column(i).Width;
            }

            for (var i = 1; i <= headerRowLength; i++)
            {
                var tarExcelRow = sheet1.Row(i);
                var srcExcelRow = worksheetTemplate.Row(i);
                tarExcelRow.Height = srcExcelRow.Height;
                tarExcelRow.CustomHeight = srcExcelRow.CustomHeight;
            }

            #endregion Header

            #region Details

            int row = headerRowLength + 1;
            foreach (var item in list)
            {
                // detail text
                sheet1.Cells[row, 1].Value = item.appl_date;
                sheet1.Cells[row, 2].Value = item.seq_no;
                sheet1.Cells[row, 3].Value = item.appl_type;
                sheet1.Cells[row, 4].Value = item.id;
                sheet1.Cells[row, 5].Value = item.name;
                sheet1.Cells[row, 6].Value = item.birthday;
                sheet1.Cells[row, 7].Value = item.func_date;
                sheet1.Cells[row, 8].Value = item.reviewamt;
                sheet1.Cells[row, 9].Value = item.reviewremark;
                sheet1.Cells[row, 10].Value = "□";

                // detail style
                for (var i = 1; i <= headerColLength; i++)
                {
                    var cell = sheet1.Cells[row, i];
                    var style = cell.Style;
                    style.Font.Size = 10;
                    style.Font.Name = "新細明體";
                    style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    style.WrapText = true;
                }

                var excelRow = sheet1.Row(row);
                excelRow.Height = 21;
                excelRow.CustomHeight = false;

                row++;
            }

            #endregion Details

            #region 小計

            const int footerRowStart = 9;
            const int footerRowLength = 32;
            var footerCells = sheet1.Cells[row, 1, row + footerRowLength, headerColLength];
            worksheetTemplate.Cells[footerRowStart, 1, footerRowStart + footerRowLength, headerColLength].Copy(footerCells);

            foreach (var cell in footerCells.AsEnumerable())
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
                        cell.Style.WrapText = true;
                    }
                }
            }

            for (var i = 1; i <= headerColLength; i++)
            {
                sheet1.Column(i).Width = worksheetTemplate.Column(i).Width;
            }

            for (var i = 0; i <= footerRowLength; i++)
            {
                var tarExcelRow = sheet1.Row(row + i);
                var srcExcelRow = worksheetTemplate.Row(footerRowStart + i);
                tarExcelRow.Height = srcExcelRow.Height;
                tarExcelRow.CustomHeight = srcExcelRow.CustomHeight;
            }

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


        private async Task<IList<Rew1600ItemDto>> GetRew1600ItemsAsyncImpl(string wkFeeYM_S, string wkFeeYM_E, string wkHospID, string[] wkDataID)
        {
            var sql =
@"SELECT f.fee_ym, 
       f.hospid, 
       f.hospname, 
       f.appl_type, 
       f.appl_date, 
       f.seq_no, 
       f.id, 
       f.name, 
       f.birthday, 
       f.reviewamt AS reviewamt, 
       f.func_date, 
       f.remark,
       CASE
           WHEN LEN(f.reviewremark) > 0
           THEN SUBSTRING(f.reviewremark, 1, LEN(f.reviewremark) - 1)
       END AS reviewremark
FROM
(
    SELECT CONVERT(VARCHAR, SUBSTRING(a.fee_ym, 1, 4) - 1911) + '/' + SUBSTRING(a.fee_ym, 5, 2) AS fee_ym, 
           c.hospid, 
           e.hospname,
           CASE
               WHEN c.appl_type = '1'
               THEN '送核'
               ELSE '補報'
           END AS appl_type, 
           CONVERT(VARCHAR, SUBSTRING(c.appl_date, 1, 4) - 1911) + '/' + SUBSTRING(c.appl_date, 5, 2) + '/' + SUBSTRING(c.appl_date, 7, 2) AS appl_date, 
           c.seq_no, 
           SUBSTRING(c.id, 1, 4) + '***' + SUBSTRING(c.id, 8, 3) AS id, 
           SUBSTRING((CASE
                          WHEN RTRIM(ISNULL(d.name, '')) = ''
                          THEN c.name
                          ELSE d.name
                      END), 1, 1) + 'O' + SUBSTRING((CASE
                                                         WHEN RTRIM(ISNULL(d.name, '')) = ''
                                                         THEN c.name
                                                         ELSE d.name
                                                     END), 3, 1) AS name, 
           CONVERT(VARCHAR, SUBSTRING(c.birthday, 1, 4) - 1911) + '/' + SUBSTRING(c.birthday, 5, 2) + '/' + SUBSTRING(c.birthday, 7, 2) AS birthday, 
           SUM(b.reviewamt) AS reviewamt, 
           CONVERT(VARCHAR, SUBSTRING(c.func_date, 1, 4) - 1911) + '/' + SUBSTRING(c.func_date, 5, 2) + '/' + SUBSTRING(c.func_date, 7, 2) AS func_date, 
           '' AS remark, 
    (
        SELECT CAST(reviewremark AS VARCHAR) + '、'
        FROM SamplingData
        WHERE fee_ym = a.fee_ym
              AND data_id = a.data_id FOR XML PATH('')
    ) AS reviewremark
    FROM SamplingList a
         JOIN SamplingData b ON a.fee_ym = b.fee_ym
                                AND a.data_id = b.data_id
         JOIN
    (
        SELECT a.fee_ym, 
               a.data_id, 
               a.hospid, 
               a.seq_no, 
               a.appl_type, 
               a.id, 
               a.birthday, 
               a.func_date, 
               a.appl_date, 
               name
        FROM iniOpDtl a
        WHERE a.fee_ym + a.data_id NOT IN
        (
            SELECT fee_ym + data_id
            FROM updOpDtl
        )
              AND a.fee_ym >= @wkFeeYM_S
              AND a.fee_ym <= @wkFeeYM_E
        UNION
        SELECT a.fee_ym, 
               a.data_id, 
               a.hospid, 
               a.seq_no, 
               a.appl_type, 
               a.id, 
               a.birthday, 
               a.func_date, 
               a.appl_date, 
               name
        FROM updOpDtl a
        WHERE a.fee_ym >= @wkFeeYM_S
              AND a.fee_ym <= @wkFeeYM_E
        UNION ALL
        SELECT a.fee_ym, 
               a.data_id, 
               a.hospid, 
               a.seq_no, 
               a.appl_type, 
               a.id, 
               a.birthday, 
               a.func_date, 
               a.appl_date, 
               name
        FROM iniDrDtl a
        WHERE a.fee_ym + a.data_id NOT IN
        (
            SELECT fee_ym + data_id
            FROM updDrDtl
        )
              AND a.fee_ym >= @wkFeeYM_S
              AND a.fee_ym <= @wkFeeYM_E
        UNION
        SELECT a.fee_ym, 
               a.data_id, 
               a.hospid, 
               a.seq_no, 
               a.appl_type, 
               a.id, 
               a.birthday, 
               a.func_date, 
               a.appl_date, 
               name
        FROM updDrDtl a
        WHERE a.fee_ym >= @wkFeeYM_S
              AND a.fee_ym <= @wkFeeYM_E
    ) c ON a.fee_ym = c.fee_ym
           AND a.data_id = c.data_id
         LEFT JOIN MhbtAgentPatient d ON c.hospid = d.hospid
                                         AND c.id = d.id
                                         AND c.birthday = d.birthday
         LEFT JOIN HospBasic e ON c.hospid = e.hospid
                                  AND e.hospseqno = '00'
    WHERE c.hospid = (CASE
                          WHEN @wkHospID = ''
                          THEN c.hospid
                          ELSE @wkHospID
                      END)
          AND a.data_id IN @wkDataID
         AND b.reviewamt <> 0
    GROUP BY a.fee_ym, 
             a.data_id, 
             c.hospid, 
             e.hospname, 
             c.appl_type, 
             c.seq_no, 
             c.appl_type, 
             c.id, 
             d.name, 
             c.name, 
             c.birthday, 
             c.func_date, 
             c.appl_date
) f
ORDER BY f.hospid, 
         f.fee_ym, 
         f.id, 
         f.func_date;";

            var items = await _conn.QueryAsync<Rew1600ItemDto>(sql, new { wkFeeYM_S, wkFeeYM_E, wkHospID, wkDataID }, commandTimeout: 2000000);
            return items.ToList();
        }

        #endregion


        #region 專業審查追扣核定總表

        /// <summary>
        /// 專業審查追扣核定總表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<RewFileViewModel> ExportRew1800Async(ExportRewRequest request)
        {
            if (request == null)
            {
                throw new ApplicationException("request參數不可為空!");
            }

            var yyyyMM_S = request.feeYmS.ToYYYYMMFromTaiwan();
            var yyyyMM_E = request.feeYmE.ToYYYYMMFromTaiwan();

            if (string.IsNullOrEmpty(yyyyMM_S) || string.IsNullOrEmpty(yyyyMM_E))
            {
                throw new ApplicationException("費用年月不可為空白!");
            }

            // follow原SQL只判斷空字串或有正確的醫院ID邏輯做處理.
            if (request.wkHospID == null)
            {
                request.wkHospID = string.Empty;
            }

            var list = await GetRew1800ItemsAsyncImpl(yyyyMM_S, yyyyMM_E, request.wkHospID, request.wkDataID);


            var printDate = DateTime.Today.ToString("yyyy/MM/dd");
            var feeYm_S = request.feeYmS.ToDateFromTaiwan().Value.ToString("yyyy/MM");
            var feeYm_E = request.feeYmE.ToDateFromTaiwan().Value.ToString("yyyy/MM");

            var vm = new RewFileViewModel
            {
                FileName = $"專業審查追扣補付核定總表(申復)({request.feeYmS.ToYYYYMMFromTaiwan()}).{request.fileType.ToString()}",
                Stream = new MemoryStream()
            };

            var fi = new FileInfo(Path.Combine(_env.ContentRootPath, "ExcelTemplates/REW1800_Template.xlsx"));

            using (var excel = new ExcelPackage())
            using (var package = new ExcelPackage(fi))
            {
                excel.Encryption.Password = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString();
                excel.Encryption.IsEncrypted = true;
                var workbookTemplate = package.Workbook;
                var worksheetTemplate = workbookTemplate.Worksheets.First();

                var g = list.GroupBy(x => new { x.hospid, x.hospname });

                int sheetNumber = 0;
                foreach (var keyValues in g)
                {
                    var hospname = keyValues.Key.hospname;
                    var hospid = keyValues.Key.hospid;


                    var excelParameters = new (string Key, string Value)[]
                    {
                        ("##hospname##",  hospname),
                        ("##PrintDate##",  printDate),
                        ("##hospid##",  hospid),
                        ("##FEE_YM_S##",  feeYm_S),
                        ("##FEE_YM_E##",  feeYm_E),
                        ("##SeqNoCount##",  keyValues.Count().ToString()),
                        ("##ReviewAmtSum##",  keyValues.Sum(x=>(x.reviewamt * -1) + x.appealsamt ) ?.ToString())
                    };

                    sheetNumber++;
                    var sheetName = $"{hospid}_{sheetNumber}";
                    var gList = keyValues.OrderBy(x => x.seq_no).ToList();

                    CreateRew1800WorksheetByData(gList, sheetName, excelParameters, excel, worksheetTemplate);
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

        private static void CreateRew1800WorksheetByData(List<Rew1800ItemDto> list, string sheetName, (string Key, string Value)[] excelParameters, ExcelPackage excel, ExcelWorksheet worksheetTemplate)
        {
            excel.Encryption.Password = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString();
            excel.Encryption.IsEncrypted = true;
            var sheet1 = excel.Workbook.Worksheets.Add(sheetName);

            #region Header
            const int headerRowLength = 7;
            const int headerColLength = 9;

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
                        cell.Style.WrapText = true;
                    }
                }
            }

            for (var i = 1; i <= headerColLength; i++)
            {
                sheet1.Column(i).Width = worksheetTemplate.Column(i).Width;
            }

            for (var i = 1; i <= headerRowLength; i++)
            {
                var tarExcelRow = sheet1.Row(i);
                var srcExcelRow = worksheetTemplate.Row(i);
                tarExcelRow.Height = srcExcelRow.Height;
                tarExcelRow.CustomHeight = srcExcelRow.CustomHeight;
            }

            #endregion Header

            #region Details

            int row = headerRowLength + 1;
            foreach (var item in list)
            {
                // detail text
                sheet1.Cells[row, 1].Value = item.appl_date;
                sheet1.Cells[row, 2].Value = item.seq_no;
                sheet1.Cells[row, 3].Value = item.appl_type;
                sheet1.Cells[row, 4].Value = item.id;
                sheet1.Cells[row, 5].Value = item.name;
                sheet1.Cells[row, 6].Value = item.birthday;
                sheet1.Cells[row, 7].Value = item.reviewamt;
                sheet1.Cells[row, 8].Value = item.appealsamt;
                sheet1.Cells[row, 9].Value = item.reviewremark;

                // detail style
                for (var i = 1; i <= headerColLength; i++)
                {
                    var cell = sheet1.Cells[row, i];
                    var style = cell.Style;
                    style.Font.Size = 10;
                    style.Font.Name = "新細明體";
                    style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    style.WrapText = true;
                }

                var excelRow = sheet1.Row(row);
                excelRow.Height = 21;
                excelRow.CustomHeight = false;

                row++;
            }

            #endregion Details

            #region 小計

            const int footerRowStart = 9;
            const int footerRowLength = 29;
            var footerCells = sheet1.Cells[row, 1, row + footerRowLength, headerColLength];
            worksheetTemplate.Cells[footerRowStart, 1, footerRowStart + footerRowLength, headerColLength].Copy(footerCells);

            foreach (var cell in footerCells.AsEnumerable())
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
                        cell.Style.WrapText = true;
                    }
                }
            }

            for (var i = 1; i <= headerColLength; i++)
            {
                sheet1.Column(i).Width = worksheetTemplate.Column(i).Width;
            }

            for (var i = 0; i <= footerRowLength; i++)
            {
                var tarExcelRow = sheet1.Row(row + i);
                var srcExcelRow = worksheetTemplate.Row(footerRowStart + i);
                tarExcelRow.Height = srcExcelRow.Height;
                tarExcelRow.CustomHeight = srcExcelRow.CustomHeight;
            }

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


        private async Task<IList<Rew1800ItemDto>> GetRew1800ItemsAsyncImpl(string wkFeeYM_S, string wkFeeYM_E, string wkHospID, string[] wkDataID)
        {
            var sql =
@"SELECT f.fee_ym, 
       f.hospid, 
       f.hospname, 
       f.appl_type, 
       f.appl_date, 
       f.seq_no, 
       f.id, 
       f.name, 
       f.birthday, 
       f.reviewamt AS reviewamt, 
       f.appealsamt, 
       f.func_date, 
       f.remark,
       CASE
           WHEN LEN(f.reviewremark) > 0
           THEN SUBSTRING(f.reviewremark, 1, LEN(f.reviewremark) - 1)
       END AS reviewremark
FROM
(
    SELECT CONVERT(VARCHAR, SUBSTRING(a.fee_ym, 1, 4) - 1911) + '/' + SUBSTRING(a.fee_ym, 5, 2) AS fee_ym, 
           c.hospid, 
           e.hospname,
           CASE
               WHEN c.appl_type = '1'
               THEN '送核'
               ELSE '補報'
           END AS appl_type, 
           CONVERT(VARCHAR, SUBSTRING(c.appl_date, 1, 4) - 1911) + '/' + SUBSTRING(c.appl_date, 5, 2) + '/' + SUBSTRING(c.appl_date, 7, 2) AS appl_date, 
           c.seq_no, 
           SUBSTRING(c.id, 1, 4) + '***' + SUBSTRING(c.id, 8, 3) AS id, 
           SUBSTRING((CASE
                          WHEN RTRIM(ISNULL(d.name, '')) = ''
                          THEN c.name
                          ELSE d.name
                      END), 1, 1) + 'O' + SUBSTRING((CASE
                                                         WHEN RTRIM(ISNULL(d.name, '')) = ''
                                                         THEN c.name
                                                         ELSE d.name
                                                     END), 3, 1) AS name, 
           CONVERT(VARCHAR, SUBSTRING(c.birthday, 1, 4) - 1911) + '/' + SUBSTRING(c.birthday, 5, 2) + '/' + SUBSTRING(c.birthday, 7, 2) AS birthday, 
           SUM(b.reviewamt) AS reviewamt, 
           SUM(b.appealsamt) AS appealsamt, 
           c.func_date, 
           '' AS remark, 
    (
        SELECT CAST(appealsremark AS VARCHAR) + '、'
        FROM SamplingData
        WHERE fee_ym = a.fee_ym
              AND data_id = a.data_id FOR XML PATH('')
    ) AS reviewremark
    FROM SamplingList a
         JOIN SamplingData b ON a.fee_ym = b.fee_ym
                                AND a.data_id = b.data_id
         JOIN
    (
        SELECT a.fee_ym, 
               a.data_id, 
               a.hospid, 
               a.seq_no, 
               a.appl_type, 
               a.id, 
               a.birthday, 
               a.func_date, 
               a.appl_date, 
               name
        FROM iniOpDtl a
        WHERE a.fee_ym + a.data_id NOT IN
        (
            SELECT fee_ym + data_id
            FROM updOpDtl
        )
              AND a.fee_ym >= @wkFeeYM_S
              AND a.fee_ym <= @wkFeeYM_E
        UNION
        SELECT a.fee_ym, 
               a.data_id, 
               a.hospid, 
               a.seq_no, 
               a.appl_type, 
               a.id, 
               a.birthday, 
               a.func_date, 
               a.appl_date, 
               name
        FROM updOpDtl a
        WHERE a.fee_ym >= @wkFeeYM_S
              AND a.fee_ym <= @wkFeeYM_E
        UNION ALL
        SELECT a.fee_ym, 
               a.data_id, 
               a.hospid, 
               a.seq_no, 
               a.appl_type, 
               a.id, 
               a.birthday, 
               a.func_date, 
               a.appl_date, 
               name
        FROM iniDrDtl a
        WHERE a.fee_ym + a.data_id NOT IN
        (
            SELECT fee_ym + data_id
            FROM updDrDtl
        )
              AND a.fee_ym >= @wkFeeYM_S
              AND a.fee_ym <= @wkFeeYM_E
        UNION
        SELECT a.fee_ym, 
               a.data_id, 
               a.hospid, 
               a.seq_no, 
               a.appl_type, 
               a.id, 
               a.birthday, 
               a.func_date, 
               a.appl_date, 
               name
        FROM updDrDtl a
        WHERE a.fee_ym >= @wkFeeYM_S
              AND a.fee_ym <= @wkFeeYM_E
    ) c ON a.fee_ym = c.fee_ym
           AND a.data_id = c.data_id
         LEFT JOIN MhbtAgentPatient d ON c.hospid = d.hospid
                                         AND c.id = d.id
                                         AND c.birthday = d.birthday
         LEFT JOIN HospBasic e ON c.hospid = e.hospid
                                  AND e.hospseqno = '00'
    WHERE c.hospid = (CASE
                          WHEN @wkHospID = ''
                          THEN c.hospid
                          ELSE @wkHospID
                      END)
          AND a.data_id IN @wkDataID
         AND b.appealsamt >= 0
    GROUP BY a.fee_ym, 
             a.data_id, 
             c.hospid, 
             e.hospname, 
             c.appl_type, 
             c.seq_no, 
             c.appl_type, 
             c.id, 
             d.name, 
             c.name, 
             c.birthday, 
             c.func_date, 
             c.appl_date
) f
ORDER BY f.hospid, 
         f.fee_ym, 
         f.id, 
         f.func_date;";

            var items = await _conn.QueryAsync<Rew1800ItemDto>(sql, new { wkFeeYM_S, wkFeeYM_E, wkHospID, wkDataID }, commandTimeout: 2000000);
            return items.ToList();
        }

        #endregion
    }
}
