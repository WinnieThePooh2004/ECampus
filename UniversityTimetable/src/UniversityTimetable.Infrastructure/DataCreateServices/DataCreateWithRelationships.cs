using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Infrastructure.DataCreateServices;

public class DataCreateWithRelationships<TModel, TRelatedModel, TRelations> : IDataCreateService<TModel>
    where TModel : class, IModel
    where TRelatedModel : class, IModel
    where TRelations : class, new()
{
    private readonly IDataCreateService<TModel> _baseCreateService;
    private readonly IRelationshipsHandler<TModel, TRelatedModel, TRelations> _relationshipsHandler;

    public DataCreateWithRelationships(IDataCreateService<TModel> baseCreateService,
        IRelationshipsHandler<TModel, TRelatedModel, TRelations> relationshipsHandler)
    {
        _baseCreateService = baseCreateService;
        _relationshipsHandler = relationshipsHandler;
    }

    public async Task<TModel> CreateAsync(TModel model, DbContext context)
    {
        var relatedModels = _relationshipsHandler.RelatedModels.GetFromProperty<IEnumerable<TRelatedModel>>(model);
        if (relatedModels is null)
        {
            return await _baseCreateService.CreateAsync(model, context);
        }

        context.AddRange(relatedModels.Select(m => 
            _relationshipsHandler.CreateRelationModel(model.Id, m.Id)));
        return await _baseCreateService.CreateAsync(model, context);
    }
}