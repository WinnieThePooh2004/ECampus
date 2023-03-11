using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Models;

namespace ECampus.Domain.QueryParameters;

public class TeacherParameters : QueryParameters<TeacherDto>, IDataSelectParameters<Teacher>
{
    public int DepartmentId { get; set; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }

    public bool UserIdCanBeNull { get; set; } = true;
}