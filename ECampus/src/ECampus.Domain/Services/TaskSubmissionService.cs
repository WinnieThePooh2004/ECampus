using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.Metadata;

namespace ECampus.Domain.Services;

[Inject(typeof(ITaskSubmissionService))]
public class TaskSubmissionService : ITaskSubmissionService
{
    private readonly ITaskSubmissionRepository _taskSubmissionRepository;
    private readonly ITaskSubmissionValidator _taskSubmissionValidator;

    public TaskSubmissionService(ITaskSubmissionRepository taskSubmissionRepository,
        ITaskSubmissionValidator taskSubmissionValidator)
    {
        _taskSubmissionRepository = taskSubmissionRepository;
        _taskSubmissionValidator = taskSubmissionValidator;
    }

    public async Task UpdateContent(int submissionId, string content)
    {
        var validationResult = await _taskSubmissionValidator.ValidateUpdateContent(submissionId, content);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(typeof(TaskSubmissionDto), validationResult);
        }

        await _taskSubmissionRepository.UpdateContent(submissionId, content);
    }

    public async Task UpdateMark(int submissionId, int mark)
    {
        var validationResult = await _taskSubmissionValidator.ValidateUpdateMark(submissionId, mark);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(typeof(TaskSubmissionDto), validationResult);
        }

        await _taskSubmissionRepository.UpdateMark(submissionId, mark);
    }
}