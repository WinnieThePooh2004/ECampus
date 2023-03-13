using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Requests.User;
using ECampus.Domain.Responses;
using ECampus.Domain.Responses.User;
using ECampus.Services.Contracts.Services;
using ECampus.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Tests.Unit.Tests.BackEnd.Api.Controllers;

public class UserRoleControllerTests
{
    private readonly UsersController _sut;
    private readonly IBaseService<UserDto> _service = Substitute.For<IBaseService<UserDto>>();

    private readonly IGetByParametersHandler<MultipleUserResponse, UserParameters> _getByParametersHandler =
        Substitute.For<IGetByParametersHandler<MultipleUserResponse, UserParameters>>();

    private readonly Fixture _fixture = new();

    public UserRoleControllerTests()
    {
        _sut = new UsersController(_service, _getByParametersHandler);
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
        var data = _fixture.Build<ListWithPaginationData<MultipleUserResponse>>()
            .With(l => l.Data, Enumerable.Range(0, 5)
                .Select(_ => _fixture.Create<MultipleUserResponse>()).ToList()).Create();

        _getByParametersHandler.GetByParametersAsync(Arg.Any<UserParameters>()).Returns(data);
        var actionResult = await _sut.Get(new UserParameters());

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().Value.Should().Be(data);
        await _getByParametersHandler.Received().GetByParametersAsync(Arg.Any<UserParameters>());
    }
}