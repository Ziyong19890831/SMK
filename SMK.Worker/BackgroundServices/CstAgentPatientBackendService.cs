using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Worker.FileProcess;

namespace SMK.Worker.BackgroundServices
{
    public class CstAgentPatientService : FileWatchBackendService
    {
        public IConfiguration Configuration { get; set; }

        public CstAgentPatientService(ILogger<CstAgentPatientService> logger, 
            IConfiguration configuration, 
            SMKWEBContext context,
            CstAgentPatientProcessor fileInProcessor) : base(logger, configuration, context)
        {
            Configuration = configuration;
            InputFolder = Configuration["FileWatchService:CstAgentPatientRootPath"];
            FileInProcessor = fileInProcessor;
        }
    }
}