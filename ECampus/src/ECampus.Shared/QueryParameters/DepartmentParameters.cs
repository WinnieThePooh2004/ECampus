using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class DepartmentParameters : QueryParameters<DepartmentDto>, IDataSelectParameters<Department>
{
    public int FacultyId { get; set; }
    public string? DepartmentName { get; set; }
}