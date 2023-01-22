﻿using System.Net;
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
    private readonly ILogger<RelationsDataAccess<TLeftTable, TRightTable, TRelations>> _logger;
    private readonly IRelationshipsHandler<TLeftTable, TRightTable, TRelations> _relationshipsHandler;

    public RelationsDataAccess(ILogger<RelationsDataAccess<TLeftTable, TRightTable, TRelations>> logger,
        IRelationshipsHandler<TLeftTable, TRightTable, TRelations> relationshipsHandler)
    {
        _logger = logger;
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
        catch (Exception e)
        {
            _logger.LogError(e, "cannot add relation between object of type {LeftTable} with id={RightTableId} " +
                                "on between object of type {RightTable} with id={LeftTableId} ", typeof(TRightTable),
                rightTableId, typeof(TLeftTable), leftTableId);
            throw new InfrastructureExceptions(HttpStatusCode.NotFound, e.Message);
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
        catch (Exception e)
        {
            _logger.LogError(e, "cannot delete relation between object of type {LeftTable} with id={RightTableId} " +
                                "on between object of type {RightTable} with id={LeftTableId} ", typeof(TRightTable),
                rightTableId, typeof(TLeftTable), leftTableId);
            throw new InfrastructureExceptions(HttpStatusCode.NotFound, e.Message);
        }
    }
}