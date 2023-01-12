using System.Net;
using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Infrastructure.DataCreateServices;

public class DataCreateWithRelationships<TModel, TRelatedModel, TRelations> : IDataCreateService<TModel>
    where TModel : class, IModel, IModelWithManyToManyRelations<TRelatedModel, TRelations>, new()
    where TRelatedModel : class, IModel, new()
    where TRelations : class, IRelationModel<TModel, TRelatedModel>, new()
{
    private readonly IDataCreateService<TModel> _baseCreateService;

    public DataCreateWithRelationships(IDataCreateService<TModel> baseCreateService)
    {
        _baseCreateService = baseCreateService;
    }

    public async Task<TModel> CreateAsync(TModel model, DbContext context)
    {
        if (model.RelatedModels is null)
        {
            throw new InfrastructureExceptions(HttpStatusCode.BadRequest,
                $"Please, send related models of object of type '{typeof(TModel)}' as empty list instead of null",
                model);
        }

        model.RelationModels = model.RelatedModels
            .Select(r => new TRelations { RightTableId = r.Id, LeftTableId = model.Id }).ToList();
        model.RelatedModels = null;
        return await _baseCreateService.CreateAsync(model, context);
    }
}