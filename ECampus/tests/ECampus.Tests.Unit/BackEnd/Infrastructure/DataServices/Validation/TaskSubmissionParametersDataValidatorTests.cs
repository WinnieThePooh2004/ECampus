using System.Security.Claims;
using ECampus.Infrastructure;
using ECampus.Infrastructure.ValidationDataAccess;
using ECampus.Shared.Auth;
using ECampus.Shared.Models;
using ECampus.Shared.Models.RelationModels;
using ECampus.Shared.QueryParameters;
using ECampus.Shared.Validation;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.AspNetCore.Http;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.Validation;

public class TaskSubmissionParametersDataValidatorTests
{
    private readonly TaskSubmissionParametersDataValidator _sut;
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();

    private readonly ClaimsPrincipal _user =
        new(new ClaimsIdentity(new List<Claim> { new(CustomClaimTypes.TeacherId, "1") }));

    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();

    public TaskSubmissionParametersDataValidatorTests()
    {
        _httpContextAccessor.HttpContext = Substitute.For<HttpContext>();
        _httpContextAccessor.HttpContext.User.Returns(_user);
        _sut = new TaskSubmissionParametersDataValidator(_context, _httpContextAccessor);
    }

    [Fact]
    public async Task Validate_ShouldAddError_WhenTeacherIdIsNotValid()
    {
        _context.Teachers = new DbSetMock<Teacher>();

        var validationResult = await _sut.ValidateAsync(new TaskSubmissionParameters { CourseTaskId = 10 });

        var errors = validationResult.ToList();
        errors.Count.Should().Be(1);
        errors.Should().Contain(new ValidationError(nameof(TaskSubmissionParameters.CourseTaskId),
            "You are not teaching this course, so you can`t view its task"));
    }

    [Fact]
    public async Task Validate_ShouldNotAddError_WhenTeacherIdIsValid()
    {
        var set = new DbSetMock<Teacher>(new Teacher
        {
            Id = 1, CourseTeachers = new List<CourseTeacher> { new() { CourseId = 10, TeacherId = 1 } },
            Courses = new List<Course> { new() { Id = 10, Tasks = new List<CourseTask> { new() { Id = 10 } } } }
        }).Object;
        _context.Teachers = set;

        var validationResult = await _sut.ValidateAsync(new TaskSubmissionParameters { CourseTaskId = 10 });

        var errors = validationResult.ToList();
        errors.Should().BeEmpty();
    }
}