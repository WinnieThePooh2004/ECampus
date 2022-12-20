using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;

namespace UniversityTimetable.Shared.Interfaces.Repositories;

/// <summary>
/// use this interface when you need to create/delete relationships between two tables, but don`t want take anything from that tables
/// </summary>
/// <typeparam name="TLeftTable"></typeparam>
/// <typeparam name="TRightTable"></typeparam>
/// <typeparam name="TRelations"></typeparam>
public interface IRelationRepository<TLeftTable, TRightTable, TRelations>
    where TLeftTable: class, IModel
    where TRightTable: class, IModel
    where TRelations: class, IModel, IRelationModel<TLeftTable, TRightTable>
{
    Task<TRelations> CreateRelation(int leftTableId, int rightTableId);
    Task<TRelations> DeleteRelation(int leftTableId, int rightTableId);
}