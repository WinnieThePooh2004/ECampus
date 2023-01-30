using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using MockQueryable.EntityFrameworkCore;

namespace ECampus.Tests.Shared.Mocks.EntityFramework;

public class QueryProviderWithSqlSupport<T> : IAsyncQueryProvider
{
    private TestAsyncEnumerableEfCore<T> Provider { get; }

    public QueryProviderWithSqlSupport(IEnumerable<T> dataSource)
    {
        Provider = new TestAsyncEnumerableEfCore<T>(dataSource);
    }

    public IQueryable CreateQuery(Expression expression)
    {
        return Provider.CreateQuery(expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return Provider.CreateQuery<TElement>(expression);
    }

    public object Execute(Expression expression)
    {
        return default!;
    }

    public TResult Execute<TResult>(Expression expression)
    {
        return default!;
    }

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = new())
    {
        return default!;
    }
}