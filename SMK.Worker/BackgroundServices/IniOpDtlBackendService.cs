using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Worker.FileProcess;

namespace SMK.Worker.BackgroundServices
{
    public class IniOpDtlBackendService : FileWatchBackendService
    {
        public IConfiguration Configuration { get; set; }

        public IniOpDtlBackendService(ILogger<IniOpDtlBackendService> logger, 
            IConfiguration configuration, 
            SMKWEBContext context,
            IniOpDtlProcessor fileInProcessor) : base(logger, configuration, context)
        {
            Configuration = configuration;
            
            InputFolder = Configuration["FileWatchService:IniOpDtlRootPath"];
            FileInProcessor = fileInProcessor;
        }
    }
}