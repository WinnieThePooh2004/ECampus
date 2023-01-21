using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class CourseTaskParameters : QueryParameters, IQueryParameters<CourseTask>
{
    public int CourseId { get; set; }
}