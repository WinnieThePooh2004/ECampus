using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.Interfaces.ModelsRelationships
{
    public interface IModelWithManyToManyRelations<TRelations>
        where TRelations : class, IModel
    {
        List<TRelations> RelatedModels { get; set; }

    }
}
