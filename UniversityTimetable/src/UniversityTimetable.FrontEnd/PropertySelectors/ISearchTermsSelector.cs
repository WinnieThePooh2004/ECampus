using System.Linq.Expressions;

namespace UniversityTimetable.FrontEnd.PropertySelectors;

public interface ISearchTermsSelector<T>
{
    List<(Expression<Func<string>> propertyExpression, string displayName)> PropertiesExpressions(T item);
}