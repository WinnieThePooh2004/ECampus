using ECampus.Domain.Data;
using ECampus.Domain.Entities;
using ECampus.Domain.Metadata;
using ECampus.Domain.Responses.Auditory;
using ECampus.Domain.Responses.Group;
using ECampus.Domain.Responses.Teacher;

namespace ECampus.Domain.DataTransferObjects;

[Dto<User>(InjectBaseService = false)]
public class UserProfile : IDataTransferObject
{
    public int Id { get; set; }
    
    public string Username { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public List<MultipleGroupResponse>? SavedGroups { get; set; }
    
    public List<MultipleTeacherResponse>? SavedTeachers { get; set; }
    
    public List<MultipleAuditoryResponse>? SavedAuditories { get; set; }
}