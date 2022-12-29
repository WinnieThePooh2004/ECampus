using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Interfaces.Data.DataServices;

public interface IRelationsDataAccess<TLeftTable, TRightTable, TRelations>
    where TRelations : IRelationModel<TLeftTable, TRightTable> 
    where TRightTable : class, IModel 
    where TLeftTable : class, IModel
{
    Task<TRelations> CreateRelation(int leftTableId, int rightTableId);
    Task<TRelations> DeleteRelation(int leftTableId, int rightTableId);
}