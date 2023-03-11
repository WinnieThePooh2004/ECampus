using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Entities;
using ECampus.Domain.QueryParameters;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class MultipleCourseTaskSelector : IParametersSelector<CourseTask, CourseTaskParameters>
{
    public IQueryable<CourseTask> SelectData(ApplicationDbContext context, CourseTaskParameters parameters) =>
        context.CourseTasks.Where(c => c.CourseId == parameters.CourseId);
}