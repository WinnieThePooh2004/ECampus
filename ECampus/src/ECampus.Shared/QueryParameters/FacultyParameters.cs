using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class FacultyParameters : QueryParameters, IDataSelectParameters<Faculty>
{
    public string? Name { get; set; }
}