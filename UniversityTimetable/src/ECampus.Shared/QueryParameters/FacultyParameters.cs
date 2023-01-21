using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class FacultyParameters : QueryParameters, IQueryParameters<Faculty>
{
    public string? Name { get; set; }
}