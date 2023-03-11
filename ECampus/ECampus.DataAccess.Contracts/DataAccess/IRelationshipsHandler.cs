using System.Reflection;
using ECampus.Domain.Data;

// ReSharper disable UnusedTypeParameter

namespace ECampus.DataAccess.Contracts.DataAccess;

public interface IRelationshipsHandler<in TLeftTable, out TRightTable, out TRelationEntity>
    where TLeftTable : IEntity
    where TRightTable : IEntity
    where TRelationEntity : class, new()
{
    PropertyInfo RightTableId { get; }
    PropertyInfo LeftTableId { get; }
    PropertyInfo RelatedModels { get; }
    PropertyInfo RelationModels { get; }
}

public static class RelationshipsHandlerExtensions
{
    public static TRelation CreateRelationModel<TRelation, TLeftTable, TRightTable>(
        this IRelationshipsHandler<TLeftTable, TRightTable, TRelation> handler, int leftTableId, int rightTableId)
        where TRelation : class, new() where TRightTable : IEntity where TLeftTable : IEntity
    {
        var result = new TRelation();
        handler.RightTableId.SetMethod?.Invoke(result, new object[] { rightTableId });
        handler.LeftTableId.SetMethod?.Invoke(result, new object[] { leftTableId });
        return result;
    }
}