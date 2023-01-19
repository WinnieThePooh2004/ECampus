using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Interfaces.Data.DataServices;

// ReSharper disable once UnusedTypeParameter
public interface IRelationshipsCreateHandler<in TLeftTable, TRightTable, out TRelationModel>
    where TLeftTable : IModel 
    where TRightTable : IModel 
    where TRelationModel : class, new()
{
    IEnumerable<TRelationModel> TransformRelatedModelsToRelationModels(TLeftTable leftTableModel);

}