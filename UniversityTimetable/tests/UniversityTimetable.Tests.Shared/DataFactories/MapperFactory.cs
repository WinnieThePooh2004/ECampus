using AutoMapper;
using UniversityTimetable.Domain.Mapping;

namespace UniversityTimetable.Tests.Shared.DataFactories;

public static class MapperFactory
{
    private static IMapper? _mapper;
    public static IMapper Mapper =>
        _mapper ??= new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
        {
            new AuditoryProfile(),
            new ClassProfile(),
            new DepartmentProfile(),
            new FacultyProfile(),
            new GroupProfile(),
            new SubjectProfile(),
            new TeacherProfile(),
            new UserProfile()
        })).CreateMapper();
    
}