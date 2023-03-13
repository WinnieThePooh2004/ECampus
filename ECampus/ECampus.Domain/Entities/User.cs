using ECampus.Domain.Data;
using ECampus.Domain.Entities.RelationEntities;
using ECampus.Domain.Enums;
using ECampus.Domain.Metadata.Relationships;

namespace ECampus.Domain.Entities;

[ManyToMany(typeof(Auditory), typeof(UserAuditory))]
[ManyToMany(typeof(Group), typeof(UserGroup))]
[ManyToMany(typeof(Teacher), typeof(UserTeacher))]
public class User : IEntity, IIsDeleted
{
    public int Id { get; set; }
    
    public string Email { get; set; } = string.Empty;
    
    public string Password { get; set; } = "tempPass1";
    
    public bool IsDeleted { get; set; }
    
    public string Username { get; set; } = string.Empty;
    
    public UserRole Role { get; set; }
    
    public int? StudentId { get; set; }
    
    public int? TeacherId { get; set; }

    
    public List<Group>? SavedGroups { get; set; }
    
    public List<Teacher>? SavedTeachers { get; set; }
    
    public List<Auditory>? SavedAuditories { get; set; }
    
    public List<UserGroup>? SavedGroupsIds { get; set; }
    
    public List<UserAuditory>? SavedAuditoriesIds { get; set; }
    
    public List<UserTeacher>? SavedTeachersIds { get; set; }
    
    
    public Teacher? Teacher { get; set; }
    
    public Student? Student { get; set; }
}