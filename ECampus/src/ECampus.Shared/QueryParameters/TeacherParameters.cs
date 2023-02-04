using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class TeacherParameters : QueryParameters, IDataSelectParameters<Teacher>
{
    public int DepartmentId { get; set; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }

    public bool UserIdCanBeNull { get; set; } = true;
}