using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.SingleItemSelectors;

public class SingleTaskSubmissionSelector : ISingleItemSelector<TaskSubmission>
{
    public async Task<TaskSubmission?> SelectModel(int id, DbSet<TaskSubmission> dataSource, CancellationToken token = default)
    {
        return await dataSource
            .Include(submission => submission.Student)
            .Include(submission => submission.CourseTask)
            .SingleOrDefaultAsync(submission => submission.Id == id, cancellationToken: token);
    }
}