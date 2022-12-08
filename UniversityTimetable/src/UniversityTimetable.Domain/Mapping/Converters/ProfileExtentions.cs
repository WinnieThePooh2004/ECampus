using AutoMapper;
using UniversityTimetable.Shared.Interfaces.Models;
using UniversityTimetable.Shared.Pagination;

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

        public static void CreateTimetableMap<TClass, TClassTo>(this Profile profile)
            where TClass : IClass
            where TClassTo : IClass
        {
            profile.CreateMap<Timetable<TClass>, Timetable<TClassTo>>()
                .ConvertUsing<TimetableConvert<TClass, TClassTo>>();
        }
    }
}
