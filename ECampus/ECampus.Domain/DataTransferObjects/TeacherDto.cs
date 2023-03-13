using ECampus.Domain.Data;
using ECampus.Domain.Entities;
using ECampus.Domain.Enums;
using ECampus.Domain.Metadata;
using ECampus.Domain.Responses.Subject;

namespace ECampus.Domain.DataTransferObjects;

[Dto<Teacher>]
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
    
    public List<MultipleSubjectResponse>? Subjects { get; set; }
}