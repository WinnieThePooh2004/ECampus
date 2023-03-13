namespace ECampus.Tests.Unit.Tests.BackEnd.Api.Controllers;

public class AuditoriesControllerTests
{
    // private readonly IGetByParametersHandler<MultipleAuditoryResponse, AuditoryParameters> _handler =
    //     Substitute.For<IGetByParametersHandler<MultipleAuditoryResponse, AuditoryParameters>>();
    //
    // private readonly IBaseService<AuditoryDto> _baseService = Substitute.For<IBaseService<AuditoryDto>>();
    //
    // private readonly AuditoriesController _sut;
    // private readonly Fixture _fixture;
    //
    // public AuditoriesControllerTests()
    // {
    //     _sut = new AuditoriesController(_handler, _baseService);
    //     _fixture = new Fixture();
    //     _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    // }
    //
    // [Fact]
    // public async Task GetById_ReturnsFromService_ServiceCalled()
    // {
    //     var data = _fixture.Build<AuditoryDto>().With(t => t.Id, 10).Create();
    //     _baseService.GetByIdAsync(10).Returns(data);
    //     
    //     var actionResult = await _sut.Get(10);
    //
    //     actionResult.Should().BeOfType<OkObjectResult>();
    //     actionResult.As<OkObjectResult>().Value.Should().Be(data);
    //     await _baseService.Received().GetByIdAsync(10);
    // }
    //
    // [Fact]
    // public async Task Delete_ReturnsIdFromService_ServiceCalled()
    // {
    //     var data = _fixture.Build<AuditoryDto>().With(t => t.Id, 10).Create();
    //     _baseService.DeleteAsync(10).Returns(data);
    //     
    //     var actionResult = await _sut.Delete(10);
    //
    //     actionResult.Should().BeOfType<OkObjectResult>();
    //     actionResult.As<OkObjectResult>().Value.Should().Be(data);
    //     await _baseService.Received().DeleteAsync(10);
    // }
    //
    // [Fact]
    // public async Task Create_ReturnsFromService_ServiceCalled()
    // {
    //     var data = _fixture.Create<AuditoryDto>();
    //     _baseService.CreateAsync(data).Returns(data);
    //
    //     var actionResult = await _sut.Post(data);
    //
    //     actionResult.Should().BeOfType<OkObjectResult>();
    //     actionResult.As<OkObjectResult>().Value.Should().Be(data);
    //     await _baseService.Received().CreateAsync(data);
    // }
    //
    // [Fact]
    // public async Task Update_ReturnsFromService_ServiceCalled()
    // {
    //     var data = _fixture.Create<AuditoryDto>();
    //     _baseService.UpdateAsync(data).Returns(data);
    //
    //     var actionResult = await _sut.Put(data);
    //
    //     actionResult.Should().BeOfType<OkObjectResult>();
    //     actionResult.As<OkObjectResult>().Value.Should().Be(data);
    //     await _baseService.Received().UpdateAsync(data);
    // }
    //
    // [Fact]
    // public async Task GetByParameters_ReturnsFromService_ServiceCalled()
    // {
    //     var data = _fixture.Build<ListWithPaginationData<MultipleAuditoryResponse>>()
    //         .With(l => l.Data, Enumerable.Range(0, 5)
    //             .Select(_ => _fixture.Create<MultipleAuditoryResponse>()).ToList())
    //         .Create();
    //
    //     _handler.GetByParametersAsync(Arg.Any<AuditoryParameters>()).Returns(data);
    //     var actionResult = await _sut.Get(new AuditoryParameters());
    //
    //     actionResult.Should().BeOfType<OkObjectResult>();
    //     actionResult.As<OkObjectResult>().Value.Should().Be(data);
    //     await _handler.Received().GetByParametersAsync(Arg.Any<AuditoryParameters>());
    // }
}