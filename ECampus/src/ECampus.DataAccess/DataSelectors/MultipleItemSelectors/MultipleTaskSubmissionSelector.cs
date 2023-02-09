using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class MultipleTaskSubmissionSelector : IMultipleItemSelector<TaskSubmission, TaskSubmissionParameters>
{
    public IQueryable<TaskSubmission> SelectData(ApplicationDbContext context, TaskSubmissionParameters parameters)
        => context.TaskSubmissions.Include(submission => submission.Student)
                .Include(t => t.CourseTask)
                .Where(t => t.CourseTaskId == parameters.CourseTaskId);
}