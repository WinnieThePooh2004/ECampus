using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Messaging.Users;

namespace ECampus.Domain.Mapping.Messages;

public static class MessagesMapping
{
    public static UserCreated ToCreatedUserMessage(this UserDto user)
        => new()
        {
            UserId = user.Id,
            Email = user.Email,
            Username = user.Username,
            Role = user.Role.ToString()
        };

    public static UserUpdated ToUserUpdatedMessage(this UserDto user)
        => new()
        {
            UserId = user.Id,
            Email = user.Email,
            Username = user.Username,
            Role = user.Role.ToString()
        };

    public static UserDeleted ToUserDeletedMessage(this UserDto user)
        => new()
        {
            UserId = user.Id,
            Email = user.Email,
            Username = user.Username
        };
}