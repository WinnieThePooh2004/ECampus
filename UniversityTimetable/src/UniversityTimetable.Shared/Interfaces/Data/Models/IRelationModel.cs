namespace UniversityTimetable.Shared.Interfaces.Data.Models;

// ReSharper disable twice UnusedTypeParameter
public interface IRelationModel<TLeftTable, TRightTable>
    where TLeftTable : class, IModel
    where TRightTable : class, IModel
{
    int RightTableId { get; init; }
    int LeftTableId { init; }
}