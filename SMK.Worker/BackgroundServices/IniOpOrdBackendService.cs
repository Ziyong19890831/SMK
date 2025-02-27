using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Worker.FileProcess;

namespace SMK.Worker.BackgroundServices
{
    public class IniOpOrdBackendService : FileWatchBackendService
    {
        public IConfiguration Configuration { get; set; }

        public IniOpOrdBackendService(ILogger<IniOpOrdBackendService> logger, 
            IConfiguration configuration, 
            SMKWEBContext context,
            IniOpOrdProcessor fileInProcessor) : base(logger, configuration, context)
        {
            Configuration = configuration;
            
            InputFolder = Configuration["FileWatchService:IniOpOrdRootPath"];
            FileInProcessor = fileInProcessor;
        }
    }
}