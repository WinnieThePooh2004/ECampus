using System.Linq.Expressions;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Shared.Models
{
    public class User : IModel, IIsDeleted,
        IModelWithManyToManyRelations<Auditory>,
        IModelWithManyToManyRelations<Group>,
        IModelWithManyToManyRelations<Teacher>,
        IModelWithOneToManyRelations<UserAuditory>,
        IModelWithOneToManyRelations<UserGroup>,
        IModelWithOneToManyRelations<UserTeacher>
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }

        public List<Group> SavedGroups { get; set; }
        public List<Teacher> SavedTeachers { get; set; }
        public List<Auditory> SavedAuditories { get; set; }
        public List<UserGroup> SavedGroupsIds { get; set; }
        public List<UserAuditory> SavedAuditoriesIds { get; set; }
        public List<UserTeacher> SavedTeachersIds { get; set; }
        List<Auditory> IModelWithManyToManyRelations<Auditory>.RelatedModels
        {
            get => SavedAuditories;
            set => SavedAuditories = value;
        }

        Expression<Func<UserTeacher, bool>> IModelWithOneToManyRelations<UserTeacher>.IsRelated => ut => ut.UserId == Id;

        List<UserTeacher> IModelWithOneToManyRelations<UserTeacher>.RelatedModels
        {
            get => SavedTeachersIds;
            set => SavedTeachersIds = value;
        }

        Expression<Func<UserGroup, bool>> IModelWithOneToManyRelations<UserGroup>.IsRelated => ug => ug.UserId == Id;

        List<UserGroup> IModelWithOneToManyRelations<UserGroup>.RelatedModels
        {
            get => SavedGroupsIds;
            set => SavedGroupsIds = value;
        }

        Expression<Func<UserAuditory, bool>> IModelWithOneToManyRelations<UserAuditory>.IsRelated => ua => ua.UserId == Id;

        List<Group> IModelWithManyToManyRelations<Group>.RelatedModels
        {
            get => SavedGroups;
            set => SavedGroups = value;
        }

        List<Teacher> IModelWithManyToManyRelations<Teacher>.RelatedModels
        {
            get => SavedTeachers;
            set => SavedTeachers = value;
        }

        List<UserAuditory> IModelWithOneToManyRelations<UserAuditory>.RelatedModels
        {
            get => SavedAuditoriesIds;
            set => SavedAuditoriesIds = value;
        }
    }
}
