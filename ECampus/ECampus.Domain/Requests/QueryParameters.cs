using ECampus.Domain.Enums;
using ECampus.Domain.Responses;

namespace ECampus.Domain.Requests;

public abstract class QueryParameters<TDto> : IQueryParameters<TDto> 
    where TDto : IMultipleItemsResponse
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