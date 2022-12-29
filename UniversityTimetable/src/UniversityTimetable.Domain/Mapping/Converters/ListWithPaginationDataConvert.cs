using AutoMapper;
using UniversityTimetable.Shared.DataContainers;

namespace UniversityTimetable.Domain.Mapping.Converters
{
    public class ListWithPaginationDataConvert<T, TTo> : ITypeConverter<ListWithPaginationData<T>, ListWithPaginationData<TTo>> 
        where T : class
        where TTo : class
    {
        public ListWithPaginationData<TTo> Convert(ListWithPaginationData<T> source, ListWithPaginationData<TTo> destination, ResolutionContext context)
        {
            return new ListWithPaginationData<TTo>
            {
                Data = context.Mapper.Map<List<TTo>>(source.Data),
                Metadata = source.Metadata
            };
        }
    }
}
