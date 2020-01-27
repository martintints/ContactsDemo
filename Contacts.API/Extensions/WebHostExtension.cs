using Contacts.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Contacts.API.Extensions
{
    public static class WebHostExtension
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
                {
                    dbContext.Database.Migrate();
                }
            }
            return host;
        }

        public static IHostBuilder AddLogging(this IHostBuilder host)
        {

            return host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration)
                .Enrich.FromLogContext()
            );
        }
    }
}
