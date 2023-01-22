using System.Linq.Expressions;
using ECampus.Shared.Extensions;
using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Interfaces.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataUpdateServices;

public class DataUpdateServiceWithRelationships<TModel, TRelatedModel, TRelations> : IDataUpdateService<TModel>
    where TModel : class, IModel
    where TRelatedModel : class, IModel
    where TRelations : class, new()
{
    private readonly IDataUpdateService<TModel> _baseUpdateService;
    private readonly IRelationshipsHandler<TModel, TRelatedModel, TRelations> _relationshipsHandler;

    public DataUpdateServiceWithRelationships(IDataUpdateService<TModel> baseUpdateService,
        IRelationshipsHandler<TModel, TRelatedModel, TRelations> relationshipsHandler)
    {
        _baseUpdateService = baseUpdateService;
        _relationshipsHandler = relationshipsHandler;
    }

    public async Task<TModel> UpdateAsync(TModel model, DbContext context)
    {
        if (_relationshipsHandler.RelatedModels.GetFromProperty<object>(model) is null)
        {
            return await _baseUpdateService.UpdateAsync(model, context);
        }

        await RemoveLostRelations(model, context);
        await AddNewRelations(model, context);

        _relationshipsHandler.RelatedModels.SetPropertyAsNull(model);

        return await _baseUpdateService.UpdateAsync(model, context);
    }

    private async Task AddNewRelations(TModel model, DbContext context)
    {
        var relatedModels =
            _relationshipsHandler.RelatedModels.GetFromProperty<IEnumerable<TRelatedModel>>(model)!.Select(m =>
                m.Id.ToString()).ToList();
        var rightTableName = context.Model.FindEntityType(typeof(TRelatedModel))!.GetTableName();
        var relationTableName = context.Model.FindEntityType(typeof(TRelations))!.GetTableName();
        var rightTableIdName = _relationshipsHandler.RightTableId.Name;
        var leftTableIdName = _relationshipsHandler.LeftTableId.Name;
        var relatedModelsIds = relatedModels.Any() ? string.Join(", ", relatedModels) : "-1";
        var sqlQuery = $"""
            SELECT * FROM {rightTableName}  AS RightTable
            WHERE RightTable.Id IN ({relatedModelsIds})
            AND {model.Id} NOT IN (SELECT Relations.{leftTableIdName} FROM {relationTableName} 
            AS Relations WHERE Relations.{rightTableIdName} = RightTable.Id)
        """;
        
        var rightTableIds =
            await context.Set<TRelatedModel>()
                .FromSqlRaw(sqlQuery).ToListAsync();

        var modelsToAdd = rightTableIds.Select(relatedModel =>
            _relationshipsHandler.CreateRelationModel(model.Id, relatedModel.Id)).ToList();
        if (!modelsToAdd.Any())
        {
            return;
        }
        context.AddRange(modelsToAdd);
    }

    private async Task RemoveLostRelations(TModel model, DbContext context)
    {
        var relationsToDelete = await context
            .Set<TRelations>()
            .AsNoTracking()
            .Where(DeletedFromModelExpression(model))
            .ToListAsync();

        if (!relationsToDelete.Any())
        {
            return;
        }

        context.RemoveRange(relationsToDelete);
    }

    private Expression<Func<TRelations, bool>> DeletedFromModelExpression(TModel model)
    {
        var relatedModels = _relationshipsHandler.RelatedModels.GetFromProperty<List<TRelatedModel>>(model);
        var parameter = Expression.Parameter(typeof(TRelations), "relationModel");
        var rightIdExpression = Expression.MakeMemberAccess(parameter, _relationshipsHandler.RightTableId);
        var isRelated = IsRelated(model.Id, parameter);

        if (relatedModels is null || !relatedModels.Any())
        {
            return Expression.Lambda<Func<TRelations, bool>>(isRelated, parameter);
        }
        
        var hasOneOfRelatedObjectsId = relatedModels
            .Select(relatedModel => Expression.Equal(rightIdExpression, Expression.Constant(relatedModel.Id)))
            .Aggregate(Expression.Or);

        return Expression.Lambda<Func<TRelations, bool>>(
            Expression.And(isRelated, Expression.Not(hasOneOfRelatedObjectsId)), parameter);
    }

    private BinaryExpression IsRelated(int leftTableId, Expression parameter)
    {
        var memberExpression = Expression.MakeMemberAccess(parameter, _relationshipsHandler.LeftTableId);
        var requiredValue = Expression.Constant(leftTableId);
        var body = Expression.Equal(memberExpression, requiredValue);
        return body;
    }
}