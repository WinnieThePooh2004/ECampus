using System.Linq.Expressions;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.Interfaces.ModelsRelationships
{
    public interface IModelWithManyToManyRelations<TRightTable, TRelations>
        where TRightTable : class, IModel
        where TRelations : class
    {
        List<TRightTable> RelatedModels { get; set; }
        List<TRelations> RelationModels { get; set; }
        Expression<Func<TRelations, bool>> IsRelated { get; }
    }
}
