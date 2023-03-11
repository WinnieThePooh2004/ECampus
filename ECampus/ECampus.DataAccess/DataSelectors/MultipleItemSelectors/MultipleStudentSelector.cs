using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Extensions;
using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class MultipleStudentSelector : IParametersSelector<Student, StudentParameters>
{
    public IQueryable<Student> SelectData(ApplicationDbContext context, StudentParameters parameters)
    {
        var result = context.Students.Search(s => s.FirstName, parameters.FirstName)
            .Search(s => s.LastName, parameters.LastName);
        if (parameters.GroupId != 0)
        {
            result = result.Where(s => s.GroupId == parameters.GroupId);
        }

        if (!parameters.UserIdCanBeNull)
        {
            result = result.Where(t => t.UserEmail == null);
        }

        return result;
    }
}