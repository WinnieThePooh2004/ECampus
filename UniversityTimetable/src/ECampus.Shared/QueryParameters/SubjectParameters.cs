using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class SubjectParameters : QueryParameters, IQueryParameters<Subject>
{
    public string? Name { get; set; }
}