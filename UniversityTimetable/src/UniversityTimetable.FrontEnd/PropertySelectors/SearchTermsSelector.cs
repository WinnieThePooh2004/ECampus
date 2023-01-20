using System.Linq.Expressions;
using System.Reflection;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Metadata;

namespace UniversityTimetable.FrontEnd.PropertySelectors;

public class SearchTermsSelector<T> : ISearchTermsSelector<T>
{
    private readonly List<(PropertyInfo property, string displayName)> _searchProperties = typeof(T).GetProperties()
        .Where(property => property.PropertyType == typeof(string) && !property.Name.Contains("OrderBy") &&
                           !property.GetCustomAttributes().OfType<NotDisplayAttribute>().Any())
        .OrderBy(property => property.DisplayOrder())
        .Select(property => (property, property.DisplayName())).ToList();

    public List<(Expression<Func<string?>>, string placeHolder)> PropertiesExpressions(T item)
    {
        return _searchProperties.Select(property => (
            Expression.Lambda<Func<string?>>(Expression.MakeMemberAccess(Expression.Constant(item), property.property)),
            property.displayName)).ToList();
    }
}