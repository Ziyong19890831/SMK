using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Worker.FileProcess;

namespace SMK.Worker.BackgroundServices
{
    public class QuitDataAllBackendService : FileWatchBackendService
    {
        public IConfiguration Configuration { get; set; }

        public QuitDataAllBackendService(ILogger<QuitDataAllBackendService> logger,
            IConfiguration configuration,
            SMKWEBContext context,
            QuitDataAllProcessor fileInProcessor) : base(logger, configuration, context)
        {
            Configuration = configuration;
            InputFolder = Configuration["FileWatchService:QuitDataAllRootPath"];
            FileInProcessor = fileInProcessor;
        }
    }
}
