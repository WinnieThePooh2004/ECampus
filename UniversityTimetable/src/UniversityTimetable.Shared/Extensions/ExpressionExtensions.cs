using System.Linq.Expressions;
using System.Reflection;

namespace UniversityTimetable.Shared.Extensions;

public static class ExpressionExtensions
{
    public static void SetFromExpression<TSelf, TProperty>(this Expression<Func<TSelf, TProperty>> expression,
        TSelf self, TProperty value)
    {
        GetPropertyInfo(expression).SetMethod?.Invoke(self, new object?[] { value });
    }

    public static TProperty GetFromExpression<TSelf, TProperty>(this Expression<Func<TSelf, TProperty>> expression,
        TSelf self)
    {
        var property = GetPropertyInfo(expression) ??
                       throw new Exception($"Cannot find member access in expression {expression}");
        var getter = property.GetMethod ?? throw new Exception($"Property {property.Name} does not have getter");
        return (TProperty)getter.Invoke(self, null)!;
    }

    public static Expression<Func<TProperty>> SetLambdaParameter<TSelf, TProperty>(
        this Expression<Func<TSelf, TProperty>> expression, TSelf self)
    {
        return Expression.Lambda<Func<TProperty>>(Expression.MakeMemberAccess(Expression.Constant(self),
            ((MemberExpression)expression.Body).Member));
    }

    private static PropertyInfo GetPropertyInfo<TSelf, TProperty>(Expression<Func<TSelf, TProperty>> expression)
    {
        var memberExpression = (MemberExpression)expression.Body;
        if (memberExpression is null)
        {
            throw new Exception($"Cannot get member expression from expression {expression}");
        }

        var property = (PropertyInfo)memberExpression.Member;
        return property;
    }
}