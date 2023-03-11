using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Models;

namespace ECampus.Domain.QueryParameters;

public class DepartmentParameters : QueryParameters<DepartmentDto>, IDataSelectParameters<Department>
{
    public int FacultyId { get; set; }
    public string? DepartmentName { get; set; }
}