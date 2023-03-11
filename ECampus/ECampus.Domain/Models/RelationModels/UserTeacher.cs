using System.ComponentModel.DataAnnotations;
using ECampus.Domain.Metadata.Relationships;

namespace ECampus.Domain.Models.RelationModels;

public class UserTeacher
{
    [Key]
    [LeftTableId(typeof(User))]
    public int UserId { get; set; }
    
    [Key]
    [RightTableId(typeof(Teacher))]
    public int TeacherId { get; set; }

    public User? User { get; set; }
    public Teacher? Teacher { get; set; }
}