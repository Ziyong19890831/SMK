using System.Collections.Generic;

namespace SMK.Worker.FileProcess.Handler
{
    public interface IFileInHandler<T>
    {
        int Header { get; set; }
        string Filename { get; }
        FileColumn[] Columns { get; }
        string Pattern { get; }
        string[] Parse(string line);
        T Transform(string[] values, Dictionary<string, object> args);
        T FindFirst(T entity);
        IEnumerable<T> Load(IEnumerable<T> entities);
    }
}