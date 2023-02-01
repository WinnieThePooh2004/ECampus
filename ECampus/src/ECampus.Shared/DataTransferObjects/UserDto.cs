using ECampus.Shared.Data;
using ECampus.Shared.Enums;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;

namespace ECampus.Shared.DataTransferObjects;

[Dto(typeof(User))]
[Validation]
public class UserDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    [NotDisplay]
    public string Password { get; set; } = string.Empty;
    [NotDisplay]
    public string PasswordConfirm { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    
    public int? StudentId { get; set; }
    
    public int? TeacherId { get; set; }
    
    public TeacherDto? Teacher { get; set; }
    public StudentDto? Student { get; set; }

    public List<AuditoryDto>? SavedAuditories { get; set; }
    public List<GroupDto>? SavedGroups { get; set; }
    public List<TeacherDto>? SavedTeachers { get; set; }
}