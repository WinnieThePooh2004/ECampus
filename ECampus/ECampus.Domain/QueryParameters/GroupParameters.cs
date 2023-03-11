using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;

namespace ECampus.Domain.QueryParameters;

public class GroupParameters : QueryParameters<GroupDto>, IDataSelectParameters<Group>
{
    public int DepartmentId { get; set; }
    public string? Name { get; set; }
}