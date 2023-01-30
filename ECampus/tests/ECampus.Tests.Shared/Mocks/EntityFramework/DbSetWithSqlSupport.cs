using Microsoft.EntityFrameworkCore;
using MockQueryable.EntityFrameworkCore;
using NSubstitute;

namespace ECampus.Tests.Shared.Mocks.EntityFramework;

public class DbSetWithSqlSupport<T> where T : class
{
    private DbSet<T> Object { get; } = Substitute.For<DbSet<T>, IQueryable<T>, IAsyncEnumerable<T>>();
    
    public DbSetWithSqlSupport(ICollection<T> source)
    {
        var queryable = source.AsQueryable();

        ((IQueryable)Object).ElementType.Returns(queryable.ElementType);
        ((IQueryable)Object).Expression.Returns(queryable.Expression);
        ((IQueryable)Object).Provider.Returns(new QueryProviderWithSqlSupport<T>(queryable));
        ((IAsyncEnumerable<T>)Object).GetAsyncEnumerator(Arg.Any<CancellationToken>())
            .Returns(new TestAsyncEnumerator<T>(source.GetEnumerator()));
    }
    
    public DbSetWithSqlSupport(params T[] source)
        : this(source.ToList())
    {
        
    }

    public static implicit operator DbSet<T>(DbSetWithSqlSupport<T> dbSetMock) => dbSetMock.Object;
}