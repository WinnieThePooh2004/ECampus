using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Infrastructure.DataUpdateServices;

public class DataUpdateServiceWithRelationships<TModel, TRelatedModel, TRelations> : IDataUpdateService<TModel>
    where TModel : class, IModel
    where TRelatedModel : class, IModel
    where TRelations : class, new()
{
    private readonly IDataUpdateService<TModel> _baseUpdateService;
    private readonly IRelationshipsUpdateHandler<TModel, TRelatedModel, TRelations> _relationshipsHandler;

    public DataUpdateServiceWithRelationships(IDataUpdateService<TModel> baseUpdateService,
        IRelationshipsUpdateHandler<TModel, TRelatedModel, TRelations> relationshipsHandler)
    {
        _baseUpdateService = baseUpdateService;
        _relationshipsHandler = relationshipsHandler;
    }

    public async Task<TModel> UpdateAsync(TModel model, DbContext context)
    {
        if (_relationshipsHandler.RelatedModelsAreNullOrEmpty(model))
        {
            return await _baseUpdateService.UpdateAsync(model, context);
        }
        await RemoveLostRelations(model, context);

        await AddNewRelations(model, context);

        _relationshipsHandler.ClearRelatedModel(model);

        return await _baseUpdateService.UpdateAsync(model, context);
    }

    private async Task AddNewRelations(TModel model, DbContext context)
    {
        var relationsToAddExpression = _relationshipsHandler.AddedToModelExpression(model);

        var navExpression = _relationshipsHandler.NavigationBetweenRightAndRelations();
        var allRelatedModels = context.Set<TRelatedModel>()
            .Include(navExpression)
            .Where(relationsToAddExpression)
            .AsAsyncEnumerable();

        var modelsToAdd = await TransformRelatedModelsToRelationModels(model, allRelatedModels);

        context.AddRange(modelsToAdd);
    }

    private async Task<List<TRelations>> TransformRelatedModelsToRelationModels(TModel model, IAsyncEnumerable<TRelatedModel> allRelatedModels)
    {
        var modelsToAdd = new List<TRelations>();

        await foreach (var relatedModel in allRelatedModels)
        {
            var relationModels = _relationshipsHandler.RelationModelsOfRightTable(relatedModel);
            if (relationModels.All(relationModel => _relationshipsHandler.LeftTableId(relationModel) != model.Id))
            {
                modelsToAdd.Add(_relationshipsHandler.CreateRelationModel(model.Id, relatedModel.Id));
            }
        }

        return modelsToAdd;
    }

    private async Task RemoveLostRelations(TModel model, DbContext context)
    {
        var relationsToDelete = context
            .Set<TRelations>()
            .AsNoTracking()
            .Where(_relationshipsHandler.DeletedFromModelExpression(model));

        context.RemoveRange(await relationsToDelete.ToListAsync());
    }
}