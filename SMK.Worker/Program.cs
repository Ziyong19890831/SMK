using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SMK.Worker.AppScope;
using SMK.Worker.BackgroundServices;
using SMK.Worker.Services;
using Yozian.DependencyInjectionPlus;

namespace SMK.Worker
{
    public class Program
    {
        public Program(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
            
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: false)
                        .Build();
                    services.AddHostedService<IniDrDtlBackendService>();
                    services.AddHostedService<IniDrOrdBackendService>();
                    services.AddHostedService<IniOpDtlBackendService>();
                    services.AddHostedService<IniOpOrdBackendService>();
                    services.AddHostedService<CstAgentPatientService>();
                    services.AddHostedService<CstQsCureService>();
                    services.AddHostedService<CstQsDataBackendService>();
                    services.AddHostedService<CstQsData2BackendService>();
                    services.AddHostedService<CstQsStateBackendService>();
                    services.AddHostedService<QuitDataAllBackendService>();
                    services.AddHostedService<ExportScheduleService>();
                    services.RegisterDb(config.GetConnectionString("db")).RegisterServices();
                })
            ;
    }
}
