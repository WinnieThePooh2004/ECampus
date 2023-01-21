using System.ComponentModel.DataAnnotations;
using UniversityTimetable.Shared.Metadata.Relationships;

namespace UniversityTimetable.Shared.Models.RelationModels;

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