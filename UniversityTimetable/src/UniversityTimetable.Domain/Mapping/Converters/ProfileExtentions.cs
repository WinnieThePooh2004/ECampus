using AutoMapper;
using UniversityTimetable.Shared.DataContainers;

namespace UniversityTimetable.Domain.Mapping.Converters
{
    static class ProfileExtentions
    {
        public static void CreateListWithPaginationDataMap<T, TTo>(this Profile profile)
            where T : class
            where TTo : class
        {
            profile.CreateMap<ListWithPaginationData<T>, ListWithPaginationData<TTo>>()
                .ConvertUsing<ListWithPaginationDataConvert<T, TTo>>();
        }
    }
}
