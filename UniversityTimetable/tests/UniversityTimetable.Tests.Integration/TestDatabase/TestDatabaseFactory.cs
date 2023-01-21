using Microsoft.EntityFrameworkCore;

namespace UniversityTimetable.Tests.Integration.TestDatabase;

public static class TestDatabaseFactory
{
    public static DbContextOptionsBuilder UseInMemoryDb(this DbContextOptionsBuilder optionsBuilder)
    {
        return optionsBuilder.UseSqlite("Data Source=:memory:");
    }
}