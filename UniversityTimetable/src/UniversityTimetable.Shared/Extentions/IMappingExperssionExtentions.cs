using AutoMapper;
using System.Linq.Expressions;

namespace UniversityTimetable.Shared.Extentions
{
    public static class IMappingExperssionExtentions
    {
        public static IMappingExpression<T, TTo> IgnoreMember<T, TTo, TMember>(this IMappingExpression<T, TTo> expression,
            Expression<Func<TTo, TMember>> member)
            => expression.ForMember(member, opt => opt.Ignore());

        public static IMappingExpression<T, TTo> UseAsValue<T, TTo, TMember>(this IMappingExpression<T, TTo> expression,
            Expression<Func<TTo, TMember>> member, TMember value)
            => expression.ForMember(member, opt => opt.MapFrom(c => value));
    }
}
