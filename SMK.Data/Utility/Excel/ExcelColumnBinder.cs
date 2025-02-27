using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Yozian.WebCore.Library.Utility.Excel
{
    public class ExcelColumnBinder<TModel>
    {
        private List<ExcelColumn<TModel>> columns = new List<ExcelColumn<TModel>>();


        public IEnumerable<ExcelColumn<TModel>> GetExcelColumns()
        {
            return this.columns;
        }

        public ExcelColumn<TModel> ColumnFor(Expression<Func<TModel, object>> expression, string columnName = "")
        {
            MemberInfo member;
            Expression bodyExpression = expression.Body;


            if (bodyExpression.NodeType.Equals(ExpressionType.Convert)
                && bodyExpression is UnaryExpression)
            {
                Expression operand = ((UnaryExpression)expression.Body).Operand;
                member = ((MemberExpression)operand).Member;
            }
            else
            {
                member = ((MemberExpression)bodyExpression).Member;
            }

            // try to get display namq of property
            var attribute = member.GetCustomAttribute<DisplayAttribute>(false);

            var column = new ExcelColumn<TModel>()
            {
                ColumnName = attribute?.Name ?? member.Name, // default to property name
                PropertyName = member.Name,
                ColumnType = ColumnType.Expression
            };
            // override if provied column name
            if (!string.IsNullOrEmpty(columnName))
            {
                column.ColumnName = columnName;
            }

            columns.Add(column);
            return column;
        }  


        /// <summary>
        /// 檔案匯出的多載
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="columnName"></param>
        /// <param name="checkdata">傳入True，代表要會出此欄位</param>
        /// <returns></returns>
        public ExcelColumn<TModel> ColumnFor(Expression<Func<TModel, object>> expression, string columnName = "",bool checkdata = true)
        {
            MemberInfo member;
            Expression bodyExpression = expression.Body;


            if (bodyExpression.NodeType.Equals(ExpressionType.Convert)
                && bodyExpression is UnaryExpression)
            {
                Expression operand = ((UnaryExpression)expression.Body).Operand;
                member = ((MemberExpression)operand).Member;
            }
            else
            {
                member = ((MemberExpression)bodyExpression).Member;
            }

            // try to get display namq of property
            var attribute = member.GetCustomAttribute<DisplayAttribute>(false);

            var column = new ExcelColumn<TModel>()
            {
                ColumnName = attribute?.Name ?? member.Name, // default to property name
                PropertyName = member.Name,
                ColumnType = ColumnType.Expression
            };
            // override if provied column name
            if (!string.IsNullOrEmpty(columnName))
            {
                column.ColumnName = columnName;
            }
            if(checkdata)
                columns.Add(column);
            return column;
        }

        /// <summary>
        /// 字典專用的輸出Excel方法
        /// </summary>
        /// <param name="expression">Linq抓取字典方法</param>
        /// <param name="key">字典的Key</param>
        /// <param name="columnName">Excel欄位名稱</param>
        /// <returns></returns>
        public ExcelColumn<TModel> ColumnForDictionary(Expression<Func<TModel, object>> expression,string key, string columnName = "")
        {
            MemberInfo member;
            Expression bodyExpression = expression.Body;


            if (bodyExpression.NodeType.Equals(ExpressionType.Convert)
                && bodyExpression is UnaryExpression)
            {
                Expression operand = ((UnaryExpression)expression.Body).Operand;
                member = ((MemberExpression)operand).Member;
            }
            else
            {
                member = ((MemberExpression)bodyExpression).Member;
            }

            // try to get display namq of property
            var attribute = member.GetCustomAttribute<DisplayAttribute>(false);

            var column = new ExcelColumn<TModel>()
            {
                ColumnName = attribute?.Name ?? member.Name, // default to property name
                PropertyName = key,
                ColumnType = ColumnType.Dictionary
            };
            // override if provied column name
            if (!string.IsNullOrEmpty(columnName))
            {
                column.ColumnName = columnName;
            }

            columns.Add(column);
            return column;
        }    
        
        public ExcelColumn<TModel> ColumnFor(string columnName, Func<TModel, object> valueProvider)
        {
            var column = new ExcelColumn<TModel>()
            {
                ColumnName = columnName, // default to property name
                ColumnType = ColumnType.ValueProvider,
                ValueProvider = valueProvider
            };

            columns.Add(column);
            return column;
        }

        public ExcelColumn<TModel> ColumnFor(string columnName, object value)
        {
            var column = new ExcelColumn<TModel>()
            {
                ColumnName = columnName, // default to property name
                ColumnValue = value,
                ColumnType = ColumnType.Customized
            };

            columns.Add(column);
            return column;
        }



    }
}
