using System.Linq.Expressions;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.Interfaces.ModelsRelationships
{
    public interface IModelWithManyToManyRelations<TRightTable, TRelations>
        where TRightTable : class, IModel
        where TRelations : class
    {
        List<TRightTable> RelatedModels { get; }
        List<TRelations> RelationModels { get; }
        Expression<Func<TRelations, bool>> IsRelated { get; }
    }
}
