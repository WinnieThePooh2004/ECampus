using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Interfaces.DataAccess;

namespace UniversityTimetable.Infrastructure.DataCreate;

public class DataCreateWithRelationships<TModel, TRelatedModel, TRelations> : IDataCreate<TModel>
    where TModel : class, IModel, IModelWithManyToManyRelations<TRelatedModel, TRelations>, new()
    where TRelatedModel : class, IModel, new()
    where TRelations : class, IRelationModel<TModel, TRelatedModel>, new()
{
    private readonly IRelationshipsDataAccess<TModel, TRelatedModel, TRelations> _relationships;
    private readonly IDataCreate<TModel> _baseCreate;

    public DataCreateWithRelationships(IRelationshipsDataAccess<TModel, TRelatedModel, TRelations> relationships,
        IDataCreate<TModel> baseCreate)
    {
        _relationships = relationships;
        _baseCreate = baseCreate;
    }

    public async Task<TModel> CreateAsync(TModel model, DbContext context)
    {
        _relationships.CreateRelationModels(model);
        return await _baseCreate.CreateAsync(model, context);
    }
}