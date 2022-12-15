using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.QueryParameters
{
    public interface IQueryParameters
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
        string SearchTerm { get; set; }
    }

    public interface IQueryParameters<T> : IQueryParameters
        where T : class, IModel
    {
        IQueryable<T> Filter(IQueryable<T> items);
    }
}