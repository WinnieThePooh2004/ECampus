using System.Linq.Expressions;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.Interfaces.ModelsRelationships
{
    public interface IModelWithOneToManyRelations<TRelations> : IModel
        where TRelations : class, IModel
    {
        List<TRelations> RelatedModels { get; set; }
        public Expression<Func<TRelations, bool>> IsRelated { get; }
    }
}
