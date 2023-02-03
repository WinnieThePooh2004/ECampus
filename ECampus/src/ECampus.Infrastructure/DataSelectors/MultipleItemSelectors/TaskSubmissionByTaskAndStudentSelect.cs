using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class TaskSubmissionByTaskAndStudentSelect 
    : IMultipleItemSelector<TaskSubmission, TaskSubmissionByStudentAndCourseParameters>
{
    public IQueryable<TaskSubmission> SelectData(ApplicationDbContext context,
        TaskSubmissionByStudentAndCourseParameters parameters) =>
        context.TaskSubmissions.Include(t => t.Student)
            .Include(t => t.CourseTask)
            .Where(t => t.StudentId == parameters.StudentId &&
                        t.CourseTaskId == parameters.CourseTaskId);
}