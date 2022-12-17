using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;
using UniversityTimetable.Shared.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Models;
using System.Linq.Expressions;
using System.Linq;

namespace UniversityTimetable.Infrastructure.Repositories
{
    public class RelationshipsRepository<TRightTable, TLeftTable, TRelations>
        : IRelationshipsRepository<TRightTable, TLeftTable, TRelations>
        where TRightTable : class, IModel, IModelWithRelations<TLeftTable>, IModelWithRelations<TRelations>, new()
        where TLeftTable : class, IModel, new()
        where TRelations : class, IRelationModel<TRightTable, TLeftTable>, new()
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RelationshipsRepository<TRightTable, TLeftTable, TRelations>> _logger;

        public RelationshipsRepository(ApplicationDbContext context, ILogger<RelationshipsRepository<TRightTable, TLeftTable, TRelations>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void CreateRelationModels(TRightTable model)
        {
            ((IModelWithRelations<TRelations>)model).Relationships = ((IModelWithRelations<TLeftTable>)model).Relationships
                .Select(r => new TRelations { LeftTableId = r.Id }).ToList();
            ((IModelWithRelations<TRelations>)model).Relationships = null;
        }

        public void UpdateLoadedRelations(TRightTable model)
        {
            var modelAsRelatedToTRelations = (IModelWithRelations<TRelations>)model;
            var modelRelations = modelAsRelatedToTRelations.Relationships;

            var modelAsRelatedToLeftTable = (IModelWithRelations<TLeftTable>)model;
            _context.RemoveRange(modelRelations.Where(st => !modelAsRelatedToLeftTable.Relationships.Any(s => s.Id == st.LeftTableId)));
            _context.AddRange(modelAsRelatedToLeftTable.Relationships
                .Where(s => !modelRelations.Any(st => s.Id == st.LeftTableId))
                .Select(s => new TRelations { RightTableId = model.Id, LeftTableId = s.Id }));

            modelAsRelatedToLeftTable.Relationships = null;
            ((IModelWithRelations<TRelations>)model).Relationships = null;
        }

        public async Task UpdateRelations(TRightTable model)
        {
            (model as IModelWithRelations<TRelations>).Relationships =
                await _context.Set<TRelations>().Where((model as IModelWithRelations<TRelations>).IsRelated).ToListAsync();
            UpdateLoadedRelations(model);
        }
    }
}
