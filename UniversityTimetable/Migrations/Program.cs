using Microsoft.EntityFrameworkCore;

namespace Migrations
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dbBuilder = new ApplicationContextFactory();
            using var context = dbBuilder.CreateDbContext(args);
            context.Database.Migrate();
        }
    }
}
