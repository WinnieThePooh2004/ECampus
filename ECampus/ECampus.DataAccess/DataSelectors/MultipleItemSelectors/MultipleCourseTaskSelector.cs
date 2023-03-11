using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class MultipleCourseTaskSelector : IParametersSelector<CourseTask, CourseTaskParameters>
{
    public IQueryable<CourseTask> SelectData(ApplicationDbContext context, CourseTaskParameters parameters) =>
        context.CourseTasks.Where(c => c.CourseId == parameters.CourseId);
}