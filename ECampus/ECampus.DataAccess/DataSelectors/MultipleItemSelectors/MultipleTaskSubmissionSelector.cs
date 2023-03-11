using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;
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