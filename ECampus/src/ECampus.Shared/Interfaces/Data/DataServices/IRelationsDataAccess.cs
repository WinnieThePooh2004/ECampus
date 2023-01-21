using ECampus.Shared.Interfaces.Data.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable UnusedTypeParameter

namespace ECampus.Shared.Interfaces.Data.DataServices;

public interface IRelationsDataAccess<TLeftTable, TRightTable, TRelations>
    where TLeftTable : IModel
    where TRightTable : IModel
    where TRelations : class, new()
{
    Task CreateRelation(int leftTableId, int rightTableId, DbContext context);

    Task DeleteRelation(int leftTableId, int rightTableId, DbContext context);
}