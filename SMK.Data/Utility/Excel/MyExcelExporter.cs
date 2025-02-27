using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace Yozian.WebCore.Library.Utility.Excel
{
    public class MyExcelExporter<TModel>
    {

        private static readonly int startIndex = 1;

        private string title = string.Empty;

        private string subTitle = string.Empty;

        private ExcelColumnBinder<TModel> columnBinder = new ExcelColumnBinder<TModel>();

        private IEnumerable<TModel> source;


        public MyExcelExporter(IEnumerable<TModel> source)
        {
            this.source = source;
        }

        public MyExcelExporter<TModel> Title(string title)
        {
            this.title = title;
            return this;
        }

        public MyExcelExporter<TModel> SubTitle(string subTitle)
        {
            this.subTitle = subTitle;
            return this;
        }

        public MyExcelExporter<TModel> DefineColumns(Action<ExcelColumnBinder<TModel>> binder)
        {
            binder(this.columnBinder);

            return this;
        }


        public byte[] GetResult(string SheetNewName = "data")
        {

            using (var package = new ExcelPackage())
            {
                var sheetName = SheetNewName;
                var ws = package.Workbook.Worksheets.Add(sheetName);
                var columns = this.columnBinder.GetExcelColumns();
                var props = TypeDescriptor.GetProperties(this.source.FirstOrDefault());
                var currentRowIndex = startIndex;
                string pwd = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString();
                ws.Workbook.Protection.SetPassword(pwd);
                ws.Protection.IsProtected = true;
                if (!string.IsNullOrEmpty(this.title))
                {
                    ws.Cells[currentRowIndex, 1].Value = this.title;
                    if (!string.IsNullOrEmpty(this.subTitle))
                    {
                        ws.Cells[currentRowIndex, 2].Value = this.subTitle;
                    }
                    currentRowIndex++;
                }

                // columns
                var columnIndex = 1;

                foreach (var column in columns)
                {
                    var cell = ws.Cells[currentRowIndex, columnIndex++];
                    cell.Value = column.ColumnName;
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Fill.BackgroundColor.SetColor(Color.DarkGreen);
                    cell.Style.Font.Color.SetColor(Color.White);
                    cell.AutoFitColumns();
                }

                foreach (var model in this.source)
                {
                    // next row
                    currentRowIndex++;
                    // reset index
                    columnIndex = startIndex;

                    foreach (var column in columns)
                    {
                        switch (column.ColumnType)
                        {
                            case ColumnType.Expression:
                                var prop = props[column.PropertyName];
                                var value = prop.GetValue(model);
                                column.ColumnValue = value;
                                break;
                            case ColumnType.ValueProvider:
                                column.ColumnValue = column.ValueProvider(model);
                                break;
                            case ColumnType.Dictionary:
                                // 這邊可能需要進一步的處理，視情況而定
                                // 這只是一個簡單的示例，假設 column.PropertyName 是字典中的鍵
                                if (model is IDictionary<string, object> dictionaryModel)
                                {
                                    if (dictionaryModel.ContainsKey(column.PropertyName))
                                    {
                                        column.ColumnValue = dictionaryModel[column.PropertyName];
                                    }
                                }
                                break;

                        }

                        column.ColumnValue = column.Formatter(column.ColumnValue, model);

                        var cell = ws.Cells[currentRowIndex, columnIndex++];
                        column.StyleDecorator(cell.Style);
                        cell.Value = column.ColumnValue;
                        cell.AutoFitColumns();
                    }
                }

                ws.Cells[ws.Dimension.Address].AutoFilter = true;
                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                return package.GetAsByteArray(pwd);
            }

        }

    }
}
