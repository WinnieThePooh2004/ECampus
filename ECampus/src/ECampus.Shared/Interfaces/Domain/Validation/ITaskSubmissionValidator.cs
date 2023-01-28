using ECampus.Shared.Validation;

namespace ECampus.Shared.Interfaces.Domain.Validation;

public interface ITaskSubmissionValidator
{
    Task<ValidationResult> ValidateUpdateContentAsync(int submissionId, string content);
    Task<ValidationResult> ValidateUpdateMarkAsync(int submissionId, int mark);
}