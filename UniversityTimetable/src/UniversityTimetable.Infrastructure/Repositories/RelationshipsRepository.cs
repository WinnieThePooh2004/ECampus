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
        where TRightTable : class, IModel, IModelWithManyToManyRelations<TLeftTable>, IModelWithOneToManyRelations<TRelations>, new()
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
            ((IModelWithOneToManyRelations<TRelations>)model).RelatedModels = ((IModelWithManyToManyRelations<TLeftTable>)model).RelatedModels
                .Select(r => new TRelations { LeftTableId = r.Id }).ToList();
            ((IModelWithManyToManyRelations<TRelations>)model).RelatedModels = null;
        }

        public void UpdateLoadedRelations(TRightTable model)
        {
            var modelAsRelatedToTRelations = (IModelWithOneToManyRelations<TRelations>)model;
            var modelRelations = modelAsRelatedToTRelations.RelatedModels;

            var modelAsRelatedToLeftTable = (IModelWithManyToManyRelations<TLeftTable>)model;
            _context.RemoveRange(modelRelations.Where(st => 
                !modelAsRelatedToLeftTable.RelatedModels.Any(s => s.Id == st.LeftTableId)));
            _context.AddRange(modelAsRelatedToLeftTable.RelatedModels
                .Where(s => !modelRelations.Any(st => s.Id == st.LeftTableId))
                .Select(s => new TRelations { RightTableId = model.Id, LeftTableId = s.Id }));

            modelAsRelatedToLeftTable.RelatedModels = null;
            ((IModelWithOneToManyRelations<TRelations>)model).RelatedModels = null;
        }

        public async Task UpdateRelations(TRightTable model)
        {
            (model as IModelWithOneToManyRelations<TRelations>).RelatedModels =
                await _context.Set<TRelations>().Where((model as IModelWithOneToManyRelations<TRelations>).IsRelated).ToListAsync();
            UpdateLoadedRelations(model);
        }
    }
}
