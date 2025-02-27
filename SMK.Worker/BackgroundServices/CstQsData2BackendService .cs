using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Worker.FileProcess;

namespace SMK.Worker.BackgroundServices
{
    public class CstQsData2BackendService : FileWatchBackendService
    {
        public IConfiguration Configuration { get; set; }

        public CstQsData2BackendService(ILogger<CstQsData2BackendService> logger, 
            IConfiguration configuration, 
            SMKWEBContext context,
            CstQsData2Processor fileInProcessor) : base(logger, configuration, context)
        {
            Configuration = configuration;
            InputFolder = Configuration["FileWatchService:CstQsData2RootPath"];
            FileInProcessor = fileInProcessor;
        }
    }
}