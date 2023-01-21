﻿using System.Reflection;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Metadata.Relationships;

namespace UniversityTimetable.Infrastructure.Relationships;

public class RelationshipsHandler<TLeftTable, TRightTable, TRelationModel>
    : IRelationshipsHandler<TLeftTable, TRightTable, TRelationModel>
    where TLeftTable : IModel
    where TRightTable : IModel
    where TRelationModel : class, new()

{
    public PropertyInfo RightTableId { get; }
    public PropertyInfo LeftTableId { get; }
    public PropertyInfo RelatedModels { get; }

    public RelationshipsHandler()
    {
        RightTableId = typeof(TRelationModel).GetProperties().Single(p =>
            p.GetCustomAttributes(false).OfType<RightTableIdAttribute>().Any(attribute =>
                attribute.RightTableType == typeof(TRightTable)));

        LeftTableId = typeof(TRelationModel).GetProperties().Single(p =>
            p.GetCustomAttributes(false).OfType<LeftTableIdAttribute>().Any(attribute =>
                attribute.LeftTableType == typeof(TLeftTable)));

        RelatedModels = typeof(TLeftTable).GetProperties().Single(p =>
            p.PropertyType.GetInterfaces().Any(i => i == typeof(IEnumerable<TRightTable>)));
    }
}