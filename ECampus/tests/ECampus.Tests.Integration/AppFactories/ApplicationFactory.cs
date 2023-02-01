using Amazon.SimpleNotificationService;
using ECampus.Api;
using ECampus.Core.Extensions;
using ECampus.Infrastructure;
using ECampus.Tests.Shared.TestDatabase;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace ECampus.Tests.Integration.AppFactories;

public class ApplicationFactory : WebApplicationFactory<Program>
{
    public static ApplicationDbContext Context =>
        new((DbContextOptions<ApplicationDbContext>)
            new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDb().Options);

    public static IAmazonSimpleNotificationService AmazonSnsMock { get; set; } = default!;

    static ApplicationFactory()
    {
        Context.Database.EnsureCreated();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.UserInstallersFromAssemblyContaining<ApplicationFactory>(Substitute.For<IConfiguration>());
        });
    }
}