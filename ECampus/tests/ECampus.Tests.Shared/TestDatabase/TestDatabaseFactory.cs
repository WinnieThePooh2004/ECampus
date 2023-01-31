using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Shared.TestDatabase;

public static class TestDatabaseFactory
{
    public static DbContextOptionsBuilder UseInMemoryDb(this DbContextOptionsBuilder optionsBuilder)
    {
        return optionsBuilder.UseSqlite("DataSource=file::memory:?cache=shared");
    }
}