using System.Linq.Expressions;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Shared.Models;

public class Subject : IIsDeleted, IModel, IModelWithManyToManyRelations<Teacher, SubjectTeacher>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public List<Class>? Classes { get; set; }
    public List<SubjectTeacher>? TeacherIds { get; set; }
    public List<Teacher>? Teachers { get; set; }

    Expression<Func<SubjectTeacher, bool>> IModelWithManyToManyRelations<Teacher, SubjectTeacher>.IsRelated => st => st.SubjectId == Id;

    List<Teacher>? IModelWithManyToManyRelations<Teacher, SubjectTeacher>.RelatedModels
    {
        get => Teachers;
        set => Teachers = value;
    }

    List<SubjectTeacher>? IModelWithManyToManyRelations<Teacher, SubjectTeacher>.RelationModels
    {
        get => TeacherIds;
        set => TeacherIds = value;
    }
}