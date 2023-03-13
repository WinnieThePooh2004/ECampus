using ECampus.Domain.Responses.TaskSubmission;

namespace ECampus.Domain.Requests.TaskSubmission;

public class TaskSubmissionParameters : QueryParameters<MultipleTaskSubmissionResponse>, IDataSelectParameters<Entities.TaskSubmission>
{
    public int CourseTaskId { get; set; }
}