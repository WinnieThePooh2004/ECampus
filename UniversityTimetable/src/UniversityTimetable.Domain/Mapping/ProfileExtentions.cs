using AutoMapper;
using UniversityTimetable.Shared.Pagination;

namespace UniversityTimetable.Domain.Mapping
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
