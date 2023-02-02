using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Extensions;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleStudentSelector : IMultipleItemSelector<Student, StudentParameters>
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