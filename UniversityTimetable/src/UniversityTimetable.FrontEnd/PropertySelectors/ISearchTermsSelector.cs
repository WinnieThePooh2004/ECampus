using System.Linq.Expressions;

namespace UniversityTimetable.FrontEnd.PropertySelectors;

public interface ISearchTermsSelector<in T>
{
    List<(Expression<Func<string?>>, string placeHolder)> PropertiesExpressions(T item);
}