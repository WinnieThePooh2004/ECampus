using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;

namespace ECampus.Infrastructure.Repositories;

[Inject(typeof(ITaskSubmissionRepository))]
public class TaskSubmissionRepository : ITaskSubmissionRepository
{
    private readonly ApplicationDbContext _context;

    public TaskSubmissionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task UpdateContent(int submissionId, string content)
    {
        var submission = await _context.TaskSubmissions.FindAsync(submissionId) ??
                         throw new ObjectNotFoundByIdException(typeof(TaskSubmission), submissionId);

        submission.SubmissionContent = content;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateMark(int submissionId, int mark)
    {
        var submission = await _context.TaskSubmissions.FindAsync(submissionId) ??
                         throw new ObjectNotFoundByIdException(typeof(TaskSubmission), submissionId);

        submission.TotalPoints = mark;
        await _context.SaveChangesAsync();
    }
}