using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;

namespace UniversityTimetable.Shared.Models.RelationModels
{
    public class UserGroup: IModel, IIsDeleted, IRelationModel<User, Group>
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }

        public User User { get; set; }
        public Group Group { get; set; }
        int IRelationModel<User, Group>.RightTableId { init => UserId = value; }
        int IRelationModel<User, Group>.LeftTableId { get => GroupId; init => GroupId = value; }
    }
}
