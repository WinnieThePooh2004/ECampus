using ECampus.Shared.Validation;

namespace ECampus.Domain.Interfaces.Validation;

public interface ITaskSubmissionValidator
{
    Task<ValidationResult> ValidateUpdateContentAsync(int submissionId, string content);
    Task<ValidationResult> ValidateUpdateMarkAsync(int submissionId, int mark);
}