using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Interfaces.DataAccess;

namespace UniversityTimetable.Infrastructure.DataUpdate;

public class DataUpdateWithRelationships<TModel, TRelatedModel, TRelations> : IDataUpdate<TModel>
    where TModel : class, IModel, IModelWithManyToManyRelations<TRelatedModel, TRelations>, new()
    where TRelatedModel : class, IModel, new()
    where TRelations : class, IRelationModel<TModel, TRelatedModel>, new()
{
    private readonly IRelationshipsDataAccess<TModel, TRelatedModel, TRelations> _relationships;
    private readonly ApplicationDbContext _context;
    private readonly IDataUpdate<TModel> _baseUpdate;

    public DataUpdateWithRelationships(IRelationshipsDataAccess<TModel, TRelatedModel, TRelations> relationships,
        ApplicationDbContext context, IDataUpdate<TModel> baseUpdate)
    {
        _relationships = relationships;
        _context = context;
        _baseUpdate = baseUpdate;
    }

    public async Task<TModel> UpdateAsync(TModel model, DbContext context)
    {
        await _relationships.UpdateRelations(model, context);
        return await _baseUpdate.UpdateAsync(model, context);
    }
}