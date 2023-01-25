using ECampus.Shared.Enums;
using ECampus.Shared.Interfaces.Data.Models;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;

namespace ECampus.Shared.DataTransferObjects;

[Dto(typeof(Teacher))]
[Validation]
public class TeacherDto : IDataTransferObject
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
    public List<SubjectDto>? Subjects { get; set; }
}