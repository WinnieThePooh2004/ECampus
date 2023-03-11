using ECampus.Core.Messages;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Services.Contracts.Messaging;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Services.Messaging;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Messaging;

public class UserMessagesServiceTests
{
    private readonly UserMessagingService _sut;
    private readonly ISnsMessenger _snsMessenger = Substitute.For<ISnsMessenger>();
    private readonly IBaseService<UserDto> _baseService = Substitute.For<IBaseService<UserDto>>();

    public UserMessagesServiceTests()
    {
        _sut = new UserMessagingService(_baseService, _snsMessenger);
    }

    [Fact]
    public async Task Delete_ShouldReturnFromBaseServiceAndSendMessage()
    {
        var user = new UserDto { Email = "email", Id = 10, Username = "username" };
        _baseService.DeleteAsync(10).Returns(user);

        var result = await _sut.DeleteAsync(10);

        result.Should().Be(user);
        await _snsMessenger.Received()
            .PublishMessageAsync(Arg.Is<UserDeleted>(t =>
                t.Email == user.Email && t.Username == user.Username));
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
}