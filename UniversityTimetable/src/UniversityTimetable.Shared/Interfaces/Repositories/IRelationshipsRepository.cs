using System.Linq.Expressions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.ModelsRelationships;

namespace UniversityTimetable.Shared.Interfaces.Repositories
{
    /// <summary>
    /// objects implementing this interface should not invoke DbContext.SaveChanges(Async) 
    /// it only helps to create/update relation models between TLeftTable and TRightTable objects 
    /// if you need to save some changes, do it in main repository
    /// </summary>
    public interface IRelationshipsRepository<in TLeftTable, TRightTable, TRelations>
        where TLeftTable : class, IModel, IModelWithManyToManyRelations<TRightTable, TRelations>, new()
        where TRightTable : class, IModel, new()
        where TRelations : class, IRelationModel<TLeftTable, TRightTable>, new()
    {
        /// <summary>
        /// this method replaces all navigation properties of type TLeftTable ot TRelations, 
        /// if you need to use objects from related, save it before using this method
        /// </summary>
        /// <param name="model"></param>
        void CreateRelationModels(TLeftTable model);
        /// <summary>
        /// this method will call database to find required relation models, if your model already has relation models, use UpdateLoadedRelations 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task UpdateRelations(TLeftTable model);
        Task UpdateLoadedRelations(TLeftTable model);
    }
}
