using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Models;

namespace ECampus.Domain.QueryParameters;

public class CourseSummaryParameters : QueryParameters<CourseSummary>, IDataSelectParameters<Course>
{
    public int StudentId { get; set; }
}