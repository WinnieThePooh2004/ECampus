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

        //this very strange construction needed to ensure that context won`t be returned until its creation has ended
        //really, don`t try to touch it, 1 of 10 cases some tests can fall 
        if (!_creatingStarted)
        {
            _creatingStarted = true;
            await context.Database.EnsureCreatedAsync();
            _creatingEnded = true;
        }
        else
        {
            while (!_creatingEnded)
            {
                
            }
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