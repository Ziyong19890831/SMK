using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SMK.Data;
using Microsoft.Extensions.Logging;

namespace SMK.Web.AppScope
{
    public static class DbContextRegister
    {

        public static IServiceCollection RegisterDb(this IServiceCollection services,string connectionString)
        {
            services.AddDbContextPool<SMKWEBContext>(
                options =>
                {
                    options.UseSqlServer(connectionString);
#if DEBUG
                    options.UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }));
                    options.EnableSensitiveDataLogging();
#endif
                });

            return services;
        }
    }
}
