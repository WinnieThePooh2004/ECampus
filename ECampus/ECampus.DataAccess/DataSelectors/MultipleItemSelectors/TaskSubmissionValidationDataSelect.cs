using ECampus.Contracts.DataSelectParameters;
using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Models;
using ECampus.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class TaskSubmissionValidationDataSelect : IParametersSelector<TaskSubmission, TaskSubmissionValidationParameters>
{
    public IQueryable<TaskSubmission> SelectData(ApplicationDbContext context, TaskSubmissionValidationParameters parameters)
    {
        IQueryable<TaskSubmission> source = context.TaskSubmissions;
        if (parameters.IncludeCourseTask)
        {
            source = source
                .Include(submission => submission.CourseTask);
        }

        return source.Where(submission => submission.Id == parameters.TaskSubmissionId);
    }
}