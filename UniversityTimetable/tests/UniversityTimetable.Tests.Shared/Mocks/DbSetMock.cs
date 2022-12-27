using Microsoft.EntityFrameworkCore;
using MockQueryable.EntityFrameworkCore;
using Moq;
using NSubstitute;

namespace UniversityTimetable.Tests.Shared.Mocks;

public sealed class DbSetMock<T>
    where T : class
{
    public DbSet<T> Object { get; } = Substitute.For<DbSet<T>, IQueryable<T>>();
    public DbSetMock(ICollection<T> source)
    {
        var queryable = source.AsQueryable();

        ((IQueryable)Object).ElementType.Returns(queryable.ElementType);
        ((IQueryable)Object).Expression.Returns(queryable.Expression);
        ((IQueryable)Object).Provider.Returns(new TestAsyncEnumerableEfCore<T>(queryable));

        Object.GetAsyncEnumerator(Arg.Any<CancellationToken>())
            .Returns(new TestAsyncEnumerator<T>(source.GetEnumerator()));
        Object.Add(Arg.Do<T>(source.Add));
        Object.Remove(Arg.Do<T>(model => source.Remove(model)));
    }
}