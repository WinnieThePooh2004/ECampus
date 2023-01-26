using ECampus.Shared.Validation;

namespace ECampus.Shared.Interfaces.Domain.Validation;

public interface ITaskSubmissionValidator
{
    Task<ValidationResult> ValidateUpdateContent(int submissionId, string content);
    Task<ValidationResult> ValidateUpdateMark(int submissionId, int mark);
}