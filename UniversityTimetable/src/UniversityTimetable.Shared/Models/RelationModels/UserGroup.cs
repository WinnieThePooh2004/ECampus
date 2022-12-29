using System.ComponentModel.DataAnnotations;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Models.RelationModels;

public class UserGroup: IRelationModel<User, Group>
{
    [Key] public int UserId { get; set; }
    [Key] public int GroupId { get; set; }

    public User User { get; set; }
    public Group Group { get; set; }
    int IRelationModel<User, Group>.RightTableId { get => GroupId; init => GroupId = value; }
    int IRelationModel<User, Group>.LeftTableId { init => UserId = value; }
}