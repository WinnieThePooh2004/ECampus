using System.Security.Claims;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.DataSelectParameters;
using ECampus.Domain.Validation.UpdateValidators;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using ECampus.Tests.Unit.Extensions;
using FluentValidation;
using FluentValidation.Results;
using ValidationResult = ECampus.Shared.Validation.ValidationResult;
using FluentValidationResult = FluentValidation.Results.ValidationResult;

namespace ECampus.Tests.Unit.BackEnd.Domain.Validation.UpdateValidators;

public class TaskSubmissionUpdateValidatorTests
{
    private const string Content = nameof(TaskSubmissionUpdateValidatorTests);

    private readonly TaskSubmissionValidator _sut;
    private readonly IValidator<TaskSubmissionDto> _fluentValidator = Substitute.For<IValidator<TaskSubmissionDto>>();
    private readonly ClaimsPrincipal _user = Substitute.For<ClaimsPrincipal>();
    private readonly IDataAccessManager _dataAccess = Substitute.For<IDataAccessManager>();

    public TaskSubmissionUpdateValidatorTests()
    {
        _sut = new TaskSubmissionValidator(_user.CreateContextAccessor(), _fluentValidator, _dataAccess);
    }

    [Fact]
    public async Task ValidateUpdateContent__ShouldNotDoMore_WhenFluentValidatorReturnErrors()
    {
        _fluentValidator.ValidateAsync(Arg.Is<TaskSubmissionDto>(t => t.SubmissionContent == Content))
            .Returns(new FluentValidationResult(new List<ValidationFailure> { new("name", "error") }));

        var result = await _sut.ValidateUpdateContentAsync(10, Content);

        result.Should().BeEquivalentTo(new ValidationResult(new ValidationError("name", "error")));
    }

    [Fact]
    public async Task ValidateUpdateContent_ShouldReturnError_WhenUserDoesNotHaveStudentIdClaim()
    {
        _fluentValidator.ValidateAsync(Arg.Is<TaskSubmissionDto>(t => t.SubmissionContent == Content))
            .Returns(new FluentValidationResult());

        var result = await _sut.ValidateUpdateContentAsync(10, Content);

        result.Should().BeEquivalentTo(new ValidationResult(new ValidationError("user",
            "User must have claim 'StudentId'")));
    }

    [Fact]
    public async Task ValidateUpdateContent_ShouldReturnError_WhenUsersStudentIdClaimValueIsNotANumber()
    {
        _fluentValidator.ValidateAsync(Arg.Is<TaskSubmissionDto>(t => t.SubmissionContent == Content))
            .Returns(new FluentValidationResult());
        _user.FindFirst(CustomClaimTypes.StudentId).Returns(new Claim("", "abc"));

        var result = await _sut.ValidateUpdateContentAsync(10, Content);

        result.Should().BeEquivalentTo(new ValidationResult(new ValidationError("user",
            "Claim 'StudentId' must be a number, not 'abc'")));
    }

    [Fact]
    public async Task ValidateUpdateContent_ShouldReturnError_WhenUsersStudentIdClaimValueNotEqualToUserIdFromDb()
    {
        _fluentValidator.ValidateAsync(Arg.Is<TaskSubmissionDto>(t => t.SubmissionContent == Content))
            .Returns(new FluentValidationResult());
        _user.FindFirst(CustomClaimTypes.StudentId).Returns(new Claim("", "10"));
        var submission = new TaskSubmission { StudentId = 15 };
        var dbOutput = submission.ToAsyncQueryable();
        _dataAccess.GetByParameters<TaskSubmission, TaskSubmissionValidationParameters>(
            Arg.Any<TaskSubmissionValidationParameters>()).Returns(dbOutput);

        var result = await _sut.ValidateUpdateContentAsync(10, Content);

        result.Should().BeEquivalentTo(new ValidationResult(new ValidationError("studentId",
            "Current user is logged in as student with id = 10, " +
            "but to make changes to this submission user`s student id must be 15")));
    }

    [Fact]
    public async Task ValidateUpdateContent_ShouldReturnNoErrors_WhenAllIsGood()
    {
        _fluentValidator.ValidateAsync(Arg.Is<TaskSubmissionDto>(t => t.SubmissionContent == Content))
            .Returns(new FluentValidationResult());
        _user.FindFirst(CustomClaimTypes.StudentId).Returns(new Claim("", "10"));
        var submission = new TaskSubmission { StudentId = 10 };
        var dbOutput = submission.ToAsyncQueryable();
        _dataAccess.GetByParameters<TaskSubmission, TaskSubmissionValidationParameters>(
            Arg.Any<TaskSubmissionValidationParameters>()).Returns(dbOutput);

        var result = await _sut.ValidateUpdateContentAsync(10, Content);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ValidateUpdateMark_ShouldAddError_WhenTeacherIdClaimIsNull()
    {
        var result = await _sut.ValidateUpdateMarkAsync(1, 1);

        result.Should().BeEquivalentTo(new ValidationResult(new ValidationError("_user",
            $"Current user does now have a claim of type {nameof(CustomClaimTypes.TeacherId)}")));
    }

    [Fact]
    public async Task ValidateUpdateMark_ShouldAddError_WhenTeacherIdClaimValueIsNotNumber()
    {
        _user.FindFirst(CustomClaimTypes.TeacherId).Returns(new Claim("", "abc"));

        var result = await _sut.ValidateUpdateMarkAsync(1, 1);

        result.Should().BeEquivalentTo(new ValidationResult(new ValidationError("teacherIdClaim",
            $"Current user`s claim of type {nameof(CustomClaimTypes.TeacherId)} must be number, not abc")));
    }

    [Fact]
    public async Task ValidateUpdateMark_ShouldAddError_WhenMarkIsBiggerThanMax()
    {
        _user.FindFirst(CustomClaimTypes.TeacherId).Returns(new Claim("", "10"));
        var submission = new TaskSubmission { CourseTask = new CourseTask { MaxPoints = 1 } };
        var dbOutput = submission.ToAsyncQueryable();
        _dataAccess.GetByParameters<TaskSubmission, TaskSubmissionValidationParameters>(
            Arg.Any<TaskSubmissionValidationParameters>()).Returns(dbOutput);

        var result = await _sut.ValidateUpdateMarkAsync(1, 100);

        result.Should().BeEquivalentTo(new ValidationResult(new ValidationError("mark",
            $"Max mark for this task is 1, but you are passed 100")));
    }

    [Fact]
    public async Task ValidateUpdateMark_ShouldAddError_WhenTeacherDoNotTeachesCourse()
    {
        _user.FindFirst(CustomClaimTypes.TeacherId).Returns(new Claim("", "10"));
        var submission = new TaskSubmission { CourseTask = new CourseTask { MaxPoints = 100 } };
        var dbOutput = submission.ToAsyncQueryable();
        _dataAccess.GetByParameters<TaskSubmission, TaskSubmissionValidationParameters>(
            Arg.Any<TaskSubmissionValidationParameters>()).Returns(dbOutput);
        var teacher = new Teacher { Id = 20 }.ToAsyncQueryable();
        _dataAccess.GetByParameters<Teacher, TeacherByCourseParameters>(Arg.Any<TeacherByCourseParameters>())
            .Returns(teacher);

        var result = await _sut.ValidateUpdateMarkAsync(1, 10);

        result.Should().BeEquivalentTo(new ValidationResult(new ValidationError("teacherId",
            "Current user is logged in as teacher with id = 10," +
            " but this teacher does not teaches submission author`s group")));
    }

    [Fact]
    public async Task ValidateUpdateMark_ShouldReturnEmpty_WhenTeacherNotTeachesCourse()
    {
        _user.FindFirst(CustomClaimTypes.TeacherId).Returns(new Claim("", "10"));
        var submission = new TaskSubmission { CourseTask = new CourseTask { MaxPoints = 100 } };
        var dbOutput = submission.ToAsyncQueryable();
        _dataAccess.GetByParameters<TaskSubmission, TaskSubmissionValidationParameters>(
            Arg.Any<TaskSubmissionValidationParameters>()).Returns(dbOutput);
        var teacher = new Teacher { Id = 10 }.ToAsyncQueryable();
        _dataAccess.GetByParameters<Teacher, TeacherByCourseParameters>(Arg.Any<TeacherByCourseParameters>())
            .Returns(teacher);

        var result = await _sut.ValidateUpdateMarkAsync(1, 10);

        result.Should().BeEmpty();
    }
}