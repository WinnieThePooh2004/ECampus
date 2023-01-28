using ECampus.Api.Controllers;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Interfaces.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Tests.Unit.BackEnd.Api.Controllers;

public class UserRoleControllerTests
{
    private readonly UserRolesController _sut;
    private readonly IUserRolesService _service = Substitute.For<IUserRolesService>();

    public UserRoleControllerTests()
    {
        _sut = new UserRolesController(_service);
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
}