using ECampus.DataAccess.Interfaces;
using ECampus.Shared.Data;

namespace ECampus.DataAccess.Extensions;

public static class RelationshipsHandlerExtensions
{
    public static TRelation CreateRelationModel<TRelation, TLeftTable, TRightTable>(
        this IRelationshipsHandler<TLeftTable, TRightTable, TRelation> handler, int leftTableId, int rightTableId)
        where TRelation : class, new() where TRightTable : IModel where TLeftTable : IModel
    {
        var result = new TRelation();
        handler.RightTableId.SetMethod?.Invoke(result, new object[] { rightTableId });
        handler.LeftTableId.SetMethod?.Invoke(result, new object[] { leftTableId });
        return result;
    }
}