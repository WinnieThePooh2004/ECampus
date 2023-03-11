using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Models;
using ECampus.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class TaskSubmissionByTaskAndStudentSelect 
    : IParametersSelector<TaskSubmission, TaskSubmissionByStudentAndCourseParameters>
{
    public IQueryable<TaskSubmission> SelectData(ApplicationDbContext context,
        TaskSubmissionByStudentAndCourseParameters parameters) =>
        context.TaskSubmissions.Include(t => t.Student)
            .Include(t => t.CourseTask)
            .Where(t => t.StudentId == parameters.StudentId &&
                        t.CourseTaskId == parameters.CourseTaskId);
}