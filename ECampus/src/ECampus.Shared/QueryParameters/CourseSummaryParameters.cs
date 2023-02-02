using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class CourseSummaryParameters : QueryParameters, IQueryParameters<Course>
{
    public int StudentId { get; set; }
}