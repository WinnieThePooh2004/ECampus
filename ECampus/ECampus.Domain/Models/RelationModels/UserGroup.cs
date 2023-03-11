using System.ComponentModel.DataAnnotations;
using ECampus.Domain.Metadata.Relationships;

namespace ECampus.Domain.Models.RelationModels;

public class UserGroup
{
    [Key]
    [LeftTableId(typeof(User))]
    public int UserId { get; set; }
    
    [Key] 
    [RightTableId(typeof(Group))]
    public int GroupId { get; set; }

    public User? User { get; set; }
    public Group? Group { get; set; }
}