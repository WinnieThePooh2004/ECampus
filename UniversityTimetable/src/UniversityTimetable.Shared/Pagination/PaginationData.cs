namespace UniversityTimetable.Shared.Pagination
{
    public class PaginationData
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int MaxPageNumber => (TotalCount + PageSize - 1) / PageSize;
    }
}
