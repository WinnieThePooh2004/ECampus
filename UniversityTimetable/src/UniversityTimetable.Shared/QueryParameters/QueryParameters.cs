using UniversityTimetable.Shared.Enums;

namespace UniversityTimetable.Shared.QueryParameters;

public abstract class QueryParameters : IQueryParameters
{
    private const int MaxPageSize = 100;
    private int _pageSize = 5;
    public int PageNumber { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public string? OrderBy { get; set; }
    public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
}