using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.ValidationDataAccess;

[Inject(typeof(ITaskSubmissionDataValidator))]
public class TaskSubmissionDataValidator : ITaskSubmissionDataValidator
{
    private readonly ApplicationDbContext _context;

    public TaskSubmissionDataValidator(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TaskSubmission> LoadSubmissionData(int taskSubmissionId)
    {
        return await _context.TaskSubmissions.FindAsync(taskSubmissionId) ??
               throw new ObjectNotFoundByIdException(typeof(TaskSubmission), taskSubmissionId);
    }

    public async Task<ValidationResult> ValidateTeacherId(int teacherId, int taskSubmissionId)
    {
        var result = new ValidationResult();
        var submissionAuthorGroup = await FindSubmissionAuthorGroup(taskSubmissionId);

        if (submissionAuthorGroup is null)
        {
            result.AddError(new ValidationError(nameof(taskSubmissionId),
                $"Cannot find submission with id = {taskSubmissionId}"));
            return result;
        }

        await ValidateTeacherTeachesAuthorsGroup(teacherId, submissionAuthorGroup.Id, result);

        return result;
    }

    private async Task<Group?> FindSubmissionAuthorGroup(int taskSubmissionId)
    {
        var submissionAuthorGroup = await _context.Groups
            .Include(g => g.Students)!
            .ThenInclude(s => s.Submissions)
            .SingleOrDefaultAsync(g =>
                g.Students!.Any(s => s.Submissions!.Any(submission => submission.Id == taskSubmissionId)));
        return submissionAuthorGroup;
    }

    private async Task ValidateTeacherTeachesAuthorsGroup(int teacherId, int submissionAuthorGroupId,
        ValidationResult result)
    {
        if (!await _context.Teachers
                .Include(t => t.Courses)!
                .ThenInclude(c => c.Groups)
                .AnyAsync(t => t.Courses!.Any(c => c.Groups!.Any(g => g.Id == submissionAuthorGroupId))))
        {
            result.AddError(new ValidationError(nameof(teacherId),
                $"Current user is logged in as teacher with id = {teacherId}," +
                " but this teacher does not teaches submission author`s group"));
        }
    }
}