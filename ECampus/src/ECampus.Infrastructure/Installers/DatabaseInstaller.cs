using ECampus.Core.Installers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Infrastructure.Installers;

public class DatabaseInstaller : IInstaller
{
    public int InstallOrder => -1;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(ApplicationDbContext.ConnectionKey),
                builder => builder.MigrationsAssembly("ECampus.Infrastructure")));
        services.AddHostedService<MigrationsService>();
        services.AddScoped<DbContext, ApplicationDbContext>();
    }
}