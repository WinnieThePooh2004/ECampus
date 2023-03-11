using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Models;

namespace ECampus.Domain.QueryParameters;

public class GroupParameters : QueryParameters<GroupDto>, IDataSelectParameters<Group>
{
    public int DepartmentId { get; set; }
    public string? Name { get; set; }
}