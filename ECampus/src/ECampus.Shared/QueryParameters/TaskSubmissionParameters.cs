using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class TaskSubmissionParameters : QueryParameters, IQueryParameters<TaskSubmission>
{
    public int StudentId { get; set; }
    public int CourseTaskId { get; set; }
}