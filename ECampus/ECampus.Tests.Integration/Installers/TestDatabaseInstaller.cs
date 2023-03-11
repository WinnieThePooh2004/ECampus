using ECampus.Core.Installers;
using ECampus.Infrastructure;
using ECampus.Tests.Shared.TestDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Tests.Integration.Installers;

public class TestDatabaseInstaller : IInstaller
{
    public int InstallOrder => -1;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var descriptor = services.SingleOrDefault(service =>
            service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
        if (descriptor is not null)
        {
            services.Remove(descriptor);
        }

        services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDb());
    }
}