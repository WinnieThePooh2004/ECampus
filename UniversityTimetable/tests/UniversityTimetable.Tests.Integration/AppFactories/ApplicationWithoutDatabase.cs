using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using UniversityTimetable.Infrastructure;

namespace UniversityTimetable.Tests.Integration.AppFactories;

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
            var descriptor = services.SingleOrDefault(_ =>
                services.GetType() == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }
            services.AddSingleton(Context);
        });
    }
}