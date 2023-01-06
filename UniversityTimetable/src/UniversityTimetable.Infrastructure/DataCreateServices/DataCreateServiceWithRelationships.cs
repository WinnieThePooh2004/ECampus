using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Interfaces.DataAccess;

namespace UniversityTimetable.Infrastructure.DataCreateServices;

public class DataCreateServiceWithRelationships<TModel, TRelatedModel, TRelations> : IDataCreateService<TModel>
    where TModel : class, IModel, IModelWithManyToManyRelations<TRelatedModel, TRelations>, new()
    where TRelatedModel : class, IModel, new()
    where TRelations : class, IRelationModel<TModel, TRelatedModel>, new()
{
    private readonly IRelationshipsDataAccess<TModel, TRelatedModel, TRelations> _relationships;
    private readonly IDataCreateService<TModel> _baseCreateService;

    public DataCreateServiceWithRelationships(IRelationshipsDataAccess<TModel, TRelatedModel, TRelations> relationships,
        IDataCreateService<TModel> baseCreateService)
    {
        _relationships = relationships;
        _baseCreateService = baseCreateService;
    }

    public async Task<TModel> CreateAsync(TModel model, DbContext context)
    {
        _relationships.CreateRelationModels(model);
        return await _baseCreateService.CreateAsync(model, context);
    }
}