using System.ComponentModel.DataAnnotations;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Models.RelationModels;

public class UserTeacher : IRelationModel<User, Teacher>
{
    [Key] public int UserId { get; set; }
    [Key] public int TeacherId { get; set; }

    public User? User { get; set; }
    public Teacher? Teacher { get; set; }
    int IRelationModel<User, Teacher>.RightTableId { get => TeacherId; init => TeacherId = value; }
    int IRelationModel<User, Teacher>.LeftTableId { init => UserId = value; }
}