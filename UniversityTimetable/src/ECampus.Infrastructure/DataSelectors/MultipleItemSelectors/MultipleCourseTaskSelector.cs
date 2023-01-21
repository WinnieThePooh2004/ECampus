using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleCourseTaskSelector : IMultipleItemSelector<CourseTask, CourseTaskParameters>
{
    public IQueryable<CourseTask> SelectData(DbSet<CourseTask> data, CourseTaskParameters parameters)
    {
        return data.Where(c => c.CourseId == parameters.CourseId);
    }
}