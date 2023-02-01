using System.Linq.Expressions;
using ECampus.Infrastructure.Extensions;
using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Data;
using ECampus.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataUpdateServices;

public class ManyToManyRelationshipsUpdate<TModel, TRelatedModel, TRelations> : IDataUpdateService<TModel>
    where TModel : class, IModel
    where TRelatedModel : class, IModel
    where TRelations : class, new()
{
    private readonly IDataUpdateService<TModel> _baseUpdateService;
    private readonly IRelationshipsHandler<TModel, TRelatedModel, TRelations> _relationshipsHandler;

    public ManyToManyRelationshipsUpdate(IDataUpdateService<TModel> baseUpdateService,
        IRelationshipsHandler<TModel, TRelatedModel, TRelations> relationshipsHandler)
    {
        _baseUpdateService = baseUpdateService;
        _relationshipsHandler = relationshipsHandler;
    }

    public async Task<TModel> UpdateAsync(TModel model, ApplicationDbContext context)
    {
        var relatedModels = _relationshipsHandler.RelatedModels.GetFromProperty<ICollection<TRelatedModel>>(model);
        if (relatedModels is null)
        {
            return await _baseUpdateService.UpdateAsync(model, context);
        }

        await RemoveLostRelations(model, context, relatedModels);
        await AddNewRelations(model, context, relatedModels);

        _relationshipsHandler.RelatedModels.SetPropertyAsNull(model);

        return await _baseUpdateService.UpdateAsync(model, context);
    }

    private async Task AddNewRelations(TModel model, DbContext context, ICollection<TRelatedModel> relatedModels)
    {
        var relatedModelsIdsList = relatedModels.Select(m => m.Id.ToString()).ToList();
        var rightTableName = context.Model.FindEntityType(typeof(TRelatedModel))!.GetTableName();
        var relationTableName = context.Model.FindEntityType(typeof(TRelations))!.GetTableName();
        var rightTableIdName = _relationshipsHandler.RightTableId.Name;
        var leftTableIdName = _relationshipsHandler.LeftTableId.Name;
        var relatedModelsIds = relatedModels.Any() ? string.Join(", ", relatedModelsIdsList) : "-1";

        var sqlQuery = CreateSqlQuery(model, rightTableName, relatedModelsIds, leftTableIdName, relationTableName,
            rightTableIdName);

        var rightTableIds =
            await context.Set<TRelatedModel>()
                .FromSqlRaw(sqlQuery).ToListAsync();

        var modelsToAdd = rightTableIds.Select(relatedModel =>
            _relationshipsHandler.CreateRelationModel(model.Id, relatedModel.Id)).ToList();
        
        context.AddRange(modelsToAdd);
    }

    private static string CreateSqlQuery(TModel model, string? rightTableName, string relatedModelsIds,
        string leftTableIdName, string? relationTableName, string rightTableIdName)
    {
        var sqlQuery = $"""
                        SELECT * FROM [{rightTableName}] AS [RightTable]
                        WHERE [RightTable].[Id] IN ({relatedModelsIds})
                        AND {model.Id} NOT IN (SELECT [Relations].[{leftTableIdName}] FROM [{relationTableName}]
                        AS [Relations] WHERE [Relations].[{rightTableIdName}] = [RightTable].[Id])
                        """;
        return sqlQuery;
    }

    private async Task RemoveLostRelations(TModel model, DbContext context, ICollection<TRelatedModel> relatedModels)
    {
        var relationsToDelete = await context
            .Set<TRelations>()
            .AsNoTracking()
            .Where(DeletedFromModelExpression(model, relatedModels))
            .ToListAsync();
        
        context.RemoveRange(relationsToDelete);
    }

    private Expression<Func<TRelations, bool>> DeletedFromModelExpression(TModel model, ICollection<TRelatedModel> relatedModels)
    {
        var parameter = Expression.Parameter(typeof(TRelations), "relationModel");
        var rightIdExpression = Expression.MakeMemberAccess(parameter, _relationshipsHandler.RightTableId);
        var isRelated = IsRelated(model.Id, parameter);

        if (!relatedModels.Any())
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