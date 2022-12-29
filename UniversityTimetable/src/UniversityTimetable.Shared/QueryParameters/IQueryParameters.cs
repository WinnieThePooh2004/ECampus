using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.QueryParameters;

public interface IQueryParameters
{
    int PageNumber { get; set; }
    int PageSize { get; set; }
    
    string? SearchTerm { get; set; }
}

// ReSharper disable once UnusedTypeParameter
public interface IQueryParameters<T> : IQueryParameters
    where T : class, IModel
{
}