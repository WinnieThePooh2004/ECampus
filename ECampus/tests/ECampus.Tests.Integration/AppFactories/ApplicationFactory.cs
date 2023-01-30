using ECampus.Infrastructure;
using ECampus.Tests.Integration.TestDatabase;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Tests.Integration.AppFactories;

public class ApplicationFactory : WebApplicationFactory<Program>
{
    public static ApplicationDbContext Context =>
        new((DbContextOptions<ApplicationDbContext>)
            new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDb().Options);

    static ApplicationFactory()
    {
        Context.Database.EnsureCreated();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(service =>
                service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDb());
        });
    }
}