namespace ECampus.Domain.Responses;

public class ListWithPaginationData<T>
    where T: IMultipleItemsResponse
{
    public PaginationData Metadata { get; set; } = new();
    
    public List<T> Data { get; set; } = new();
    
    public ListWithPaginationData(List<T> data, int totalCount, int pageNumber, int pageSize)
    {
        Data = data;
        Metadata = new PaginationData
        {
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public ListWithPaginationData() { }
}