using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters
{
    public class DepartmentParameters : QueryParameters, IDataSelectParameters<Department>
    {
        public int FacultyId { get; set; }
        public string? DepartmentName { get; set; }
    }
}
