using System.Net;
using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Interfaces.DataAccess;

namespace UniversityTimetable.Infrastructure.Relationships;

public class RelationshipsDataAccess<TLeftTable, TRightTable, TRelations> : IRelationshipsDataAccess<TLeftTable,
    TRightTable,
    TRelations>
    where TLeftTable : class, IModel, IModelWithManyToManyRelations<TRightTable, TRelations>, new()
    where TRightTable : class, IModel, new()
    where TRelations : class, IRelationModel<TLeftTable, TRightTable>, new()
{
    public void CreateRelationModels(TLeftTable model)
    {
        if (model.RelatedModels is null)
        {
            throw new InfrastructureExceptions(HttpStatusCode.BadRequest,
                $"Please, send related models of object of type '{typeof(TLeftTable)}' as empty list instead of null",
                model);
        }

        model.RelationModels = model.RelatedModels
            .Select(r => new TRelations { RightTableId = r.Id, LeftTableId = model.Id }).ToList();
        model.RelatedModels = null;
    }

    public async Task UpdateRelations(TLeftTable model, DbContext context)
    {
        if (model.RelatedModels is null)
        {
            throw new InfrastructureExceptions(HttpStatusCode.BadRequest,
                $"Please, send related models of object of type '{typeof(TLeftTable)}' as empty list instead of null",
                model);
        }
        
        model.RelationModels = await context.Set<TRelations>().Where(model.IsRelated).ToListAsync();
        context.RemoveRange(model.RelationModels.Where(st => model.RelatedModels.All(s => s.Id != st.RightTableId)));
        context.AddRange(model.RelatedModels.Where(s => model.RelationModels.All(st => s.Id != st.RightTableId))
            .Select(s => new TRelations { LeftTableId = model.Id, RightTableId = s.Id }));
        model.RelationModels = null;
        model.RelatedModels = null;
    }
}