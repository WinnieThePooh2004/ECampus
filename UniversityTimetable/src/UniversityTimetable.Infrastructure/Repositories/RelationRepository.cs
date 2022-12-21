using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Infrastructure.Repositories;

public class RelationRepository<TLeftTable, TRightTable, TRelations> : IRelationRepository<TLeftTable, TRightTable, TRelations>
    where TLeftTable : class, IModel, IModelWithManyToManyRelations<TRightTable, TRelations>, new()
    where TRightTable : class, IModel
    where TRelations : class, IRelationModel<TLeftTable, TRightTable>, new()
{
    private readonly ILogger<RelationRepository<TLeftTable, TRightTable, TRelations>> _logger;
    private readonly ApplicationDbContext _context;

    public RelationRepository(ILogger<RelationRepository<TLeftTable, TRightTable, TRelations>> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<TRelations> CreateRelation(int leftTableId, int rightTableId)
    {
        var relation = new TRelations { RightTableId = rightTableId, LeftTableId = leftTableId };
        _context.Add(relation);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch(Exception e)
        {
            _logger.LogError(e, "cannot add relation between object of type {LeftTable} with id={RightTableId} " +
                                "on between object of type {RightTable} with id={LeftTableId} ", typeof(TRightTable),
                rightTableId, typeof(TLeftTable), leftTableId);
            throw new InfrastructureExceptions(HttpStatusCode.NotFound,e.Message);
        }
        return relation;
    }

    public async Task<TRelations> DeleteRelation(int leftTableId, int rightTableId)
    {
        var relation = new TRelations { RightTableId = rightTableId, LeftTableId = leftTableId };
        _context.Remove(relation);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch(Exception e)
        {
            _logger.LogError(e, "cannot add relation between object of type {LeftTable} with id={RightTableId} " +
                                "on between object of type {RightTable} with id={LeftTableId} ", typeof(TRightTable),
                rightTableId, typeof(TLeftTable), leftTableId);
            throw new InfrastructureExceptions(HttpStatusCode.NotFound, e.Message);
        }

        return relation;
    }
}