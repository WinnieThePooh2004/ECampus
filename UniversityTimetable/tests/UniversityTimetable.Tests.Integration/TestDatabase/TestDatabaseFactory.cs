using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Infrastructure;

namespace UniversityTimetable.Tests.Integration.TestDatabase;

public static class TestDatabaseFactory
{
    public static DbContextOptionsBuilder UseInMemoryDb(this DbContextOptionsBuilder optionsBuilder)
    {
        return optionsBuilder.UseInMemoryDatabase("DataBase");
    }
}