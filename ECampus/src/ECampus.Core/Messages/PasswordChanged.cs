namespace ECampus.Core.Messages;

public class PasswordChanged : ISqsMessage
{
    public required int UserId { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
}