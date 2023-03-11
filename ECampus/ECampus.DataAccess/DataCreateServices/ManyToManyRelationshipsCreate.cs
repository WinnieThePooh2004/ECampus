using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Data;
using ECampus.Domain.Extensions;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.DataCreateServices;

public class ManyToManyRelationshipsCreate<TEntity, TRelatedEntity, TRelations> : IDataCreateService<TEntity>
    where TEntity : class, IEntity
    where TRelatedEntity : class, IEntity
    where TRelations : class, new()
{
    private readonly IDataCreateService<TEntity> _baseCreateService;
    private readonly IRelationshipsHandler<TEntity, TRelatedEntity, TRelations> _relationshipsHandler;

    public ManyToManyRelationshipsCreate(IDataCreateService<TEntity> baseCreateService,
        IRelationshipsHandler<TEntity, TRelatedEntity, TRelations> relationshipsHandler)
    {
        _baseCreateService = baseCreateService;
        _relationshipsHandler = relationshipsHandler;
    }

    public TEntity Create(TEntity model, ApplicationDbContext context)
    {
        var relatedModels = _relationshipsHandler.RelatedModels.GetFromProperty<IEnumerable<TRelatedEntity>>(model);
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