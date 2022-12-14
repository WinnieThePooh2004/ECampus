using AutoMapper;
using System.Linq.Expressions;

namespace UniversityTimetable.Shared.Extentions
{
    public static class IMappingExperssionExtentions
    {
        public static IMappingExpression<T, TTo> IgnoreProperties<T, TTo>(this IMappingExpression<T, TTo> expression
            , params Expression<Func<TTo, object>>[] properties)
        {
            foreach (var property in properties)
            {
                expression = expression.ForMember(property, opt => opt.Ignore());
            }
            return expression;
        }
    }
}
