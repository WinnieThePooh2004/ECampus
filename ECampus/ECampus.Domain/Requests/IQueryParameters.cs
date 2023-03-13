using ECampus.Domain.Enums;
using ECampus.Domain.Responses;

namespace ECampus.Domain.Requests;

/// <summary>
/// should not be used outside of it generic version
/// </summary>
public interface IQueryParameters
{
    
}

// ReSharper disable once UnusedTypeParameter
public interface IQueryParameters<TDto> : IQueryParameters
    where TDto : IMultipleItemsResponse
{
    int PageNumber { get; set; }
    int PageSize { get; set; }
    string? OrderBy { get; set; }
    SortOrder SortOrder { get; set; }
}