using System.ComponentModel.DataAnnotations;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Models.RelationModels;

public class UserAuditory : IRelationModel<User, Auditory>
{
    [Key] public int UserId { get; set; }
    [Key] public int AuditoryId { get; set; }

    public Auditory Auditory { get; set; }
    public User User { get; set; }
    int IRelationModel<User, Auditory>.RightTableId { get => AuditoryId; init => AuditoryId = value; }
    int IRelationModel<User, Auditory>.LeftTableId { init => UserId = value; }
}