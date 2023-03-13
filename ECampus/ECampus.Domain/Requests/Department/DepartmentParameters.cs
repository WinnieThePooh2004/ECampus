using ECampus.Domain.Responses.Department;

namespace ECampus.Domain.Requests.Department;

public class DepartmentParameters : QueryParameters<MultipleDepartmentResponse>, IDataSelectParameters<Entities.Department>
{
    public int FacultyId { get; set; }
    public string? DepartmentName { get; set; }
}