using ECampus.Services.Contracts.Services;
using ECampus.Shared.DataContainers;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.QueryParameters;
using ECampus.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Tests.Unit.Tests.BackEnd.Api.Controllers;

public class FacultiesControllerTests
{
    private readonly IParametersService<FacultyDto, FacultyParameters> _service =
        Substitute.For<IParametersService<FacultyDto, FacultyParameters>>();

    private readonly IBaseService<FacultyDto> _baseService = Substitute.For<IBaseService<FacultyDto>>();
    private readonly FacultiesController _sut;
    private readonly Fixture _fixture;

    public FacultiesControllerTests()
    {
        _sut = new FacultiesController(_service, _baseService);
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task GetById_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Build<FacultyDto>().With(t => t.Id, 10).Create();
        _baseService.GetByIdAsync(10).Returns(data);
        
        var actionResult = await _sut.Get(10);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().GetByIdAsync(10);
    }

    [Fact]
    public async Task Delete_ReturnsIdFromService_ServiceCalled()
    {
        var data = _fixture.Build<FacultyDto>().With(t => t.Id, 10).Create();
        _baseService.DeleteAsync(10).Returns(data);
        
        var actionResult = await _sut.Delete(10);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().DeleteAsync(10);
    }

    [Fact]
    public async Task Create_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Create<FacultyDto>();
        _baseService.CreateAsync(data).Returns(data);

        var actionResult = await _sut.Post(data);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().CreateAsync(data);
    }

    [Fact]
    public async Task Update_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Create<FacultyDto>();
        _baseService.UpdateAsync(data).Returns(data);

        var actionResult = await _sut.Put(data);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().UpdateAsync(data);
    }

    [Fact]
    public async Task GetByParameters_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Build<ListWithPaginationData<FacultyDto>>()
            .With(l => l.Data, Enumerable.Range(0, 5)
                .Select(_ => _fixture.Create<FacultyDto>()).ToList())
            .Create();

        _service.GetByParametersAsync(Arg.Any<FacultyParameters>()).Returns(data);
        var actionResult = await _sut.Get(new FacultyParameters());

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _service.Received().GetByParametersAsync(Arg.Any<FacultyParameters>());
    }
}