using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleCourseTaskSelector : IMultipleItemSelector<CourseTask, CourseTaskParameters>
{
    public IQueryable<CourseTask> SelectData(ApplicationDbContext context, CourseTaskParameters parameters) =>
        context.CourseTasks.Where(c => c.CourseId == parameters.CourseId);
}