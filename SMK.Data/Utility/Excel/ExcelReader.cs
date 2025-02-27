using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yozian.Extension;

namespace Yozian.WebCore.Library.Utility.Excel
{
    public class ExcelReader
    {
        private static readonly int startIndex = 0;

        public List<List<string>> ReadAsLIstOfList(string filePath, string sheetName = "")
        {

            var fi = new FileInfo(filePath);
            var stream = fi.OpenRead();

            return this.readAsListOfList(stream, sheetName);
        }

        public List<List<string>> ReadAsLIstOfList(Stream stream, string sheetName = "")
        {
            return this.readAsListOfList(stream, sheetName);
        }

        public List<Dictionary<string, string>> ReadAsHashMapList(string filePath, string sheetName = "", int columnNamesIndex = 2)
        {

            var rows = this.ReadAsLIstOfList(filePath, sheetName);
            var columnNames = rows
                .Skip(columnNamesIndex - 1)
                .Take(1)
                .First();

            return rows
                .Select(record =>
                {
                    var map = new Dictionary<string, string>();
                    columnNames
                        .ForEach((name, columnIndex) =>
                        {
                            map.Add(name, record[columnIndex]);
                        });

                    return map;
                })
                .ToList();
        }

        public List<Dictionary<string, string>> ReadAsHashMapList(Stream stream, string sheetName = "", int columnNamesIndex = 2)
        {
            var rows = this.ReadAsLIstOfList(stream, sheetName);
            var columnNames = rows
                .Skip(columnNamesIndex - 1)
                .Take(1)
                .First();

            return rows
                .Select(record =>
                {
                    var map = new Dictionary<string, string>();


                    columnNames.ForEach((name, columnIndex) =>
                        {
                            map.Add(name, record[columnIndex]);
                        });

                    return map;
                })
                .ToList();
        }


        private List<List<string>> readAsListOfList(Stream stream, string sheetName = "")
        {
            using (var package = new ExcelPackage())
            {
                package.Load(stream);
                var sheet = string.IsNullOrEmpty(sheetName) ? package.Workbook.Worksheets[startIndex] : package.Workbook.Worksheets[sheetName];
                int rows = startIndex;
                int columns = startIndex;

                var totalRows = sheet.Dimension.Rows;
                var totalColumns = sheet.Dimension.Columns;

                return Enumerable
                      .Range(1, totalRows)
                      .Select(rowIndex =>
                      {
                          var record = new List<string>();
                          Enumerable
                              .Range(1, totalColumns)
                              .ForEach(columnIndex =>
                              {
                                  var value = sheet.Cells[rowIndex, columnIndex].Value.SafeToString();
                                  record.Add(value);
                              });
                          return record;
                      })
                      .Where(record => !record.TrueForAll(val => string.IsNullOrEmpty(val)))
                      .ToList();

            }
        }

    }

}
