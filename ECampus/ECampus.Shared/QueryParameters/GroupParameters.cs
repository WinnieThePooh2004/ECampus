using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class GroupParameters : QueryParameters<GroupDto>, IDataSelectParameters<Group>
{
    public int DepartmentId { get; set; }
    public string? Name { get; set; }
}