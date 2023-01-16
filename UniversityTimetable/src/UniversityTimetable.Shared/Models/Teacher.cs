using System.Linq.Expressions;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Shared.Models;

public class Teacher : IIsDeleted, IModel, IModelWithManyToManyRelations<Subject, SubjectTeacher>
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public ScienceDegree ScienceDegree { get; set; }
    public int? UserId { get; set; }

    public bool IsDeleted { get; set; }
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
    public User? User { get; set; }

    public List<SubjectTeacher>? SubjectIds { get; set; }
    public List<Subject>? Subjects { get; set; }
    public List<Class>? Classes { get; set; }
    public List<User>? Users { get; set; }
    public List<UserTeacher>? UsersIds { get; set; }

    Expression<Func<SubjectTeacher, bool>> IModelWithManyToManyRelations<Subject, SubjectTeacher>.IsRelated 
        => st => st.TeacherId == Id;

    List<Subject>? IModelWithManyToManyRelations<Subject, SubjectTeacher>.RelatedModels
    {
        get => Subjects;
        set => Subjects = value;
    }

    List<SubjectTeacher>? IModelWithManyToManyRelations<Subject, SubjectTeacher>.RelationModels
    {
        get => SubjectIds;
        set => SubjectIds = value;
    }
}