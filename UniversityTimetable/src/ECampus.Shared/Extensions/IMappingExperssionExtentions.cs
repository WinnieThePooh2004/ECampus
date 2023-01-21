using System.Linq.Expressions;
using AutoMapper;

namespace ECampus.Shared.Extensions
{
    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<T, TTo> IgnoreMember<T, TTo, TMember>(this IMappingExpression<T, TTo> expression,
            Expression<Func<TTo, TMember>> member)
            => expression.ForMember(member, opt => opt.Ignore());
    }
}
