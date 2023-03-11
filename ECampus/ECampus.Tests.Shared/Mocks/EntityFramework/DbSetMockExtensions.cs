using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Shared.Mocks.EntityFramework;

public static class DbSetMockExtensions
{
    public static IQueryable<T> ToAsyncQueryable<T>(this T source) where T : class
    {
        return new DbSetMock<T>(source).Object;
    }

    public static DbSet<T> AsDbSet<T>(this ICollection<T> source) where T : class
    {
        return new DbSetMock<T>(source).Object;
    }
}