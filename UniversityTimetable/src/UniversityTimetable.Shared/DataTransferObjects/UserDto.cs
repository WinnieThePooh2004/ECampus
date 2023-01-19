using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Metadata;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.DataTransferObjects;

[Dto<User>]
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

    public List<AuditoryDto>? SavedAuditories { get; set; }
    public List<GroupDto>? SavedGroups { get; set; }
    public List<TeacherDto>? SavedTeachers { get; set; }
}