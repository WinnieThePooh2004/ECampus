namespace UniversityTimetable.Shared.QueryParameters;

public abstract class QueryParameters : IQueryParameters
{
    public static int PagesInLine => 5;

    private const int MaxPageSize = 100;
    private int _pageSize = 5;
    public int PageNumber { get; set; } = 1;
    public string? SearchTerm { get; set; } = string.Empty;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public override string ToString()
    {
        return "Parameters:\n" +
               $"PageSize: {PageSize},\n" +
               $"PageNumber: {PageNumber}";
    }
}