using ECampus.Infrastructure.DataCreateServices;
using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.DataCreate;

public class StudentCreateServiceTests
{
    private readonly StudentCreateService _sut;
    private readonly DbContext _context = Substitute.For<DbContext>();
    private readonly IDataCreateService<Student> _baseCreate = Substitute.For<IDataCreateService<Student>>();

    public StudentCreateServiceTests()
    {
        _sut = new StudentCreateService(_baseCreate);
    }

    [Fact]
    public async Task Create_ShouldAddSubmissions()
    {
        var tasks = Enumerable.Range(0, 5).Select(i => new CourseTask { Id = i }).ToList();
        var group = new Group { Id = 10, Courses = new List<Course> { new() { Tasks = tasks } } };
        var set = (DbSet<Group>)new DbSetMock<Group>(group);
        _context.Set<Group>().Returns(set);
        var student = new Student { GroupId = 10 };
        var expectedSubmissions = tasks.Select(t => new TaskSubmission { CourseTaskId = t.Id });
        _baseCreate.CreateAsync(student, _context).Returns(student);

        var result = await _sut.CreateAsync(student, _context);

        result.Should().Be(student);
        await _baseCreate.Received().CreateAsync(student, _context);
        student.Submissions.Should().BeEquivalentTo(expectedSubmissions);
    }
}