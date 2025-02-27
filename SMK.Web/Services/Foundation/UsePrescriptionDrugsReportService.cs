using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SMK.Data;
using System.Data;
using Yozian.DependencyInjectionPlus.Attributes;
using Dapper;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Linq;
using System;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Collections.Generic;

namespace SMK.Web.Services.Foundation
{
    [ScopedService]
    public class UsePrescriptionDrugsReportService : GenericService
    {
        private readonly IDbConnection _conn = null;
        private readonly IWebHostEnvironment _env;
        public UsePrescriptionDrugsReportService(SMKWEBContext context, SessionManager smgr, IWebHostEnvironment env)
        : base(context, smgr)
        {
            _conn = context.Database.GetDbConnection();
            _env = env;
        }
        private class orderQtyDot
        {
            public string Order_Code { get; set; }
            public string OrderChiName { get; set; }
            public string OrderEngName { get; set; }
            public decimal? 數量 { get; set; }
            public decimal? 補助費用 { get; set; }
        }
        public async Task<byte[]> Export(int sy, int ey)
        {
            using (var excel = new ExcelPackage())
            {
                excel.Encryption.Password = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString();
                excel.Encryption.IsEncrypted = true;
                string syear = (sy + 1911).ToString() + "0000";
                string eyear = (ey + 1911).ToString() + "9999";
                excel.Workbook.Worksheets.Add("合計 (排序)");
                
                ExcelWorksheet sheet = excel.Workbook.Worksheets["合計 (排序)"];
                sheet.DefaultRowHeight = 39.75;
                var data = await context.GenDrugBasic.Where(x =>
                    (x.DrugNo.StartsWith("A") || x.DrugNo.StartsWith("B")) &&
                    ((string.Compare(x.OrderEndDate, syear) > 0 || string.IsNullOrEmpty(x.OrderEndDate)) && string.Compare(eyear, x.OrderStartDate) >= 0)
                ).ToListAsync();
                var medicine = data.Select(x => x.DrugNo).Distinct().ToList();
                Dictionary<int, List<orderQtyDot>> stocks = new Dictionary<int, List<orderQtyDot>>();
                for (int i = sy; i <= ey; i++)
                {
                    string YSTART = (i + 1911).ToString() + "01";
                    string YEND = (i + 1911).ToString() + "12";
                    var JoinStatement = await context.OrdOfB7.GroupJoin(context.GenOrderCode, a => a.Order_Code, b => b.OrderCode, (a, b) => new { a, b })
                        .SelectMany(x => x.b.DefaultIfEmpty(), (a, b) => new { a, b })
                        .Where(x => string.Compare(x.a.a.Fee_YM, YSTART) >= 0 && string.Compare(YEND, x.a.a.Fee_YM) >= 0
                                      && medicine.Contains(x.a.a.Order_Code))
                        .GroupBy(x => new { x.a.a.Order_Code, x.b.OrderChiName, x.b.OrderEngName })
                        .OrderBy(x => x.Key.Order_Code)
                        .Select(x => new orderQtyDot
                        {
                            Order_Code = x.Key.Order_Code,
                            OrderChiName = x.Key.OrderChiName,
                            OrderEngName = x.Key.OrderEngName,
                            數量 = x.Sum(y => y.a.a.Order_Qty),
                            補助費用 = x.Sum(y => y.a.a.Order_Dot)
                        }).ToListAsync();
                    stocks.Add(i, JoinStatement);
                }


                sheet.Cells[1, 7].Value = sy.ToString()+"-"+ey.ToString()+"年戒菸輔助用藥數量與費用";
                sheet.Cells[2, 1].Value = "編號";
                sheet.Cells[2, 1].Style.WrapText = true;
                sheet.Cells[2, 1].Style.Font.Bold = true;
                sheet.Column(1).Width = 5.13;
                sheet.Cells[2, 1, 3, 1].Merge = true;
                sheet.Cells[2, 2].Value = "健保署\r\n編碼";
                sheet.Cells[2, 2].Style.WrapText = true;
                sheet.Cells[2, 2].Style.Font.Bold = true;
                sheet.Column(2).Width = 13.13;
                sheet.Cells[2, 2, 3, 2].Merge = true;
                sheet.Cells[2, 3].Value = "有無健保價/處方藥或指示用藥";
                sheet.Cells[2, 3].Style.WrapText = true;
                sheet.Cells[2, 3].Style.Font.Bold = true;
                sheet.Column(3).Width = 13.88;
                sheet.Cells[2, 3, 3, 3].Merge = true;
                sheet.Cells[2, 4].Value = "品名";
                sheet.Cells[2, 4].Style.WrapText = true;
                sheet.Cells[2, 4].Style.Font.Bold = true;
                sheet.Column(4).Width = 53;
                sheet.Cells[2, 4, 3, 4].Merge = true;
                sheet.Cells[2, 5].Value = "廠商";
                sheet.Cells[2, 5].Style.WrapText = true;
                sheet.Cells[2, 5].Style.Font.Bold = true;
                sheet.Column(5).Width = 22.75;
                sheet.Cells[2, 5, 3, 5].Merge = true;
                sheet.Cells[2, 6].Value = "目前補助額度(元)";
                sheet.Cells[2, 6].Style.WrapText = true;
                sheet.Cells[2, 6].Style.Font.Bold = true;
                sheet.Column(6).Width = 11.88;
                sheet.Cells[2, 6, 3, 6].Merge = true;
                for (int i = sy; i <= ey; i++)
                {
                    sheet.Cells[2,7 + 4 * (i -sy )].Value = i;
                    sheet.Cells[2,7 + 4 * (i -sy ),2, 10 + 4 * (i - sy )].Merge = true;
                    //sheet.Cells[2,7 + 4 * (i -sy ),2, 10 + 4 * (i - sy )].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[3,7 + 4 * (i -sy )].Value = "數量";
                    sheet.Column(7 + 4 * (i - sy)).Width = 15.13;
                    sheet.Cells[3,8 + 4 * (i -sy )].Value = "補助費用(元)";
                    sheet.Column(8 + 4 * (i - sy)).Width = 15.13;
                    sheet.Cells[3,9 + 4 * (i - sy)].Value = "補助費用佔比";
                    sheet.Column(9 + 4 * (i - sy)).Width = 15.88;
                    sheet.Cells[3,10 + 4 * (i - sy)].Value = "補助費用排序";
                    sheet.Column(10 + 4 * (i - sy)).Width = 15.13;
                }
                var com = data.GroupBy(x => x.DrugCompany).Select(x => new { companyName = x.Key, CompanyMedNum = x.Count() });
                int currentrow = 4;
                foreach (var item in com)
                {
                    sheet.Cells[currentrow,5].Value = item.companyName;
                    sheet.Cells[currentrow, 5].Style.WrapText = true;
                    foreach (var item2 in data.Where(x => x.DrugCompany == item.companyName))
                    {
                        sheet.Cells[currentrow, 1].Value = currentrow-3;
                        sheet.Cells[currentrow, 2].Value = item2.DrugNo;
                        sheet.Cells[currentrow, 3].Value = item2.prescription;
                        sheet.Cells[currentrow, 4].Value = item2.OrderChiName + "\r\n" + item2.OrderEngName;
                        sheet.Cells[currentrow, 4].Style.WrapText = true;
                        sheet.Cells[currentrow, 6].Value = item2.prescription == "處方藥" ? "依健保價(" + item2.UnitPrice.ToString() + ")" : item2.UnitPrice.ToString();
                        for (int i = sy; i <= ey; i++)
                        {
                            if(stocks.FirstOrDefault(x=>x.Key==i).Value!=null &&
                                stocks.FirstOrDefault(x => x.Key == i).Value.FirstOrDefault(x => x.Order_Code.Trim() == item2.DrugNo)!=null
                                )
                            {
                                var drug = stocks.FirstOrDefault(x => x.Key == i).Value.FirstOrDefault(x => x.Order_Code.Trim() == item2.DrugNo);
                                var drugs = stocks.FirstOrDefault(x => x.Key == i).Value.OrderByDescending(x => x.補助費用).ToList();
                                
                                
                                sheet.Cells[currentrow, 7 + 4 * (i - sy)].Value = drug.數量.Value.ToString("N0");
                                sheet.Cells[currentrow, 8 + 4 * (i - sy)].Value = drug.補助費用.Value.ToString("N0");
                                sheet.Cells[currentrow, 9 + 4 * (i - sy)].Value = Math.Round(((decimal)drug.補助費用 / (decimal)drugs.Sum(x => x.補助費用))*100, 1, MidpointRounding.AwayFromZero)+"%";
                                sheet.Cells[currentrow, 10 + 4 * (i - sy)].Value = drugs.IndexOf(drug)+1;
                            }
                            else
                            {
                                sheet.Cells[currentrow, 7 + 4 * (i - sy)].Value = "-";
                                sheet.Cells[currentrow, 8 + 4 * (i - sy)].Value = "-";
                                sheet.Cells[currentrow, 9 + 4 * (i - sy)].Value = "-";
                                sheet.Cells[currentrow, 10 + 4 * (i - sy)].Value = "-";
                            }
                            
                        }
                        currentrow++;
                    }

                    sheet.Cells[currentrow-(data.Where(x => x.DrugCompany == item.companyName).Count()),5, currentrow-1, 5].Merge = true;
                    
                }

                
                

                var allCells = sheet.Cells[1, 1, sheet.Dimension.End.Row, sheet.Dimension.End.Column];
                var cellFont = allCells.Style.Font;
                cellFont.SetFromFont(new Font("標楷體", 12));
                allCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                allCells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                excel.Workbook.Worksheets.Add("費用分析");
                sheet = excel.Workbook.Worksheets["費用分析"];
                Dictionary<int,List<string>> drugYearList = new Dictionary<int,List<string>>();
                Dictionary<int, int?> 戒菸服務總額 = new Dictionary<int, int?>();
                Dictionary<int, int?> 藥費總額 = new Dictionary<int, int?>();
                int currentColumn = 1;
                for (int i = sy; i <= ey; i++)
                {
                    string YSTART = (i + 1911).ToString() + "0000";
                    string YEND = (i + 1911).ToString() + "1299";
                    var drugs = await context.GenDrugBasic.Where(x =>
                    (x.DrugNo.StartsWith("A") || x.DrugNo.StartsWith("B")) &&
                    ((string.Compare(x.OrderEndDate, YSTART) > 0 || string.IsNullOrEmpty(x.OrderEndDate)) && string.Compare(YEND, x.OrderStartDate) >= 0)
                ).ToListAsync();
                    medicine = drugs.Select(x => x.DrugNo).ToList();
                    var HealthCarePriceList = drugs.Where(x => x.HealthCarePrice == true).Select(x=>x.DrugNo).ToList();
                    var NoHealthCarePriceList = drugs.Where(x => x.HealthCarePrice == false).Select(x => x.DrugNo).ToList();
                    var NoHealthCarePriceList1 = drugs.Where(x => x.HealthCarePrice == false && x.prescription== "處方藥").Select(x => x.DrugNo).ToList();
                    var NoHealthCarePriceList2 = drugs.Where(x => x.HealthCarePrice == false && x.prescription == "指示用藥").Select(x => x.DrugNo).ToList();
                    drugYearList.Add(i, medicine);
                    if(i==sy || medicine.Count!=drugYearList.First(x=>x.Key==i-1).Value.Count){ // 如果要印標題
                        sheet.Cells[ 2, currentColumn].Value = "藥費分類";
                        sheet.Column(currentColumn).Width = 25.63;
                        sheet.Row(2).Height =19.5;
                        sheet.Row(3).Height = 49.5;
                        sheet.Cells[ 2,currentColumn,3, currentColumn].Merge = true;
                        sheet.Cells[ 4,currentColumn].Value = HealthCarePriceList.Count().ToString()+"項健保價(處方藥)藥費";
                        sheet.Row(4).Height = 40.5;
                        sheet.Cells[ 5,currentColumn].Value = NoHealthCarePriceList.Count().ToString() + "項無健保價藥費";
                        sheet.Row(5).Height = 40.5;
                        sheet.Cells[ 6, currentColumn].Value = "總計";
                        sheet.Row(5).Height = 40.5;

                        sheet.Cells[9, currentColumn].Value = "藥費分類";
                        sheet.Row(9).Height = 19.5;
                        sheet.Row(10).Height = 49.5;
                        sheet.Cells[9, currentColumn, 10, currentColumn].Merge = true;
                        sheet.Cells[11, currentColumn].Value = NoHealthCarePriceList1.Count().ToString() + "項處方藥";
                        sheet.Row(11).Height = 40.5;
                        sheet.Cells[12, currentColumn].Value = NoHealthCarePriceList2.Count().ToString() + "項指示用藥";
                        sheet.Row(12).Height = 40.5;
                        sheet.Cells[13, currentColumn].Value = "總計";
                        sheet.Row(13).Height = 40.5;
                        currentColumn++;
                    }
                    sheet.Cells[2, currentColumn].Value = i.ToString();
                    sheet.Column(currentColumn).Width = 15;
                    sheet.Cells[3, currentColumn].Value = "補助費用";
                    sheet.Cells[3, currentColumn].Style.WrapText = true;
                    sheet.Cells[3, currentColumn+1].Value = "占總藥費比率";
                    sheet.Cells[3, currentColumn + 1].Style.WrapText = true;
                    sheet.Column(currentColumn + 1).Width = 9.25;
                    sheet.Cells[3, currentColumn+2].Value = "占戒菸服務總費用比率";
                    sheet.Cells[3, currentColumn + 2].Style.WrapText = true;
                    sheet.Column(currentColumn + 2).Width = 11;
                    sheet.Cells[2, currentColumn, 2, currentColumn + 2].Merge = true;
                    int? HealthCarePrice = await context.OrdOfB7.Where(x => string.Compare(x.Fee_YM, YSTART) >= 0 && string.Compare(YEND, x.Fee_YM) >= 0
                                      && HealthCarePriceList.Contains(x.Order_Code)).SumAsync(x => x.Order_Dot);
                    int? NoHealthCarePrice = await context.OrdOfB7.Where(x => string.Compare(x.Fee_YM, YSTART) >= 0 && string.Compare(YEND, x.Fee_YM) >= 0
                                      && NoHealthCarePriceList.Contains(x.Order_Code)).SumAsync(x => x.Order_Dot);
                    int? AllHealthCarePrice = await context.OrdOfB7.Where(x => string.Compare(x.Fee_YM, YSTART) >= 0 && string.Compare(YEND, x.Fee_YM) >= 0
                                      && medicine.Contains(x.Order_Code)).SumAsync(x => x.Order_Dot);
                    sheet.Cells[4, currentColumn].Value = HealthCarePrice.Value.ToString("N0");
                    sheet.Cells[5, currentColumn].Value = NoHealthCarePrice.Value.ToString("N0");
                    sheet.Cells[6, currentColumn].Value = AllHealthCarePrice.Value.ToString("N0");
                    sheet.Cells[4, currentColumn + 1].Value = ((decimal)AllHealthCarePrice.Value!=0?   Math.Round(((decimal)HealthCarePrice.Value   *100/ (decimal)AllHealthCarePrice.Value  ), 1, MidpointRounding.AwayFromZero).ToString() + "%" : "-");
                    sheet.Cells[5, currentColumn + 1].Value = ((decimal)AllHealthCarePrice.Value!= 0 ? Math.Round(((decimal)NoHealthCarePrice.Value * 100 / (decimal)AllHealthCarePrice.Value), 1, MidpointRounding.AwayFromZero).ToString() + "%" : "-");
                    sheet.Cells[6, currentColumn+1].Value = "100.0%";
                    int? totalservice = await context.OrdOfB7.Where(x => string.Compare(x.Fee_YM, YSTART) >= 0 && string.Compare(YEND, x.Fee_YM) >= 0
                                      ).SumAsync(x => x.Order_Dot);
                    int? totaldrug = await context.OrdOfB7.Where(x => string.Compare(x.Fee_YM, YSTART) >= 0 && string.Compare(YEND, x.Fee_YM) >= 0
                                     && (x.Order_Code.StartsWith("A") || x.Order_Code.StartsWith("B"))).SumAsync(x => x.Order_Dot);
                    // 計算戒菸服務總額
                    戒菸服務總額.Add(i, totalservice);
                    藥費總額.Add(i, totaldrug);
                    sheet.Cells[4, currentColumn + 2].Value = (decimal)totalservice.Value != 0 ? Math.Round(((decimal)HealthCarePrice.Value    *100/ (decimal)totalservice.Value  ), 1, MidpointRounding.AwayFromZero).ToString()+"%":"-";
                    sheet.Cells[5, currentColumn + 2].Value = (decimal)totalservice.Value != 0 ? Math.Round(((decimal)NoHealthCarePrice.Value  *100/ (decimal)totalservice.Value  ), 1, MidpointRounding.AwayFromZero).ToString()+"%" : "-";
                    sheet.Cells[6, currentColumn + 2].Value = (decimal)totalservice.Value != 0 ? Math.Round(((decimal)AllHealthCarePrice.Value * 100 / (decimal)totalservice.Value), 1, MidpointRounding.AwayFromZero).ToString() + "%" : "-";
                    //
                    sheet.Cells[9, currentColumn].Value = i.ToString();
                    sheet.Column(currentColumn).Width = 15;
                    sheet.Cells[10, currentColumn].Value = "補助費用";
                    sheet.Cells[10, currentColumn].Style.WrapText = true;
                    sheet.Cells[10, currentColumn + 1].Value = "占總藥費比率";
                    sheet.Cells[10, currentColumn + 1].Style.WrapText = true;
                    sheet.Column(currentColumn + 1).Width = 9.25;
                    sheet.Cells[10, currentColumn + 2].Value = "占戒菸服務總費用比率";
                    sheet.Cells[10, currentColumn + 2].Style.WrapText = true;
                    sheet.Column(currentColumn + 2).Width = 11;
                    sheet.Cells[9, currentColumn, 9, currentColumn + 2].Merge = true;
                    int? NoHealthCarePrice1 = await context.OrdOfB7.Where(x => string.Compare(x.Fee_YM, YSTART) >= 0 && string.Compare(YEND, x.Fee_YM) >= 0
                                      && NoHealthCarePriceList1.Contains(x.Order_Code)).SumAsync(x => x.Order_Dot);
                    int? NoHealthCarePrice2 = await context.OrdOfB7.Where(x => string.Compare(x.Fee_YM, YSTART) >= 0 && string.Compare(YEND, x.Fee_YM) >= 0
                                      && NoHealthCarePriceList2.Contains(x.Order_Code)).SumAsync(x => x.Order_Dot);

                    sheet.Cells[11, currentColumn].Value = NoHealthCarePrice1.Value.ToString("N0");
                    sheet.Cells[12, currentColumn].Value = NoHealthCarePrice2.Value.ToString("N0");
                    sheet.Cells[13, currentColumn].Value = NoHealthCarePrice.Value.ToString("N0");
                    sheet.Cells[11, currentColumn + 1].Value = ((decimal)NoHealthCarePrice.Value != 0 ?  Math.Round(  ((decimal)NoHealthCarePrice1.Value*100 / (decimal)NoHealthCarePrice.Value), 1, MidpointRounding.AwayFromZero).ToString()+"%" : "-");
                    sheet.Cells[12, currentColumn + 1].Value = ((decimal)NoHealthCarePrice.Value != 0 ?  Math.Round(  ((decimal)NoHealthCarePrice2.Value*100 / (decimal)NoHealthCarePrice.Value),1,MidpointRounding.AwayFromZero).ToString() + "%" : "-");
                    sheet.Cells[13, currentColumn + 1].Value = "100.0%";
                    
                    // 計算戒菸服務總額
                    sheet.Cells[11, currentColumn + 2].Value = (decimal)totalservice.Value != 0 ? Math.Round(((decimal)NoHealthCarePrice1.Value*100 / (decimal)totalservice.Value ), 1, MidpointRounding.AwayFromZero).ToString()+"%" : "-";
                    sheet.Cells[12, currentColumn + 2].Value = (decimal)totalservice.Value != 0 ? Math.Round(((decimal)NoHealthCarePrice2.Value*100 / (decimal)totalservice.Value ), 1, MidpointRounding.AwayFromZero).ToString()+"%" : "-";
                    sheet.Cells[13, currentColumn + 2].Value = (decimal)totalservice.Value != 0 ? Math.Round(((decimal)NoHealthCarePrice.Value * 100 / (decimal)totalservice.Value), 1, MidpointRounding.AwayFromZero).ToString() + "%" : "-";

                    ///
                    currentColumn = currentColumn + 3;
                }
                currentColumn++;
                sheet.Cells[2, currentColumn].Value = "年度";
                sheet.Cells[2, currentColumn+1].Value = "戒菸服務總額";
                sheet.Column(currentColumn + 1).Width = 15.75;
                sheet.Cells[2, currentColumn + 2].Value = "藥費總額";
                sheet.Column(currentColumn + 2).Width = 15.75;
                for (int i = sy; i <= ey; i++)
                {
                    sheet.Cells[2+ i - sy + 1, currentColumn].Value = i.ToString();
                    sheet.Cells[2 + i - sy + 1, currentColumn+1].Value = 戒菸服務總額.First(x=>x.Key== i).Value.Value.ToString("N0");
                    sheet.Cells[2 + i - sy + 1, currentColumn+2].Value = 藥費總額.First(x => x.Key == i).Value.Value.ToString("N0");
                }
                allCells = sheet.Cells[1, 1, sheet.Dimension.End.Row, sheet.Dimension.End.Column];
                cellFont = allCells.Style.Font;
                cellFont.SetFromFont(new Font("標楷體", 12));
                allCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                allCells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                byte[] file = excel.GetAsByteArray(DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString());
                return file;
            }
        }
    }

}
