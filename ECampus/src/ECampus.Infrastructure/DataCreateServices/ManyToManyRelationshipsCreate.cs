using ECampus.Shared.Extensions;
using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Interfaces.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataCreateServices;

public class ManyToManyRelationshipsCreate<TModel, TRelatedModel, TRelations> : IDataCreateService<TModel>
    where TModel : class, IModel
    where TRelatedModel : class, IModel
    where TRelations : class, new()
{
    private readonly IDataCreateService<TModel> _baseCreateService;
    private readonly IRelationshipsHandler<TModel, TRelatedModel, TRelations> _relationshipsHandler;

    public ManyToManyRelationshipsCreate(IDataCreateService<TModel> baseCreateService,
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

        var relationModels = relatedModels.Select(m =>
            _relationshipsHandler.CreateRelationModel(model.Id, m.Id)).ToList();
        
        _relationshipsHandler.RelationModels.SetFromProperty(model, relationModels);
        _relationshipsHandler.RelatedModels.SetPropertyAsNull(model);
        return await _baseCreateService.CreateAsync(model, context);
    }
}