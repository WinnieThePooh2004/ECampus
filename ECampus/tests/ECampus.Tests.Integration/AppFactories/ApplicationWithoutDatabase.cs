using ECampus.Api;
using ECampus.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace ECampus.Tests.Integration.AppFactories;

public class ApplicationWithoutDatabase : WebApplicationFactory<Program>
{
    public ApplicationDbContext Context { get; }

    public ApplicationWithoutDatabase()
    {
        Context = Substitute.For<ApplicationDbContext>();
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
            services.AddSingleton(Context);
        });
    }
}