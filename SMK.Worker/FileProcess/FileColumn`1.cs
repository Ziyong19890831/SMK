using System;
using System.Linq.Expressions;

namespace SMK.Worker.FileProcess
{
    public class FileColumn<T> : FileColumn
    {
        public Expression<Func<T, object>> Transform { get; set; }

        public FileColumn(string name, string example, int length) : base(name, example, length)
        {
        }

        public FileColumn(string name, int length) : base(name, length)
        {
        }

        public void ColumnFor(Expression<Func<T, object>> expression, string columnName = "")
        {
            
        }
    }
}