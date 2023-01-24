namespace ECampus.Shared.Messaging.Users;

public class UserDeleted : IMessage
{
    public required int UserId { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
}