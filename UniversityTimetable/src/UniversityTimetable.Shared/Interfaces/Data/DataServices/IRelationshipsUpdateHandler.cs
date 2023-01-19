using System.Linq.Expressions;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Interfaces.Data.DataServices;

public interface IRelationshipsUpdateHandler<in TLeftTable, TRightTable, TRelationModel>
    where TLeftTable : IModel 
    where TRightTable : IModel 
    where TRelationModel : class, new()
{
    Expression<Func<TRightTable, bool>> AddedToModelExpression(TLeftTable leftTableModel);
    Expression<Func<TRightTable, List<TRelationModel>?>> NavigationBetweenRightAndRelations();
    List<TRelationModel> RelationModelsOfRightTable(TRightTable rightTableModel);
    int LeftTableId(TRelationModel relationModel);
    Expression<Func<TRelationModel, bool>> DeletedFromModelExpression(TLeftTable leftTableModel);
    void ClearRelatedModel(TLeftTable model);
    TRelationModel CreateRelationModel(int leftTableId, int rightTableId);
}