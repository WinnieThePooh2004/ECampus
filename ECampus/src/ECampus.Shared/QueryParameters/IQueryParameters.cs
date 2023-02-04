using ECampus.Shared.Enums;

namespace ECampus.Shared.QueryParameters;

public interface IQueryParameters
{
    int PageNumber { get; set; }
    int PageSize { get; set; }
    string? OrderBy { get; set; }
    SortOrder SortOrder { get; set; }
}