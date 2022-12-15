using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Shared.Extentions
{
    public static class IQueryableExtentions
    {
        public static IQueryable<T> Search<T>(this IQueryable<T> source, Expression<Func<T, string>> propertySelector, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return source;

            var isNotNullExpression = Expression.NotEqual(propertySelector.Body,
                                                          Expression.Constant(null));

            var searchTermExpression = Expression.Constant(searchTerm.Trim().ToLower());
            var toLowerProportyValueExpression = Expression.Call(propertySelector.Body, typeof(string).GetMethod("ToLower", new Type[] { }));
            var checkContainsExpression = Expression.Call(toLowerProportyValueExpression,
                                                          typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),
                                                          searchTermExpression);

            var notNullAndContainsExpression = Expression.AndAlso(isNotNullExpression, checkContainsExpression);

            var methodCallExpression = Expression.Call(typeof(Queryable),
                                                       "Where",
                                                       new Type[] { source.ElementType },
                                                       source.Expression,
                                                       Expression.Lambda<Func<T, bool>>(notNullAndContainsExpression, propertySelector.Parameters));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }

        public static IQueryable<T> Filter<T, TParams>(this IQueryable<T> sourse, TParams parameters)
            where T : class, IModel
            where TParams : IQueryParameters<T>
            => parameters.Filter(sourse);
    }
}
