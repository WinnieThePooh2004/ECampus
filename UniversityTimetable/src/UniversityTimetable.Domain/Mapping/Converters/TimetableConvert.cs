using AutoMapper;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Domain.Mapping.Converters
{
    public class TimetableConvert : ITypeConverter<TimetableData, Timetable>
    {
        public Timetable Convert(TimetableData source, Timetable destination, ResolutionContext context)
            => new Timetable(context.Mapper.Map<IEnumerable<ClassDto>>(source.Classes))
            {
                Auditory = context.Mapper.Map<AuditoryDto>(source.Auditory),
                Group = context.Mapper.Map<GroupDto>(source.Group),
                Teacher = context.Mapper.Map<TeacherDto>(source.Teacher)
            };
    }
}
