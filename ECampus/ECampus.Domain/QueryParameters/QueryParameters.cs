using ECampus.Domain.Data;
using ECampus.Domain.Enums;

namespace ECampus.Domain.QueryParameters;

public abstract class QueryParameters<TDto> : IQueryParameters<TDto> 
    where TDto : IDataTransferObject
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