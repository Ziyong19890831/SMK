using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Worker.FileProcess;

namespace SMK.Worker.BackgroundServices
{
    public class CstQsDataBackendService : FileWatchBackendService
    {
        public IConfiguration Configuration { get; set; }

        public CstQsDataBackendService(ILogger<CstQsDataBackendService> logger, 
            IConfiguration configuration, 
            SMKWEBContext context,
            CstQsDataProcessor fileInProcessor) : base(logger, configuration, context)
        {
            Configuration = configuration;
            InputFolder = Configuration["FileWatchService:CstQsDataRootPath"];
            FileInProcessor = fileInProcessor;
        }
    }
}