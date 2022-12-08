namespace UniversityTimetable.Shared.DataContainers
{
    public class ListWithPaginationData<T>
    {
        public PaginationData Metadata { get; set; }
        public List<T> Data { get; set; }
        public ListWithPaginationData(List<T> data, int totalCount, int pageNumber, int PageSize)
        {
            Data = data;
            Metadata = new()
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = PageSize
            };
        }

        public ListWithPaginationData() { }
    }
}
