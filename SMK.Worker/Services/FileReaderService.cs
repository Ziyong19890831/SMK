using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SMK.Data;

namespace SMK.Worker.Services
{
    public class FileReaderService<T>
    {
        public int Length { get; set; }
        public int PageSize { get; set; } = 5000;
        public string FileName { get; set; }
        public Predicate<string> PreCheck { get; set; }
        public Action<Dictionary<string, object>> PrepareEnvironment { get; set; }
        public Action<IEnumerable<T>> Loader { get; set; }

        public IEnumerable<IEnumerable<T>> Read(Func<string[], T> mapper)
        {
            if (PreCheck(""))
            {
                
            }

            var values = new Dictionary<string, object>();
            if (PrepareEnvironment != null)
            {
                PrepareEnvironment(values);
            }
            
            string line;
            var streamReader = new StreamReader(FileName);
            streamReader.ReadLine(); //跳過第一行

            var i = 0;
            var entities = new List<T>();
            while ((line = streamReader.ReadLine()) != null)
            {
                //開始一行一行讀取
                var Data = line.Split(',');
                if (Length == 0 || Data.Length == Length)
                {
                    entities.Add(mapper(Data));
                    i++;
                    if (i >= PageSize)
                    {
                        yield return entities;
                        entities.Clear();
                    }
                }
                else
                {
                    string fileName = @"ExceptionFile\iniDrDtlException" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    // WriteException(fileName, line);
                }
            }
            streamReader.Close();
            streamReader.Dispose();
            yield return entities;
        }
        
        public interface FileLoader
        {
            public void Load(IEnumerable<T> entities);
        }

        public void Load(Func<string[], T> mapper)
        {
            var entities = Read(mapper);
            foreach (var entity in entities)
            {
                Loader(entity);
            }
        }
    }
}