using ECampus.Infrastructure;
using ECampus.Tests.Shared.TestDatabase;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.InMemoryDb;

public static class InMemoryDbFactory
{
    private static bool _dbCreated;
    public static async Task<ApplicationDbContext> GetContext()
    {
        var context = new ApplicationDbContext(CreateOptions());
        if (_dbCreated)
        {
            return context;
        }

        _dbCreated = true;
        await context.Database.EnsureCreatedAsync();
        await context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys=OFF;");
        return context;
    }

    private static DbContextOptions<ApplicationDbContext> CreateOptions()
    {
        return (DbContextOptions<ApplicationDbContext>)new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDb()
            .EnableSensitiveDataLogging()
            .Options;
    }
}