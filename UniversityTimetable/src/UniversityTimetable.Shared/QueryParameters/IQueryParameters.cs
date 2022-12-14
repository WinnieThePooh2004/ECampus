namespace UniversityTimetable.Shared.QueryParameters
{
    public interface IQueryParameters
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
        string SearchTerm { get; set; }
    }
    public interface IQueryParameters<T> where T : class
    {
        IQueryable<T> Filter(IQueryable<T> items);
    }
}