using ECampus.Contracts.Services;
using ECampus.Core.Messages;
using ECampus.Domain.Interfaces;
using ECampus.Domain.Messaging;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;

namespace ECampus.Tests.Unit.BackEnd.Domain.Messaging;

public class UserRoleMessagesServiceTests
{
    private readonly UserRolesMessagingService _sut;
    private readonly IUserRolesService _baseService = Substitute.For<IUserRolesService>();
    private readonly ISnsMessenger _snsMessenger = Substitute.For<ISnsMessenger>();

    public UserRoleMessagesServiceTests()
    {
        _sut = new UserRolesMessagingService(_baseService, _snsMessenger);
    }
    
    [Fact]
    public async Task Update_ShouldReturnFromBaseServiceAndSendMessage()
    {
        var user = new UserDto { Email = "email", Id = 10, Username = "username", Role = UserRole.Admin };
        _baseService.UpdateAsync(user).Returns(user);

        var result = await _sut.UpdateAsync(user);

        result.Should().Be(user);
        await _snsMessenger.Received()
            .PublishMessageAsync(Arg.Is<UserUpdated>(t =>
                t.Email == user.Email && t.Username == user.Username));
    }
    
    [Fact]
    public async Task Create_ShouldReturnFromBaseServiceAndSendMessage()
    {
        var user = new UserDto { Email = "email", Id = 10, Username = "username", Role = UserRole.Admin };
        _baseService.CreateAsync(user).Returns(user);

        var result = await _sut.CreateAsync(user);

        result.Should().Be(user);
        await _snsMessenger.Received()
            .PublishMessageAsync(Arg.Is<UserCreated>(t =>
                t.Email == user.Email && t.Username == user.Username));
    }
    
    [Fact]
    public async Task GetById_ShouldReturnFromBaseService()
    {
        var user = new UserDto();
        _baseService.GetByIdAsync(10).Returns(user);

        var result = await _sut.GetByIdAsync(10);

        result.Should().Be(user);
        await _baseService.Received(1).GetByIdAsync(10);
    }
    
    [Fact]
    public async Task Delete_ShouldReturnFromBaseService()
    {
        var user = new UserDto();
        _baseService.DeleteAsync(10).Returns(user);

        var result = await _sut.DeleteAsync(10);

        result.Should().Be(user);
        await _baseService.Received(1).DeleteAsync(10);
    }
}