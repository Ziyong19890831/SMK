using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Data.Utility;
using SMK.Worker.FileProcess.Handler;
using SMK.Worker.Services;
using Yozian.Extension;

namespace SMK.Worker.FileProcess
{
    public class FileInProcessor<T> : IFileInProcessor
    {
        public string FileName { get; set; }
        public FileInHandler<T> FileInHandler { get; set; }
        public Action<IEnumerable<T>> Loader { get; set; }
        public Action<Dictionary<string, object>> PrepareEnvironment { get; set; }
        public Action<Dictionary<string, object>> PostProcess { get; set; }
        public Action<Dictionary<string, object>> OnProcessCompleted { get; set; }
        private DateTime Timestamp { get; set; }
        public void Start()
        {
            Timestamp = DateTime.Now;
            var filename = Path.GetFileName(FileName);
            var ctrlId = 0;
            IIniFileInCtrlWriter ctrlWriter = null;
            if (this is IIniFileInCtrlWriter) ctrlWriter = ((IIniFileInCtrlWriter) this);
            if (ctrlWriter != null) ctrlId = ctrlWriter.Initialize(filename);

            if (string.IsNullOrWhiteSpace((filename))) return;
            var r = new Regex(FileInHandler.FilenamePattern, RegexOptions.IgnoreCase);

            // Match the regular expression pattern against a text string.
            var m = r.Match(filename);
            if (m.Success)
            {
                var args = new Dictionary<string, object>();
                if (PrepareEnvironment != null) PrepareEnvironment(args);

                args["Now"] = DateTime.Now;
                var entities = Execute(args);
                if (ctrlWriter != null) ctrlWriter.Write(ctrlId, FileInStatus.Running);
                foreach (var entity in entities)
                {
                    try
                    {
                        if (Loader == null) FileInHandler.Load(entity);
                        else Loader(entity);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }
                    if (ctrlWriter != null) ctrlWriter.Write(ctrlId, FileInStatus.Loading);
                }
                if (PostProcess != null) PostProcess(args);
                if (ctrlWriter != null) ctrlWriter.Write(ctrlId, FileInStatus.Completed);
                //if (OnProcessCompleted != null) OnProcessCompleted(args);
            }
        }

        public bool IsMatched()
        {
            var filename = Path.GetFileName(FileName);
            if (string.IsNullOrWhiteSpace((filename))) return false;
            var r = new Regex(FileInHandler.FilenamePattern, RegexOptions.IgnoreCase);

            // Match the regular expression pattern against a text string.
            var m = r.Match(filename);
            return m.Success;
        }

        private IEnumerable<IEnumerable<T>> Execute(Dictionary<string, object> args)
        {
            if (!File.Exists(FileName))
            {
                throw new FileNotFoundException();
            }
            string line;
            var streamReader = new StreamReader(FileName);
            int total = 0;
            int succeed = 0;
            int failed = 0;
            int skipped = 0;
            var i = 0;
            var entities = new List<T>();
            while ((line = streamReader.ReadLine()) != null)
            {
                total++;
                if (FileInHandler.Header != 0)
                {
                    if (FileInHandler.Header >= total) continue;
                }
                //開始一行一行讀取
                var Data = FileInHandler.Parse(line);
                if (FileInHandler.Length == 0 || Data.Length == FileInHandler.Length)
                {
                    try
                    {
                        var list = new List<T>();
                        var item = FileInHandler.Transform(Data, args);
                        if (item is IHasSeqNo)
                        {
                            ((IHasSeqNo) item).SeqNo = total;
                        }
                        if (list.Contains(item))
                        {
                            skipped++;
                        }
                        else
                        {
                            list.Add(item);
                            succeed++;
                        }
                        entities.AddRange(list);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                        failed++;
                    }
                    
                    if (entities.Count >= FileInHandler.PageSize)
                    {
                        yield return entities;
                        entities.Clear();
                    }
                }
                else
                {
                    // string fileName = @"ExceptionFile\iniDrDtlException" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    // WriteException(fileName, line);
                    failed++;
                }
                i++;
            }

            total = succeed + failed + skipped;
            streamReader.Close();
            streamReader.Dispose();
            yield return entities;
        }
        public void WriteExceptionLog(SMKWEBContext context,Exception exception)
        {
            
            var stackTrace = DumpDetail(exception);

            var errLog = new ExceptionLog()
            {
                Id = MyGuid.NewGuid(),
                Category = "Worker",
                Message = exception.Message,
                Source = exception.Source,
                StackTrace = stackTrace
            };

            context.ExceptionLog.Add(errLog);

            context.SaveChanges();
        }
        private static string DumpDetail(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            var st = new StackTrace(ex, true);
            var innerExeption = ex;
            while (innerExeption != null)
            {
                sb.AppendLine(innerExeption.Message);
                innerExeption = innerExeption.InnerException;
            }
            sb.AppendLine("--------Data----------");

            sb.AppendLine(JsonConvert.SerializeObject(ex.Data));

            sb.AppendLine("--------Stacktrace----------");

            st.GetFrames()
                          .Select(frame => new
                          {
                              FileName = frame.GetFileName(),
                              LineNumber = frame.GetFileLineNumber(),
                              ColumnNumber = frame.GetFileColumnNumber(),
                              Method = frame.GetMethod(),
                              Class = frame.GetMethod().DeclaringType
                          })
                          //.Where(x => !x.Class.Namespace.StartsWith("System") && !x.Class.Namespace.StartsWith("Microsoft"))
                          .Reverse()
                          .ForEach((info) =>
                          {
                              sb.AppendLine($"class:{info.Class}");
                              sb.AppendLine($"method:{info.Method}");
                              sb.AppendLine($"line:{info.LineNumber}");
                              sb.AppendLine($"column:{info.ColumnNumber}");
                              sb.AppendLine($"file:{info.FileName}");
                              sb.AppendLine("----------Next-----------");
                          });


            return sb.ToString();
        }
    }
}