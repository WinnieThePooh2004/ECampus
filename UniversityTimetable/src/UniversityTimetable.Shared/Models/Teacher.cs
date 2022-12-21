using System.Linq.Expressions;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Shared.Models
{
    public class Teacher : IIsDeleted, IModel, IModelWithManyToManyRelations<Subject, SubjectTeacher>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ScienceDegree ScienceDegree { get; set; }

        public bool IsDeleted { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public List<SubjectTeacher> SubjectIds { get; set; } = new();
        public List<Subject> Subjects { get; set; } = new();
        public List<Class> Classes { get; set; } = new();
        public List<User> Users { get; set; } = new();
        public List<UserTeacher> UsersIds { get; set; } = new();

        Expression<Func<SubjectTeacher, bool>> IModelWithManyToManyRelations<Subject, SubjectTeacher>.IsRelated => st => st.TeacherId == Id;

        List<Subject> IModelWithManyToManyRelations<Subject, SubjectTeacher>.RelatedModels => Subjects;

        List<SubjectTeacher> IModelWithManyToManyRelations<Subject, SubjectTeacher>.RelationModels => SubjectIds;
    }
}
