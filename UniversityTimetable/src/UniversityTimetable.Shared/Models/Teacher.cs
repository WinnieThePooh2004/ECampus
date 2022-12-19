using System.Linq.Expressions;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Shared.Models
{
    public class Teacher : IIsDeleted, IModel, IModelWithManyToManyRelations<Subject>, IModelWithOneToManyRelations<SubjectTeacher>
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
        public List<User> Users { get; set; }
        public List<UserTeacher> UsersIds { get; set; }

        Expression<Func<SubjectTeacher, bool>> IModelWithOneToManyRelations<SubjectTeacher>.IsRelated => st => st.TeacherId == Id;

        List<Subject> IModelWithManyToManyRelations<Subject>.RelatedModels 
        {
            get => Subjects;
            set => Subjects = value;
        }

        List<SubjectTeacher> IModelWithOneToManyRelations<SubjectTeacher>.RelatedModels
        {
            get => SubjectIds;
            set => SubjectIds = value;
        }
    }
}
