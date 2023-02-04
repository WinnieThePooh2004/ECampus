using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class TaskSubmissionParameters : QueryParameters, IDataSelectParameters<TaskSubmission>
{
    public int CourseTaskId { get; set; }
}