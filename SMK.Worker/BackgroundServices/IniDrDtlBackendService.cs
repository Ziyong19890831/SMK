using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Worker.FileProcess;

namespace SMK.Worker.BackgroundServices
{
    public class IniDrDtlBackendService : FileWatchBackendService
    {
        public IConfiguration Configuration { get; set; }

        public IniDrDtlBackendService(ILogger<IniDrDtlBackendService> logger, 
            IConfiguration configuration, 
            SMKWEBContext context,
            IniDrDtlProcessor fileInProcessor) : base(logger, configuration, context)
        {
            Configuration = configuration;
            
            InputFolder = Configuration["FileWatchService:IniDrDtlRootPath"];
            FileInProcessor = fileInProcessor;
        }
    }
}