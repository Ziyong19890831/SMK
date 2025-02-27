using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SMK.Data;

namespace SMK.Worker.AppScope
{
    public static class DbContextRegister
    {
        public static IServiceCollection RegisterDb(this IServiceCollection services,string connectionString)
        {
            services.AddTransient(provider =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<SMKWEBContext>();
                optionsBuilder.UseSqlServer(connectionString);
                // SqlMapper.Settings.CommandTimeout = 600; // 2 min
                return new SMKWEBContext(optionsBuilder.Options);
            });


            return services;
        }
    }
}
