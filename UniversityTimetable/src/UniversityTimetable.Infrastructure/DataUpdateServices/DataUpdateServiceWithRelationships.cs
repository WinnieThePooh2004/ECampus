using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Interfaces.DataAccess;

namespace UniversityTimetable.Infrastructure.DataUpdateServices;

public class DataUpdateServiceWithRelationships<TModel, TRelatedModel, TRelations> : IDataUpdateService<TModel>
    where TModel : class, IModel, IModelWithManyToManyRelations<TRelatedModel, TRelations>, new()
    where TRelatedModel : class, IModel, new()
    where TRelations : class, IRelationModel<TModel, TRelatedModel>, new()
{
    private readonly IRelationshipsDataAccess<TModel, TRelatedModel, TRelations> _relationships;
    private readonly IDataUpdateService<TModel> _baseUpdateService;

    public DataUpdateServiceWithRelationships(IRelationshipsDataAccess<TModel, TRelatedModel, TRelations> relationships,
        IDataUpdateService<TModel> baseUpdateService)
    {
        _relationships = relationships;
        _baseUpdateService = baseUpdateService;
    }

    public async Task<TModel> UpdateAsync(TModel model, DbContext context)
    {
        await _relationships.UpdateRelations(model, context);
        return await _baseUpdateService.UpdateAsync(model, context);
    }
}