using ECampus.DataAccess.DataCreateServices;
using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.Mocks.EntityFramework;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.DataCreate;

public class StudentCreateServiceTests
{
    // private readonly StudentCreateService _sut;
    // private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();
    // private readonly IDataCreateService<Student> _baseCreate = Substitute.For<IDataCreateService<Student>>();
    //
    // public StudentCreateServiceTests()
    // {
    //     _sut = new StudentCreateService(_baseCreate);
    // }
    //
    // [Fact]
    // public async Task Create_ShouldAddSubmissions()
    // {
    //     var tasks = Enumerable.Range(0, 5).Select(i => new CourseTask { Id = i }).ToList();
    //     var group = new Group { Id = 10, Courses = new List<Course> { new() { Tasks = tasks } } };
    //     _context.Groups = new DbSetMock<Group>(group);
    //     var student = new Student { GroupId = 10 };
    //     var expectedSubmissions = tasks.Select(t => new TaskSubmission { CourseTaskId = t.Id });
    //     _baseCreate.CreateAsync(student, _context).Returns(student);
    //
    //     var result = await _sut.CreateAsync(student, _context);
    //
    //     result.Should().Be(student);
    //     await _baseCreate.Received().CreateAsync(student, _context);
    //     student.Submissions.Should().BeEquivalentTo(expectedSubmissions);
    // }
}