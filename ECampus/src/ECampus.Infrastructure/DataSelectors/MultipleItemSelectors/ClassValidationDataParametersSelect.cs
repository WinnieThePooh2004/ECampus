using System.Linq.Expressions;
using ECampus.Contracts.DataSelectParameters;
using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Enums;
using ECampus.Shared.Models;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class ClassValidationDataParametersSelect : IMultipleItemSelector<Class, ClassValidationDataParameters>
{
    public IQueryable<Class> SelectData(ApplicationDbContext context, ClassValidationDataParameters parameters)
    {
        return context.Classes.Where(IsOnSameTime(parameters));
    }

    private static Expression<Func<Class, bool>> IsOnSameTime(ClassValidationDataParameters parameters)
    {
        if (parameters.WeekDependency == WeekDependency.None)
        {
            return c => c.Number == parameters.Number && c.DayOfWeek == parameters.DayOfWeek;
        }

        return c => c.Number == parameters.Number && c.DayOfWeek == parameters.DayOfWeek &&
                    c.WeekDependency == parameters.WeekDependency;
    }
}