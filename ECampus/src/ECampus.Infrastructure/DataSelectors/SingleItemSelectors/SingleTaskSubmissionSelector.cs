using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.SingleItemSelectors;

public class SingleTaskSubmissionSelector : ISingleItemSelector<TaskSubmission>
{
    public async Task<TaskSubmission?> SelectModel(int id, DbSet<TaskSubmission> dataSource)
    {
        return await dataSource
            .Include(submission => submission.Student)
            .Include(submission => submission.CourseTask)
            .SingleOrDefaultAsync(submission => submission.Id == id);
    }
}