using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Requests.Student;
using ECampus.Domain.Responses;
using ECampus.Domain.Responses.Student;
using ECampus.Services.Contracts.Services;
using ECampus.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Tests.Unit.Tests.BackEnd.Api.Controllers;

public class StudentsControllerTests
{
    private readonly IGetByParametersHandler<MultipleStudentResponse, StudentParameters> _handler =
        Substitute.For<IGetByParametersHandler<MultipleStudentResponse, StudentParameters>>();

    private readonly IBaseService<StudentDto> _baseService = Substitute.For<IBaseService<StudentDto>>();
    private readonly StudentsController _sut;
    private readonly Fixture _fixture;

    public StudentsControllerTests()
    {
        _sut = new StudentsController(_handler, _baseService);
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task GetById_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Build<StudentDto>().With(t => t.Id, 10).Create();
        _baseService.GetByIdAsync(10).Returns(data);

        var actionResult = await _sut.Get(10);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().GetByIdAsync(10);
    }

    [Fact]
    public async Task Delete_ReturnsIdFromService()
    {
        var data = _fixture.Build<StudentDto>().With(t => t.Id, 10).Create();
        _baseService.DeleteAsync(10).Returns(data);

        var actionResult = await _sut.Delete(10);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().DeleteAsync(10);
    }

    [Fact]
    public async Task Create_ReturnsFromService()
    {
        var data = _fixture.Create<StudentDto>();
        _baseService.CreateAsync(data).Returns(data);

        var actionResult = await _sut.Post(data);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().CreateAsync(data);
    }

    [Fact]
    public async Task Update_ReturnsFromService()
    {
        var data = _fixture.Create<StudentDto>();
        _baseService.UpdateAsync(data).Returns(data);

        var actionResult = await _sut.Put(data);

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _baseService.Received().UpdateAsync(data);
    }

    [Fact]
    public async Task GetByParameters_ReturnsFromService()
    {
        var data = _fixture.Build<ListWithPaginationData<MultipleStudentResponse>>()
            .With(l => l.Data, Enumerable.Range(0, 5)
                .Select(_ => _fixture.Create<MultipleStudentResponse>()).ToList())
            .Create();

        _handler.GetByParametersAsync(Arg.Any<StudentParameters>()).Returns(data);
        var actionResult = await _sut.Get(new StudentParameters());

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _handler.GetByParametersAsync(Arg.Any<StudentParameters>());
    }
}