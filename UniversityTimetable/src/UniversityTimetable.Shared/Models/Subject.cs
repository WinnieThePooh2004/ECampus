using System.Linq.Expressions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;

namespace UniversityTimetable.Shared.Models
{
    public class Subject : IIsDeleted, IModel, IModelWithRelations<SubjectTeacher>, IModelWithRelations<Teacher>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public List<Class> Classes { get; set; }
        public List<SubjectTeacher> TeacherIds { get; set; }
        public List<Teacher> Teachers { get; set; }

        Expression<Func<Teacher, bool>> IModelWithRelations<Teacher>.IsRelated => subject => false;
        Expression<Func<SubjectTeacher, bool>> IModelWithRelations<SubjectTeacher>.IsRelated => st => st.SubjectId == Id;

        List<Teacher> IModelWithRelations<Teacher>.Relationships
        {
            get => Teachers;
            set => Teachers = value;
        }

        List<SubjectTeacher> IModelWithRelations<SubjectTeacher>.Relationships
        {
            get => TeacherIds;
            set => TeacherIds = value;
        }

    }
}
