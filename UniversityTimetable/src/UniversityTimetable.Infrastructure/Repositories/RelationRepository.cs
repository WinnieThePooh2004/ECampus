using System.Net;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Infrastructure.Repositories;

public class RelationRepository<TLeftTable, TRightTable, TRelations> : IRelationRepository<TLeftTable, TRightTable, TRelations>
    where TLeftTable: class, IModel, IModelWithManyToManyRelations<TRightTable, TRelations>, new()
    where TRightTable: class, IModel
    where TRelations: class, IModel, IRelationModel<TLeftTable, TRightTable>, new()
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
        catch
        {
            _logger.LogAndThrowException(new InfrastructureExceptions(HttpStatusCode.NotFound,
                $"cannot find object of type {typeof(TLeftTable)} with id={leftTableId} " +
                $"or object of type {typeof(TRightTable)} with id={rightTableId}"));
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
        catch
        {
            _logger.LogAndThrowException(new InfrastructureExceptions(HttpStatusCode.NotFound,
                $"cannot find object of relation model of type {typeof(TRelations)}" +
                $"with id of object of type {typeof(TLeftTable)} = {leftTableId} and " +
                $"id of object of type {typeof(TRightTable)} = {rightTableId}"));
        }

        return relation;
    }
}