using UniversityTimetable.Shared.Pagination;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Api.Models
{
    /// <summary>
    /// this type of object shouldn`t be used out of the 'Index' methods of controllers
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TParams"></typeparam>
    public class IndexModel<TData, TParams>
        where TData : class
        where TParams : QueryParameters
    {
        public List<TData> Data { get; set; } = default!;
        public PaginationData Metadata { get; set; } = default!;
        public TParams Parameters { get; set; } = default!;

        public IndexModel(ListWithPaginationData<TData> data, TParams parameters)
        {
            Data = data.Data;
            Metadata = data.Metadata;
            Parameters = parameters;
        }

        public static IndexModel<TData, TParams> Create(ListWithPaginationData<TData> data, TParams parameters)
            => new(data, parameters);
    }

    public static class IndexModel
    {
        public static IndexModel<TData, TParams> Create<TData, TParams>(ListWithPaginationData<TData> data, TParams parameters)
            where TData : class
            where TParams : QueryParameters
            => new(data, parameters);

    }
}
