using ECampus.Contracts.Services;
using ECampus.Core.Messages;
using ECampus.Domain.Interfaces;
using ECampus.Domain.Messaging;
using ECampus.Services.Services.Messaging;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;

namespace ECampus.Tests.Unit.BackEnd.Domain.Services;

public class PasswordChangeMessagingServiceTests
{
    private readonly IPasswordChangeService _baseService = Substitute.For<IPasswordChangeService>();
    private readonly PasswordChangeMessagingService _sut;
    private readonly ISnsMessenger _snsMessenger = Substitute.For<ISnsMessenger>();

    public PasswordChangeMessagingServiceTests()
    {
        _sut = new PasswordChangeMessagingService(_baseService, _snsMessenger);
    }

    [Fact]
    public async Task Validate_ShouldReturnFromBaseService()
    {
        var validationResult = new ValidationResult();
        var passwordChange = new PasswordChangeDto();
        _baseService.ValidatePasswordChange(passwordChange).Returns(validationResult);

        var actual = await _sut.ValidatePasswordChange(passwordChange);

        ((object)actual).Should().Be(validationResult);
    }

    [Fact]
    public async Task ChangePassword_ShouldSendMessage()
    {
        var user = new UserDto { Email = "email", Username = "username" };
        var passwordChange = new PasswordChangeDto();
        _baseService.ChangePassword(passwordChange).Returns(user);

        var result = await _sut.ChangePassword(passwordChange);

        result.Should().Be(user);
        await _snsMessenger.Received(1)
            .PublishMessageAsync(Arg.Is<PasswordChanged>(p => p.Username == user.Username && p.Email == user.Email));
    }
}