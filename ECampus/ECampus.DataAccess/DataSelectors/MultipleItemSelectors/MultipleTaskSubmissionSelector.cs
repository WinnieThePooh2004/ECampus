using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Entities;
using ECampus.Domain.Requests.TaskSubmission;
using ECampus.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class MultipleTaskSubmissionSelector : IParametersSelector<TaskSubmission, TaskSubmissionParameters>
{
    public IQueryable<TaskSubmission> SelectData(ApplicationDbContext context, TaskSubmissionParameters parameters)
        => context.TaskSubmissions.Include(submission => submission.Student)
                .Include(t => t.CourseTask)
                .Where(t => t.CourseTaskId == parameters.CourseTaskId);
}