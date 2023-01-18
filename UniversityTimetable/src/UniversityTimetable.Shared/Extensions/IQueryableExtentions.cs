using System.Diagnostics;
using System.Linq.Expressions;
using UniversityTimetable.Shared.Enums;

namespace UniversityTimetable.Shared.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> Search<T>(this IQueryable<T> source, Expression<Func<T, string>> propertySelector,
        string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return source;
        }

        var isNotNullExpression = Expression.NotEqual(propertySelector.Body,
            Expression.Constant(null));

        var searchTermExpression = Expression.Constant(searchTerm.Trim().ToLower());
        var toLowerPropertyValueExpression = Expression.Call(propertySelector.Body,
            typeof(string).GetMethod("ToLower", new Type[] { })!);
        var checkContainsExpression = Expression.Call(toLowerPropertyValueExpression,
            typeof(string).GetMethod("Contains", new[] { typeof(string) }) ??
            throw new UnreachableException(),
            searchTermExpression);

        var notNullAndContainsExpression = Expression.AndAlso(isNotNullExpression, checkContainsExpression);

        var methodCallExpression = Expression.Call(typeof(Queryable),
            "Where",
            new[] { source.ElementType },
            source.Expression,
            Expression.Lambda<Func<T, bool>>(notNullAndContainsExpression, propertySelector.Parameters));

        return source.Provider.CreateQuery<T>(methodCallExpression);
    }

    public static IQueryable<T> Sort<T>(this IQueryable<T> source, string? propertyName, SortOrder sortOrder)
    {
        if (propertyName is null)
        {
            return source;
        }
        var parameter = Expression.Parameter(typeof(T), "parameter");
        var propertyAccess = Expression.Property(parameter, propertyName);
        var sortLambda = Expression.Lambda<Func<T, object>>(propertyAccess, parameter);
        return sortOrder switch
        {
            SortOrder.Ascending => source.OrderBy(sortLambda),
            SortOrder.Descending => source.OrderByDescending(sortLambda),
            _ => throw new UnreachableException($"{sortOrder} is out of enum {typeof(SortOrder)}",
                new ArgumentOutOfRangeException(nameof(sortOrder)))
        };
    }
}