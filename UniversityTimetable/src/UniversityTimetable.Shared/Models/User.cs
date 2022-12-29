using System.Linq.Expressions;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Data.Models;
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

    public List<Group> SavedGroups { get; set; } = new();
    public List<Teacher> SavedTeachers { get; set; } = new();
    public List<Auditory> SavedAuditories { get; set; } = new();
    public List<UserGroup> SavedGroupsIds { get; set; } = new();
    public List<UserAuditory> SavedAuditoriesIds { get; set; } = new();
    public List<UserTeacher> SavedTeachersIds { get; set; } = new();
    List<Auditory> IModelWithManyToManyRelations<Auditory, UserAuditory>.RelatedModels
    {
        get => SavedAuditories;
        set => SavedAuditories = value;
    }

    Expression<Func<UserTeacher, bool>> IModelWithManyToManyRelations<Teacher, UserTeacher>.IsRelated => ut => ut.UserId == Id;
    List<UserTeacher> IModelWithManyToManyRelations<Teacher, UserTeacher>.RelationModels
    {
        get => SavedTeachersIds;
        set => SavedTeachersIds = value;
    }

    Expression<Func<UserGroup, bool>> IModelWithManyToManyRelations<Group, UserGroup>.IsRelated => ug => ug.UserId == Id;
    List<UserGroup> IModelWithManyToManyRelations<Group, UserGroup>.RelationModels
    {
        get => SavedGroupsIds;
        set => SavedGroupsIds = value;
    }

    Expression<Func<UserAuditory, bool>> IModelWithManyToManyRelations<Auditory, UserAuditory>.IsRelated => ua => ua.UserId == Id;
    List<Group> IModelWithManyToManyRelations<Group, UserGroup>.RelatedModels
    {
        get => SavedGroups;
        set => SavedGroups = value;
    }

    List<Teacher> IModelWithManyToManyRelations<Teacher, UserTeacher>.RelatedModels
    {
        get => SavedTeachers;
        set => SavedTeachers = value;
    }

    List<UserAuditory> IModelWithManyToManyRelations<Auditory, UserAuditory>.RelationModels
    {
        get => SavedAuditoriesIds;
        set => SavedAuditoriesIds = value;
    }
}