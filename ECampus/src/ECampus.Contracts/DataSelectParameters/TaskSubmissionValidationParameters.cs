using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public class TaskSubmissionValidationParameters : IDataSelectParameters<TaskSubmission>
{
    public bool IncludeCourseTask { get; set; }
    public int TaskSubmissionId { get; set; }
}