using ECampus.Domain.DataContainers;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.QueryParameters;
using ECampus.Services.Contracts.Services;
using ECampus.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Tests.Unit.Tests.BackEnd.Api.Controllers;

public class TaskSubmissionControllerTests
{
    private readonly TaskSubmissionsController _sut;
    private readonly ITaskSubmissionService _service = Substitute.For<ITaskSubmissionService>();

    private readonly IParametersService<TaskSubmissionDto, TaskSubmissionParameters> _parametersService =
        Substitute.For<IParametersService<TaskSubmissionDto, TaskSubmissionParameters>>();

    private readonly Fixture _fixture = new();

    public TaskSubmissionControllerTests()
    {
        _sut = new TaskSubmissionsController(_parametersService, _service);
    }

    [Fact]
    public async Task GetByParameters_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Build<ListWithPaginationData<TaskSubmissionDto>>()
            .With(l => l.Data, Enumerable.Range(0, 5)
                .Select(_ => _fixture.Create<TaskSubmissionDto>()).ToList())
            .Create();

        _parametersService.GetByParametersAsync(Arg.Any<TaskSubmissionParameters>()).Returns(data);
        var actionResult = await _sut.Get(new TaskSubmissionParameters());

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _parametersService.Received().GetByParametersAsync(Arg.Any<TaskSubmissionParameters>());
    }

    [Fact]
    public async Task GetById_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Build<TaskSubmissionDto>().With(t => t.Id, 10).Create();
        _service.GetByIdAsync(10).Returns(data);

        var actionResult = await _sut.Get(10);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _service.Received().GetByIdAsync(10);
    }

    [Fact]
    public async Task GetByCourse_ShouldReturnFromService()
    {
        var data = _fixture.Build<TaskSubmissionDto>().With(t => t.Id, 10).Create();
        _service.GetByCourseAsync(10).Returns(data);

        var actionResult = await _sut.GetByCourse(10);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _service.Received().GetByCourseAsync(10);
    }

    [Fact]
    public async Task UpdateMark_ShouldReturnFromService()
    {
        var actionResult = await _sut.UpdateMark(new UpdateSubmissionMarkDto { SubmissionId = 10, Mark = 20 });

        actionResult.Should().BeOfType<OkObjectResult>();
        await _service.Received(1).UpdateMarkAsync(10, 20);
    }

    [Fact]
    public async Task UpdateContent_ShouldReturnFromService()
    {
        var actionResult = await _sut.UpdateContent(new UpdateSubmissionContentDto
            { SubmissionId = 10, Content = "New content" });

        actionResult.Should().BeOfType<OkObjectResult>();
        await _service.Received(1).UpdateContentAsync(10, "New content");
    }
}