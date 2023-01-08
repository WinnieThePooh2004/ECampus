using System.Net;
using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Infrastructure.DataUpdateServices;

public class DataUpdateServiceWithRelationships<TModel, TRelatedModel, TRelations> : IDataUpdateService<TModel>
    where TModel : class, IModel, IModelWithManyToManyRelations<TRelatedModel, TRelations>, new()
    where TRelatedModel : class, IModel, new()
    where TRelations : class, IRelationModel<TModel, TRelatedModel>, new()
{
    private readonly IDataUpdateService<TModel> _baseUpdateService;

    public DataUpdateServiceWithRelationships(IDataUpdateService<TModel> baseUpdateService)
    {
        _baseUpdateService = baseUpdateService;
    }

    public async Task<TModel> UpdateAsync(TModel model, DbContext context)
    {
        if (model.RelatedModels is null)
        {
            throw new InfrastructureExceptions(HttpStatusCode.BadRequest,
                $"Please, send related models of object of type '{typeof(TModel)}' as empty list instead of null",
                model);
        }

        model.RelationModels = await context.Set<TRelations>().Where(model.IsRelated).ToListAsync();
        context.RemoveRange(model.RelationModels.Where(st => model.RelatedModels.All(s => s.Id != st.RightTableId)));
        context.AddRange(model.RelatedModels.Where(s => model.RelationModels.All(st => s.Id != st.RightTableId))
            .Select(s => new TRelations { LeftTableId = model.Id, RightTableId = s.Id }));
        model.RelationModels = null;
        model.RelatedModels = null;
        return await _baseUpdateService.UpdateAsync(model, context);
    }
}