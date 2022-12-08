using AutoMapper;
using UniversityTimetable.Shared.Interfaces.Models;
using UniversityTimetable.Shared.Pagination;

namespace UniversityTimetable.Domain.Mapping.Converters
{
    public class TimetableConvert<TClass, TClassTo> : ITypeConverter<Timetable<TClass>, Timetable<TClassTo>>
        where TClass : IClass
        where TClassTo : IClass
    {
        public Timetable<TClassTo> Convert(Timetable<TClass> source, Timetable<TClassTo> destination, ResolutionContext context)
        {
            var result = new Timetable<TClassTo>()
            {
                GroupId = source.GroupId,
                TeacherId = source.TeacherId,
                AuditoryId = source.AuditoryId,
            };
            foreach (var @class in source.DailyClasses)
            {
                if(@class is not null)
                {
                    var mappedClass = context.Mapper.Map<TClassTo>(@class);
                    Console.WriteLine(mappedClass);
                    result.Add(mappedClass);
                }
            }
            return result;
        }
    }
}
