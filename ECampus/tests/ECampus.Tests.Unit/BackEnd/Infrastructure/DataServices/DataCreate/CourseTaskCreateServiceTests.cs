using ECampus.Infrastructure;
using ECampus.Infrastructure.DataCreateServices;
using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.Mocks.EntityFramework;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.DataCreate;

public class CourseTaskCreateServiceTests
{
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();
    private readonly IDataCreateService<CourseTask> _baseCreate = Substitute.For<IDataCreateService<CourseTask>>();
    private readonly CourseTaskCreateService _sut;

    public CourseTaskCreateServiceTests()
    {
        _sut = new CourseTaskCreateService(_baseCreate);
    }

    [Fact]
    public async Task Create_ShouldAddSubmissionsSelectedFromDb()
    {
        var students = Enumerable.Range(0, 5).Select(i => new Student { Id = i }).ToList();
        var course = new Course { Id = 10, Groups = new List<Group> { new() { Students = students } } };
        _context.Courses = new DbSetMock<Course>(course);
        var expectedSubmissions = Enumerable.Range(0, 5).Select(i => new TaskSubmission { StudentId = i }).ToList();
        var task = new CourseTask { CourseId = 10 };
        _baseCreate.CreateAsync(task, _context).Returns(task);

        var result = await _sut.CreateAsync(task, _context);

        result.Should().Be(task);
        await _baseCreate.Received(1).CreateAsync(task, _context);
        result.Submissions.Should().BeEquivalentTo(expectedSubmissions);
    }
}