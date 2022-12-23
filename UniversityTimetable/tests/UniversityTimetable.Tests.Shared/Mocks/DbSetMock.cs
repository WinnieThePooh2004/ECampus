using Microsoft.EntityFrameworkCore;
using MockQueryable.EntityFrameworkCore;
using Moq;

namespace UniversityTimetable.Tests.Shared.Mocks;

public sealed class DbSetMock<T> : Mock<DbSet<T>>
    where T : class
{
    public DbSetMock(ICollection<T> source)
    {
        var queryable = source.AsQueryable();

        As<IQueryable>()
            .Setup(q => q.Expression)
            .Returns(queryable.Expression);

        As<IQueryable>()
            .Setup(q => q.ElementType)
            .Returns(queryable.ElementType);
        
        As<IQueryable<T>>()
            .Setup(m => m.Provider)
            .Returns(new TestAsyncEnumerableEfCore<T>(queryable));
        
        As<IAsyncEnumerable<T>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<T>(source.GetEnumerator()));
        Setup(m => m.Remove(It.IsAny<T>())).Callback<T>(entity => source.Remove(entity));
        Setup(m => m.Add(It.IsAny<T>())).Callback<T>(source.Add);
    }
}