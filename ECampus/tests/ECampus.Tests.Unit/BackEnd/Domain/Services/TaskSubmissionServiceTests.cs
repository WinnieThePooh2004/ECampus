using System.Security.Claims;
using ECampus.Domain.Services;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.Interfaces.Messaging;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using ECampus.Tests.Shared.DataFactories;
using Microsoft.AspNetCore.Http;

namespace ECampus.Tests.Unit.BackEnd.Domain.Services;

public class TaskSubmissionServiceTests
{
    private readonly TaskSubmissionService _sut;
    private readonly ITaskSubmissionRepository _repository = Substitute.For<ITaskSubmissionRepository>();
    private readonly ITaskSubmissionValidator _validator = Substitute.For<ITaskSubmissionValidator>();
    private readonly ClaimsPrincipal _user = Substitute.For<ClaimsPrincipal>();
    private readonly ISnsMessenger _snsMessenger = Substitute.For<ISnsMessenger>();

    public TaskSubmissionServiceTests()
    {
        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.Returns(Substitute.For<HttpContext>());
        httpContextAccessor.HttpContext?.User.Returns(_user);
        _sut = new TaskSubmissionService(_repository, _validator, httpContextAccessor, _snsMessenger,
            MapperFactory.Mapper);
    }

    [Fact]
    public async Task UpdateMark_ShouldCallRepository_WhenIsValid()
    {
        var errors = new ValidationResult();
        _validator.ValidateUpdateMarkAsync(1, 1).Returns(errors);
        _repository.UpdateMarkAsync(1, 1).Returns(new TaskSubmission { CourseTask = new CourseTask() });

        await _sut.UpdateMarkAsync(1, 1);

        await _repository.Received(1).UpdateMarkAsync(1, 1);
    }

    [Fact]
    public async Task UpdateMark_ShouldThrow_WhenIsNotValid()
    {
        var errors = new ValidationResult(new ValidationError("name", "message"));
        _validator.ValidateUpdateMarkAsync(1, 1).Returns(errors);

        await new Func<Task>(() => _sut.UpdateMarkAsync(1, 1)).Should()
            .ThrowAsync<ValidationException>()
            .WithMessage(new ValidationException(typeof(TaskSubmissionDto), errors).Message);

        await _repository.DidNotReceive().UpdateMarkAsync(Arg.Any<int>(), Arg.Any<int>());
    }

    [Fact]
    public async Task UpdateContent_ShouldCallRepository_WhenIsValid()
    {
        var errors = new ValidationResult();
        _validator.ValidateUpdateContentAsync(1, "abc").Returns(errors);
        _repository.UpdateContentAsync(1, "abc").Returns(new TaskSubmission { CourseTask = new CourseTask() });

        await _sut.UpdateContentAsync(1, "abc");

        await _repository.Received(1).UpdateContentAsync(1, "abc");
    }

    [Fact]
    public async Task UpdateContent_ShouldThrow_WhenIsNotValid()
    {
        var errors = new ValidationResult(new ValidationError("name", "message"));
        _validator.ValidateUpdateContentAsync(1, "abc").Returns(errors);

        await new Func<Task>(() => _sut.UpdateContentAsync(1, "abc")).Should()
            .ThrowAsync<ValidationException>()
            .WithMessage(new ValidationException(typeof(TaskSubmissionDto), errors).Message);

        await _repository.DidNotReceive().UpdateContentAsync(Arg.Any<int>(), Arg.Any<string>());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFromRepository()
    {
        var submission = new TaskSubmission { Id = 10 };
        _repository.GetByIdAsync(10).Returns(submission);

        var result = await _sut.GetByIdAsync(10);

        result.Id.Should().Be(10);
    }

    [Fact]
    public async Task GetByCourse_ShouldThrowException_WhenNotFoundStudentIdClaim()
    {
        await new Func<Task>(() => _sut.GetByCourse(10)).Should()
            .ThrowAsync<DomainException>()
            .WithMessage("Current user is not logged in as student or claim " +
                         $"'{CustomClaimTypes.StudentId}' is not a number\nError code: 403");
        await _repository.DidNotReceive().GetByStudentAndCourse(Arg.Any<int>(), Arg.Any<int>());
    }

    [Fact]
    public async Task GetByCourse_ShouldThrowException_WhenStudentIdClaimIsNotNumber()
    {
        _user.FindFirst(CustomClaimTypes.StudentId).Returns(new Claim("", "abc"));

        await new Func<Task>(() => _sut.GetByCourse(10)).Should()
            .ThrowAsync<DomainException>()
            .WithMessage("Current user is not logged in as student or claim " +
                         $"'{CustomClaimTypes.StudentId}' is not a number\nError code: 403");
        await _repository.DidNotReceive().GetByStudentAndCourse(Arg.Any<int>(), Arg.Any<int>());
    }

    [Fact]
    public async Task GetCourse_ShouldReturnFromRepository_WhenNoExceptionThrown()
    {
        _user.FindFirst(CustomClaimTypes.StudentId).Returns(new Claim("", "1"));
        var submission = new TaskSubmission { Id = 10 };
        _repository.GetByStudentAndCourse(1, 10).Returns(submission);

        var result = await _sut.GetByCourse(10);

        result.Id.Should().Be(10);
    }
}