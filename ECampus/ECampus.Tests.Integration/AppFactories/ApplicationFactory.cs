using Amazon.SimpleNotificationService;
using ECampus.Host;
using ECampus.Core.Extensions;
using ECampus.Domain.Entities;
using ECampus.Infrastructure;
using ECampus.Tests.Shared.TestDatabase;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ILogger = Serilog.ILogger;

namespace ECampus.Tests.Integration.AppFactories;

/// <summary>
/// current id is 800, next group of endpoints should use object with ids 900+
/// </summary>
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
        //500
        context.Add(new Faculty { Id = 1, Name = "f1Name" });
        context.Add(new Department { Id = 1, FacultyId = 1, Name = "d1Name" });
        context.Add(new Subject { Id = 1 });
        context.Add(new Course { Id = 1, Name = "c1Name", SubjectId = 1 });
        context.Add(new Group { Id = 1, DepartmentId = 1, Name = "g1Name" });
        context.SaveChangesAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders());
        builder.ConfigureServices(services =>
        {
            services.UserInstallersFromAssemblyContaining<ApplicationFactory>(Substitute.For<IConfiguration>());
            var loggerDescriptor = services.Single(serviceDescriptor =>
                serviceDescriptor.ServiceType == typeof(ILogger));
            services.Remove(loggerDescriptor);
            services.AddSingleton(Substitute.For<ILogger>());
        });
    }
}