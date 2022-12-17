using System.Linq.Expressions;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.Interfaces.ModelsRelationships
{
    public interface IModelWithRelations<TRelations> : IModel
    {
        List<TRelations> Relationships { get; set; }
        public Expression<Func<TRelations, bool>> IsRelated { get; }
    }
}
