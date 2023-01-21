using ECampus.Shared.Enums;
using ECampus.Shared.Interfaces.Data.Models;

namespace ECampus.Shared.QueryParameters;

public interface IQueryParameters
{
    int PageNumber { get; set; }
    int PageSize { get; set; }
    string? OrderBy { get; set; }
    SortOrder SortOrder { get; set; }
}

// ReSharper disable once UnusedTypeParameter
public interface IQueryParameters<T> : IQueryParameters
    where T : class, IModel
{
}