using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SMK.Data;
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
    public class DataQueryService : GenericService
    {
        private readonly IDbConnection _conn = null;
        private readonly IWebHostEnvironment _env;
        public DataQueryService(SMKWEBContext context, SessionManager smgr, IWebHostEnvironment env)
        : base(context, smgr)
        {
            _conn = context.Database.GetDbConnection();
            _env = env;
        }
        public async Task<RewFileViewModel> ExportMedicineReportAsync(ExportMedicineReportRequest request)
        {
            if (request == null)
            {
                throw new ApplicationException("request參數不可為空!");
            }

            var yyyyMM_S = request.feeYmS;
            var yyyyMM_E = request.feeYmE;

            if (string.IsNullOrEmpty(yyyyMM_S) || string.IsNullOrEmpty(yyyyMM_E))
            {
                throw new ApplicationException("費用年月不可為空白!");
            }

            if (string.IsNullOrEmpty(request.wkHospID))
            {
                throw new ApplicationException("醫事機構代碼不可為空白!");
            }

            var CheckHospID = request.wkHospID.Substring(0, 2);
            var list = new MedicineReportViewModel();
            if (CheckHospID != "59")
            {
                list = new MedicineReportViewModel()
                {
                    IniOpDtl = this.context.IniOpDtl.Where(x => x.DataId == request.data_id && x.FeeYm == yyyyMM_S).FirstOrDefault(),
                    //IniOpOrds = this.context.IniOpOrd.Where(x => x.DataId == request.data_id && x.FeeYm == yyyyMM_S).ToList(),
                    IniOpOrds = (
                                from iniOpOrd in this.context.IniOpOrd
                                join genOrderCode in this.context.GenOrderCode on iniOpOrd.OrderCode equals genOrderCode.OrderCode
                                join ordOfB7 in this.context.OrdOfB7
                                on new { key1 = iniOpOrd.DataId, key2 = iniOpOrd.FeeYm, key3 = iniOpOrd.OrderSeqNo } equals new { key1 = ordOfB7.Data_ID, key2 = ordOfB7.Fee_YM, key3 = ordOfB7.Order_Seq_No }
                                where iniOpOrd.DataId == request.data_id && iniOpOrd.FeeYm == yyyyMM_S
                                select new IniOpOrdView
                                {
                                    DataId = iniOpOrd.DataId,
                                    OrderSeqNo = iniOpOrd.OrderSeqNo,
                                    FeeYm = iniOpOrd.FeeYm,
                                    OrderType = iniOpOrd.OrderType,
                                    OrderCode = iniOpOrd.OrderCode,
                                    RelMode = iniOpOrd.RelMode,
                                    ChrMark = iniOpOrd.ChrMark,
                                    DrugNum = iniOpOrd.DrugNum,
                                    DrugFre = iniOpOrd.DrugFre,
                                    DrugPath = iniOpOrd.DrugPath,
                                    OrderUprice = iniOpOrd.OrderUprice,
                                    OrderQty = iniOpOrd.OrderQty,
                                    OrderDot = iniOpOrd.OrderDot,
                                    ExePrsnId = iniOpOrd.ExePrsnId,
                                    CurePath = iniOpOrd.CurePath,
                                    OrderDrugDay = iniOpOrd.OrderDrugDay,
                                    TranDate = iniOpOrd.TranDate,
                                    OrderChiName = genOrderCode.OrderChiName,
                                    Exe_Prsn_ID = ordOfB7.Exe_Prsn_ID,
                                }
                                ).ToList(),
                    CheckOpOrDr = "醫療院所"
                };
            }
            else
            {
                list = new MedicineReportViewModel()
                {
                    IniDrDtl = this.context.IniDrDtl.Where(x => x.DataId == request.data_id && x.FeeYm == yyyyMM_S).FirstOrDefault(),
                    //IniDrOrds = this.context.IniDrOrd.Where(x => x.DataId == request.data_id && x.FeeYm == yyyyMM_S).ToList(),
                    IniDrOrds = (
                                from iniDrOrd in this.context.IniDrOrd
                                join genOrderCode in this.context.GenOrderCode on iniDrOrd.OrderCode equals genOrderCode.OrderCode
                                join ordOfB7 in this.context.OrdOfB7
                                on new { key1 = iniDrOrd.DataId, key2 = iniDrOrd.FeeYm, key3 = iniDrOrd.OrderSeqNo } equals new { key1 = ordOfB7.Data_ID, key2 = ordOfB7.Fee_YM, key3 = ordOfB7.Order_Seq_No }
                                where iniDrOrd.DataId == request.data_id && iniDrOrd.FeeYm == yyyyMM_S
                                select new IniDrOrdView
                                {
                                    DataId = iniDrOrd.DataId,
                                    OrderSeqNo = iniDrOrd.OrderSeqNo,
                                    FeeYm = iniDrOrd.FeeYm,
                                    OrderType = iniDrOrd.OrderType,
                                    OrderCode = iniDrOrd.OrderCode,
                                    DrugNum = iniDrOrd.DrugNum,
                                    DrugFre = iniDrOrd.DrugFre,
                                    DrugPath = iniDrOrd.DrugPath,
                                    OrderUprice = iniDrOrd.OrderUprice,
                                    OrderQty = iniDrOrd.OrderQty,
                                    OrderDot = iniDrOrd.OrderDot,
                                    ExePrsnId = iniDrOrd.ExePrsnId,
                                    OrderDrugDay = iniDrOrd.OrderDrugDay,
                                    TranDate = iniDrOrd.TranDate,
                                    OrderChiName = genOrderCode.OrderChiName,
                                    Exe_Prsn_ID = ordOfB7.Exe_Prsn_ID,
                                    RelMode = "",
                                }
                                ).ToList(),
                    CheckOpOrDr = "藥局"
                };
            }

            var printDate = DateTime.Today.ToString("yyyy/MM/dd");

            var vm = new RewFileViewModel
            {
                FileName = $"Medical_Orders_and_Points({request.data_id}-{request.feeYmE}).xlsx",
                Stream = new MemoryStream()
            };

            var fi = new FileInfo(Path.Combine(_env.ContentRootPath, "ExcelTemplates/Medical_Orders_and_Points.xlsx"));

            using (var excel = new ExcelPackage())
            using (var package = new ExcelPackage(fi))
            {
                excel.Encryption.Password = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString();
                excel.Encryption.IsEncrypted = true;
                var workbookTemplate = package.Workbook;
                var worksheetTemplate = workbookTemplate.Worksheets.First();

                //var g
                //    = list
                //        .GroupBy(x => new { x.hospid, x.id, x.birthday, x.func_date })
                //        .OrderBy(x => x.Key.hospid).ThenBy(x => x.Key.id).ThenBy(x => x.Key.birthday).ThenBy(x => x.Key.func_date);

                //int sheetNumber = 0;
                //foreach (var keyValues in g)
                //{
                //    var hospid = keyValues.Key.hospid;
                //    var id = keyValues.Key.id;
                //    var birthday = keyValues.Key.birthday;
                //    var func_date = keyValues.Key.func_date;
                var excelParameters = new (string Key, string Value)[] { };
                var sheetName = String.Empty;
                if (CheckHospID != "59")
                {
                    excelParameters = new (string Key, string Value)[]
                    {
                        ("##seq_no##",  list.IniOpDtl.SeqNo.ToString()),
                        ("##No##",  list.IniOpDtl.DataId+"_"+list.IniOpDtl.FeeYm),
                        ("##Today##",  printDate),
                        ("##hosp_data_type##",  list.IniOpDtl.HospDataType.ToString()),
                        ("##data_id##",  list.IniOpDtl.DataId.ToString()),
                        ("##hospid##",  list.IniOpDtl.HospId.ToString()),
                        ("##HospSeqNo##",  list.IniOpDtl.HospSeqNo.ToString()),
                        ("##hospname##",  context.HospBasic.Where(x => x.HospId == list.IniOpDtl.HospId && x.HospSeqNo == list.IniOpDtl.HospSeqNo).FirstOrDefault().HospName.ToString()),
                        ("##fee_ym##",  list.IniOpDtl.FeeYm.ToString()),
                        ("##exemyear##",  list.IniOpDtl.ExamYear.ToString()),
                        ("##examtime##",  list.IniOpDtl.ExamTime == null ? "" : list.IniOpDtl.ExamTime.ToString()),
                        ("##weekcount##",  list.IniOpDtl.WeekCount == null ? "" :list.IniOpDtl.WeekCount.ToString()),
                        ("##instructexamyear##",  list.IniOpDtl.InstructExamYear.ToString()),
                        ("##instructexamtime##",  list.IniOpDtl.InstructExamTime == null ? "" : list.IniOpDtl.InstructExamTime.ToString()),
                        ("##firsttreatdate##",  list.IniOpDtl.FirstTreatDate.ToString()),
                        ("##appl_type##",  list.IniOpDtl.ApplType.ToString()),
                        ("##appl_date##",  list.IniOpDtl.ApplDate.ToString()),
                        ("##case_type##",  list.IniOpDtl.CaseType.ToString()),
                        ("##name##",  list.IniOpDtl.Name.ToString()),
                        ("##id_sex##",  list.IniOpDtl.IdSex.ToString()),
                        ("##func_date##",  list.IniOpDtl.FuncDate.ToString()),
                        ("##func_type##",  list.IniOpDtl.FuncType.ToString()),
                        ("##rel_date##",  ""),
                        ("##real_hosp_id##",  list.IniOpDtl.RealHospId.ToString()),
                        ("##drug_days##",  list.IniOpDtl.DrugDays.ToString()),
                        ("##birthday##",  list.IniOpDtl.Birthday.ToString()),
                        ("##id##",  list.IniOpDtl.Id.ToString()),
                        ("##func_seq_no##",  list.IniOpDtl.FuncSeqNo.ToString()),
                        ("##pay_type##",  list.IniOpDtl.PayType.ToString()),
                        ("##part_code##",  list.IniOpDtl.PartCode.ToString()),
                        ("##orig_hosp_id##",  ""),//醫療院所沒有
                        ("##icd9cm_code##",  list.IniOpDtl.Icd9cmCode.ToString()),
                        ("##icd9cm_code1##",  list.IniOpDtl.Icd9cmCode1.ToString()),
                        ("##icd9cm_code2##",  list.IniOpDtl.Icd9cmCode2.ToString()),
                        ("##icd10cm_code2##",  ""),//醫療院所沒有
                        ("##icd10cm_code3##",  list.IniOpDtl.Icd10cmCode3.ToString()),
                        ("##icd10cm_code4##",  list.IniOpDtl.Icd10cmCode4.ToString()),
                        ("##cure_item1##",  list.IniOpDtl.CureItem1.ToString()),
                        ("##cure_item2##",  list.IniOpDtl.CureItem2.ToString()),
                        ("##cure_item3##",  list.IniOpDtl.CureItem3.ToString()),
                        ("##cure_item4##",  list.IniOpDtl.CureItem4.ToString()),
                        ("##corr_hosp_id##",  list.IniOpDtl.CorrHospId.ToString()),
                        ("##area_service##",  list.IniOpDtl.AreaService.ToString()),
                        ("##rel_mode##",  list.IniOpDtl.RelMode.ToString()),
                        ("##medapply##",  list.IniOpDtl.MedApply.ToString()),
                        ("##instructapply##",  list.IniOpDtl.InstructApply.ToString()),
                        ("##traceapply##",  list.IniOpDtl.TraceApply.ToString()),
                        ("##releaseapply##",  list.IniOpDtl.ReleaseApply == null ? "" : list.IniOpDtl.ReleaseApply.ToString()),

                        ("##drug_dot##",  list.IniOpDtl.DrugDot.ToString()),
                        ("##prsn_id##",  list.IniOpDtl.PrsnId.ToString()),
                        ("##drug_prsn_id##", list.IniOpDtl.DrugPrsnId.ToString()),
                        ("##cure_dot##",  list.IniOpDtl.CureDot.ToString()),
                        ("##dsvc_dot##",  list.IniOpDtl.DsvcDot.ToString()),
                        ("##other_part_amt##",  ""),//醫療院所沒有
                        ("##order_dot##",  list.IniOpOrds.Sum(x=> x.OrderDot)?.ToString()),
                        ("##part_amt##",  list.IniOpDtl.PartAmt.ToString()),
                        ("##appl_dot##",  list.IniOpDtl.ApplDot.ToString()),
                        ("##diag_code##",  list.IniOpDtl.DiagCode.ToString()),
                        ("##diag_dot##",  list.IniOpDtl.DiagDot.ToString()),
                        ("##agency_part_amt##",  list.IniOpDtl.AgencyPartAmt.ToString()),
                        ("##dsvc_code##",  list.IniOpDtl.DsvcCode.ToString()),
                        ("##firstinstrucdate##",  list.IniOpDtl.FirstInstructDate.ToString()),
                        ("##inctructserial##",  list.IniOpDtl.InctructSerial == null ? "" : list.IniOpDtl.InctructSerial.ToString()),
                        ("##orig_case_type##",  ""),//醫療院所沒有
                        ("##supp_area##",  list.IniOpDtl.SuppArea.ToString()),
                        ("##exp_dot##",  list.IniOpDtl.ExpDot.ToString()),
                    };

                    sheetName = $"{list.IniOpDtl.DataId}_{list.IniOpDtl.FeeYm}";
                    if (list.IniOpDtl == null || list.IniOpDtl == null)
                    {
                        excel.Workbook.Worksheets.Add("查無資料");
                        excel.Workbook.Worksheets.First().Cells[1, 1, 1, 1].Value = "查無資料";
                    }
                }
                else
                {
                    excelParameters = new (string Key, string Value)[]
                    {
                        ("##seq_no##",  list.IniDrDtl.SeqNo.ToString()),
                        ("##No##",  list.IniDrDtl.DataId+"_"+list.IniDrDtl.FeeYm),
                        ("##Today##",  printDate),
                        ("##hosp_data_type##",  "30"),
                        ("##data_id##",  list.IniDrDtl.DataId.ToString()),
                        ("##hospid##",  list.IniDrDtl.HospId.ToString()),
                        ("##HospSeqNo##",  list.IniDrDtl.HospSeqNo.ToString()),
                        ("##hospname##",  context.HospBasic.Where(x => x.HospId == list.IniDrDtl.HospId && x.HospSeqNo == list.IniDrDtl.HospSeqNo).FirstOrDefault().HospName.ToString()),
                        ("##fee_ym##",  list.IniDrDtl.FeeYm.ToString()),
                        ("##exemyear##",  list.IniDrDtl.ExamYear.ToString()),
                        ("##examtime##",  list.IniDrDtl.ExamTime == null ? "" : list.IniDrDtl.ExamTime.ToString()),
                        ("##weekcount##",  list.IniDrDtl.WeekCount == null ? "" : list.IniDrDtl.WeekCount.ToString()),
                        ("##instructexamyear##",  list.IniDrDtl.InstructExamYear.ToString()),
                        ("##instructexamtime##",  list.IniDrDtl.InstructExamTime == null ? "" : list.IniDrDtl.InstructExamTime.ToString()),
                        ("##firsttreatdate##",  list.IniDrDtl.FirstTreatDate.ToString()),
                        ("##appl_type##",  list.IniDrDtl.ApplType.ToString()),
                        ("##appl_date##",  list.IniDrDtl.ApplDate.ToString()),
                        ("##case_type##",  list.IniDrDtl.CaseType.ToString()),
                        ("##name##",  list.IniDrDtl.Name.ToString()),
                        ("##id_sex##",  list.IniDrDtl.IdSex.ToString()),
                        ("##func_date##",  list.IniDrDtl.FuncDate.ToString()),
                        ("##func_type##",  list.IniDrDtl.FuncType.ToString()),
                        ("##rel_date##",  list.IniDrDtl.RelDate.ToString()),
                        ("##real_hosp_id##",  ""),//藥局沒有
                        ("##drug_days##",  list.IniDrDtl.DrugDays.ToString()),
                        ("##birthday##",  list.IniDrDtl.Birthday.ToString()),
                        ("##id##",  list.IniDrDtl.Id.ToString()),
                        ("##func_seq_no##",  list.IniDrDtl.FuncSeqNo.ToString()),
                        ("##pay_type##",  list.IniDrDtl.PayType.ToString()),
                        ("##part_code##",  list.IniDrDtl.PartCode.ToString()),
                        ("##orig_hosp_id##",  list.IniDrDtl.OrigHospId.ToString()),
                        ("##icd9cm_code##",  list.IniDrDtl.Icd9cmCode.ToString()),
                        ("##icd9cm_code1##",  list.IniDrDtl.Icd9cmCode1.ToString()),
                        ("##icd9cm_code2##",  list.IniDrDtl.Icd9cmCode2.ToString()),
                        ("##icd10cm_code2##",  list.IniDrDtl.Icd10cmCode2.ToString()),
                        ("##icd10cm_code3##",  list.IniDrDtl.Icd10cmCode3.ToString()),
                        ("##icd10cm_code4##",  list.IniDrDtl.Icd10cmCode4.ToString()),
                        ("##cure_item1##",  list.IniDrDtl.CureItem1.ToString()),
                        ("##cure_item2##",  list.IniDrDtl.CureItem2.ToString()),
                        ("##cure_item3##",  list.IniDrDtl.CureItem3.ToString()),
                        ("##cure_item4##",  list.IniDrDtl.CureItem4.ToString()),
                        ("##corr_hosp_id##",  list.IniDrDtl.CorrHospId.ToString()),
                        ("##area_service##",  list.IniDrDtl.AreaService.ToString()),
                        ("##rel_mode##",  ""),//藥局沒有
                        ("##medapply##",  list.IniDrDtl.MedApply.ToString()),
                        ("##instructapply##",  list.IniDrDtl.InstructApply.ToString()),
                        ("##traceapply##",  list.IniDrDtl.TraceApply.ToString()),
                        ("##releaseapply##",  list.IniDrDtl.ReleaseApply == null ? "" : list.IniDrDtl.ReleaseApply.ToString()),

                        ("##drug_dot##",  list.IniDrDtl.DrugDot.ToString()),
                        ("##prsn_id##",  list.IniDrDtl.PrsnId.ToString()),
                        ("##drug_prsn_id##", list.IniDrDtl.DrugPrsnId.ToString()),
                        ("##cure_dot##",  list.IniDrDtl.CureDot.ToString()),
                        ("##dsvc_dot##",  list.IniDrDtl.DsvcDot.ToString()),
                        ("##other_part_amt##",  list.IniDrDtl.OtherPartAmt.ToString()),
                        ("##order_dot##",  list.IniDrOrds.Sum(x=> x.OrderDot)?.ToString()),
                        ("##part_amt##",  list.IniDrDtl.PartAmt.ToString()),
                        ("##appl_dot##",  list.IniDrDtl.ApplDot.ToString()),
                        ("##diag_code##",  ""),//藥局沒有
                        ("##diag_dot##",  ""),//藥局沒有
                        ("##agency_part_amt##",  ""),//藥局沒有
                        ("##dsvc_code##",  list.IniDrDtl.DsvcCode.ToString()),
                        ("##firstinstrucdate##",  list.IniDrDtl.FirstInstructDate.ToString()),
                        ("##inctructserial##",  list.IniDrDtl.InctructSerial == null ? "" : list.IniDrDtl.InctructSerial.ToString()),
                        ("##orig_case_type##",  list.IniDrDtl.OrigCaseType.ToString()),
                        ("##supp_area##",  ""),//藥局沒有
                        ("##exp_dot##",  list.IniDrDtl.ExpDot.ToString()),
                    }; sheetName = $"{list.IniDrDtl.DataId}_{list.IniDrDtl.FeeYm}";

                    if (list.IniDrDtl == null)
                    {
                        excel.Workbook.Worksheets.Add("查無資料");
                        excel.Workbook.Worksheets.First().Cells[1, 1, 1, 1].Value = "查無資料";
                    }
                }
                //    sheetNumber++;
                //   
                //    var gList = keyValues.OrderBy(x => x.seq_no).ToList();


                //}
                CreateMedicineReportsheetByData(list, sheetName, excelParameters, excel, worksheetTemplate);


                excel.SaveAs(vm.Stream, DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString());
                vm.Stream.Seek(0, SeekOrigin.Begin);
            }

            return vm;
        }

        private static void CreateMedicineReportsheetByData(MedicineReportViewModel list, string sheetName, (string Key, string Value)[] excelParameters, ExcelPackage excel, ExcelWorksheet worksheetTemplate)
        {

            excel.Encryption.Password = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString();
            excel.Encryption.IsEncrypted = true;
            var sheet1 = excel.Workbook.Worksheets.Add(sheetName);

            #region Header
            const int headerRowLength = 11;
            const int headerColLength = 14;

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
            //var Data_List = new MedicineReportViewModel();
            List<dynamic> Data_List = new List<dynamic>();
            switch (list.CheckOpOrDr)
            {
                case "醫療院所":
                    Data_List.Add(list.IniOpOrds);
                    break;
                case "藥局":
                    Data_List.Add(list.IniDrOrds);
                    break;
            }

            int row = headerRowLength + 1;
            foreach (var item in Data_List[0])
            {
                // detail text
                sheet1.Cells[row, 1].Value = item.OrderSeqNo;
                sheet1.Cells[row, 2].Value = Convert.ToInt16(item.OrderType);
                sheet1.Cells[row, 3].Value = item.OrderCode;
                sheet1.Cells[row, 4].Value = item.OrderChiName;// item..orderchiname;
                sheet1.Cells[row, 5].Value = item.Exe_Prsn_ID;
                sheet1.Cells[row, 6].Value = item.RelMode;
                sheet1.Cells[row, 7].Value = item.DrugNum;
                sheet1.Cells[row, 8].Value = item.DrugFre;
                sheet1.Cells[row, 9].Value = item.DrugPath;
                sheet1.Cells[row, 10].Value = item.OrderUprice;
                sheet1.Cells[row, 11].Value = item.OrderDrugDay;
                sheet1.Cells[row, 12].Value = item.OrderQty;
                sheet1.Cells[row, 13].Value = item.OrderDot;

                // detail style
                for (var i = 1; i <= headerColLength; i++)
                {
                    var cell = sheet1.Cells[row, i];
                    var style = cell.Style;
                    style.Font.Size = 15;
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
            const int footerRowLength = 11;
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
            //tarPrinterSettings.RepeatColumns = srcPrinterSettings.RepeatColumns;
            //tarPrinterSettings.RepeatRows = srcPrinterSettings.RepeatRows;
            tarPrinterSettings.PrintArea = sheet1.Cells;
            // 橫向
            tarPrinterSettings.Orientation = eOrientation.Landscape;
            tarPrinterSettings.PaperSize = ePaperSize.A4;
            tarPrinterSettings.VerticalCentered = true;
            tarPrinterSettings.HorizontalCentered = true;
            #endregion 列印
        }
        /// <summary>
        /// Dtl字典初始化
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Tuple<string, bool>> DictionaryDtlData()
        {
            Dictionary<string, Tuple<string, bool>> DictionaryForDtl = new Dictionary<string, Tuple<string, bool>>()
            {
                {"費用年月", new Tuple<string, bool>("FeeYm", true)},
                {"療程年度", new Tuple<string, bool>("ExamYear", true)},
                {"療程次數", new Tuple<string, bool>("ExamTime", true)},
                {"週數", new Tuple<string, bool>("WeekCount", true)},
                {"初診日", new Tuple<string, bool>("FirstTreatDate", true)},
                {"療程年度(衛)", new Tuple<string, bool>("InstructExamYear", true)},
                {"療程次數(衛)", new Tuple<string, bool>("InstructExamTime", true)},
                {"初診日(衛)", new Tuple<string, bool>("FirstInstructDate", true)},
                {"衛教序次", new Tuple<string, bool>("InctructSerial", true)},
                {"藥物申報", new Tuple<string, bool>("MedApply", true)},
                {"衛教申報", new Tuple<string, bool>("InstructApply", true)},
                {"追蹤申報", new Tuple<string, bool>("TraceApply", true)},
                {"釋出申報", new Tuple<string, bool>("ReleaseApply", true)},
                {"申報類別", new Tuple<string, bool>("ApplType", true)},
                {"機構代碼", new Tuple<string, bool>("HospId", true)},
                {"申報日期", new Tuple<string, bool>("ApplDate", true)},
                {"案件類別", new Tuple<string, bool>("CaseType", true)},
                {"流水號", new Tuple<string, bool>("SeqNo", true)},
                {"就醫科別", new Tuple<string, bool>("FuncType", true)},
                {"就醫日期", new Tuple<string, bool>("FuncDate", true)},
                {"調劑日期", new Tuple<string, bool>("RelDate", true)},
                {"出生日期", new Tuple<string, bool>("Birthday", true)},
                {"身分證號", new Tuple<string, bool>("Id", true)},
                {"就醫序號", new Tuple<string, bool>("FuncSeqNo", true)},
                {"給付類別", new Tuple<string, bool>("PayType", true)},
                {"部分負擔代號", new Tuple<string, bool>("PartCode", true)},
                {"主診斷代碼", new Tuple<string, bool>("Icd9cmCode", true)},
                {"次診斷代碼(一)", new Tuple<string, bool>("Icd9cmCode1", true)},
                {"次診斷代碼(二)", new Tuple<string, bool>("Icd9cmCode2", true)},
                {"給藥日份", new Tuple<string, bool>("DrugDays", true)},
                {"調劑方式", new Tuple<string, bool>("RelMode", true)},
                {"醫事人員身分證號", new Tuple<string, bool>("PrsnId", true)},
                {"藥師身分證號", new Tuple<string, bool>("DrugPrsnId", true)},
                {"藥費點數", new Tuple<string, bool>("DrugDot", true)},
                {"診療費點數", new Tuple<string, bool>("CureDot", true)},
                {"診察費代碼", new Tuple<string, bool>("DiagCode", true)},
                {"診察費點數", new Tuple<string, bool>("DiagDot", true)},
                {"藥事服務費代碼", new Tuple<string, bool>("DsvcCode", true)},
                {"藥事服務費點數", new Tuple<string, bool>("DsvcDot", true)},
                {"醫療費用點數", new Tuple<string, bool>("ExpDot", true)},
                {"部份負擔金額", new Tuple<string, bool>("PartAmt", true)},
                {"申請金額", new Tuple<string, bool>("ApplDot", true)},
                {"性別", new Tuple<string, bool>("IdSex", true)},
                {"修改備註", new Tuple<string, bool>("Remark", true)},
                {"特定治療項目(一)", new Tuple<string, bool>("CureItem1", true)},
                {"特定治療項目(二)", new Tuple<string, bool>("CureItem2", true)},
                {"特定治療項目(三)", new Tuple<string, bool>("CureItem3", true)},
                {"特定治療項目(四)", new Tuple<string, bool>("CureItem4", true)},
                {"釋出院所", new Tuple<string, bool>("OrigHospId", true)},
                {"特定地區醫療服務", new Tuple<string, bool>("AreaService", true)},
                {"支援區域", new Tuple<string, bool>("SuppArea", true)},
                {"實際提供醫療服務之醫事服務機構", new Tuple<string, bool>("RealHospId", true)},
                {"醫事類別", new Tuple<string, bool>("HospDataType", true)},
                {"原案件分類", new Tuple<string, bool>("OrigCaseType", true)},
                {"代辦部分負擔金額", new Tuple<string, bool>("AgencyPartAmt", true)},
                {"行政協助項目部分負擔點數", new Tuple<string, bool>("OtherPartAmt", true)},
                {"姓名", new Tuple<string, bool>("Name", true)},
                {"補報原因註記", new Tuple<string, bool>("ApplCauseMark", true)},
                {"國際疾病分類碼(三)", new Tuple<string, bool>("Icd10cmCode2", true)},
                {"國際疾病分類碼(四)", new Tuple<string, bool>("Icd10cmCode3", true)},
                {"國際疾病分類碼(五)", new Tuple<string, bool>("Icd10cmCode4", true)},
                {"特殊材料明細點數小計", new Tuple<string, bool>("MetDot", true)},
                {"矯正機關代號", new Tuple<string, bool>("CorrHospId", true)},
                {"電腦序號", new Tuple<string, bool>("data_id", true)},
            };
            return DictionaryForDtl;
        }

        /// <summary>
        /// Ord字典初始化
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Tuple<string, bool>> DictionaryOrdData()
        {
            Dictionary<string, Tuple<string, bool>> DictionaryForOrd = new Dictionary<string, Tuple<string, bool>>()
            {
                {"醫令類別", new Tuple<string, bool>("OrderType", true)},
                {"醫令代碼", new Tuple<string, bool>("OrderCode", true)},
                {"調劑方式", new Tuple<string, bool>("RelMode", true)},
                {"連續處方註記", new Tuple<string, bool>("ChrMark", true)},
                {"藥品用量", new Tuple<string, bool>("DrugNum", true)},
                {"藥品使用頻率", new Tuple<string, bool>("DrugFre", true)},
                {"用藥途徑/作用部位", new Tuple<string, bool>("DrugPath", true)},
                {"醫令單價", new Tuple<string, bool>("OrderUprice", true)},
                {"醫令數量", new Tuple<string, bool>("OrderQty", true)},
                {"醫令點數", new Tuple<string, bool>("OrderDot", true)},
                {"執行醫事人員代號", new Tuple<string, bool>("ExePrsnId", true)},
                {"備註", new Tuple<string, bool>("Note", true)},
                {"診療部位", new Tuple<string, bool>("CurePath", true)},
                {"給藥日份", new Tuple<string, bool>("OrderDrugDay", true)},
                {"電腦序號", new Tuple<string, bool>("data_id", true)},
            };
            return DictionaryForOrd;
        }
    }
}
