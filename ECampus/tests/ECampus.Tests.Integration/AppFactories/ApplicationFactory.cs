using Amazon.SimpleNotificationService;
using ECampus.Api;
using ECampus.Core.Extensions;
using ECampus.Infrastructure;
using ECampus.Shared.Models;
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
        using var context = Context;
        context.Database.EnsureCreated();
        //this is used to provide valid FK for create another values, please, do not try to update them, all other 
        //entities` ids must be at least 100 and add 100 for each class uses this  factory, current value is:
        //100
        context.Add(new Faculty { Id = 1, Name = "f1Name" });
        context.Add(new Department { Id = 1, FacultyId = 1, Name = "d1Name" });
        context.Add(new Subject { Id = 1 });
        context.SaveChangesAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.UserInstallersFromAssemblyContaining<ApplicationFactory>(Substitute.For<IConfiguration>());
        });
    }
}