using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;

namespace ECampus.Domain.Validation.CreateValidators;

[InstallerIgnore]
public class TaskSubmissionCreateValidator : ICreateValidator<TaskSubmissionDto>
{
    public Task<ValidationResult> ValidateAsync(TaskSubmissionDto dataTransferObject)
    {
        return Task.FromResult(new ValidationResult(new ValidationError(nameof(dataTransferObject.Id),
            $"Objects of type {typeof(TaskSubmission)}" +
            $" can be created only during creating new objects of type {typeof(CourseTaskDto)}")));
    }
}