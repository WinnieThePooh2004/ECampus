using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class SubjectParameters : QueryParameters, IDataSelectParameters<Subject>
{
    public string? Name { get; set; }
}