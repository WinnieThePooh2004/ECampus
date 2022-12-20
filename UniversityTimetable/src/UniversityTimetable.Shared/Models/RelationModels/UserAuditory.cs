using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;

namespace UniversityTimetable.Shared.Models.RelationModels
{
    public class UserAuditory : IModel, IIsDeleted, IRelationModel<User, Auditory>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AuditoryId { get; set; }
        public bool IsDeleted { get; set; }

        public Auditory Auditory { get; set; }
        public User User { get; set; }
        int IRelationModel<User, Auditory>.RightTableId { init => UserId = value; }
        int IRelationModel<User, Auditory>.LeftTableId { get => AuditoryId; init => AuditoryId = value; }
    }
}
