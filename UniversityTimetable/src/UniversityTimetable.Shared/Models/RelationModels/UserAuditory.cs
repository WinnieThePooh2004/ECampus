using System.ComponentModel.DataAnnotations;
using UniversityTimetable.Shared.Metadata.Relationships;

namespace UniversityTimetable.Shared.Models.RelationModels;

public class UserAuditory
{
    [Key]
    [LeftTableId(typeof(User), typeof(Auditory))]
    public int UserId { get; set; }
    
    [Key] 
    [RightTableId(typeof(Auditory))]
    public int AuditoryId { get; set; }

    public Auditory? Auditory { get; set; }
    public User? User { get; set; }
}