using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;

namespace UniversityTimetable.Shared.Models.RelationModels
{
    public class UserTeacher : IModel, IIsDeleted, IRelationModel<User, Teacher>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TeacherId { get; set; }
        public bool IsDeleted { get; set; }

        public User User { get; set; }
        public Teacher Teacher { get; set; }
        int IRelationModel<User, Teacher>.RightTableId { init => UserId = value; }
        int IRelationModel<User, Teacher>.LeftTableId { get => TeacherId; init => TeacherId = value; }
    }
}
