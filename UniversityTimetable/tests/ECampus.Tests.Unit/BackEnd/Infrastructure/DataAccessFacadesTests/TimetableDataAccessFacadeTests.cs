using ECampus.Infrastructure;
using ECampus.Infrastructure.DataAccessFacades;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataAccessFacadesTests;

public class TimetableDataAccessFacadeTests
{
    private readonly TimetableDataAccessFacade _sut;
    private readonly ApplicationDbContext _context;

    public TimetableDataAccessFacadeTests()
    {
        _context = Substitute.For<ApplicationDbContext>();

        _sut = new TimetableDataAccessFacade(_context);
    }

    [Fact]
    public async Task TimetableForAuditory_ShouldReturnAuditoryAndClasses_WhenAuditoryExists()
    {
        var classes = new List<Class>
        {
            new() { AuditoryId = 10 },
            new() { AuditoryId = 10 },
            new() { AuditoryId = 11 },
            new() { AuditoryId = 11 },
        };
        _context.Classes = new DbSetMock<Class>(classes).Object;
        var auditory = new Auditory { Id = 10 };
        _context.Auditories = new DbSetMock<Auditory>(auditory).Object;

        var result = await _sut.GetTimetableForAuditoryAsync(10);

        result.Auditory.Should().Be(auditory);
        result.Group.Should().BeNull();
        result.Teacher.Should().BeNull();
        result.Classes.Should().Contain(classes[0]);
        result.Classes.Should().Contain(classes[1]);
        _context.Classes.Received().Include(c => c.Subject);
        _context.Classes.Received().Include(c => c.Group);
        _context.Classes.Received().Include(c => c.Teacher);
    }
    
    [Fact]
    public async Task TimetableForGroup_ShouldReturnAuditoryAndClasses_WhenAuditoryExists()
    {
        var classes = new List<Class>
        {
            new() { GroupId = 10 },
            new() { GroupId = 10 },
            new() { GroupId = 11 },
            new() { GroupId = 11 }
        };
        _context.Classes = new DbSetMock<Class>(classes).Object;
        var group = new Group { Id = 10 };
        _context.Groups = new DbSetMock<Group>(group).Object;

        var result = await _sut.GetTimetableForGroupAsync(10);

        result.Auditory.Should().BeNull();
        result.Group.Should().Be(group);
        result.Teacher.Should().BeNull();
        result.Classes.Should().Contain(classes[0]);
        result.Classes.Should().Contain(classes[1]);
        _context.Classes.Received().Include(c => c.Auditory);
        _context.Classes.Received().Include(c => c.Subject);
        _context.Classes.Received().Include(c => c.Teacher);
    }
    
    [Fact]
    public async Task TimetableForTeacher_ShouldReturnAuditoryAndClasses_WhenAuditoryExists()
    {
        var classes = new List<Class>
        {
            new() { TeacherId = 10 },
            new() { TeacherId = 10 },
            new() { TeacherId = 11 },
            new() { TeacherId = 11 }
        };
        _context.Classes = new DbSetMock<Class>(classes).Object;
        var teacher = new Teacher { Id = 10 };
        _context.Teachers = new DbSetMock<Teacher>(teacher).Object;

        var result = await _sut.GetTimetableForTeacherAsync(10);

        result.Teacher.Should().Be(teacher);
        result.Group.Should().BeNull();
        result.Auditory.Should().BeNull();
        result.Classes.Count().Should().Be(2);
        result.Classes.Should().Contain(classes[0]);
        result.Classes.Should().Contain(classes[1]);
        _context.Classes.Received().Include(c => c.Auditory);
        _context.Classes.Received().Include(c => c.Group);
        _context.Classes.Received().Include(c => c.Subject);
    }

    [Fact]
    public async Task TimetableForAuditory_ShouldThrowException_WhenAuditoryIsNull()
    {
        _context.Auditories = new DbSetMock<Auditory>().Object;

        await new Func<Task>(() => _sut.GetTimetableForAuditoryAsync(10)).Should()
            .ThrowAsync<ObjectNotFoundByIdException>()
            .WithMessage(new ObjectNotFoundByIdException(typeof(Auditory), 10).Message);
    }

    [Fact]
    public async Task TimetableForGroup_ShouldThrowException_WhenAuditoryIsNull()
    {
        _context.Groups = new DbSetMock<Group>().Object;

        await new Func<Task>(() => _sut.GetTimetableForGroupAsync(10)).Should()
            .ThrowAsync<ObjectNotFoundByIdException>()
            .WithMessage(new ObjectNotFoundByIdException(typeof(Group), 10).Message);
    }

    [Fact]
    public async Task TimetableForTeacher_ShouldThrowException_WhenAuditoryIsNull()
    {
        _context.Teachers = new DbSetMock<Teacher>().Object;

        await new Func<Task>(() => _sut.GetTimetableForTeacherAsync(10)).Should()
            .ThrowAsync<ObjectNotFoundByIdException>()
            .WithMessage(new ObjectNotFoundByIdException(typeof(Teacher), 10).Message);
    }
}