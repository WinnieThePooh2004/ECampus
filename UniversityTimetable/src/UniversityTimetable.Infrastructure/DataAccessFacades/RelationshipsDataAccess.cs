using System.Net;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Infrastructure.DataAccessFacades;

public class RelationshipsDataAccess<TLeftTable, TRightTable, TRelations> : IRelationshipsDataAccess<TLeftTable, TRightTable, TRelations>
    where TLeftTable : class, IModel, IModelWithManyToManyRelations<TRightTable, TRelations>, new()
    where TRightTable : class, IModel, new()
    where TRelations : class, IRelationModel<TLeftTable, TRightTable>, new()
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<RelationshipsDataAccess<TLeftTable, TRightTable, TRelations>> _logger;

    public RelationshipsDataAccess(ApplicationDbContext context,
        ILogger<RelationshipsDataAccess<TLeftTable, TRightTable, TRelations>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public void CreateRelationModels(TLeftTable model)
    {
        if (model.RelatedModels is null)
        {
            return;
        }
        model.RelationModels?.AddRange(model.RelatedModels
            .Select(r => new TRelations { RightTableId = r.Id, LeftTableId = model.Id}));
        model.RelatedModels = null;
    }
    
    public async Task UpdateRelations(TLeftTable model)
    {
        model.RelationModels = await _context.Set<TRelations>().Where(model.IsRelated).ToListAsync();
        UpdateLoadedRelations(model);
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
        catch(Exception e)
        {
            _logger.LogError(e, "cannot delete relation between object of type {LeftTable} with id={RightTableId} " +
                                "on between object of type {RightTable} with id={LeftTableId} ", typeof(TRightTable),
                rightTableId, typeof(TLeftTable), leftTableId);
            throw new InfrastructureExceptions(HttpStatusCode.NotFound, e.Message);
        }

        return relation;
    }
    
    private void UpdateLoadedRelations(TLeftTable model)
    {
        if (model.RelatedModels is null || model.RelationModels is null)
        {
            return;
        }
        _context.RemoveRange(model.RelationModels.Where(st => model.RelatedModels.All(s => s.Id != st.RightTableId)));
        _context.AddRange(model.RelatedModels.Where(s => model.RelationModels.All(st => s.Id != st.RightTableId))
            .Select(s => new TRelations { LeftTableId = model.Id, RightTableId = s.Id }));
        model.RelationModels = null;
        model.RelatedModels = null;
    }
}