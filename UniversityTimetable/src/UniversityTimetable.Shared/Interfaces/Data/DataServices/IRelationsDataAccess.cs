using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Interfaces.Data.DataServices;

public interface IRelationsDataAccess
{
    Task CreateRelation<TRelations, TLeftTable, TRightTable>(int leftTableId, int rightTableId, DbContext context)
        where TRelations : IRelationModel<TLeftTable, TRightTable>, new()
        where TRightTable : class, IModel 
        where TLeftTable : class, IModel;
    
    Task DeleteRelation<TRelations, TLeftTable, TRightTable>(int leftTableId, int rightTableId, DbContext context)
        where TRelations : IRelationModel<TLeftTable, TRightTable>, new()
        where TRightTable : class, IModel 
        where TLeftTable : class, IModel;

}