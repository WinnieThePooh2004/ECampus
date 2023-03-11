using System.Linq.Expressions;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Data;
using ECampus.Domain.Extensions;
using ECampus.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataUpdateServices;

public class ManyToManyRelationshipsUpdate<TEntity, TRelatedEntity, TRelations> : IDataUpdateService<TEntity>
    where TEntity : class, IEntity
    where TRelatedEntity : class, IEntity
    where TRelations : class, new()
{
    private readonly IDataUpdateService<TEntity> _baseUpdateService;
    private readonly IRelationshipsHandler<TEntity, TRelatedEntity, TRelations> _relationshipsHandler;

    public ManyToManyRelationshipsUpdate(IDataUpdateService<TEntity> baseUpdateService,
        IRelationshipsHandler<TEntity, TRelatedEntity, TRelations> relationshipsHandler,
        CancellationToken token = default)
    {
        _baseUpdateService = baseUpdateService;
        _relationshipsHandler = relationshipsHandler;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, ApplicationDbContext context, CancellationToken token = default)
    {
        var relatedModels = _relationshipsHandler.RelatedModels.GetFromProperty<ICollection<TRelatedEntity>>(entity);
        if (relatedModels is null)
        {
            return await _baseUpdateService.UpdateAsync(entity, context, token);
        }

        await RemoveLostRelations(entity, context, relatedModels, token);
        await AddNewRelations(entity, context, relatedModels, token);

        _relationshipsHandler.RelatedModels.SetPropertyAsNull(entity);

        return await _baseUpdateService.UpdateAsync(entity, context, token);
    }

    private async Task AddNewRelations(TEntity model, DbContext context, ICollection<TRelatedEntity> relatedModels,
        CancellationToken token = default)
    {
        var relatedModelsIdsList = relatedModels.Select(m => m.Id.ToString()).ToList();
        var rightTableName = context.Model.FindEntityType(typeof(TRelatedEntity))!.GetTableName();
        var relationTableName = context.Model.FindEntityType(typeof(TRelations))!.GetTableName();
        var rightTableIdName = _relationshipsHandler.RightTableId.Name;
        var leftTableIdName = _relationshipsHandler.LeftTableId.Name;
        var relatedModelsIds = relatedModels.Any() ? string.Join(", ", relatedModelsIdsList) : "-1";

        var sqlQuery = CreateSqlQuery(model, rightTableName, relatedModelsIds, leftTableIdName, relationTableName,
            rightTableIdName);

        var rightTableIds =
            await context.Set<TRelatedEntity>()
                .FromSqlRaw(sqlQuery).ToListAsync(token);

        var modelsToAdd = rightTableIds.Select(relatedModel =>
            _relationshipsHandler.CreateRelationModel(model.Id, relatedModel.Id)).ToList();

        context.AddRange(modelsToAdd);
    }

    private static string CreateSqlQuery(TEntity model, string? rightTableName, string relatedModelsIds,
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

    private async Task RemoveLostRelations(TEntity model, DbContext context, ICollection<TRelatedEntity> relatedModels,
        CancellationToken token)
    {
        var relationsToDelete = await context
            .Set<TRelations>()
            .AsNoTracking()
            .Where(DeletedFromModelExpression(model, relatedModels))
            .ToListAsync(token);

        context.RemoveRange(relationsToDelete);
    }

    private Expression<Func<TRelations, bool>> DeletedFromModelExpression(TEntity model,
        ICollection<TRelatedEntity> relatedModels)
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