using ECampus.Api.Controllers;
using ECampus.Contracts.Services;
using ECampus.Shared.DataContainers;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.QueryParameters;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Tests.Unit.BackEnd.Api.Controllers;

public class UserRoleControllerTests
{
    private readonly UsersController _sut;
    private readonly IBaseService<UserDto> _service = Substitute.For<IBaseService<UserDto>>();

    private readonly IParametersService<UserDto, UserParameters> _parametersService =
        Substitute.For<IParametersService<UserDto, UserParameters>>();

    private readonly Fixture _fixture = new();

    public UserRoleControllerTests()
    {
        _sut = new UsersController(_service, _parametersService);
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task Put_ShouldReturnFromService()
    {
        var user = new UserDto();
        _service.UpdateAsync(user).Returns(user);

        var result = await _sut.Put(user);

        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(user);
        await _service.Received(1).UpdateAsync(user);
    }

    [Fact]
    public async Task Post_ShouldReturnFromService()
    {
        var user = new UserDto();
        _service.CreateAsync(user).Returns(user);

        var result = await _sut.Post(user);

        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(user);
        await _service.Received(1).CreateAsync(user);
    }

    [Fact]
    public async Task Get_ShouldReturnFromService()
    {
        var user = new UserDto();
        _service.GetByIdAsync(10).Returns(user);

        var result = await _sut.Get(10);

        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(user);
        await _service.Received(1).GetByIdAsync(10);
    }

    [Fact]
    public async Task GetByParameters_ReturnsFromService_ServiceCalled()
    {
        var data = _fixture.Build<ListWithPaginationData<UserDto>>()
            .With(l => l.Data, Enumerable.Range(0, 5)
                .Select(_ => _fixture.Create<UserDto>()).ToList()).Create();

        _parametersService.GetByParametersAsync(Arg.Any<UserParameters>()).Returns(data);
        var actionResult = await _sut.Get(new UserParameters());

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _parametersService.Received().GetByParametersAsync(Arg.Any<UserParameters>());
    }
}