namespace ECampus.Messaging.Messaging.Users;

public class UserDeleted : ISqsMessage
{
    public required int UserId { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
}