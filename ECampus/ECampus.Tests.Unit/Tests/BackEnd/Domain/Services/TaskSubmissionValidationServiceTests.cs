using System.Security.Claims;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Contracts.Validation;
using ECampus.Services.Services.ValidationServices;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Validation;
using Microsoft.AspNetCore.Http;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Services;

public class TaskSubmissionValidationServiceTests
{
    private readonly TaskSubmissionValidationService _sut;
    private readonly ITaskSubmissionService _baseService = Substitute.For<ITaskSubmissionService>();
    private readonly ITaskSubmissionValidator _validator = Substitute.For<ITaskSubmissionValidator>();
    private readonly ClaimsPrincipal _user = Substitute.For<ClaimsPrincipal>();

    public TaskSubmissionValidationServiceTests()
    {
        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext = Substitute.For<HttpContext>();
        httpContextAccessor.HttpContext.User.Returns(_user);
        _sut = new TaskSubmissionValidationService(_baseService, _validator, httpContextAccessor);
    }

    [Fact]
    public async Task GetById_ShouldThrowValidationException_WhenRoleClaimIsNotFound()
    {
        await new Func<Task>(() => _sut.GetByIdAsync(10)).Should()
            .ThrowExactlyAsync<ValidationException>();
    }

    [Fact]
    public async Task GetById_ShouldThrowValidationException_WhenRoleClaimIsNotValidEnum()
    {
        _user.FindFirst(ClaimTypes.Role).Returns(new Claim("", "abc"));

        await new Func<Task>(() => _sut.GetByIdAsync(10)).Should()
            .ThrowExactlyAsync<ValidationException>();
    }

    [Theory]
    [InlineData(nameof(UserRole.Admin))]
    [InlineData(nameof(UserRole.Teacher))]
    public async Task GetById_ShouldNotValidateAnything_WhenRoleIsTeacherOrAdmin(string userRole)
    {
        var submission = new TaskSubmissionDto();
        _user.FindFirst(ClaimTypes.Role).Returns(new Claim("", userRole));
        _baseService.GetByIdAsync(10).Returns(submission);

        var result = await _sut.GetByIdAsync(10);

        result.Should().Be(submission);
    }

    [Fact]
    public async Task GetById_ShouldThrowException_WhenUserIsStudentStudentIdClaimIsNotValid()
    {
        var submission = new TaskSubmissionDto { StudentId = 10 };
        _user.FindFirst(ClaimTypes.Role).Returns(new Claim("", nameof(UserRole.Student)));
        _baseService.GetByIdAsync(10).Returns(submission);

        await new Func<Task>(() => _sut.GetByIdAsync(10)).Should()
            .ThrowExactlyAsync<DomainException>()
            .WithMessage($"You are registered as student, but your claim " +
                         $"'{nameof(UserRole.Student)}' is not valid\rError code: 403");
    }

    [Fact]
    public async Task GetById_ShouldThrowException_WhenUserIsStudentAndIdsNotMatch()
    {
        var submission = new TaskSubmissionDto { StudentId = 10 };
        _user.FindFirst(CustomClaimTypes.StudentId).Returns(new Claim("", "11"));
        _user.FindFirst(ClaimTypes.Role).Returns(new Claim("", nameof(UserRole.Student)));
        _baseService.GetByIdAsync(10).Returns(submission);

        await new Func<Task>(() => _sut.GetByIdAsync(10)).Should()
            .ThrowExactlyAsync<DomainException>()
            .WithMessage("You can view only your submissions\nError code: 403");
    }

    [Fact]
    public async Task GetByCourseId_ShouldReturnFromBaseService_WhenUserIsStudentAndIdsMatch()
    {
        var submission = new TaskSubmissionDto { StudentId = 10 };
        _user.FindFirst(CustomClaimTypes.StudentId).Returns(new Claim("", "10"));
        _user.FindFirst(ClaimTypes.Role).Returns(new Claim("", nameof(UserRole.Student)));
        _baseService.GetByIdAsync(10).Returns(submission);

        var result = await _sut.GetByIdAsync(10);

        result.Should().Be(submission);
    }

    [Fact]
    public async Task GetByCourseId_ShouldThrowException_WhenUserIsStudentStudentIdClaimIsNotValid()
    {
        var submission = new TaskSubmissionDto { StudentId = 10 };
        _baseService.GetByCourseAsync(10).Returns(submission);

        await new Func<Task>(() => _sut.GetByCourseAsync(10)).Should()
            .ThrowExactlyAsync<DomainException>()
            .WithMessage($"You are registered as student, but your claim " +
                         $"'{nameof(UserRole.Student)}' is not valid\rError code: 403");
    }

    [Fact]
    public async Task GetByCourseId_ShouldThrowException_WhenUserIsStudentAndIdsNotMatch()
    {
        var submission = new TaskSubmissionDto { StudentId = 10 };
        _user.FindFirst(CustomClaimTypes.StudentId).Returns(new Claim("", "11"));
        _baseService.GetByCourseAsync(10).Returns(submission);

        await new Func<Task>(() => _sut.GetByCourseAsync(10)).Should()
            .ThrowExactlyAsync<DomainException>()
            .WithMessage("You can view only your submissions\nError code: 403");
    }

    [Fact]
    public async Task GetById_ShouldReturnFromBaseService_WhenUserIsStudentAndIdsMatch()
    {
        var submission = new TaskSubmissionDto { StudentId = 10 };
        _user.FindFirst(CustomClaimTypes.StudentId).Returns(new Claim("", "10"));
        _baseService.GetByCourseAsync(10).Returns(submission);

        var result = await _sut.GetByCourseAsync(10);

        result.Should().Be(submission);
    }

    [Fact]
    public async Task UpdateMark_ShouldThrowException_WhenValidatorReturnsErrors()
    {
        _validator.ValidateUpdateMarkAsync(10, 10).Returns(
            new ValidationResult(new ValidationError("", "")));

        await new Func<Task>(() => _sut.UpdateMarkAsync(10, 10)).Should()
            .ThrowExactlyAsync<ValidationException>();

        await _baseService.DidNotReceive().UpdateMarkAsync(Arg.Any<int>(), Arg.Any<int>());
    }
    
    [Fact]
    public async Task UpdateMark_ShouldReturnFromBaseService_WhenMarkUpdated()
    {
        _validator.ValidateUpdateMarkAsync(10, 10).Returns(new ValidationResult());
        var submission = new TaskSubmissionDto();
        _baseService.UpdateMarkAsync(10, 10).Returns(submission);

        var result = await _sut.UpdateMarkAsync(10, 10);

        result.Should().Be(submission);
    }
    
    [Fact]
    public async Task UpdateContent_ShouldThrowException_WhenValidatorReturnsErrors()
    {
        _validator.ValidateUpdateContentAsync(10, "").Returns(
            new ValidationResult(new ValidationError("", "")));

        await new Func<Task>(() => _sut.UpdateContentAsync(10, "")).Should()
            .ThrowExactlyAsync<ValidationException>();

        await _baseService.DidNotReceive().UpdateContentAsync(Arg.Any<int>(), Arg.Any<string>());
    }
    
    [Fact]
    public async Task UpdateContent_ShouldReturnFromBaseService_WhenContentUpdated()
    {
        _validator.ValidateUpdateContentAsync(10, "").Returns(new ValidationResult());
        var submission = new TaskSubmissionDto();
        _baseService.UpdateContentAsync(10, "").Returns(submission);

        var result = await _sut.UpdateContentAsync(10, "");

        result.Should().Be(submission);
    }
}