using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Requests.Course;
using ECampus.Domain.Responses;
using ECampus.Domain.Responses.Course;
using ECampus.Services.Contracts.Services;
using ECampus.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Tests.Unit.Tests.BackEnd.Api.Controllers;

public class CoursesControllerTests
{
    private readonly IGetByParametersHandler<MultipleCourseResponse, CourseParameters> _handler =
        Substitute.For<IGetByParametersHandler<MultipleCourseResponse, CourseParameters>>();

    private readonly IBaseService<CourseDto> _baseService = Substitute.For<IBaseService<CourseDto>>();
    private readonly CoursesController _sut;
    private readonly Fixture _fixture;

    public CoursesControllerTests()
    {
        _sut = new CoursesController(_handler, _baseService);
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task GetById_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Build<CourseDto>().With(t => t.Id, 10).Create();
        _baseService.GetByIdAsync(10).Returns(data);

        var actionResult = await _sut.Get(10);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().GetByIdAsync(10);
    }

    [Fact]
    public async Task Delete_ReturnsIdFromService_ServiceCalled()
    {
        var data = _fixture.Build<CourseDto>().With(t => t.Id, 10).Create();
        _baseService.DeleteAsync(10).Returns(data);

        var actionResult = await _sut.Delete(10);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().DeleteAsync(10);
    }

    [Fact]
    public async Task Create_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Create<CourseDto>();
        _baseService.CreateAsync(data).Returns(data);

        var actionResult = await _sut.Post(data);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().CreateAsync(data);
    }

    [Fact]
    public async Task Update_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Create<CourseDto>();
        _baseService.UpdateAsync(data).Returns(data);

        var actionResult = await _sut.Put(data);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().UpdateAsync(data);
    }

    [Fact]
    public async Task GetByParameters_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Build<ListWithPaginationData<MultipleCourseResponse>>()
            .With(l => l.Data, Enumerable.Range(0, 5)
                .Select(_ => _fixture.Create<MultipleCourseResponse>()).ToList())
            .Create();

        _handler.GetByParametersAsync(Arg.Any<CourseParameters>()).Returns(data);
        var actionResult = await _sut.Get(new CourseParameters());

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _handler.Received().GetByParametersAsync(Arg.Any<CourseParameters>());
    }
}