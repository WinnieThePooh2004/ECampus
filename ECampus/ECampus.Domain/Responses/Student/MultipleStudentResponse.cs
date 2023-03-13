using ECampus.Domain.Metadata;

namespace ECampus.Domain.Responses.Student;

public class MultipleStudentResponse : IMultipleItemsResponse<Entities.Student>
{
    public int Id { get; set; }

    [DisplayName("Last name", 0)] public string LastName { get; set; } = string.Empty;

    [DisplayName("First name", 1)] public string FirstName { get; set; } = string.Empty;

    public int GroupId { get; set; }

    [DisplayName("Email", 2)] public string? UserEmail { get; set; }
}