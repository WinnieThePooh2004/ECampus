using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;

namespace UniversityTimetable.Shared.Interfaces.Repositories;

public interface IRelationshipsRepository<in TLeftTable, TRightTable, TRelations>
    where TLeftTable : class, IModel, IModelWithManyToManyRelations<TRightTable, TRelations>, new()
    where TRightTable : class, IModel, new()
    where TRelations : class, IRelationModel<TLeftTable, TRightTable>, new()
{
    void CreateRelationModels(TLeftTable model);
    Task UpdateRelations(TLeftTable model);
    void UpdateLoadedRelations(TLeftTable model);
    Task<TRelations> CreateRelation(int leftTableId, int rightTableId);
    Task<TRelations> DeleteRelation(int leftTableId, int rightTableId);

}