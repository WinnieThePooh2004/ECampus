namespace UniversityTimetable.Shared.DataContainers;

public class ListWithPaginationData<T>
{
    public required PaginationData Metadata { get; set; }
    public required List<T> Data { get; set; }
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