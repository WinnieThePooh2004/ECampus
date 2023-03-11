using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class CourseTaskParameters : QueryParameters<CourseTaskDto>, IDataSelectParameters<CourseTask>
{
    public int CourseId { get; set; }
}