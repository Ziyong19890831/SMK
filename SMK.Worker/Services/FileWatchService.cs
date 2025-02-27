using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Worker.FileProcess;

namespace SMK.Worker.Services
{
    public class FileWatchService : BackgroundService
    {
        private readonly ILogger<FileWatchService> _logger;
        private FileSystemWatcher _folderWatcher;
        private string InputFolder { get; }
        private readonly IConfiguration configuration;
        private readonly SMKWEBContext context;
        public IniFileInCtrlService IniFileInCtrlService { get; set; }
        public IniOpDtlProcessor IniOpDtlProcessor { get; set; }
        public IniOpOrdProcessor IniOpOrdProcessor { get; set; }
        public IniDrDtlProcessor IniDrDtlProcessor { get; set; }
        public IniDrOrdProcessor IniDrOrdProcessor { get; set; }
        public CstAgentPatientProcessor CstAgentPatientProcessor { get; set; }
        public CstQsDataProcessor CstQsDataProcessor { get; set; }
        public CstQsCureProcessor CstQsCureProcessor { get; set; }
        public CstQsStateProcessor CstQsStateProcessor { get; set; }

        public FileWatchService(ILogger<FileWatchService> logger,
            IConfiguration configuration,
            SMKWEBContext context,
            IniFileInCtrlService iniFileInCtrlService,
            IniOpDtlProcessor iniOpDtlProcessor,
            IniOpOrdProcessor iniOpOrdProcessor,
            IniDrDtlProcessor iniDrDtlProcessor,
            IniDrOrdProcessor iniDrOrdProcessor,
            CstAgentPatientProcessor cstAgentPatientProcessor,
            CstQsDataProcessor cstQsDataProcessor,
            CstQsCureProcessor cstQsCureProcessor,
            CstQsStateProcessor cstQsStateProcessor)
        {
            _logger = logger;
            this.configuration = configuration;
            IniDrDtlProcessor = iniDrDtlProcessor;
            IniOpDtlProcessor = iniOpDtlProcessor;
            IniOpOrdProcessor = iniOpOrdProcessor;

            InputFolder = configuration["FileWatchService:DataImportRootPath"];
            this.context = context;
            IniFileInCtrlService = iniFileInCtrlService;
            IniDrOrdProcessor = iniDrOrdProcessor;

            CstAgentPatientProcessor = cstAgentPatientProcessor;
            CstQsDataProcessor = cstQsDataProcessor;
            CstQsCureProcessor = cstQsCureProcessor;
            CstQsStateProcessor = cstQsStateProcessor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Name} Starting", GetType().ShortDisplayName());
            if (!Directory.Exists(InputFolder))
            {
                _logger.LogWarning(
                    $"Please make sure the InputFolder [{InputFolder}] exists, then restart the service.");
                return Task.CompletedTask;
            }

            _logger.LogInformation($"Binding Events from Input Folder: {InputFolder}");
            _folderWatcher = new FileSystemWatcher(InputFolder, "*.TXT")
            {
                NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName |
                               NotifyFilters.DirectoryName
            };
            _folderWatcher.Created += Input_OnChanged;
            _folderWatcher.EnableRaisingEvents = true;
            _folderWatcher.IncludeSubdirectories = true;

            return base.StartAsync(cancellationToken);
        }

        protected void Input_OnChanged(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created ||
                e.ChangeType == WatcherChangeTypes.Changed ||
                e.ChangeType == WatcherChangeTypes.Renamed)
            {
                _logger.LogInformation($"InBound Change Event Triggered by [{e.FullPath}]");
                WaitForFile(e.FullPath);
                try
                {
                    var processors = new IFileInProcessor[]
                    {
                        IniOpOrdProcessor,
                        IniOpDtlProcessor,
                        IniDrDtlProcessor,
                        IniDrOrdProcessor,
                        CstAgentPatientProcessor,
                        new QuitDataAllProcessor(context) { FileName = e.FullPath },
                        CstAgentPatientProcessor,
                        CstQsDataProcessor,
                        CstQsCureProcessor,
                        CstQsStateProcessor,
                    };
                    foreach (var processor in processors)
                    {
                        try
                        {
                            processor.FileName = e.FullPath;
                            if (processor.IsMatched())
                            {
                                processor.Start();
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message);
                            _logger.LogError(ex.StackTrace);
                        }
                    }
                    _logger.LogInformation("Done with Inbound Change Event");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    _logger.LogError(ex.StackTrace);
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Service");
            _folderWatcher.EnableRaisingEvents = false;
            await base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation("Disposing Service");
            _folderWatcher.Dispose();
            base.Dispose();
        }
        
        private void WaitForFile(string fullPath)
        {
            while (true)
            {
                try
                {
                    using (var stream = new StreamReader(fullPath))
                    {
                        break;
                    }
                }
                catch
                {
                    Thread.Sleep(1000);
                }
            }
        }

    }
}