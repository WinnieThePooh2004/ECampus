using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;

namespace ECampus.Domain.QueryParameters;

public class TaskSubmissionParameters : QueryParameters<TaskSubmissionDto>, IDataSelectParameters<TaskSubmission>
{
    public int CourseTaskId { get; set; }
}