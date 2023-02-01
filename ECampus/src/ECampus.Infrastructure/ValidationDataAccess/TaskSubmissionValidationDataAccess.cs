using ECampus.Contracts.DataValidation;
using ECampus.Core.Metadata;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;

namespace ECampus.Infrastructure.ValidationDataAccess;

[Inject(typeof(IValidationDataAccess<TaskSubmission>))]
public class TaskSubmissionValidationDataAccess : IValidationDataAccess<TaskSubmission>
{
    private readonly ApplicationDbContext _context;

    public TaskSubmissionValidationDataAccess(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Task<TaskSubmission> LoadRequiredDataForCreateAsync(TaskSubmission model)
    {
        return Task.FromResult(new TaskSubmission());
    }

    public async Task<TaskSubmission> LoadRequiredDataForUpdateAsync(TaskSubmission model)
    {
        return await _context.FindAsync<TaskSubmission>(model.Id)
               ?? throw new ObjectNotFoundByIdException(typeof(TaskSubmission), model.Id);
    }
}