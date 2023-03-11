using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class CourseSummaryParameters : QueryParameters<CourseSummary>, IDataSelectParameters<Course>
{
    public int StudentId { get; set; }
}