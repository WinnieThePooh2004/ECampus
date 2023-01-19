using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Infrastructure.DataCreateServices;

public class DataCreateWithRelationships<TModel, TRelatedModel, TRelations> : IDataCreateService<TModel>
    where TModel : class, IModel
    where TRelatedModel : class, IModel
    where TRelations : class, new()
{
    private readonly IDataCreateService<TModel> _baseCreateService;
    private readonly IRelationshipsCreateHandler<TModel, TRelatedModel, TRelations> _relationshipsHandler;

    public DataCreateWithRelationships(IDataCreateService<TModel> baseCreateService,
        IRelationshipsCreateHandler<TModel, TRelatedModel, TRelations> relationshipsHandler)
    {
        _baseCreateService = baseCreateService;
        _relationshipsHandler = relationshipsHandler;
    }

    public async Task<TModel> CreateAsync(TModel model, DbContext context)
    {
        context.AddRange(_relationshipsHandler.TransformRelatedModelsToRelationModels(model));
        return await _baseCreateService.CreateAsync(model, context);
    }
}