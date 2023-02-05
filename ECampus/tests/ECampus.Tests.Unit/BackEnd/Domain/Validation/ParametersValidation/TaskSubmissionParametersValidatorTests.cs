using System.Security.Claims;
using ECampus.Domain.Validation.ParametersValidators;
using ECampus.Shared.Auth;
using ECampus.Shared.Enums;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.QueryParameters;
using ECampus.Shared.Validation;
using Microsoft.AspNetCore.Http;

namespace ECampus.Tests.Unit.BackEnd.Domain.Validation.ParametersValidation;

public class TaskSubmissionParametersValidatorTests
{
    // private readonly TaskSubmissionParametersValidator _sut;
    //
    // private readonly IParametersDataValidator<TaskSubmissionParameters> _dataValidator =
    //     Substitute.For<IParametersDataValidator<TaskSubmissionParameters>>();
    //
    // private readonly ClaimsPrincipal _user = Substitute.For<ClaimsPrincipal>();
    //
    // public TaskSubmissionParametersValidatorTests()
    // {
    //     var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    //     httpContextAccessor.HttpContext = Substitute.For<HttpContext>();
    //     httpContextAccessor.HttpContext.User = _user;
    //     _sut = new TaskSubmissionParametersValidator(httpContextAccessor);
    // }
    //
    // [Fact]
    // public async Task Validate_ShouldThrowException_WhenRoleClaimNotFound()
    // {
    //     await new Func<Task>(() => _sut.ValidateAsync(new TaskSubmissionParameters())).Should()
    //         .ThrowExactlyAsync<DomainException>()
    //         .WithMessage("Role claim not found\nError code: 401");
    // }
    //
    // [Fact]
    // public async Task Validate_ShouldThrowException_WhenRoleClaimIsInvalid()
    // {
    //     _user.FindFirst(ClaimTypes.Role).Returns(new Claim("", "abc"));
    //
    //     await new Func<Task>(() => _sut.ValidateAsync(new TaskSubmissionParameters())).Should()
    //         .ThrowExactlyAsync<DomainException>()
    //         .WithMessage("No such role 'abc'\nError code: 403");
    // }
    //
    // [Fact]
    // public async Task Validate_ShouldReturnEmptyResult_WhenRoleIsAdmin()
    // {
    //     _user.FindFirst(ClaimTypes.Role).Returns(new Claim("", nameof(UserRole.Admin)));
    //
    //     var validationResult = await _sut.ValidateAsync(new TaskSubmissionParameters());
    //
    //     validationResult.Should().BeEmpty();
    // }
    //
    // [Theory]
    // [InlineData(UserRole.Guest)]
    // [InlineData(UserRole.Student)]
    // public async Task Validate_ShouldThrowException_WhenRoleIsGuestOrStudent(UserRole role)
    // {
    //     _user.FindFirst(ClaimTypes.Role).Returns(new Claim("", role.ToString()));
    //
    //     await new Func<Task>(() => _sut.ValidateAsync(new TaskSubmissionParameters())).Should()
    //         .ThrowExactlyAsync<DomainException>()
    //         .WithMessage("You must be at least teacher to perform this action\nError code: 403");
    // }
    //
    // [Fact]
    // public async Task Validate_ShouldAddError_WhenRoleIsTeacherAndTeacherIdClaimIsNull()
    // {
    //     _user.FindFirst(ClaimTypes.Role).Returns(new Claim("", nameof(UserRole.Teacher)));
    //
    //     var validationResult = await _sut.ValidateAsync(new TaskSubmissionParameters());
    //
    //     validationResult.Should().BeEquivalentTo(new ValidationResult(new ValidationError("user",
    //         "User must have claim 'TeacherId'")));
    // }
    //
    // [Fact]
    // public async Task Validate_ShouldAddError_WhenRoleIsTeacherAndTeacherIdClaimIsNotANumber()
    // {
    //     _user.FindFirst(ClaimTypes.Role).Returns(new Claim("", nameof(UserRole.Teacher)));
    //     _user.FindFirst(CustomClaimTypes.TeacherId).Returns(new Claim("", "abc"));
    //
    //     var validationResult = await _sut.ValidateAsync(new TaskSubmissionParameters());
    //
    //     validationResult.Should().BeEquivalentTo(new ValidationResult(new ValidationError("user",
    //         "Claim 'TeacherId' must be a number, not 'abc'")));
    // }
    //
    // [Fact]
    // public async Task Validate_ShouldReturnFromDataValidator_WhenNoDomainErrorsFound()
    // {
    //     _user.FindFirst(ClaimTypes.Role).Returns(new Claim("", nameof(UserRole.Teacher)));
    //     _user.FindFirst(CustomClaimTypes.TeacherId).Returns(new Claim("", "10"));
    //     var fixture = new Fixture();
    //     var dataValidationResult = new ValidationResult(fixture.CreateMany<ValidationError>(10));
    //     var parameters = fixture.Create<TaskSubmissionParameters>();
    //     _dataValidator.ValidateAsync(parameters).Returns(dataValidationResult);
    //
    //     var validationResult = await _sut.ValidateAsync(parameters);
    //     
    //     ((object)validationResult).Should().Be(validationResult);
    // }
}