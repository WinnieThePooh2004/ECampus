using System.Linq.Expressions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Shared.Models
{
    public class Subject : IIsDeleted, IModel, IModelWithManyToManyRelations<Teacher>, IModelWithOneToManyRelations<SubjectTeacher>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public List<Class> Classes { get; set; }
        public List<SubjectTeacher> TeacherIds { get; set; }
        public List<Teacher> Teachers { get; set; }

        Expression<Func<SubjectTeacher, bool>> IModelWithOneToManyRelations<SubjectTeacher>.IsRelated => st => st.SubjectId == Id;

        List<Teacher> IModelWithManyToManyRelations<Teacher>.RelatedModels
        {
            get => Teachers;
            set => Teachers = value;
        }

        List<SubjectTeacher> IModelWithOneToManyRelations<SubjectTeacher>.RelatedModels
        {
            get => TeacherIds;
            set => TeacherIds = value;
        }

    }
}
