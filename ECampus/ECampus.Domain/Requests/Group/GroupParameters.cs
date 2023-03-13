using ECampus.Domain.Responses.Group;

namespace ECampus.Domain.Requests.Group;

public class GroupParameters : QueryParameters<MultipleGroupResponse>, IDataSelectParameters<Entities.Group>
{
    public int DepartmentId { get; set; }
    public string? Name { get; set; }
}