using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ECampus.Tests.Shared.TestDatabase;

public static class TestDatabaseFactory
{
    public static DbContextOptionsBuilder UseInMemoryDb(this DbContextOptionsBuilder optionsBuilder)
    {
        return optionsBuilder.UseSqlite("DataSource=file::memory:?cache=shared");
    }

    public static DbContextOptionsBuilder UseInMemoryDb(this DbContextOptionsBuilder optionsBuilder,
        Action<SqliteDbContextOptionsBuilder> optionsBuilderAction)
    {
        return optionsBuilder.UseSqlite("DataSource=file::memory:?cache=shared", optionsBuilderAction);
    }
}