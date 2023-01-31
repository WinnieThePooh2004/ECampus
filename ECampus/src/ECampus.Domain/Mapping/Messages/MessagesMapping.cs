using ECampus.Core.Messages;
using ECampus.Shared.DataTransferObjects;

namespace ECampus.Domain.Mapping.Messages;

public static class MessagesMapping
{
    public static UserCreated ToCreatedUserMessage(this UserDto user)
        => new()
        {
            Email = user.Email,
            Username = user.Username,
        };

    public static UserUpdated ToUserUpdatedMessage(this UserDto user)
        => new()
        {
            Email = user.Email,
            Username = user.Username,
        };

    public static UserDeleted ToUserDeletedMessage(this UserDto user)
        => new()
        {
            Email = user.Email,
            Username = user.Username
        };
}