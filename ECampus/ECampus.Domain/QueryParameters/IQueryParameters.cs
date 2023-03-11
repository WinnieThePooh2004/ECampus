﻿using ECampus.Domain.Data;
using ECampus.Domain.Enums;

namespace ECampus.Domain.QueryParameters;

/// <summary>
/// should not be used outside of it generic version
/// </summary>
public interface IQueryParameters
{
    
}

// ReSharper disable once UnusedTypeParameter
public interface IQueryParameters<TDto> : IQueryParameters
    where TDto : IDataTransferObject
{
    int PageNumber { get; set; }
    int PageSize { get; set; }
    string? OrderBy { get; set; }
    SortOrder SortOrder { get; set; }
}