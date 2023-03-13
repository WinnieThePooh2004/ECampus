using ECampus.Domain.Responses.Student;

namespace ECampus.Domain.Requests.Student;

public class StudentParameters : QueryParameters<MultipleStudentResponse>, IDataSelectParameters<Entities.Student>
{
    public string? LastName { get; init; }
    public string? FirstName { get; init; }
    public int GroupId { get; set; }

    public bool UserIdCanBeNull { get; set; } = true;
}