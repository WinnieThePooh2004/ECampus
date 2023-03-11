using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Models;

namespace ECampus.Domain.QueryParameters;

public class TaskSubmissionParameters : QueryParameters<TaskSubmissionDto>, IDataSelectParameters<TaskSubmission>
{
    public int CourseTaskId { get; set; }
}