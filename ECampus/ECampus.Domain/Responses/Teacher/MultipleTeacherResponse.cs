using ECampus.Domain.Enums;
using ECampus.Domain.Metadata;

namespace ECampus.Domain.Responses.Teacher;

public class MultipleTeacherResponse : IMultipleItemsResponse<Entities.Teacher>
{
    public int Id { get; set; }

    [DisplayName("First name", 0)]
    public string FirstName { get; set; } = string.Empty;
    
    [DisplayName("Last name", 1)]
    public string LastName { get; set; } = string.Empty;
    
    [DisplayName("Science degree", 2)]
    public ScienceDegree ScienceDegree { get; set; }
    
    [DisplayName("Email", 3)]
    public string? UserEmail { get; set; }

    public int DepartmentId { get; set; }
}