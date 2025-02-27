using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using SMK.Data;
using SMK.Worker.FileProcess;
using SMK.Worker.Services;

namespace SMK.Worker.BackgroundServices
{
    public abstract class FileWatchBackendService : BackgroundService
    {
        private readonly ILogger<FileWatchBackendService> _logger;
        private FileSystemWatcher _folderWatcher;
        // protected virtual string InputFolder { get; }
        private readonly SMKWEBContext context;
        // public virtual IFileInProcessor FileInProcessor { get; set; }
        
        public string InputFolder { get; set; }
        public IFileInProcessor FileInProcessor { get; set; }

        public FileWatchBackendService(ILogger<FileWatchBackendService> logger,
            IConfiguration configuration,
            SMKWEBContext context)
        {
            _logger = logger;
            this.context = context;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Name} Starting", GetType().ShortDisplayName());
            var inputFolder = InputFolder;
            if (!Directory.Exists(inputFolder))
            {
                _logger.LogWarning(
                    $"Please make sure the InputFolder [{inputFolder}] exists, then restart the service.");
                return Task.CompletedTask;
            }

            _logger.LogInformation($"Binding Events from Input Folder: {inputFolder}");
            _folderWatcher = new FileSystemWatcher(inputFolder, "*.TXT")
            {
                NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName |
                               NotifyFilters.DirectoryName | NotifyFilters.LastAccess
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
                    try
                    {
                        FileInProcessor.FileName = e.FullPath;
                        if (FileInProcessor.IsMatched())
                        {
                            FileInProcessor.Start();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        _logger.LogError(ex.StackTrace);
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