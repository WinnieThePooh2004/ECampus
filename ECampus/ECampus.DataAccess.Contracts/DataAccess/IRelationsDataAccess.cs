using ECampus.Domain.Data;

namespace ECampus.DataAccess.Contracts.DataAccess;

public interface IRelationsDataAccess
{
    void CreateRelation<TLeftTable, TRightTable, TRelations>(int leftTableId, int rightTableId)
        where TLeftTable : IEntity
        where TRightTable : IEntity
        where TRelations : class, new();

    void DeleteRelation<TLeftTable, TRightTable, TRelations>(int leftTableId, int rightTableId)
        where TLeftTable : IEntity
        where TRightTable : IEntity
        where TRelations : class, new();
}