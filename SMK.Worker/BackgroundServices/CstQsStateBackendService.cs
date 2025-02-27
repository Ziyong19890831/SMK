using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Worker.FileProcess;

namespace SMK.Worker.BackgroundServices
{
    public class CstQsStateBackendService : FileWatchBackendService
    {
        public IConfiguration Configuration { get; set; }

        public CstQsStateBackendService(ILogger<CstQsStateBackendService> logger,
            IConfiguration configuration,
            SMKWEBContext context,
            CstQsStateProcessor fileInProcessor) : base(logger, configuration, context)
        {
            Configuration = configuration;
            InputFolder = Configuration["FileWatchService:CstQsStateRootPath"];
            FileInProcessor = fileInProcessor;
        }
    }
}