using System.Linq.Expressions;
using System.Reflection;

namespace ECampus.Domain.Extensions;

public static class ExpressionExtensions
{
    public static void SetFromExpression<TProperty>(this Expression<Func<TProperty>> expression, TProperty value)
    {
        var memberExpression = (MemberExpression)expression.Body;
        var property = (PropertyInfo)memberExpression.Member;
        var expressionParameter = (ConstantExpression?)memberExpression.Expression ?? throw new Exception();
        property.SetMethod?.Invoke(expressionParameter.Value, new object?[] { value });
        Console.WriteLine();
    }
}