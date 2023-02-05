using Microsoft.EntityFrameworkCore;
using MockQueryable.EntityFrameworkCore;
using NSubstitute;

namespace ECampus.Tests.Shared.Mocks.EntityFramework;

public sealed class DbSetMock<T>
    where T : class
{
    public DbSet<T> Object { get; } = Substitute.For<DbSet<T>, IQueryable<T>, IAsyncEnumerable<T>>();
    public DbSetMock(ICollection<T> source)
    {
        var queryable = source.AsQueryable();

        ((IQueryable)Object).ElementType.Returns(queryable.ElementType);
        ((IQueryable)Object).Expression.Returns(queryable.Expression);
        ((IQueryable)Object).Provider.Returns(new TestAsyncEnumerableEfCore<T>(queryable));
        ((IAsyncEnumerable<T>)Object).GetAsyncEnumerator(Arg.Any<CancellationToken>())
            .Returns(new TestAsyncEnumerator<T>(source.GetEnumerator()));
    }

    public DbSetMock()
        : this(new List<T>())
    {
        
    }

    public DbSetMock(params T[] source)
        : this(source.ToList())
    {
        
    }

    public static implicit operator DbSet<T>(DbSetMock<T> dbSetMock) => dbSetMock.Object;
}