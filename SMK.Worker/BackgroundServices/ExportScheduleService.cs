using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Data.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Yozian.DependencyInjectionPlus.Attributes;

namespace SMK.Worker.Services
{
    [SingletonService]
    public class ExportScheduleService : IHostedService
    {
        public IConfiguration Configuration { get; set; }
        private readonly ILogger<ExportScheduleService> _logger;
        private string OutputFolder { get; }
        // protected virtual string InputFolder { get; }
        public SMKWEBContext Context;
        private Timer _timer;
        public ExportScheduleService(ILogger<ExportScheduleService> logger,
            IConfiguration configuration,
            SMKWEBContext context)
        {
            logger = _logger;
            Context = context;
            OutputFolder = configuration["FileWatchService:DataExportRootPath"];
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // timer repeates call to RemoveScheduledAccounts every 24 hours.
            _timer = new Timer(
                ExportFile,
                null,
                TimeSpan.Zero,
                TimeSpan.FromDays(1)
            );

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void ExportFile(object state)
        {
            foreach (var item in Context.IniExportInCtrl.Where(x=>x.Status == FileInStatus.Initialized).ToList())
            {
                item.Status = FileInStatus.Running;
                Context.SaveChanges(); ;
                
                string path = OutputFolder+"/" + item.Id.ToString() ;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                
                string[] files = {"inidrdtl","inidrord","iniopdtl", "iniopord"};
                foreach (string file in files)
                {
                    using (var fileStream = new FileStream(path + "/" + file + ".txt", FileMode.Append))
                    using (StreamWriter sw = new StreamWriter(fileStream))
                    {
                        Type t;
                        PropertyInfo[] props;
                        switch (file)
                        {
                            case "inidrdtl":

                                var data = new SMK.Data.Entity.IniDrDtl();
                                t = data.GetType();
                                props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                                sw.WriteLine(string.Join(",", props.Select(x=>x.Name)));
                                
                                    
                                foreach (var line in Context.IniDrDtl.Where(x=>x.FeeYm==item.fee_ym).ToList())
                                {
                                    List<string> input = new List<string>();
                                    foreach (var prop in props)
                                    {
                                        input.Add(prop.GetValue(line)==null?"": prop.GetValue(line).ToString());
                                    }
                                    sw.WriteLine(string.Join(",", input.ToArray()));
                                }
                                break;
                            case "inidrord":

                                var data2 = new SMK.Data.Entity.IniDrOrd();
                                t = data2.GetType();
                                props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                                sw.WriteLine(string.Join(",", props.Select(x => x.Name)));

                                
                                foreach (var line in Context.IniDrOrd.Where(x => x.FeeYm == item.fee_ym).ToList())
                                {
                                    List<string> input = new List<string>();
                                    foreach (var prop in props)
                                    {
                                        input.Add(prop.GetValue(line) == null ? "" : prop.GetValue(line).ToString());
                                    }
                                    sw.WriteLine(string.Join(",", input.ToArray()));
                                }
                                break;
                            case "iniopdtl":
                                var data3 = new SMK.Data.Entity.IniOpDtl();
                                t = data3.GetType();
                                props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                                sw.WriteLine(string.Join(",", props.Select(x => x.Name)));

                                foreach (var line in Context.IniOpDtl.Where(x => x.FeeYm == item.fee_ym).ToList())
                                {
                                    List<string> input = new List<string>();
                                    foreach (var prop in props)
                                    {
                                        input.Add(prop.GetValue(line) == null ? "" : prop.GetValue(line).ToString());
                                    }
                                    sw.WriteLine(string.Join(",", input.ToArray()));
                                }
                                break;
                            case "iniopord":
                                var data4 = new SMK.Data.Entity.IniOpOrd();
                                t = data4.GetType();
                                props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                                sw.WriteLine(string.Join(",", props.Select(x => x.Name)));

                                foreach (var line in Context.IniOpOrd.Where(x => x.FeeYm == item.fee_ym).ToList())
                                {
                                    List<string> input = new List<string>();
                                    foreach (var prop in props)
                                    {
                                        input.Add(prop.GetValue(line) == null ? "" : prop.GetValue(line).ToString());
                                    }
                                    sw.WriteLine(string.Join(",", input.ToArray()));
                                }
                                break;
                            default:
                                break;
                        }
                        
                    }
                }
                
                item.Status = FileInStatus.Completed;
                Context.SaveChanges();
            }
        }
    }
}
