using System.Linq.Expressions;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.Interfaces.ModelsRelationships
{
    public interface IRelationModel<TLeftTable, TRightTable> : IModel
        where TLeftTable: class, IModel
        where  TRightTable: class, IModel
    {
        int RightTableId { get; set; }
        int LeftTableId { get; set; }
    }
}
