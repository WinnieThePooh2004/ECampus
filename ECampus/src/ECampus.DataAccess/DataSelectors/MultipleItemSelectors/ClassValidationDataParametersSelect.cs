using System.Linq.Expressions;
using ECampus.Contracts.DataSelectParameters;
using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Enums;
using ECampus.Shared.Models;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class ClassValidationDataParametersSelect : IParametersSelector<Class, ClassValidationDataParameters>
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