using AutoMapper;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Domain.Mapping.Converters
{
    public class TimetableConvert : ITypeConverter<TimetableData, Timetable>
    {
        public Timetable Convert(TimetableData source, Timetable destination, ResolutionContext context)
            => new Timetable(context.Mapper.Map<IEnumerable<ClassDTO>>(source.Classes))
            {
                Auditory = context.Mapper.Map<AuditoryDTO>(source.Auditory),
                Group = context.Mapper.Map<GroupDTO>(source.Group),
                Teacher = context.Mapper.Map<TeacherDTO>(source.Teacher)
            };
    }
}
