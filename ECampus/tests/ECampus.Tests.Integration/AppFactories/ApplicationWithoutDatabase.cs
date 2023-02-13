using ECampus.Api;
using ECampus.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ILogger = Serilog.ILogger;

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
        builder.ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders());
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(service =>
                service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            var loggerDescriptor = services.Single(serviceDescriptor =>
                serviceDescriptor.ServiceType == typeof(ILogger));
            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.Remove(loggerDescriptor);
            services.AddSingleton(Substitute.For<ILogger>());
            services.AddSingleton(Context);
            var backGroundDescriptors = services
                .Where(serviceDescriptor => serviceDescriptor.ServiceType == typeof(IHostedService)).ToList();
            foreach (var backGroundDescriptor in backGroundDescriptors)
            {
                services.Remove(backGroundDescriptor);
            }
        });
    }
}