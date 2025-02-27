using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Yozian.WebCore.Library.Utility.Excel
{
    public class ExcelColumn<TModel>
    {
        public string ColumnName { get; set; }

        public object ColumnValue { get; set; }

        public string PropertyName { get; set; }

        public ColumnType ColumnType { get; set; }

        public Func<TModel, object> ValueProvider { get; set; }

        public Action<ExcelStyle> StyleDecorator { get; set; } = (s) => { };

        public Func<object, TModel, object> Formatter { get; private set; }



        public ExcelColumn()
        {
            this.Formatter = (v, m) =>
            {
                return v;
            };
        }

        public ExcelColumn<TModel> Name(string name)
        {
            this.ColumnName = name;
            return this;
        }


        public ExcelColumn<TModel> Format(Func<object, TModel, string> formatter)
        {
            this.Formatter = formatter;
            return this;
        }

        public ExcelColumn<TModel> Style(Action<ExcelStyle> styleDecorator)
        {
            this.StyleDecorator = styleDecorator;
            return this;
        }



    }


    public enum ColumnType
    {
        Expression,
        Customized,
        ValueProvider,
        Dictionary

    }
}
