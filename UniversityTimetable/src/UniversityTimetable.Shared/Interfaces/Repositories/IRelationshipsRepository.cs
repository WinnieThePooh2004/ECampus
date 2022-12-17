using System.Linq.Expressions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;

namespace UniversityTimetable.Shared.Interfaces.Repositories
{
    /// <summary>
    /// objects of this interface should not invoke DbContext.SaveChanges(Async)
    /// if you need to do this use it in main repository
    /// </summary>
    public interface IRelationshipsRepository<TRightTable, TLeftTable, TRelations>
        where TRightTable : class, IModel, IModelWithRelations<TLeftTable>, new()
        where TLeftTable : class, IModel, new()
        where TRelations : IRelationModel<TRightTable, TLeftTable>, new()
    {
        /// <summary>
        /// this method replaces all navigation properties of type TLeftTable ot TRalations, 
        /// if you need to use objects from related, save it before using this method
        /// </summary>
        /// <param name="model"></param>
        void CreateRelationModels(TRightTable model);
        Task UpdateRelations(TRightTable model);
        void UpdateLoadedRelations(TRightTable model);
    }
}
