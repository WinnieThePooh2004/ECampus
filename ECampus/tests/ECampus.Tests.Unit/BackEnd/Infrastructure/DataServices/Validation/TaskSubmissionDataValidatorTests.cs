using ECampus.Infrastructure;
using ECampus.Infrastructure.ValidationDataAccess;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using ECampus.Tests.Shared.Mocks.EntityFramework;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.Validation;

public class TaskSubmissionDataValidatorTests
{
    private readonly TaskSubmissionDataValidator _sut;
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();

    public TaskSubmissionDataValidatorTests()
    {
        _sut = new TaskSubmissionDataValidator(_context);
    }

    [Fact]
    public async Task LoadSubmissionData_ShouldReturnFromDb_IfExistInDb()
    {
        var submissions = new TaskSubmission { Id = 1 };
        _context.TaskSubmissions = new DbSetMock<TaskSubmission>(submissions);

        var result = await _sut.LoadSubmissionData(1);

        result.Should().Be(submissions);
    }

    [Fact]
    public async Task LoadSubmissionData_ShouldThrowException_WhenNoFoundInDb()
    {
        _context.TaskSubmissions = new DbSetMock<TaskSubmission>();

        await new Func<Task>(() => _sut.LoadSubmissionData(1)).Should()
            .ThrowAsync<ObjectNotFoundByIdException>()
            .WithMessage(new ObjectNotFoundByIdException(typeof(TaskSubmission), 1).Message);
    }

    [Fact]
    public async Task ValidateTeacherId_ShouldReturnError_WhenAuthorsGroupIsNull()
    {
        _context.Groups = new DbSetMock<Group>();

        var result = await _sut.ValidateTeacherId(1, 1);
        var errors = result.ToList();

        errors.Should().Contain(new ValidationError("taskSubmissionId", "Cannot group of submission author with id"));
        errors.Count.Should().Be(1);
    }

    [Fact]
    public async Task ValidateTeacherId_ShouldReturnError_WhenAuthorsGroupIsNotNullAndTeacherDoesNotTeachCourse()
    {
        _context.Groups = new DbSetMock<Group>(new List<Group>
        {
            new()
            {
                Students = new List<Student> { new() { Submissions = new List<TaskSubmission> { new() { Id = 10 } } } }
            }
        });
        _context.Teachers = new DbSetMock<Teacher>();

        var result = await _sut.ValidateTeacherId(1, 10);
        var errors = result.ToList();

        errors.Should().Contain(new ValidationError("teacherId",
            "Current user is logged in as teacher with id = 1," +
            " but this teacher does not teaches submission author`s group"));
        errors.Count.Should().Be(1);
    }

    [Fact]
    public async Task ValidateTeacherId_ShouldReturnEmpty_WhenAuthorsGroupIsNotNullAndTeacherTeachesCourse()
    {
        var group = new Group
        {
            Id = 1,
            Students = new List<Student> { new() { Submissions = new List<TaskSubmission> { new() { Id = 10 } } } }
        };
        var teacher = new Teacher
        {
            Id = 1, Courses = new List<Course> { new() { Groups = new List<Group> { group } } }
        };
        _context.Groups = new DbSetMock<Group>(group);
        _context.Teachers = new DbSetMock<Teacher>(teacher);

        var result = await _sut.ValidateTeacherId(1, 10);
        var errors = result.ToList();

        errors.Should().BeEmpty();
    }
}