using System.Linq.Expressions;
using System.Reflection;

namespace UniversityTimetable.Tests.Shared.DataGeneration
{
    public static class FluentArrangment
    {
        public static T Arrange<T, TProperty>(this T item, Expression<Func<T, TProperty>> property, TProperty value)
        {
            var propertyInfo = property.Body as MemberExpression;
            var setter = ((PropertyInfo)propertyInfo.Member).GetSetMethod(true);
            setter.Invoke(item, new object[] { value });
            return item;
        }
    }
}
