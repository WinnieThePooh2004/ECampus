using System.ComponentModel.DataAnnotations;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;

namespace UniversityTimetable.Shared.Models.RelationModels;

public class UserAuditory : IRelationModel<User, Auditory>
{
    [Key] public int UserId { get; set; }
    [Key] public int AuditoryId { get; set; }

    public Auditory Auditory { get; set; }
    public User User { get; set; }
    int IRelationModel<User, Auditory>.RightTableId { init => UserId = value; }
    int IRelationModel<User, Auditory>.LeftTableId { get => AuditoryId; init => AuditoryId = value; }
}