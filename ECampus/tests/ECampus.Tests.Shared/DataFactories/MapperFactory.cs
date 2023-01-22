using AutoMapper;
using ECampus.Domain.Mapping;

namespace ECampus.Tests.Shared.DataFactories;

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
            new StudentProfile(),
            new SubjectProfile(),
            new TeacherProfile(),
            new UserProfile(),
            new CourseProfile(),
            new CourseTaskProfile(),
            new TaskSubmissionProfile()
        })).CreateMapper();
    
}