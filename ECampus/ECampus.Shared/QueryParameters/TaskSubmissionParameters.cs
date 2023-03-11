using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class TaskSubmissionParameters : QueryParameters<TaskSubmissionDto>, IDataSelectParameters<TaskSubmission>
{
    public int CourseTaskId { get; set; }
}