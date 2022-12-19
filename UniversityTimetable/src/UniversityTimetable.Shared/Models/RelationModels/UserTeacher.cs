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
        int IRelationModel<User, Teacher>.RightTableId { get => UserId; set => UserId = value; }
        int IRelationModel<User, Teacher>.LeftTableId { get => TeacherId; set => TeacherId = value; }
    }
}
