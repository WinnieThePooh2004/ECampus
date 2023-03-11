using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Entities;
using ECampus.Domain.Extensions;
using ECampus.Domain.QueryParameters;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class MultipleTeacherSelector : IParametersSelector<Teacher, TeacherParameters>
{
    public IQueryable<Teacher> SelectData(ApplicationDbContext context, TeacherParameters parameters)
    {
        var result = context.Teachers.Search(s => s.FirstName, parameters.FirstName)
            .Search(s => s.LastName, parameters.LastName);
        if (parameters.DepartmentId != 0)
        {
            result = result.Where(s => s.DepartmentId == parameters.DepartmentId);
        }

        if (!parameters.UserIdCanBeNull)
        {
            result = result.Where(t => t.UserEmail == null);
        }

        return result;
    }
}