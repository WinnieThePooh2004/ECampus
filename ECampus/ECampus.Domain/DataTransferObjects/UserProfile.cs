using ECampus.Domain.Data;
using ECampus.Domain.Metadata;
using ECampus.Domain.Models;

namespace ECampus.Domain.DataTransferObjects;

[Dto<User>(InjectBaseService = false)]
public class UserProfile : IDataTransferObject
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<GroupDto>? SavedGroups { get; set; }
    public List<TeacherDto>? SavedTeachers { get; set; }
    public List<AuditoryDto>? SavedAuditories { get; set; }
}