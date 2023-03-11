using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Data;
using ECampus.Shared.Extensions;

namespace ECampus.DataAccess.DataCreateServices;

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

    public TModel Create(TModel model, ApplicationDbContext context)
    {
        var relatedModels = _relationshipsHandler.RelatedModels.GetFromProperty<IEnumerable<TRelatedModel>>(model);
        if (relatedModels is null)
        {
            return _baseCreateService.Create(model, context);
        }

        var relationModels = relatedModels.Select(m =>
            _relationshipsHandler.CreateRelationModel(model.Id, m.Id)).ToList();
        
        _relationshipsHandler.RelationModels.SetFromProperty(model, relationModels);
        _relationshipsHandler.RelatedModels.SetPropertyAsNull(model);
        return _baseCreateService.Create(model, context);
    }
}