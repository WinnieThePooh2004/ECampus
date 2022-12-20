using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;
using UniversityTimetable.Shared.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace UniversityTimetable.Infrastructure.Repositories;

public class RelationshipsRepository<TLeftTable, TRightTable, TRelations> : IRelationshipsRepository<TLeftTable, TRightTable, TRelations>
    where TLeftTable : class, IModel, IModelWithManyToManyRelations<TRightTable, TRelations>, new()
    where TRightTable : class, IModel, new()
    where TRelations : class, IRelationModel<TLeftTable, TRightTable>, new()
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<RelationshipsRepository<TLeftTable, TRightTable, TRelations>> _logger;

    public RelationshipsRepository(ApplicationDbContext context,
        ILogger<RelationshipsRepository<TLeftTable, TRightTable, TRelations>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public void CreateRelationModels(TLeftTable model)
    {
        model.RelationModels.AddRange(model.RelatedModels.Select(r => new TRelations { LeftTableId = r.Id }));
        model.RelatedModels.Clear();
    }

    public void UpdateLoadedRelations(TLeftTable model)
    {
        _context.RemoveRange(model.RelationModels.Where(st => model.RelatedModels.All(s => s.Id != st.LeftTableId)));
        _context.AddRange(model.RelatedModels.Where(s => model.RelationModels.All(st => s.Id != st.LeftTableId))
            .Select(s => new TRelations { RightTableId = model.Id, LeftTableId = s.Id }));
        model.RelationModels.Clear();
        model.RelatedModels.Clear();
    }

    public async Task UpdateRelations(TLeftTable model)
    {
        model.RelationModels.AddRange(await _context.Set<TRelations>().Where(model.IsRelated).ToListAsync());
        UpdateLoadedRelations(model);
    }
}