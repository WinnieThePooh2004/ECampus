using System.Reflection;
using ECampus.Shared.Interfaces.Data.Models;

// ReSharper disable UnusedTypeParameter

namespace ECampus.Infrastructure.Interfaces;

public interface IRelationshipsHandler<in TLeftTable, out TRightTable, out TRelationModel>
    where TLeftTable : IModel
    where TRightTable : IModel
    where TRelationModel : class, new()
{
    PropertyInfo RightTableId { get; }
    PropertyInfo LeftTableId { get; }
    PropertyInfo RelatedModels { get; }
    PropertyInfo RelationModels { get; }
}