using System.Security.Claims;
using AutoMapper;
using ECampus.Contracts.DataAccess;
using ECampus.Services.Services;
using ECampus.Shared.Auth;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using ECampus.Tests.Shared.DataFactories;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.AspNetCore.Http;

namespace ECampus.Tests.Unit.BackEnd.Domain.Services;

public class TaskSubmissionServiceTests
{
    private readonly TaskSubmissionService _sut;
    private readonly ClaimsPrincipal _user = Substitute.For<ClaimsPrincipal>();
    private readonly IMapper _mapper = MapperFactory.Mapper;
    private readonly IDataAccessManager _dataAccessManager = Substitute.For<IDataAccessManager>();

    private readonly IParametersDataAccessManager
        _parametersDataAccess = Substitute.For<IParametersDataAccessManager>();

    public TaskSubmissionServiceTests()
    {
        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext = Substitute.For<HttpContext>();
        httpContextAccessor.HttpContext.User.Returns(_user);
        _sut = new TaskSubmissionService(httpContextAccessor, _mapper, _dataAccessManager, _parametersDataAccess);
    }

    [Fact]
    public async Task GetById_ShouldReturnFromDataAccess()
    {
        var submission = new TaskSubmission { Id = new Random().Next() };
        _dataAccessManager.GetByIdAsync<TaskSubmission>(1).Returns(submission);

        var result = await _sut.GetByIdAsync(1);

        result.Id.Should().Be(submission.Id);
    }

    [Fact]
    public async Task UpdateContext_ShouldUpdateContent()
    {
        var submission = new TaskSubmission { Id = new Random().Next(), SubmissionContent = "old" };
        _dataAccessManager.GetByIdAsync<TaskSubmission>(1).Returns(submission);

        await _sut.UpdateContentAsync(1, "new");

        submission.SubmissionContent.Should().Be("new");
        await _dataAccessManager.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task UpdateMark_ShouldUpdateMark()
    {
        var submission = new TaskSubmission { Id = new Random().Next(), TotalPoints = 0 };
        _dataAccessManager.GetByIdAsync<TaskSubmission>(1).Returns(submission);

        await _sut.UpdateMarkAsync(1, 20);

        submission.TotalPoints.Should().Be(20);
        await _dataAccessManager.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task GetByCourse_ShouldReturnValue_WhenDataAccessReturnsSingle()
    {
        _user.FindFirst(CustomClaimTypes.StudentId).Returns(new Claim("", "10"));
        var values = new List<TaskSubmission> { new() };
        var asyncQueryable = new DbSetMock<TaskSubmission>(values).Object;
        _parametersDataAccess.GetByParameters<TaskSubmission, TaskSubmissionByStudentAndCourseParameters>(
            Arg.Any<TaskSubmissionByStudentAndCourseParameters>()).Returns(asyncQueryable);

        var result = await _sut.GetByCourseAsync(10);

        result.Should().BeEquivalentTo(values[0]);
    }

    [Fact]
    public async Task GetByCourse_ShouldThrowException_WhenDataAccessReturnsEmpty()
    {
        _user.FindFirst(CustomClaimTypes.StudentId).Returns(new Claim("", "15"));
        var set = new DbSetMock<TaskSubmission>().Object;
        _parametersDataAccess.GetByParameters<TaskSubmission, TaskSubmissionByStudentAndCourseParameters>(
            Arg.Any<TaskSubmissionByStudentAndCourseParameters>()).Returns(set);

        await new Func<Task>(() => _sut.GetByCourseAsync(10)).Should()
            .ThrowExactlyAsync<InfrastructureExceptions>()
            .WithMessage("There is not any submissions with StudentId=15 and TaskId=10\nError code: 404");
    }
}