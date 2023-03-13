using ECampus.Domain.Responses.Teacher;

namespace ECampus.Domain.Requests.Teacher;

public class TeacherParameters : QueryParameters<MultipleTeacherResponse>, IDataSelectParameters<Entities.Teacher>
{
    public int DepartmentId { get; set; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }

    public bool UserIdCanBeNull { get; set; } = true;
}