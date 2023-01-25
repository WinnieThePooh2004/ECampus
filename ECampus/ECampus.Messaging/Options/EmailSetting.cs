namespace ECampus.Messaging.Options;

public class EmailSetting
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string HostName { get; init; }
}