using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public readonly struct TaskSubmissionValidationParameters : IDataSelectParameters<TaskSubmission>
{
    public bool IncludeCourseTask { get; init; }
    public required int TaskSubmissionId { get; init; }
}