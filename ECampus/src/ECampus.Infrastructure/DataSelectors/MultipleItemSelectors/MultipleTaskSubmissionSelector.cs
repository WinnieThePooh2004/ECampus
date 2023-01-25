using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleTaskSubmissionSelector : IMultipleItemSelector<TaskSubmission, TaskSubmissionParameters>
{
    public IQueryable<TaskSubmission> SelectData(DbSet<TaskSubmission> data, TaskSubmissionParameters parameters) =>
        parameters.StudentId switch
        {
            0 => data.Where(t => t.CourseTaskId == parameters.CourseTaskId),
            _ => data.Where(t => t.CourseTaskId == parameters.CourseTaskId && t.StudentId == parameters.StudentId)
        };
}