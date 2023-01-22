using System.Net;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Interfaces.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECampus.Infrastructure.Relationships;

public class RelationsDataAccess<TLeftTable, TRightTable, TRelations> :
    IRelationsDataAccess<TLeftTable, TRightTable, TRelations>
    where TLeftTable : IModel
    where TRightTable : IModel
    where TRelations : class, new()
{
    private readonly IRelationshipsHandler<TLeftTable, TRightTable, TRelations> _relationshipsHandler;

    public RelationsDataAccess(IRelationshipsHandler<TLeftTable, TRightTable, TRelations> relationshipsHandler)
    {
        _relationshipsHandler = relationshipsHandler;
    }

    public async Task CreateRelation(int leftTableId, int rightTableId, DbContext context)
    {
        var relation = _relationshipsHandler.CreateRelationModel(leftTableId, rightTableId);
        context.Add(relation);
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new InfrastructureExceptions(HttpStatusCode.NotFound,
                $"cannot add relation between object of type {typeof(TLeftTable)} with id={rightTableId} " +
                $"on between object of type {typeof(TRightTable)} with id={rightTableId} ");
        }
        catch (Exception e)
        {
            throw new UnhandledInfrastructureException(e);
        }
    }

    public async Task DeleteRelation(int leftTableId, int rightTableId, DbContext context)
    {
        var relation = _relationshipsHandler.CreateRelationModel(leftTableId, rightTableId);
        context.Remove(relation);
        try
        {
            await context.SaveChangesAsync();
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
}