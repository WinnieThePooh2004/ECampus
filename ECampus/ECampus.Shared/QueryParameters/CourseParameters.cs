using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class CourseParameters : QueryParameters<CourseDto>, IDataSelectParameters<Course>
{
    public int GroupId { get; set; }
    public int TeacherId { get; set; }
    public string? Name { get; set; }
}