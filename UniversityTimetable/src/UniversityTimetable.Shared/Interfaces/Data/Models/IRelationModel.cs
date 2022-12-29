namespace UniversityTimetable.Shared.Interfaces.Data.Models;
public interface IRelationModel<TLeftTable, TRightTable>
    where TLeftTable : class, IModel
    where TRightTable : class, IModel
{
    int RightTableId { get; init; }
    int LeftTableId { init; }
}