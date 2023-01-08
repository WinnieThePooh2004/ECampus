using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.ValidationDataAccess;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Tests.Shared.Mocks.EntityFramework;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataServices.Validation;

public class ClassValidationDataAccessTests
{
    private readonly ClassValidationDataAccess _sut;
    private readonly ApplicationDbContext _context;

    public ClassValidationDataAccessTests()
    {
        _context = Substitute.For<ApplicationDbContext>();
        _sut = new ClassValidationDataAccess(_context);
    }

    [Fact]
    public async Task LoadRequiredDataForCreate_ShouldReturnFromDb()
    {
        var (subject, auditory, group, teacher) = CreateRelatedData();
        CreateDbSets(subject, auditory, group, teacher);
        var @class = new Class { SubjectId = 10, AuditoryId = 10, GroupId = 10, TeacherId = 10 };

        var result = await _sut.LoadRequiredDataForCreateAsync(@class);

        result.Subject.Should().Be(subject);
        result.Auditory.Should().Be(auditory);
        result.Group.Should().Be(group);
        result.Teacher.Should().Be(teacher);
        result.SubjectId.Should().Be(@class.SubjectId);
        result.AuditoryId.Should().Be(@class.AuditoryId);
        result.GroupId.Should().Be(@class.GroupId);
        result.TeacherId.Should().Be(@class.TeacherId);
    }
    
    [Fact]
    public async Task LoadRequiredDataForUpdate_ShouldReturnFromDb()
    {
        var (subject, auditory, group, teacher) = CreateRelatedData();
        CreateDbSets(subject, auditory, group, teacher);
        var @class = new Class { SubjectId = 10, AuditoryId = 10, GroupId = 10, TeacherId = 10 };

        var result = await _sut.LoadRequiredDataForUpdateAsync(@class);

        result.Subject.Should().Be(subject);
        result.Auditory.Should().Be(auditory);
        result.Group.Should().Be(group);
        result.Teacher.Should().Be(teacher);
        result.SubjectId.Should().Be(@class.SubjectId);
        result.AuditoryId.Should().Be(@class.AuditoryId);
        result.GroupId.Should().Be(@class.GroupId);
        result.TeacherId.Should().Be(@class.TeacherId);
    }

    private void CreateDbSets(Subject subject, Auditory auditory, Group group, Teacher teacher)
    {
        _context.Auditories = new DbSetMock<Auditory>(auditory);
        _context.Groups = new DbSetMock<Group>(group);
        _context.Subjects = new DbSetMock<Subject>(subject);
        _context.Teachers = new DbSetMock<Teacher>(teacher);
    }

    private static (Subject, Auditory, Group, Teacher) CreateRelatedData()
    {
        return (new Subject { Id = 10 }, new Auditory { Id = 10 }, new Group { Id = 10 }, new Teacher { Id = 10 });
    }
}