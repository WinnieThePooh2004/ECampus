using System.Security.Claims;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.Domain.Auth;
using ECampus.Domain.Entities;
using ECampus.Domain.Enums;
using ECampus.Domain.Exceptions.DomainExceptions;
using ECampus.Domain.Requests.TaskSubmission;
using ECampus.Domain.Validation;
using ECampus.Services.Validation.ParametersValidators;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using ECampus.Tests.Unit.Extensions;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Validation.ParametersValidation;

public class TaskSubmissionParametersValidatorTests
{
    private readonly TaskSubmissionParametersValidator _sut;
    private readonly ClaimsPrincipal _user = Substitute.For<ClaimsPrincipal>();
    private readonly IDataAccessFacade _dataAccess = Substitute.For<IDataAccessFacade>();

    public TaskSubmissionParametersValidatorTests()
    {
        _sut = new TaskSubmissionParametersValidator(_user.CreateContextAccessor(), _dataAccess);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenRoleClaimNotFound()
    {
        await new Func<Task>(() => _sut.ValidateAsync(new TaskSubmissionParameters())).Should()
            .ThrowExactlyAsync<DomainException>()
            .WithMessage("Role claim not found or it is not number\nError code: 401");
    }

    [Theory]
    [InlineData(nameof(UserRole.Student))]
    [InlineData(nameof(UserRole.Guest))]
    public async Task Validate_ShouldThrowException_WhenRoleClaimIsStudentOrGuest(string roleName)
    {
        _user.FindFirst(ClaimTypes.Role).Returns(new Claim("", roleName));

        await new Func<Task>(() => _sut.ValidateAsync(new TaskSubmissionParameters())).Should()
            .ThrowExactlyAsync<DomainException>()
            .WithMessage("You must be at least teacher to call this action\nError code: 403");
    }

    [Fact]
    public async Task Validate_ShouldReturnEmptyResult_WhenRoleIsAdmin()
    {
        _user.FindFirst(ClaimTypes.Role).Returns(new Claim("", nameof(UserRole.Admin)));

        var validationResult = await _sut.ValidateAsync(new TaskSubmissionParameters());

        validationResult.Should().BeEmpty();
    }

    [Fact]
    public async Task Validate_ShouldAddError_WhenRoleIsTeacherAndTeacherIdClaimIsNull()
    {
        _user.FindFirst(ClaimTypes.Role).Returns(new Claim("", nameof(UserRole.Teacher)));

        var validationResult = await _sut.ValidateAsync(new TaskSubmissionParameters());

        validationResult.Should().BeEquivalentTo(new ValidationResult(new ValidationError("user",
            "User must have claim 'TeacherId'")));
    }

    [Fact]
    public async Task Validate_ShouldAddError_WhenRoleIsTeacherAndTeacherIdClaimIsNotANumber()
    {
        _user.FindFirst(ClaimTypes.Role).Returns(new Claim("", nameof(UserRole.Teacher)));
        _user.FindFirst(CustomClaimTypes.TeacherId).Returns(new Claim("", "abc"));

        var validationResult = await _sut.ValidateAsync(new TaskSubmissionParameters());

        validationResult.Should().BeEquivalentTo(new ValidationResult(new ValidationError("user",
            "Claim 'TeacherId' must be a number, not 'abc'")));
    }

    [Fact]
    public async Task Validate_ShouldReturnEmpty_WhenTeacherNotTeachCourse()
    {
        _user.FindFirst(ClaimTypes.Role).Returns(new Claim("", nameof(UserRole.Teacher)));
        _user.FindFirst(CustomClaimTypes.TeacherId).Returns(new Claim("", "10"));
        var parameters = new TaskSubmissionParameters { CourseTaskId = 10 };
        var teacher = new Teacher().ToAsyncQueryable();
        _dataAccess.GetByParameters<Teacher, TeacherRelatedToTaskParameters>(Arg.Any<TeacherRelatedToTaskParameters>())
            .Returns(teacher);

        var validationResult = await _sut.ValidateAsync(parameters);

        ((object)validationResult).Should().Be(validationResult);
    }
    
    [Fact]
    public async Task Validate_ShouldReturnEmpty_WhenTeacherTeachesCourse()
    {
        _user.FindFirst(ClaimTypes.Role).Returns(new Claim("", nameof(UserRole.Teacher)));
        _user.FindFirst(CustomClaimTypes.TeacherId).Returns(new Claim("", "10"));
        var teacher = new Teacher { Id = 10 }.ToAsyncQueryable();
        _dataAccess.GetByParameters<Teacher, TeacherRelatedToTaskParameters>(Arg.Any<TeacherRelatedToTaskParameters>())
            .Returns(teacher);

        var result = await _sut.ValidateAsync(new TaskSubmissionParameters());

        result.Should().BeEmpty();
    }
}