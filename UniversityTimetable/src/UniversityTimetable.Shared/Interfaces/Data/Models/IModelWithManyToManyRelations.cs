using System.Linq.Expressions;

namespace UniversityTimetable.Shared.Interfaces.Data.Models
{
    public interface IModelWithManyToManyRelations<TRightTable, TRelations>
        where TRightTable : class, IModel
        where TRelations : class
    {
        List<TRightTable>? RelatedModels { get; set; }
        List<TRelations>? RelationModels { get; set; }
        Expression<Func<TRelations, bool>> IsRelated { get; }
    }
}
