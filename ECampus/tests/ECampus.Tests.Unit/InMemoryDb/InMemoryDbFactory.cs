using ECampus.Infrastructure;
using ECampus.Tests.Shared.TestDatabase;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.InMemoryDb;

public static class InMemoryDbFactory
{
    private static bool _creatingEnded;
    private static bool _creatingStarted;
    public static async Task<ApplicationDbContext> GetContext()
    {
        var context = new ApplicationDbContext(CreateOptions());
        if (_creatingEnded)
        {
            return context;
        }

        if (!_creatingStarted)
        {
            return await CreateDataAndReturn(context);
        }

        return ReturnWhenDataIsCreated(context);
    }

    private static async Task<ApplicationDbContext> CreateDataAndReturn(ApplicationDbContext context)
    {
        _creatingStarted = true;
        await context.Database.EnsureCreatedAsync();
        _creatingEnded = true;
        return context;
    }

    private static ApplicationDbContext ReturnWhenDataIsCreated(ApplicationDbContext context)
    {
        while (!_creatingEnded)
        {
        }

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