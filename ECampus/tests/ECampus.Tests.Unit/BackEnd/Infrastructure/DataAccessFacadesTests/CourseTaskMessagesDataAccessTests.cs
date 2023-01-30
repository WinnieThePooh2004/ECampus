using ECampus.Infrastructure.DataAccessFacades;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataAccessFacadesTests;

public class CourseTaskMessagesDataAccessTests
{
    private readonly CourseTaskMessageDataAccess _sut;
    private readonly DbContext _context = Substitute.For<DbContext>();

    public CourseTaskMessagesDataAccessTests()
    {
        _sut = new CourseTaskMessageDataAccess(_context);
    }

    [Fact]
    public async Task LoadDataForSendMessage_ShouldThrowException_WhenNoCourseFound()
    {
        var set = (DbSet<Course>)new DbSetMock<Course>();
        _context.Set<Course>().Returns(set);

        await new Func<Task>(() => _sut.LoadDataForSendMessage(10)).Should()
            .ThrowAsync<ObjectNotFoundByIdException>()
            .WithMessage(new ObjectNotFoundByIdException(typeof(Course), 10).Message);
    }

    [Fact]
    public async Task LoadDataForSendMessage_ShouldReturnData_WhenCourseExist()
    {
        var students = Enumerable.Range(0, 5).Select(i => new Student { UserEmail = $"email{i}" }).ToList();
        var course = new Course
        {
            Id = 10,
            Name = "Name",
            Groups = new List<Group> { new() { Students = students } }
        };
        var set = (DbSet<Course>)new DbSetMock<Course>(course);
        _context.Set<Course>().Returns(set);

        var result = await _sut.LoadDataForSendMessage(10);

        result.CourseName.Should().Be("Name");
        result.StudentEmails.Should().BeEquivalentTo(students.Select(s => s.UserEmail));
    }
}