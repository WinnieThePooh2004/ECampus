using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Infrastructure.Relationships;

public class RelationsDataAccess<TLeftTable, TRightTable, TRelations> : IRelationsDataAccess<TLeftTable, TRightTable, TRelations> 
    where TRelations : IRelationModel<TLeftTable, TRightTable>, new()
    where TRightTable : class, IModel
    where TLeftTable : class, IModel
{
    private readonly DbContext _context;
    private readonly ILogger<RelationsDataAccess<TLeftTable, TRightTable, TRelations>> _logger;

    public RelationsDataAccess(DbContext context, ILogger<RelationsDataAccess<TLeftTable, TRightTable, TRelations>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<TRelations> CreateRelation(int leftTableId, int rightTableId)
    {
        var relation = new TRelations { RightTableId = rightTableId, LeftTableId = leftTableId };
        _context.Add(relation);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "cannot add relation between object of type {LeftTable} with id={RightTableId} " +
                                "on between object of type {RightTable} with id={LeftTableId} ", typeof(TRightTable),
                rightTableId, typeof(TLeftTable), leftTableId);
            throw new InfrastructureExceptions(HttpStatusCode.NotFound, e.Message);
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
        catch (Exception e)
        {
            _logger.LogError(e, "cannot delete relation between object of type {LeftTable} with id={RightTableId} " +
                                "on between object of type {RightTable} with id={LeftTableId} ", typeof(TRightTable),
                rightTableId, typeof(TLeftTable), leftTableId);
            throw new InfrastructureExceptions(HttpStatusCode.NotFound, e.Message);
        }

        return relation;
    }}