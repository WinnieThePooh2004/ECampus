using System.Linq.Expressions;
using System.Reflection;
using UniversityTimetable.Shared.Metadata;

namespace UniversityTimetable.FrontEnd.PropertySelectors;

public class SearchTermsSelector<T> : ISearchTermsSelector<T>
{
    private readonly List<(PropertyInfo property, string displayName)> _searchProperties =
        typeof(T).GetProperties().Where(property => property.PropertyType == typeof(string) && !property.Name.Contains("OrderBy")).Select(property => (
            property,
            property.GetCustomAttributes(false).OfType<DisplayNameAttribute>().SingleOrDefault()?.Name ??
            property.Name)).ToList();

    public List<(Expression<Func<string>> propertyExpression, string displayName)> PropertiesExpressions(T item)
    {
        return _searchProperties.Select(property => (
            Expression.Lambda<Func<string>>(Expression.MakeMemberAccess(Expression.Constant(item), property.property)),
            property.displayName)).ToList();
    }
}