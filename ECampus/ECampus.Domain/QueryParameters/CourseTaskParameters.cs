using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Models;

namespace ECampus.Domain.QueryParameters;

public class CourseTaskParameters : QueryParameters<CourseTaskDto>, IDataSelectParameters<CourseTask>
{
    public int CourseId { get; set; }
}