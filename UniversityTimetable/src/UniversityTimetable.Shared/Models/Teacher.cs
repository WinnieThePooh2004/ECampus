using System.Linq.Expressions;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;

namespace UniversityTimetable.Shared.Models
{
    public class Teacher : IIsDeleted, IModel, IModelWithRelations<Subject>, IModelWithRelations<SubjectTeacher>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ScienceDegree ScienceDegree { get; set; }

        public bool IsDeleted { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public List<SubjectTeacher> SubjectIds { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<Class> Classes { get; set; }

        Expression<Func<Subject, bool>> IModelWithRelations<Subject>.IsRelated => subject => false;
        Expression<Func<SubjectTeacher, bool>> IModelWithRelations<SubjectTeacher>.IsRelated => st => st.TeacherId == Id;

        List<Subject> IModelWithRelations<Subject>.Relationships 
        {
            get => Subjects;
            set => Subjects = value;
        }

        List<SubjectTeacher> IModelWithRelations<SubjectTeacher>.Relationships
        {
            get => SubjectIds;
            set => SubjectIds = value;
        }
    }
}
