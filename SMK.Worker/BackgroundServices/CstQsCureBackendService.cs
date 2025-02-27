using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Worker.FileProcess;

namespace SMK.Worker.BackgroundServices
{
    public class CstQsCureService : FileWatchBackendService
    {
        public IConfiguration Configuration { get; set; }

        public CstQsCureService(ILogger<CstQsCureService> logger, 
            IConfiguration configuration, 
            SMKWEBContext context,
            CstQsCureProcessor fileInProcessor) : base(logger, configuration, context)
        {
            Configuration = configuration;
            InputFolder = Configuration["FileWatchService:CstQsCureRootPath"];
            FileInProcessor = fileInProcessor;
        }
    }
}