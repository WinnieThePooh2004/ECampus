using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.Interfaces.ModelsRelationships
{
    public interface IRelationModel<TRightTable, TLeftTable> : IModel
        where TRightTable : class, IModel, new()
        where TLeftTable : class, IModel, new()
    {
        public int RightTableId { get; set; }
        public int LeftTableId { get; set; }

        public TRightTable RightTableObject { get; set; }
        public TLeftTable LeftTableObject { get; set; }
    }
}
