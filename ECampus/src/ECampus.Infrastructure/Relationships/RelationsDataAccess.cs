using System.Net;
using ECampus.Contracts.DataAccess;
using ECampus.Core.Extensions;
using ECampus.Infrastructure.Extensions;
using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Data;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.Relationships;

public class RelationsDataAccess : IRelationsDataAccess
{
    private readonly ApplicationDbContext _context;
    private readonly IServiceProvider _provider;
    
    public RelationsDataAccess(ApplicationDbContext context, IServiceProvider provider)
    {
        _context = context;
        _provider = provider;
    }

    public async Task CreateRelation<TLeftTable, TRightTable, TRelations>(int leftTableId, int rightTableId) 
        where TRelations : class, new()
        where TRightTable : IModel 
        where TLeftTable : IModel
    {
        var relation = CreateHandler<TLeftTable, TRightTable, TRelations>().CreateRelationModel(leftTableId, rightTableId);
        _context.Add(relation);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new InfrastructureExceptions(HttpStatusCode.NotFound,
                $"cannot add relation between object of type {typeof(TLeftTable)} with id={rightTableId} " +
                $"on between object of type {typeof(TRightTable)} with id={rightTableId}");
        }
        catch (Exception e)
        {
            throw new UnhandledInfrastructureException(e);
        }
    }

    public async Task DeleteRelation<TLeftTable, TRightTable, TRelations>(int leftTableId, int rightTableId) 
        where TLeftTable : IModel 
        where TRightTable : IModel 
        where TRelations : class, new()
    {
        var relation = CreateHandler<TLeftTable, TRightTable, TRelations>().CreateRelationModel(leftTableId, rightTableId);
        _context.Remove(relation);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            throw new InfrastructureExceptions(HttpStatusCode.NotFound,
                $"cannot delete relation between object of type {typeof(TLeftTable)} with id={leftTableId} " +
                $"on between object of type {typeof(TRightTable)} with id={rightTableId} ", innerException: e);
        }
        catch (Exception e)
        {
            throw new UnhandledInfrastructureException(e);
        }
    }

    private IRelationshipsHandler<TLeftTable, TRightTable, TRelations> CreateHandler
        <TLeftTable, TRightTable, TRelations>() 
        where TLeftTable : IModel 
        where TRightTable : IModel 
        where TRelations : class, new()
    {
        return _provider.GetServiceOfType<IRelationshipsHandler<TLeftTable, TRightTable, TRelations>>();
    }
}