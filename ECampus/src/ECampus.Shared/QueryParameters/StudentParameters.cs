using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class StudentParameters : QueryParameters, IDataSelectParameters<Student>
{
    public string? LastName { get; init; }
    public string? FirstName { get; init; }
    public int GroupId { get; set; }

    public bool UserIdCanBeNull { get; set; } = true;
}