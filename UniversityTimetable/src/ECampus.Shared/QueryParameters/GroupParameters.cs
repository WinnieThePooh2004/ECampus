using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class GroupParameters : QueryParameters, IQueryParameters<Group>
{
    public int DepartmentId { get; set; }
    public string? Name { get; set; }
}