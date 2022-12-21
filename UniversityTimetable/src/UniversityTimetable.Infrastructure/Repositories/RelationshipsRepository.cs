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
        model.RelationModels.AddRange(model.RelatedModels.Select(r => new TRelations { RightTableId = r.Id }));
        model.RelatedModels = null;
    }

    public async Task UpdateLoadedRelations(TLeftTable model)
    {
        _context.Set<TRelations>()
            .RemoveRange(model.RelationModels.Where(st => model.RelatedModels.All(s => s.Id != st.RightTableId)));
        _context.Set<TRelations>()
            .AddRange(model.RelatedModels.Where(s => model.RelationModels.All(st => s.Id != st.RightTableId))
                .Select(s => new TRelations { RightTableId = model.Id, LeftTableId = s.Id }));
        await _context.SaveChangesAsync();
        model.RelationModels = null;
        model.RelatedModels = null;
    }

    public async Task UpdateRelations(TLeftTable model)
    {
        model.RelationModels.AddRange(await _context.Set<TRelations>().Where(model.IsRelated).ToListAsync());
        await UpdateLoadedRelations(model);
    }
}