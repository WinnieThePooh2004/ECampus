using ECampus.Core.Extensions;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Domain.Data;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.Relationships;

public class RelationsDataAccess : IRelationsDataAccess
{
    private readonly ApplicationDbContext _context;
    private readonly IServiceProvider _provider;
    
    public RelationsDataAccess(ApplicationDbContext context, IServiceProvider provider)
    {
        _context = context;
        _provider = provider;
    }

    public void CreateRelation<TLeftTable, TRightTable, TRelations>(int leftTableId, int rightTableId) 
        where TRelations : class, new()
        where TRightTable : IEntity 
        where TLeftTable : IEntity
    {
        var relation = GetHandler<TLeftTable, TRightTable, TRelations>().CreateRelationModel(leftTableId, rightTableId);
        _context.Add(relation);
    }

    public void DeleteRelation<TLeftTable, TRightTable, TRelations>(int leftTableId, int rightTableId) 
        where TLeftTable : IEntity 
        where TRightTable : IEntity 
        where TRelations : class, new()
    {
        var relation = GetHandler<TLeftTable, TRightTable, TRelations>().CreateRelationModel(leftTableId, rightTableId);
        _context.Remove(relation);
    }

    private IRelationshipsHandler<TLeftTable, TRightTable, TRelations> GetHandler
        <TLeftTable, TRightTable, TRelations>() 
        where TLeftTable : IEntity 
        where TRightTable : IEntity 
        where TRelations : class, new()
    {
        return _provider.GetServiceOfType<IRelationshipsHandler<TLeftTable, TRightTable, TRelations>>();
    }
}