using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Worker.FileProcess;

namespace SMK.Worker.BackgroundServices
{
    public class IniDrOrdBackendService : FileWatchBackendService
    {
        public IConfiguration Configuration { get; set; }

        public IniDrOrdBackendService(ILogger<IniDrOrdBackendService> logger, 
            IConfiguration configuration, 
            SMKWEBContext context,
            IniDrOrdProcessor fileInProcessor) : base(logger, configuration, context)
        {
            Configuration = configuration;
            
            InputFolder = Configuration["FileWatchService:IniDrOrdRootPath"];
            FileInProcessor = fileInProcessor;
        }
    }
}