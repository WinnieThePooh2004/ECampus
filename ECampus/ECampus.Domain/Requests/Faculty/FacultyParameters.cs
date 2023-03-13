using ECampus.Domain.Responses.Faculty;

namespace ECampus.Domain.Requests.Faculty;

public class FacultyParameters : QueryParameters<MultipleFacultyResponse>, IDataSelectParameters<Entities.Faculty>
{
    public string? Name { get; set; }
}