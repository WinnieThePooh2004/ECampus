using ECampus.Api.Controllers;
using ECampus.Contracts.Services;
using ECampus.Shared.DataContainers;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.QueryParameters;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Tests.Unit.BackEnd.Api.Controllers;

public class TeachersControllerTests
{
    private readonly IParametersService<TeacherDto, TeacherParameters> _service =
        Substitute.For<IParametersService<TeacherDto, TeacherParameters>>();

    private readonly IBaseService<TeacherDto> _baseService = Substitute.For<IBaseService<TeacherDto>>();
    private readonly TeachersController _sut;
    private readonly Fixture _fixture;

    public TeachersControllerTests()
    {
        _sut = new TeachersController(_service, _baseService);
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task GetById_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Build<TeacherDto>().With(t => t.Id, 10).Create();
        _baseService.GetByIdAsync(10).Returns(data);
        
        var actionResult = await _sut.Get(10);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().GetByIdAsync(10);
    }

    [Fact]
    public async Task Delete_ReturnsIdFromService_ServiceCalled()
    {
        var data = _fixture.Build<TeacherDto>().With(t => t.Id, 10).Create();
        _baseService.DeleteAsync(10).Returns(data);
        
        var actionResult = await _sut.Delete(10);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().DeleteAsync(10);
    }

    [Fact]
    public async Task Create_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Create<TeacherDto>();
        _baseService.CreateAsync(data).Returns(data);

        var actionResult = await _sut.Post(data);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().CreateAsync(data);
    }

    [Fact]
    public async Task Update_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Create<TeacherDto>();
        _baseService.UpdateAsync(data).Returns(data);

        var actionResult = await _sut.Put(data);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().UpdateAsync(data);
    }

    [Fact]
    public async Task GetByParameters_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Build<ListWithPaginationData<TeacherDto>>()
            .With(l => l.Data, Enumerable.Range(0, 5)
                .Select(_ => _fixture.Create<TeacherDto>()).ToList())
            .Create();

        _service.GetByParametersAsync(Arg.Any<TeacherParameters>()).Returns(data);
        var actionResult = await _sut.Get(new TeacherParameters());

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _service.Received().GetByParametersAsync(Arg.Any<TeacherParameters>());
    }
}