using ECampus.Shared.Data;

namespace ECampus.Contracts.DataAccess;

public interface IRelationsDataAccess
{
    void CreateRelation<TLeftTable, TRightTable, TRelations>(int leftTableId, int rightTableId)
        where TLeftTable : IModel
        where TRightTable : IModel
        where TRelations : class, new();

    void DeleteRelation<TLeftTable, TRightTable, TRelations>(int leftTableId, int rightTableId)
        where TLeftTable : IModel
        where TRightTable : IModel
        where TRelations : class, new();
}