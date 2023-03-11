using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public readonly struct TaskSubmissionValidationParameters : IDataSelectParameters<TaskSubmission>
{
    public bool IncludeCourseTask { get; init; }
    public required int TaskSubmissionId { get; init; }
}