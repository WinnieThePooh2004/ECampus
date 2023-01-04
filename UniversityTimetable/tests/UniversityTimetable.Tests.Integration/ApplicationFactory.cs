using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UniversityTimetable.Infrastructure;
using UniversityTimetable.Tests.Shared.TestDatabase;

namespace UniversityTimetable.Tests.Integration;

public class ApplicationFactory : WebApplicationFactory<Program>
{
    private readonly ApplicationDbContext _context;

    public ApplicationFactory()
    {
        _context = TestDatabaseFactory.CreateContext();
    }

    public void InitDatabase()
    {
        _context.Database.EnsureCreated();
        _context.SeedData();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(service =>
                services.GetType() == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }
            services.AddSingleton(_context);
        });
    }
}