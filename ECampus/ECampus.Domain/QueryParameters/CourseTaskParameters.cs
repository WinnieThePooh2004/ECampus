using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;

namespace ECampus.Domain.QueryParameters;

public class CourseTaskParameters : QueryParameters<CourseTaskDto>, IDataSelectParameters<CourseTask>
{
    public int CourseId { get; set; }
}