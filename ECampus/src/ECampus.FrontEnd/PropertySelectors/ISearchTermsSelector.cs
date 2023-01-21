using System.Linq.Expressions;

namespace ECampus.FrontEnd.PropertySelectors;

public interface ISearchTermsSelector<in T>
{
    List<(Expression<Func<string?>>, string placeHolder)> PropertiesExpressions(T item);
}