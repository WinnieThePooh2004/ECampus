using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;

namespace ECampus.Domain.QueryParameters;

public class DepartmentParameters : QueryParameters<DepartmentDto>, IDataSelectParameters<Department>
{
    public int FacultyId { get; set; }
    public string? DepartmentName { get; set; }
}