using System;
using System.Collections.Generic;
using System.ComponentModel;
using SMK.Data;

namespace SMK.Worker.FileProcess.Handler
{
    public abstract class FileInHandler<T> : IFileInHandler<T>
    {
        public virtual int Header { get; set; }
        public string Filename { get; }
        public abstract string FilenamePattern { get; }
        public virtual FileColumn[] Columns { get; }
        /// <summary>
        /// 文字總長度
        /// </summary>
        public virtual int Length { get; } = 0;
        public Func<IEnumerable<T>, IEnumerable<T>> Loader { get; set; }

        public string Pattern
        {
            get
            {
                var regex = new List<string>();
                foreach (var column in Columns) {
                    regex.Add(string.Format("(.{0})", column.Length));
                }
                return string.Join(",", regex.ToArray());
            }
        }

        public int PageSize { get; } = 10000;

        public virtual string[] Parse(string line)
        {
            return line.Split(",");
        }

        private Type GetGenericType()
        {
            return typeof(T);
        }

        public virtual T Transform(string[] values, Dictionary<string, object> args)
        {
            var type = GetGenericType();
            var instance = Activator.CreateInstance<T>();
            var props = TypeDescriptor.GetProperties(instance);
            
            for (var i = 0; i < values.Length; i++)
            {
                var column = Columns[i];
                var prop = props[column.ColumnName];
                prop.SetValue(instance, values[i]);
            }

            return instance;
        }

        public T FindFirst(T entity)
        {
            return default;
        }

        public virtual IEnumerable<T> Load(IEnumerable<T> entities)
        {
            return Loader(entities);
        }
    }
}