namespace ECampus.Shared.DataContainers;

public class PaginationData
{
    public int TotalCount { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5;

    public int MaxPageNumber => (TotalCount + PageSize - 1) / PageSize;
}