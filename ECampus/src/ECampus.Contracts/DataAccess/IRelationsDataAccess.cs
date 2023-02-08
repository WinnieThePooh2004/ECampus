using ECampus.Shared.Data;

namespace ECampus.Contracts.DataAccess;

public interface IRelationsDataAccess
{
    Task CreateRelation<TLeftTable, TRightTable, TRelations>(int leftTableId, int rightTableId, CancellationToken token = default)
        where TLeftTable : IModel
        where TRightTable : IModel
        where TRelations : class, new();

    Task DeleteRelation<TLeftTable, TRightTable, TRelations>(int leftTableId, int rightTableId, CancellationToken token = default)
        where TLeftTable : IModel
        where TRightTable : IModel
        where TRelations : class, new();
}