using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Infrastructure;

namespace UniversityTimetable.Tests.Shared.TestDatabase
{
    public static class TestDatabaseFactory
    {
        public static ApplicationDbContext CreateContext()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;
            return new ApplicationDbContext(options);
        }
    }
}
