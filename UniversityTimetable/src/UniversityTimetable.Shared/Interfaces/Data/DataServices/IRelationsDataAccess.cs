using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.Models;
// ReSharper disable UnusedTypeParameter

namespace UniversityTimetable.Shared.Interfaces.Data.DataServices;

public interface IRelationsDataAccess<TLeftTable, TRightTable, TRelations>
    where TLeftTable : IModel
    where TRightTable : IModel
    where TRelations : class, new()
{
    Task CreateRelation(int leftTableId, int rightTableId, DbContext context);

    Task DeleteRelation(int leftTableId, int rightTableId, DbContext context);
}