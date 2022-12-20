using System.Linq.Expressions;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Shared.Models;

public class User : IModel, IIsDeleted,
    IModelWithManyToManyRelations<Auditory, UserAuditory>,
    IModelWithManyToManyRelations<Group, UserGroup>,
    IModelWithManyToManyRelations<Teacher, UserTeacher>
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
    List<Auditory> IModelWithManyToManyRelations<Auditory, UserAuditory>.RelatedModels => SavedAuditories;
    Expression<Func<UserTeacher, bool>> IModelWithManyToManyRelations<Teacher, UserTeacher>.IsRelated => ut => ut.UserId == Id;
    List<UserTeacher> IModelWithManyToManyRelations<Teacher, UserTeacher>.RelationModels => SavedTeachersIds;
    Expression<Func<UserGroup, bool>> IModelWithManyToManyRelations<Group, UserGroup>.IsRelated => ug => ug.UserId == Id;
    List<UserGroup> IModelWithManyToManyRelations<Group, UserGroup>.RelationModels => SavedGroupsIds;
    Expression<Func<UserAuditory, bool>> IModelWithManyToManyRelations<Auditory, UserAuditory>.IsRelated => ua => ua.UserId == Id;
    List<Group> IModelWithManyToManyRelations<Group, UserGroup>.RelatedModels => SavedGroups;
    List<Teacher> IModelWithManyToManyRelations<Teacher, UserTeacher>.RelatedModels => SavedTeachers;
    List<UserAuditory> IModelWithManyToManyRelations<Auditory, UserAuditory>.RelationModels => SavedAuditoriesIds;
}