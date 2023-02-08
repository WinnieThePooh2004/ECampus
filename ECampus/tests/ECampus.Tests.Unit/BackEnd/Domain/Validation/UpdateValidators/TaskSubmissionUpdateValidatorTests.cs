using System.Security.Claims;
using ECampus.Domain.Validation.UpdateValidators;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using ValidationResult = ECampus.Shared.Validation.ValidationResult;
using FluentValidationResult = FluentValidation.Results.ValidationResult;

namespace ECampus.Tests.Unit.BackEnd.Domain.Validation.UpdateValidators;

public class TaskSubmissionUpdateValidatorTests
{
    // private const string Content = nameof(_taskSubmissionDataValidator);
    //
    // private readonly TaskSubmissionValidator _sut;
    //
    // private readonly ITaskSubmissionDataValidator _taskSubmissionDataValidator =
    //     Substitute.For<ITaskSubmissionDataValidator>();
    //
    // private readonly IValidator<TaskSubmissionDto> _fluentValidator = Substitute.For<IValidator<TaskSubmissionDto>>();
    // private readonly ClaimsPrincipal _user = Substitute.For<ClaimsPrincipal>();
    //
    // public TaskSubmissionUpdateValidatorTests()
    // {
    //     var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    //     httpContextAccessor.HttpContext = Substitute.For<HttpContext>();
    //     httpContextAccessor.HttpContext.User = _user;
    //
    //     _sut = new TaskSubmissionValidator(httpContextAccessor, _fluentValidator);
    // }
    //
    // [Fact]
    // public async Task ValidateUpdateContent__ShouldNotDoMore_WhenFluentValidatorReturnErrors()
    // {
    //     _fluentValidator.ValidateAsync(Arg.Is<TaskSubmissionDto>(t => t.SubmissionContent == Content))
    //         .Returns(new FluentValidationResult(new List<ValidationFailure> { new("name", "error") }));
    //
    //     var result = await _sut.ValidateUpdateContentAsync(10, Content);
    //
    //     result.Should().BeEquivalentTo(new ValidationResult(new ValidationError("name", "error")));
    // }
    //
    // [Fact]
    // public async Task ValidateUpdateContent_ShouldReturnError_WhenUserDoesNotHaveStudentIdClaim()
    // {
    //     _fluentValidator.ValidateAsync(Arg.Is<TaskSubmissionDto>(t => t.SubmissionContent == Content))
    //         .Returns(new FluentValidationResult());
    //
    //     var result = await _sut.ValidateUpdateContentAsync(10, Content);
    //
    //     result.Should().BeEquivalentTo(new ValidationResult(new ValidationError("_user",
    //         $"Current user does now have a claim of type {nameof(CustomClaimTypes.StudentId)}")));
    // }
    //
    // [Fact]
    // public async Task ValidateUpdateContent_ShouldReturnError_WhenUsersStudentIdClaimValueIsNotANumber()
    // {
    //     _fluentValidator.ValidateAsync(Arg.Is<TaskSubmissionDto>(t => t.SubmissionContent == Content))
    //         .Returns(new FluentValidationResult());
    //     _user.FindFirst(CustomClaimTypes.StudentId).Returns(new Claim("", "abc"));
    //
    //     var result = await _sut.ValidateUpdateContentAsync(10, Content);
    //
    //     result.Should().BeEquivalentTo(new ValidationResult(new ValidationError("studentIdClaim",
    //         $"Current user`s claim of type {nameof(CustomClaimTypes.StudentId)} must be number, not abc")));
    // }
    //
    // [Fact]
    // public async Task ValidateUpdateContent_ShouldReturnError_WhenUsersStudentIdClaimValueNotEqualToUserIdFromDb()
    // {
    //     _fluentValidator.ValidateAsync(Arg.Is<TaskSubmissionDto>(t => t.SubmissionContent == Content))
    //         .Returns(new FluentValidationResult());
    //     _user.FindFirst(CustomClaimTypes.StudentId).Returns(new Claim("", "10"));
    //     _taskSubmissionDataValidator.LoadSubmissionData(10).Returns(new TaskSubmission { StudentId = 9 });
    //
    //     var result = await _sut.ValidateUpdateContentAsync(10, Content);
    //
    //     result.Should().BeEquivalentTo(new ValidationResult(new ValidationError("studentId",
    //         "Current user is logged in as student with id = 10, " +
    //         "but to make changes to this submission user`s student id must be 9")));
    // }
    //
    // [Fact]
    // public async Task ValidateUpdateContent_ShouldReturnNoErrors_WhenAllIsGood()
    // {
    //     _fluentValidator.ValidateAsync(Arg.Is<TaskSubmissionDto>(t => t.SubmissionContent == Content))
    //         .Returns(new FluentValidationResult());
    //     _user.FindFirst(CustomClaimTypes.StudentId).Returns(new Claim("", "10"));
    //     _taskSubmissionDataValidator.LoadSubmissionData(10).Returns(new TaskSubmission { StudentId = 10 });
    //
    //     var result = await _sut.ValidateUpdateContentAsync(10, Content);
    //
    //     result.Should().BeEmpty();
    // }
    //
    // [Fact]
    // public async Task ValidateUpdateMark_ShouldAddError_WhenTeacherIdClaimIsNull()
    // {
    //     var result = await _sut.ValidateUpdateMarkAsync(1, 1);
    //
    //     result.Should().BeEquivalentTo(new ValidationResult(new ValidationError("_user",
    //         $"Current user does now have a claim of type {nameof(CustomClaimTypes.TeacherId)}")));
    // }
    //
    // [Fact]
    // public async Task ValidateUpdateMark_ShouldAddError_WhenTeacherIdClaimValueIsNotNumber()
    // {
    //     _user.FindFirst(CustomClaimTypes.TeacherId).Returns(new Claim("", "abc"));
    //
    //     var result = await _sut.ValidateUpdateMarkAsync(1, 1);
    //
    //     result.Should().BeEquivalentTo(new ValidationResult(new ValidationError("teacherIdClaim",
    //         $"Current user`s claim of type {nameof(CustomClaimTypes.TeacherId)} must be number, not abc")));
    // }
    //
    // [Fact]
    // public async Task ValidateUpdateMark_ShouldAddError_WhenMarkIsTooBig()
    // {
    //     _user.FindFirst(CustomClaimTypes.TeacherId).Returns(new Claim("", "10"));
    //     _taskSubmissionDataValidator.LoadSubmissionData(1).Returns(new TaskSubmission
    //         { CourseTask = new CourseTask { MaxPoints = 1 } });
    //
    //     var result = await _sut.ValidateUpdateMarkAsync(1, 100);
    //
    //     result.Should().BeEquivalentTo(new ValidationResult(new ValidationError("mark",
    //         $"Max mark for this task is 1, but you are passed 100")));
    // }
    //
    // [Fact]
    // public async Task ValidateUpdateMark_ShouldReturnFromDataValidator_WhenNoErrorsOnDomainLevelFound()
    // {
    //     _user.FindFirst(CustomClaimTypes.TeacherId).Returns(new Claim("", "10"));
    //     _taskSubmissionDataValidator.LoadSubmissionData(1).Returns(new TaskSubmission
    //         { CourseTask = new CourseTask { MaxPoints = 100 } });
    //     var dalValidationResult = new ValidationResult();
    //     _taskSubmissionDataValidator.ValidateTeacherId(Arg.Any<int>(), Arg.Any<int>()).Returns(dalValidationResult);
    //
    //     var result = await _sut.ValidateUpdateMarkAsync(1, 100);
    //
    //     ((object)result).Should().Be(dalValidationResult);
    // }
}