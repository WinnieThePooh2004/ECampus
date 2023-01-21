using System.ComponentModel.DataAnnotations;
using ECampus.Shared.Metadata.Relationships;

namespace ECampus.Shared.Models.RelationModels;

public class UserAuditory
{
    [Key]
    [LeftTableId(typeof(User))]
    public int UserId { get; set; }
    
    [Key] 
    [RightTableId(typeof(Auditory))]
    public int AuditoryId { get; set; }

    public Auditory? Auditory { get; set; }
    public User? User { get; set; }
}