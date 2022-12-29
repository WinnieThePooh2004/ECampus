using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Interfaces.DataAccess;

public interface IRelationshipsDataAccess<in TLeftTable, TRightTable, TRelations>
    where TLeftTable : class, IModel, IModelWithManyToManyRelations<TRightTable, TRelations>, new()
    where TRightTable : class, IModel, new()
    where TRelations : class, IRelationModel<TLeftTable, TRightTable>, new()
{
    void CreateRelationModels(TLeftTable model);
    Task UpdateRelations(TLeftTable model);
    Task<TRelations> CreateRelation(int leftTableId, int rightTableId);
    Task<TRelations> DeleteRelation(int leftTableId, int rightTableId);

}