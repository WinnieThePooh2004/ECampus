using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class CourseSummaryParameters : QueryParameters, IDataSelectParameters<Course>
{
    public int StudentId { get; set; }
}