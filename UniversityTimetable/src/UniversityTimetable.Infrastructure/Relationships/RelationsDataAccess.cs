using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Metadata;

namespace UniversityTimetable.Infrastructure.Relationships;

[Inject(typeof(IRelationsDataAccess), ServiceLifetime.Singleton)]
public class RelationsDataAccess : IRelationsDataAccess
{
    private readonly ILogger<RelationsDataAccess> _logger;

    public RelationsDataAccess(ILogger<RelationsDataAccess> logger)
    {
        _logger = logger;
    }

    public async Task CreateRelation<TRelations, TLeftTable, TRightTable>(int leftTableId, int rightTableId, DbContext context) 
        where TRelations : IRelationModel<TLeftTable, TRightTable>, new()
        where TLeftTable : class, IModel 
        where TRightTable : class, IModel
    {
        var relation = new TRelations { RightTableId = rightTableId, LeftTableId = leftTableId };
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

    public async Task DeleteRelation<TRelations, TLeftTable, TRightTable>(int leftTableId, int rightTableId, DbContext context)
        where TRelations : IRelationModel<TLeftTable, TRightTable>, new()
        where TLeftTable : class, IModel 
        where TRightTable : class, IModel
    {
        var relation = new TRelations { RightTableId = rightTableId, LeftTableId = leftTableId };
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