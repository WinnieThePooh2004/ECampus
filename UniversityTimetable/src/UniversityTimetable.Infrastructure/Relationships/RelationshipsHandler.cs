using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Metadata.Relationships;

namespace UniversityTimetable.Infrastructure.Relationships;

public class RelationshipsHandler<TLeftTable, TRightTable, TRelationModel> 
    : IRelationshipsUpdateHandler<TLeftTable, TRightTable, TRelationModel>, IRelationshipsCreateHandler<TLeftTable, TRightTable, TRelationModel>
    where TLeftTable : IModel
    where TRightTable : IModel
    where TRelationModel : class, new()

{
    private readonly PropertyInfo _rightTableId;
    private readonly PropertyInfo _leftTableId;
    private readonly PropertyInfo _relatedModels;
    private readonly PropertyInfo _rightTableRelationModels;

    public RelationshipsHandler()
    {
        _rightTableId = typeof(TRelationModel).GetProperties().Single(p =>
            p.GetCustomAttributes(false).OfType<RightTableIdAttribute>().Any(attribute =>
                attribute.RightTableType == typeof(TRightTable)));

        _leftTableId = typeof(TRelationModel).GetProperties().Single(p =>
            p.GetCustomAttributes(false).OfType<LeftTableIdAttribute>().Any(attribute =>
                attribute.LeftTableType == typeof(TLeftTable)));

        _relatedModels = typeof(TLeftTable).GetProperties().Single(p =>
            p.PropertyType.GetInterfaces().Any(i => i == typeof(IEnumerable<TRightTable>)));
        
        _relatedModels = typeof(TLeftTable).GetProperties().Single(p =>
            p.PropertyType.GetInterfaces()
                .Any(i => i == typeof(IEnumerable<TRightTable>)));

        _rightTableRelationModels = typeof(TRightTable).GetProperties().Single(p =>
            p.PropertyType.GetInterfaces()
                .Any(i => i == typeof(IEnumerable<TRelationModel>)));
    }

    public IEnumerable<TRelationModel> TransformRelatedModelsToRelationModels(TLeftTable leftTableModel)
    {
        var relatedModels = RelatedModels(leftTableModel);
        _relatedModels.SetMethod?.Invoke(leftTableModel, new object?[] { null });
        var relationModels = relatedModels.Select(m => CreateRelationModel(leftTableModel.Id, m.Id));
        return relationModels;
    }

    public Expression<Func<TRightTable, bool>> AddedToModelExpression(TLeftTable leftTableModel)
    {
        var relatedModels = RelatedModels(leftTableModel).ToList();
        var parameter = Expression.Parameter(typeof(TRightTable), "rightTableModel");
        var idExpression = Expression.Property(parameter, "Id");
        var isInRelatedModels = relatedModels.Select(relatedModel =>
                Expression.Equal(idExpression, Expression.Constant(relatedModel.Id)))
            .Aggregate(Expression.Or);

        return Expression.Lambda<Func<TRightTable, bool>>(isInRelatedModels, parameter);
    }

    public Expression<Func<TRightTable, List<TRelationModel>?>> NavigationBetweenRightAndRelations()
    {
        var parameter = Expression.Parameter(typeof(TRightTable), "rightTableModel");
        var navigationProperty = Expression.MakeMemberAccess(parameter, _rightTableRelationModels);
        return Expression.Lambda<Func<TRightTable, List<TRelationModel>?>>(navigationProperty, parameter);
    }

    public List<TRelationModel> RelationModelsOfRightTable(TRightTable rightTableModel)
    {
        return (List<TRelationModel>?)_rightTableRelationModels.GetMethod?.Invoke(rightTableModel, null)
            ?? throw new UnreachableException();
    }

    public int LeftTableId(TRelationModel relationModel)
    {
        return (int?)_leftTableId.GetMethod?.Invoke(relationModel, null) ?? throw new UnreachableException();
    }

    public Expression<Func<TRelationModel, bool>> DeletedFromModelExpression(TLeftTable leftTableModel)
    {
        var relatedModels = RelatedModels(leftTableModel);
        var parameter = Expression.Parameter(typeof(TRelationModel), "relationModel");
        var rightIdExpression = Expression.MakeMemberAccess(parameter, _rightTableId);
        var isRelated = IsRelatedToLeft(leftTableModel.Id, parameter);

        var hasOneOfRelatedObjectsId = relatedModels
            .Select(relatedModel => Expression.Equal(rightIdExpression, Expression.Constant(relatedModel.Id)))
            .Aggregate(Expression.Or);

        return Expression.Lambda<Func<TRelationModel, bool>>(
            Expression.And(isRelated, Expression.Not(hasOneOfRelatedObjectsId)), parameter);
    }

    public void ClearRelatedModel(TLeftTable model)
    {
        _relatedModels.SetMethod?.Invoke(model, new object?[] { null });
    }

    public TRelationModel CreateRelationModel(int leftTableId, int rightTableId)
    {
        var result = new TRelationModel();
        _rightTableId.SetMethod?.Invoke(result, new object[] { rightTableId });
        _leftTableId.SetMethod?.Invoke(result, new object[] { leftTableId });
        return result;
    }

    private BinaryExpression IsRelatedToLeft(int leftTableId, Expression parameter)
    {
        var memberExpression = Expression.MakeMemberAccess(parameter, _leftTableId);
        var requiredValue = Expression.Constant(leftTableId);
        var body = Expression.Equal(memberExpression, requiredValue);
        return body;
    }

    private IEnumerable<TRightTable> RelatedModels(TLeftTable leftTableModel)
    {
        var relatedModels = (IEnumerable<TRightTable>?)_relatedModels.GetMethod?.Invoke(leftTableModel, null);
        if (relatedModels is null)
        {
            throw new RelatedModelsIsNullException(leftTableModel, typeof(TLeftTable));
        }

        return relatedModels;
    }
}