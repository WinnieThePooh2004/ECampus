using AutoMapper;
using UniversityTimetable.Shared.DataContainers;

namespace UniversityTimetable.Domain.Mapping.Converters;

internal static class ProfileExtensions
{
    public static void CreateListWithPaginationDataMap<T, TTo>(this Profile profile)
        where T : class
        where TTo : class
    {
        profile.CreateMap<ListWithPaginationData<T>, ListWithPaginationData<TTo>>()
            .ConvertUsing(new ListWithPaginationDataConvert<T, TTo>());
    }
}